/**********************************************************************************************
    Script      : Tudo em Dia (Fase 2) - Apontamento / execução do checklist
    Data        : 10/07/2026
    Descricao   : Tabelas e SPs da execução do checklist do local (espelho do UH em Dia),
                  puxando checklist/periodicidade/intervalo do PRÓPRIO local (tb_cad_apartamento)
                  e recalculando a próxima data pela periodicidade do local (convenção PWA:
                  1=diária, 2=semanal, 3=mensal(1º dia), 5=bimestral, 6=trimestral, 7=semestral).
                  Item "NÃO" abre Ordem de Serviço automática (prioridade alta) — igual ao UH.

    Objetos (novos):
        - Tabela    : tb_tudo_apontamento
        - Tabela    : tb_tudo_apontamento_checklist
        - Coluna    : tb_pcm_ordem_servico.codigo_tudo_apontamento
        - Procedure : sp_select_tudo_apontamento
        - Procedure : sp_select_tudo_apontamento_checklist_item
        - Procedure : sp_insert_tudo_apontamento
        - Procedure : sp_insert_tudo_apontamento_checklist
        - Procedure : sp_status_tudo_dia
**********************************************************************************************/

/*--------------------------------------------------------------------------------------------
    1) TABELAS
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.tb_tudo_apontamento', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[tb_tudo_apontamento](
        [codigo]                            [bigint]    NOT NULL,
        [codigo_empresa]                    [smallint]  NOT NULL,
        [codigo_unidade]                    [int]       NOT NULL,
        [codigo_apartamento]                [int]       NOT NULL,
        [codigo_checklist]                  [bigint]    NULL,
        [codigo_funcionario_responsavel]    [int]       NULL,
        [data_inicio]                       [date]      NULL,
        [data_termino]                      [date]      NULL,
        [hora_inicio]                       [time](0)   NULL,
        [hora_termino]                      [time](0)   NULL,
        [nova_vistoria]                     [bit]       NULL,
        [origem]                            [varchar](20) NULL,
        [codigo_usuario_input]              [int]       NULL,
        [data_input]                        [datetime]  NULL,
     CONSTRAINT [PK_tb_tudo_apontamento] PRIMARY KEY CLUSTERED
        ([codigo] ASC, [codigo_empresa] ASC, [codigo_unidade] ASC)
    ) ON [PRIMARY]
END
GO

IF OBJECT_ID('dbo.tb_tudo_apontamento_checklist', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[tb_tudo_apontamento_checklist](
        [codigo_tudo_apontamento]   [bigint]    NOT NULL,
        [codigo_empresa]            [smallint]  NOT NULL,
        [codigo_unidade]            [int]       NOT NULL,
        [codigo_checklist]          [bigint]    NOT NULL,
        [codigo_checklist_item]     [int]       NOT NULL,
        [grupo]                     [varchar](100) NULL,
        [checklist]                 [varchar](200) NULL,
        [descricao]                 [varchar](500) NULL,
        [opcao]                     [varchar](5)   NULL,
        [observacao]                [varchar](200) NULL,
        [nova_vistoria]             [bit]       NULL,
        [codigo_ordem_servico]      [bigint]    NULL,
     CONSTRAINT [PK_tb_tudo_apontamento_checklist] PRIMARY KEY CLUSTERED
        ([codigo_tudo_apontamento] ASC, [codigo_empresa] ASC, [codigo_unidade] ASC, [codigo_checklist] ASC, [codigo_checklist_item] ASC)
    ) ON [PRIMARY]
END
GO

IF COL_LENGTH('dbo.tb_pcm_ordem_servico', 'codigo_tudo_apontamento') IS NULL
    ALTER TABLE [dbo].[tb_pcm_ordem_servico] ADD [codigo_tudo_apontamento] [bigint] NULL
GO

/*--------------------------------------------------------------------------------------------
    2) SELECT cabeçalho do apontamento + contexto do local (para a tela de execução)
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_select_tudo_apontamento', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_tudo_apontamento]
GO

CREATE PROCEDURE [dbo].[sp_select_tudo_apontamento]
@codigo_empresa     smallint,
@codigo_unidade     int,
@codigo_apartamento int,
@codigo             bigint
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        ISNULL(ap.codigo, 0)                            AS codigo,
        a.codigo                                        AS codigo_apartamento,
        a.local,
        ISNULL(s.descricao, '')                         AS setor,
        a.codigo_checklist,
        ISNULL(c.descricao, '')                         AS checklist,
        ISNULL(p.descricao, '')                         AS periodicidade,
        ISNULL(a.intervalo, 0)                          AS intervalo,
        ISNULL(CONVERT(varchar(10), a.data_proximo_tudo, 103), '') AS data_proxima,
        ISNULL(ap.codigo_funcionario_responsavel, 0)    AS codigo_funcionario_responsavel,
        ISNULL(CONVERT(varchar(10), ap.data_inicio, 103), CONVERT(varchar(10), GETDATE(), 103)) AS data_inicio,
        ISNULL(CONVERT(varchar(10), ap.data_termino, 103), CONVERT(varchar(10), GETDATE(), 103)) AS data_termino,
        ISNULL(CONVERT(varchar(5), ap.hora_inicio, 108), '') AS hora_inicio,
        ISNULL(CONVERT(varchar(5), ap.hora_termino, 108), '') AS hora_termino,
        CONVERT(bit, ISNULL(ap.nova_vistoria, 0))       AS nova_vistoria
    FROM
        tb_cad_apartamento a
        LEFT JOIN tb_cad_setor s ON s.codigo_empresa = a.codigo_empresa AND s.codigo = a.codigo_setor
        LEFT JOIN tb_chk_checklist c ON c.codigo_empresa = a.codigo_empresa AND c.codigo = a.codigo_checklist
        LEFT JOIN tb_stc_periodicidade p ON p.codigo = a.codigo_periodicidade
        LEFT JOIN tb_tudo_apontamento ap ON ap.codigo_empresa = a.codigo_empresa
             AND ap.codigo_unidade = a.codigo_unidade
             AND ap.codigo_apartamento = a.codigo
             AND ap.codigo = @codigo
    WHERE a.codigo_empresa = @codigo_empresa
    AND   a.codigo_unidade = @codigo_unidade
    AND   a.codigo = @codigo_apartamento

END
GO

/*--------------------------------------------------------------------------------------------
    3) SELECT itens do checklist (template quando @codigo = -1; respondidos caso contrário)
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_select_tudo_apontamento_checklist_item', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_tudo_apontamento_checklist_item]
GO

CREATE PROCEDURE [dbo].[sp_select_tudo_apontamento_checklist_item]
@codigo_empresa     smallint,
@codigo_unidade     int,
@codigo_apartamento int,
@codigo             bigint
AS
BEGIN

    SET NOCOUNT ON;

    IF @codigo = -1
    BEGIN

        -- Template a partir do checklist DO LOCAL (tb_cad_apartamento.codigo_checklist)
        SELECT
            ci.codigo,
            ci.grupo,
            ci.checklist,
            ci.descricao,
            'SIM'   AS opcao,
            ''      AS observacao,
            0       AS nova_vistoria
        FROM
            tb_cad_apartamento a
            INNER JOIN tb_chk_checklist_item ci ON
                ci.codigo_checklist = a.codigo_checklist AND
                ci.codigo_empresa = a.codigo_empresa
        WHERE a.codigo_empresa = @codigo_empresa
        AND   a.codigo_unidade = @codigo_unidade
        AND   a.codigo = @codigo_apartamento
        ORDER BY ci.grupo, ci.codigo

    END
    ELSE
    BEGIN

        -- Itens já respondidos
        SELECT
            ac.codigo_checklist_item                AS codigo,
            ISNULL(ac.grupo, '')                    AS grupo,
            ISNULL(ac.checklist, '')                AS checklist,
            ISNULL(ac.descricao, '')                AS descricao,
            ISNULL(ac.opcao, '')                    AS opcao,
            ISNULL(ac.observacao, '')               AS observacao,
            CONVERT(bit, ISNULL(ac.nova_vistoria, 0)) AS nova_vistoria
        FROM
            tb_tudo_apontamento_checklist ac
        WHERE ac.codigo_empresa = @codigo_empresa
        AND   ac.codigo_unidade = @codigo_unidade
        AND   ac.codigo_tudo_apontamento = @codigo
        ORDER BY ac.grupo, ac.codigo_checklist_item

    END

END
GO

/*--------------------------------------------------------------------------------------------
    4) INSERT do apontamento (cabeçalho) + recálculo inicial de próxima data / status = 5
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_insert_tudo_apontamento', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_insert_tudo_apontamento]
GO

CREATE PROCEDURE [dbo].[sp_insert_tudo_apontamento]
@codigo_empresa                 smallint,
@codigo_usuario                 int,
@codigo_unidade                 int,
@codigo_apartamento             int,
@codigo_funcionario_responsavel int,
@data_inicio                    date,
@data_termino                   date,
@hora_inicio                    time(0),
@hora_termino                   time(0),
@origem                         varchar(20) = 'WEB SITE - TUDO',
@codigo                         bigint OUTPUT,
@codigo_checklist               bigint OUTPUT
AS
BEGIN

    SET NOCOUNT ON;

    SET @codigo = ISNULL((SELECT MAX(codigo) FROM tb_tudo_apontamento
                          WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade), 0) + 1;

    SET @codigo_checklist = (SELECT codigo_checklist FROM tb_cad_apartamento
                             WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade AND codigo = @codigo_apartamento);

    INSERT INTO tb_tudo_apontamento(
        codigo, codigo_empresa, codigo_unidade, codigo_apartamento, codigo_checklist,
        codigo_funcionario_responsavel, data_inicio, data_termino, hora_inicio, hora_termino,
        nova_vistoria, origem, codigo_usuario_input, data_input)
    VALUES(
        @codigo, @codigo_empresa, @codigo_unidade, @codigo_apartamento, @codigo_checklist,
        @codigo_funcionario_responsavel, @data_inicio, @data_termino, @hora_inicio, @hora_termino,
        0, @origem, @codigo_usuario, GETDATE());

    UPDATE tb_cad_apartamento SET
        data_ultimo_tudo = @data_termino,
        data_proximo_tudo = CASE a.codigo_periodicidade
            WHEN 1 THEN DATEADD(DAY, ISNULL(a.intervalo, 1), @data_termino)
            WHEN 2 THEN DATEADD(DAY, -DATEPART(WEEKDAY, DATEADD(WEEK, ISNULL(a.intervalo, 1), @data_termino)) + 1, DATEADD(WEEK, ISNULL(a.intervalo, 1), @data_termino))
            WHEN 3 THEN DATEFROMPARTS(YEAR(DATEADD(MONTH, ISNULL(a.intervalo, 1), @data_termino)), MONTH(DATEADD(MONTH, ISNULL(a.intervalo, 1), @data_termino)), 1)
            WHEN 5 THEN DATEADD(MONTH, ((DATEDIFF(MONTH, 0, @data_termino) / 2) + 1) * 2, 0)
            WHEN 6 THEN DATEADD(MONTH, ((DATEDIFF(MONTH, 0, @data_termino) / 3) + 1) * 3, 0)
            WHEN 7 THEN DATEADD(MONTH, ((DATEDIFF(MONTH, 0, @data_termino) / 6) + 1) * 6, 0)
            ELSE DATEFROMPARTS(YEAR(DATEADD(MONTH, ISNULL(a.intervalo, 1), @data_termino)), MONTH(DATEADD(MONTH, ISNULL(a.intervalo, 1), @data_termino)), 1)
        END,
        codigo_tudo_apontamento = @codigo,
        status_tudo = 5
    FROM tb_cad_apartamento a
    WHERE a.codigo = @codigo_apartamento
    AND   a.codigo_empresa = @codigo_empresa
    AND   a.codigo_unidade = @codigo_unidade

END
GO

/*--------------------------------------------------------------------------------------------
    5) INSERT de um item respondido + abertura de OS quando "NÃO"
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_insert_tudo_apontamento_checklist', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_insert_tudo_apontamento_checklist]
GO

CREATE PROCEDURE [dbo].[sp_insert_tudo_apontamento_checklist]
@codigo_empresa             smallint,
@codigo_unidade             int,
@codigo_tudo_apontamento    bigint,
@codigo_checklist           bigint,
@codigo_checklist_item      int,
@descricao_checklist        varchar(500),
@opcao                      varchar(5),
@observacao                 varchar(200),
@nova_vistoria              bit
AS
BEGIN

    SET NOCOUNT ON;

    INSERT INTO tb_tudo_apontamento_checklist(
        codigo_tudo_apontamento, codigo_empresa, codigo_unidade, codigo_checklist, codigo_checklist_item,
        grupo, checklist, descricao, opcao, observacao, nova_vistoria)
    SELECT
        @codigo_tudo_apontamento, @codigo_empresa, @codigo_unidade, @codigo_checklist, @codigo_checklist_item,
        ISNULL(ci.grupo, 'ADICIONADO'), ci.checklist, ISNULL(ci.descricao, @descricao_checklist),
        @opcao, UPPER(@observacao), @nova_vistoria
    FROM (SELECT 1 AS x) z
        LEFT JOIN tb_chk_checklist_item ci ON
            ci.codigo_checklist = @codigo_checklist AND
            ci.codigo_empresa = @codigo_empresa AND
            ci.codigo = @codigo_checklist_item;

    IF @opcao = 'NÃO'
    BEGIN

        DECLARE @codigo_ordem_servico bigint;
        DECLARE @numero_ordem_servico bigint;
        DECLARE @descricao varchar(5000);

        SET @codigo_ordem_servico = ISNULL((SELECT MAX(codigo) FROM tb_pcm_ordem_servico
                                            WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade), 0) + 1;

        SET @numero_ordem_servico = ISNULL((SELECT MAX(numero_documento) FROM tb_pcm_ordem_servico
                                            WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade), 0) + 1;

        SET @descricao = ISNULL((SELECT TOP 1 'PROBLEMA: ' + ISNULL(ci.grupo, '') + ' - ' + ISNULL(ci.checklist, '') + ' - ' + ISNULL(ci.descricao, @descricao_checklist)
                                        + CASE WHEN ISNULL(@observacao, '') = '' THEN '' ELSE CHAR(13) + @observacao END
                                 FROM tb_chk_checklist_item ci
                                 WHERE ci.codigo_checklist = @codigo_checklist AND ci.codigo_empresa = @codigo_empresa AND ci.codigo = @codigo_checklist_item),
                                'PROBLEMA: ' + ISNULL(@descricao_checklist, ''));

        -- garante a prioridade ALTA configurada (igual ao UH)
        IF (SELECT TOP 1 codigo_prioridade FROM tb_cfg_ordem_servico_prioridade
            WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade) IS NULL
        BEGIN
            INSERT INTO tb_cfg_ordem_servico_prioridade(codigo_empresa, codigo_unidade, codigo_prioridade)
            SELECT codigo_empresa, codigo_unidade, codigo
            FROM tb_cad_prioridade
            WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade AND descricao LIKE '%ALTA'
        END

        INSERT INTO tb_pcm_ordem_servico(
            codigo, codigo_empresa, codigo_unidade, numero_documento, data,
            codigo_setor, codigo_apartamento, codigo_equipamento, codigo_prioridade, descricao,
            status, data_necessidade, codigo_usuario_input, data_input, origem, codigo_tudo_apontamento)
        SELECT
            @codigo_ordem_servico, @codigo_empresa, @codigo_unidade, @numero_ordem_servico, GETDATE(),
            a.codigo_setor, a.codigo, NULL, prio.codigo_prioridade, @descricao,
            1, DATEADD(DAY, 7, GETDATE()), ap.codigo_usuario_input, GETDATE(), 'WEB SITE - TUDO', ap.codigo
        FROM
            tb_tudo_apontamento ap
            INNER JOIN tb_cad_apartamento a ON a.codigo = ap.codigo_apartamento AND a.codigo_empresa = ap.codigo_empresa AND a.codigo_unidade = ap.codigo_unidade
            INNER JOIN tb_cfg_ordem_servico_prioridade prio ON prio.codigo_empresa = ap.codigo_empresa AND prio.codigo_unidade = ap.codigo_unidade
        WHERE ap.codigo_empresa = @codigo_empresa AND ap.codigo_unidade = @codigo_unidade AND ap.codigo = @codigo_tudo_apontamento;

        UPDATE tb_tudo_apontamento_checklist SET
            codigo_ordem_servico = @codigo_ordem_servico
        WHERE codigo_tudo_apontamento = @codigo_tudo_apontamento
        AND   codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade
        AND   codigo_checklist = @codigo_checklist AND codigo_checklist_item = @codigo_checklist_item;

        IF @nova_vistoria = 1
        BEGIN
            UPDATE tb_tudo_apontamento SET nova_vistoria = 1
            WHERE codigo = @codigo_tudo_apontamento AND codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade;

            UPDATE tb_cad_apartamento SET status_tudo = 3
            WHERE codigo_tudo_apontamento = @codigo_tudo_apontamento AND codigo_empresa = @codigo_empresa;
        END

    END

END
GO

/*--------------------------------------------------------------------------------------------
    6) RECÁLCULO definitivo de status_tudo (por OS aberta) + próxima data
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_status_tudo_dia', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_status_tudo_dia]
GO

CREATE PROCEDURE [dbo].[sp_status_tudo_dia]
@codigo_empresa             smallint,
@codigo_unidade             int,
@codigo_tudo_apontamento    bigint
AS
BEGIN

    SET NOCOUNT ON;

    DECLARE @nova_vistoria bit;
    DECLARE @codigo_apartamento int;
    DECLARE @data_termino date;

    SELECT
        @codigo_apartamento = codigo_apartamento,
        @nova_vistoria = ISNULL(nova_vistoria, 0),
        @data_termino = data_termino
    FROM tb_tudo_apontamento
    WHERE codigo = @codigo_tudo_apontamento AND codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade;

    UPDATE tb_cad_apartamento SET
        status_tudo = CASE
            WHEN os.quantidade_os = 0 AND @nova_vistoria = 1 THEN 4
            WHEN os.quantidade_os = 0 AND @nova_vistoria = 0 THEN 5
            WHEN os.quantidade_os > 0 THEN 3
        END,
        data_ultimo_tudo = @data_termino,
        data_proximo_tudo = CASE a.codigo_periodicidade
            WHEN 1 THEN DATEADD(DAY, ISNULL(a.intervalo, 1), @data_termino)
            WHEN 2 THEN DATEADD(DAY, -DATEPART(WEEKDAY, DATEADD(WEEK, ISNULL(a.intervalo, 1), @data_termino)) + 1, DATEADD(WEEK, ISNULL(a.intervalo, 1), @data_termino))
            WHEN 3 THEN DATEFROMPARTS(YEAR(DATEADD(MONTH, ISNULL(a.intervalo, 1), @data_termino)), MONTH(DATEADD(MONTH, ISNULL(a.intervalo, 1), @data_termino)), 1)
            WHEN 5 THEN DATEADD(MONTH, ((DATEDIFF(MONTH, 0, @data_termino) / 2) + 1) * 2, 0)
            WHEN 6 THEN DATEADD(MONTH, ((DATEDIFF(MONTH, 0, @data_termino) / 3) + 1) * 3, 0)
            WHEN 7 THEN DATEADD(MONTH, ((DATEDIFF(MONTH, 0, @data_termino) / 6) + 1) * 6, 0)
            ELSE DATEFROMPARTS(YEAR(DATEADD(MONTH, ISNULL(a.intervalo, 1), @data_termino)), MONTH(DATEADD(MONTH, ISNULL(a.intervalo, 1), @data_termino)), 1)
        END,
        codigo_tudo_apontamento = @codigo_tudo_apontamento
    FROM tb_cad_apartamento a
        OUTER APPLY (
            SELECT COUNT(*) AS quantidade_os
            FROM tb_pcm_ordem_servico os
                INNER JOIN tb_tudo_apontamento ap ON os.codigo_tudo_apontamento = ap.codigo
                    AND os.codigo_empresa = ap.codigo_empresa AND os.codigo_unidade = ap.codigo_unidade
            WHERE ap.codigo_empresa = a.codigo_empresa
            AND   ap.codigo_unidade = a.codigo_unidade
            AND   ap.codigo_apartamento = a.codigo
            AND   os.status NOT IN (2, 99)
        ) os
    WHERE a.codigo = @codigo_apartamento AND a.codigo_empresa = @codigo_empresa

END
GO
