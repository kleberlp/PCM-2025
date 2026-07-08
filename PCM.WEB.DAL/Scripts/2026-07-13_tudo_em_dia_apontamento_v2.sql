/**********************************************************************************************
    Script      : Tudo em Dia (Fase 5) - Melhorias da execução e do histórico
    Data        : 13/07/2026
    Descricao   : Evolução da execução do checklist do Tudo em Dia atendendo aos ajustes:
                    - Todos os tipos de item (tb_stc_tipo_item_checklist): 1 SIM/NÃO,
                      2 NUMÉRICO, 3 TEXTO, 4 DATA, 5 HORA, 8 SIM/NÃO/N.A.
                    - Resposta livre (coluna resposta) além de opcao (Sim/Não/N.A.).
                    - Foto por item reutilizando tb_pcm_picture (tipo = 'TUDO').
                    - Abertura de OS OPCIONAL (coluna abre_os) e nova vistoria opcional.
                    - Cabeçalho "start/finaliza" (o apontamento é criado ao abrir a tela para
                      permitir anexar fotos antes de concluir).
                    - Histórico via grid dinâmico (sp_select_tudo_apontamento_grid) + exclusão
                      com recálculo da próxima execução (sp_delete_tudo_apontamento).

    Objetos:
        - Colunas   : tb_tudo_apontamento.finalizado
                      tb_tudo_apontamento_checklist.codigo_tipo_item_checklist
                      tb_tudo_apontamento_checklist.resposta
                      tb_tudo_apontamento_checklist.abre_os
        - Procedure : sp_start_tudo_apontamento              (nova)
        - Procedure : sp_finaliza_tudo_apontamento           (nova - substitui o create do insert)
        - Procedure : sp_insert_tudo_apontamento_checklist   (ALTER - tipo/resposta/abre_os, OS opcional)
        - Procedure : sp_select_tudo_apontamento_checklist_item (ALTER - tipo/foto/limites)
        - Procedure : sp_select_tudo_apontamento_grid        (nova - histórico loadGridMain)
        - Procedure : sp_delete_tudo_apontamento             (nova - exclusão + recálculo)
**********************************************************************************************/

/*--------------------------------------------------------------------------------------------
    1) COLUNAS NOVAS
--------------------------------------------------------------------------------------------*/
IF COL_LENGTH('dbo.tb_tudo_apontamento', 'finalizado') IS NULL
    ALTER TABLE [dbo].[tb_tudo_apontamento] ADD [finalizado] [bit] NULL
GO

IF COL_LENGTH('dbo.tb_tudo_apontamento_checklist', 'codigo_tipo_item_checklist') IS NULL
    ALTER TABLE [dbo].[tb_tudo_apontamento_checklist] ADD [codigo_tipo_item_checklist] [smallint] NULL
GO

IF COL_LENGTH('dbo.tb_tudo_apontamento_checklist', 'resposta') IS NULL
    ALTER TABLE [dbo].[tb_tudo_apontamento_checklist] ADD [resposta] [varchar](250) NULL
GO

IF COL_LENGTH('dbo.tb_tudo_apontamento_checklist', 'abre_os') IS NULL
    ALTER TABLE [dbo].[tb_tudo_apontamento_checklist] ADD [abre_os] [bit] NULL
GO

/*--------------------------------------------------------------------------------------------
    2) START: cria (ou reaproveita) o cabeçalho aberto ao entrar na tela de execução.
       Não altera status/próxima data (isso só acontece ao finalizar).
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_start_tudo_apontamento', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_start_tudo_apontamento]
GO

CREATE PROCEDURE [dbo].[sp_start_tudo_apontamento]
@codigo_empresa     smallint,
@codigo_usuario     int,
@codigo_unidade     int,
@codigo_apartamento int,
@codigo             bigint OUTPUT,
@codigo_checklist   bigint OUTPUT
AS
BEGIN

    SET NOCOUNT ON;

    SET @codigo_checklist = (SELECT codigo_checklist FROM tb_cad_apartamento
                             WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade AND codigo = @codigo_apartamento);

    -- reaproveita um apontamento ainda não finalizado deste local
    SET @codigo = (SELECT MAX(codigo) FROM tb_tudo_apontamento
                   WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade
                   AND   codigo_apartamento = @codigo_apartamento AND ISNULL(finalizado, 0) = 0);

    IF @codigo IS NULL
    BEGIN

        SET @codigo = ISNULL((SELECT MAX(codigo) FROM tb_tudo_apontamento
                              WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade), 0) + 1;

        INSERT INTO tb_tudo_apontamento(
            codigo, codigo_empresa, codigo_unidade, codigo_apartamento, codigo_checklist,
            nova_vistoria, origem, finalizado, codigo_usuario_input, data_input)
        VALUES(
            @codigo, @codigo_empresa, @codigo_unidade, @codigo_apartamento, @codigo_checklist,
            0, 'WEB SITE - TUDO', 0, @codigo_usuario, GETDATE());

    END

END
GO

/*--------------------------------------------------------------------------------------------
    3) FINALIZA: grava responsável/datas/horas, marca finalizado e faz o recálculo inicial
       (status = 5 / próxima data pela periodicidade do local). Os itens são regravados
       pela sp_insert_tudo_apontamento_checklist (que limpa os itens antigos do apontamento).
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_finaliza_tudo_apontamento', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_finaliza_tudo_apontamento]
GO

CREATE PROCEDURE [dbo].[sp_finaliza_tudo_apontamento]
@codigo_empresa                 smallint,
@codigo_usuario                 int,
@codigo_unidade                 int,
@codigo_apartamento             int,
@codigo                         bigint,
@codigo_funcionario_responsavel int,
@data_inicio                    date,
@data_termino                   date,
@hora_inicio                    time(0),
@hora_termino                   time(0),
@codigo_checklist               bigint OUTPUT
AS
BEGIN

    SET NOCOUNT ON;

    SET @codigo_checklist = (SELECT codigo_checklist FROM tb_cad_apartamento
                             WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade AND codigo = @codigo_apartamento);

    UPDATE tb_tudo_apontamento SET
        codigo_checklist = @codigo_checklist,
        codigo_funcionario_responsavel = @codigo_funcionario_responsavel,
        data_inicio = @data_inicio,
        data_termino = @data_termino,
        hora_inicio = @hora_inicio,
        hora_termino = @hora_termino,
        nova_vistoria = 0,
        finalizado = 1,
        codigo_usuario_input = @codigo_usuario,
        data_input = GETDATE()
    WHERE codigo = @codigo AND codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade;

    -- limpa itens de um apontamento reaproveitado (regravação completa)
    DELETE FROM tb_tudo_apontamento_checklist
    WHERE codigo_tudo_apontamento = @codigo AND codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade;

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
    4) INSERT de um item respondido (tipo + resposta) e abertura de OS OPCIONAL (@abre_os = 1)
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
@codigo_tipo_item_checklist smallint,
@descricao_checklist        varchar(500),
@opcao                      varchar(5),
@resposta                   varchar(250),
@observacao                 varchar(200),
@abre_os                    bit,
@nova_vistoria              bit
AS
BEGIN

    SET NOCOUNT ON;

    INSERT INTO tb_tudo_apontamento_checklist(
        codigo_tudo_apontamento, codigo_empresa, codigo_unidade, codigo_checklist, codigo_checklist_item,
        codigo_tipo_item_checklist, grupo, checklist, descricao, opcao, resposta, observacao, abre_os, nova_vistoria)
    SELECT
        @codigo_tudo_apontamento, @codigo_empresa, @codigo_unidade, @codigo_checklist, @codigo_checklist_item,
        @codigo_tipo_item_checklist, ISNULL(ci.grupo, 'ADICIONADO'), ci.checklist, ISNULL(ci.descricao, @descricao_checklist),
        @opcao, @resposta, UPPER(@observacao), ISNULL(@abre_os, 0), ISNULL(@nova_vistoria, 0)
    FROM (SELECT 1 AS x) z
        LEFT JOIN tb_chk_checklist_item ci ON
            ci.codigo_checklist = @codigo_checklist AND
            ci.codigo_empresa = @codigo_empresa AND
            ci.codigo = @codigo_checklist_item;

    IF ISNULL(@abre_os, 0) = 1
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

    END

    IF ISNULL(@nova_vistoria, 0) = 1
    BEGIN
        UPDATE tb_tudo_apontamento SET nova_vistoria = 1
        WHERE codigo = @codigo_tudo_apontamento AND codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade;
    END

END
GO

/*--------------------------------------------------------------------------------------------
    5) SELECT itens do checklist (template quando @codigo = -1; respondidos caso contrário)
       Agora retorna o tipo do item, foto permitida e limites (numérico).
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
            ISNULL(ci.codigo_tipo_item_checklist, 8)    AS codigo_tipo_item_checklist,
            ci.grupo,
            ci.checklist,
            ci.descricao,
            CONVERT(bit, ISNULL(ci.allow_picture, 0))   AS allow_picture,
            ISNULL(ci.valor_minimo, 0)                  AS valor_minimo,
            ISNULL(ci.valor_maximo, 0)                  AS valor_maximo,
            ISNULL(ci.unidade_medida, '')               AS unidade_medida,
            ''      AS opcao,
            ''      AS resposta,
            ''      AS observacao,
            0       AS abre_os,
            0       AS nova_vistoria,
            CONVERT(bigint, 0) AS codigo_ordem_servico
        FROM
            tb_cad_apartamento a
            INNER JOIN tb_chk_checklist_item ci ON
                ci.codigo_checklist = a.codigo_checklist AND
                ci.codigo_empresa = a.codigo_empresa
        WHERE a.codigo_empresa = @codigo_empresa
        AND   a.codigo_unidade = @codigo_unidade
        AND   a.codigo = @codigo_apartamento
        ORDER BY ci.grupo, ISNULL(ci.sequencia, ci.codigo), ci.codigo

    END
    ELSE
    BEGIN

        -- Itens já respondidos
        SELECT
            ac.codigo_checklist_item                    AS codigo,
            ISNULL(ac.codigo_tipo_item_checklist, 8)    AS codigo_tipo_item_checklist,
            ISNULL(ac.grupo, '')                        AS grupo,
            ISNULL(ac.checklist, '')                    AS checklist,
            ISNULL(ac.descricao, '')                    AS descricao,
            CONVERT(bit, ISNULL(ci.allow_picture, 0))   AS allow_picture,
            ISNULL(ci.valor_minimo, 0)                  AS valor_minimo,
            ISNULL(ci.valor_maximo, 0)                  AS valor_maximo,
            ISNULL(ci.unidade_medida, '')               AS unidade_medida,
            ISNULL(ac.opcao, '')                        AS opcao,
            ISNULL(ac.resposta, '')                     AS resposta,
            ISNULL(ac.observacao, '')                   AS observacao,
            CONVERT(bit, ISNULL(ac.abre_os, 0))         AS abre_os,
            CONVERT(bit, ISNULL(ac.nova_vistoria, 0))   AS nova_vistoria,
            ISNULL(ac.codigo_ordem_servico, 0)          AS codigo_ordem_servico
        FROM
            tb_tudo_apontamento_checklist ac
            LEFT JOIN tb_chk_checklist_item ci ON
                ci.codigo_checklist = ac.codigo_checklist AND
                ci.codigo_empresa = ac.codigo_empresa AND
                ci.codigo = ac.codigo_checklist_item
        WHERE ac.codigo_empresa = @codigo_empresa
        AND   ac.codigo_unidade = @codigo_unidade
        AND   ac.codigo_tudo_apontamento = @codigo
        ORDER BY ac.grupo, ac.codigo_checklist_item

    END

END
GO

/*--------------------------------------------------------------------------------------------
    6) HISTÓRICO: grid dinâmico (loadGridMain) - 3 result sets (dados, colunas, groupBy)
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_select_tudo_apontamento_grid', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_tudo_apontamento_grid]
GO

CREATE PROCEDURE [dbo].[sp_select_tudo_apontamento_grid]
@codigo_empresa     smallint,
@codigo_unidade     int,
@codigo_apartamento int          = -1,
@data_inicio        date,
@data_termino       date
AS
BEGIN

    SET NOCOUNT ON;

    -- 1) DADOS
    SELECT
        ap.codigo,
        ap.codigo_unidade,
        ap.codigo_apartamento,
        ISNULL(s.descricao, '')                                     AS setor,
        ISNULL(a.local, '')                                         AS local,
        ISNULL(c.descricao, '')                                     AS checklist,
        ISNULL(f.nome, '')                                          AS responsavel,
        ISNULL(CONVERT(varchar(10), ap.data_inicio, 103), '')       AS data_inicio,
        ISNULL(CONVERT(varchar(10), ap.data_termino, 103), '')      AS data_termino,
        ISNULL(CONVERT(varchar(5), ap.hora_inicio, 108), '')        AS hora_inicio,
        ISNULL(CONVERT(varchar(5), ap.hora_termino, 108), '')       AS hora_termino,
        CASE
            WHEN ap.hora_inicio IS NOT NULL AND ap.hora_termino IS NOT NULL
                THEN RIGHT('0' + CONVERT(varchar(2), DATEDIFF(MINUTE, ap.hora_inicio, ap.hora_termino) / 60), 2) + ':' +
                     RIGHT('0' + CONVERT(varchar(2), DATEDIFF(MINUTE, ap.hora_inicio, ap.hora_termino) % 60), 2)
            ELSE ''
        END                                                         AS tempo
    FROM
        tb_tudo_apontamento ap
        INNER JOIN tb_cad_apartamento a ON a.codigo = ap.codigo_apartamento AND a.codigo_empresa = ap.codigo_empresa AND a.codigo_unidade = ap.codigo_unidade
        LEFT  JOIN tb_cad_setor s ON s.codigo = a.codigo_setor AND s.codigo_empresa = a.codigo_empresa
        LEFT  JOIN tb_chk_checklist c ON c.codigo = ap.codigo_checklist AND c.codigo_empresa = ap.codigo_empresa
        LEFT  JOIN tb_cad_funcionario f ON f.codigo = ap.codigo_funcionario_responsavel AND f.codigo_empresa = ap.codigo_empresa
    WHERE ap.codigo_empresa = @codigo_empresa
    AND   ap.codigo_unidade = @codigo_unidade
    AND   ISNULL(ap.finalizado, 0) = 1
    AND   (@codigo_apartamento <= 0 OR ap.codigo_apartamento = @codigo_apartamento)
    AND   ap.data_termino BETWEEN @data_inicio AND @data_termino
    ORDER BY ap.data_termino DESC, ap.codigo DESC

    -- 2) COLUNAS
    SELECT 'setor'          AS Data, 'Setor'       AS Title, CAST(1 AS bit) AS Visible, CAST(1 AS bit) AS Orderable, 'left'   AS Align
    UNION ALL SELECT 'local',       'Local',        CAST(1 AS bit), CAST(1 AS bit), 'left'
    UNION ALL SELECT 'checklist',   'Checklist',    CAST(1 AS bit), CAST(1 AS bit), 'left'
    UNION ALL SELECT 'responsavel', 'Responsável',  CAST(1 AS bit), CAST(1 AS bit), 'left'
    UNION ALL SELECT 'data_inicio', 'Início',       CAST(1 AS bit), CAST(1 AS bit), 'center'
    UNION ALL SELECT 'data_termino','Término',      CAST(1 AS bit), CAST(1 AS bit), 'center'
    UNION ALL SELECT 'tempo',       'Tempo',        CAST(1 AS bit), CAST(1 AS bit), 'center';

    -- 3) GROUP BY (vazio)
    SELECT 'setor' AS ColumnName, 0 AS Level, CAST(1 AS bit) AS Collapsible, CAST(1 AS bit) AS ShowCount, '' AS CssClass
    WHERE ((1) <> 1)

END
GO

/*--------------------------------------------------------------------------------------------
    7) EXCLUSÃO do apontamento + recálculo da próxima execução do local (se necessário)
       - Remove itens; cancela OS abertas por ele (status 99); inativa fotos (tb_pcm_picture);
         remove o cabeçalho. Se o local apontava para este apontamento, recalcula a partir do
         apontamento finalizado anterior (ou volta para Pendente quando não houver).
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_delete_tudo_apontamento', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_delete_tudo_apontamento]
GO

CREATE PROCEDURE [dbo].[sp_delete_tudo_apontamento]
@codigo_empresa     smallint,
@codigo_unidade     int,
@codigo             bigint
AS
BEGIN

    SET NOCOUNT ON;

    DECLARE @codigo_apartamento int;
    DECLARE @era_corrente bit = 0;

    SELECT @codigo_apartamento = codigo_apartamento
    FROM tb_tudo_apontamento
    WHERE codigo = @codigo AND codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade;

    IF @codigo_apartamento IS NULL
        RETURN;

    -- este apontamento é o que rege o status atual do local?
    IF EXISTS(SELECT 1 FROM tb_cad_apartamento
              WHERE codigo = @codigo_apartamento AND codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade
              AND   codigo_tudo_apontamento = @codigo)
        SET @era_corrente = 1;

    -- cancela OS abertas por este apontamento
    UPDATE tb_pcm_ordem_servico SET status = 99
    WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade AND codigo_tudo_apontamento = @codigo;

    -- inativa fotos vinculadas
    UPDATE tb_pcm_picture SET ativo = 0
    WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade
    AND   tipo = 'TUDO' AND codigo_documento = @codigo;

    -- remove itens e cabeçalho
    DELETE FROM tb_tudo_apontamento_checklist
    WHERE codigo_tudo_apontamento = @codigo AND codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade;

    DELETE FROM tb_tudo_apontamento
    WHERE codigo = @codigo AND codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade;

    IF @era_corrente = 1
    BEGIN

        DECLARE @mesAtual date = DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1);
        DECLARE @codigo_anterior bigint;
        DECLARE @dt date;

        SELECT TOP 1 @codigo_anterior = ap.codigo, @dt = ap.data_termino
        FROM tb_tudo_apontamento ap
        WHERE ap.codigo_empresa = @codigo_empresa AND ap.codigo_unidade = @codigo_unidade
        AND   ap.codigo_apartamento = @codigo_apartamento AND ISNULL(ap.finalizado, 0) = 1
        ORDER BY ap.data_termino DESC, ap.codigo DESC;

        IF @codigo_anterior IS NULL
        BEGIN
            -- não há histórico: volta para o estado "a fazer" (sem data => Pendente)
            UPDATE tb_cad_apartamento SET
                status_tudo = 2,
                data_ultimo_tudo = NULL,
                data_proximo_tudo = NULL,
                codigo_tudo_apontamento = NULL
            WHERE codigo = @codigo_apartamento AND codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade;
        END
        ELSE
        BEGIN
            -- aponta para o anterior e recalcula a próxima data pela periodicidade do local
            UPDATE tb_cad_apartamento SET
                codigo_tudo_apontamento = @codigo_anterior,
                data_ultimo_tudo = @dt,
                data_proximo_tudo = CASE a.codigo_periodicidade
                    WHEN 1 THEN DATEADD(DAY, ISNULL(a.intervalo, 1), @dt)
                    WHEN 2 THEN DATEADD(DAY, -DATEPART(WEEKDAY, DATEADD(WEEK, ISNULL(a.intervalo, 1), @dt)) + 1, DATEADD(WEEK, ISNULL(a.intervalo, 1), @dt))
                    WHEN 3 THEN DATEFROMPARTS(YEAR(DATEADD(MONTH, ISNULL(a.intervalo, 1), @dt)), MONTH(DATEADD(MONTH, ISNULL(a.intervalo, 1), @dt)), 1)
                    WHEN 5 THEN DATEADD(MONTH, ((DATEDIFF(MONTH, 0, @dt) / 2) + 1) * 2, 0)
                    WHEN 6 THEN DATEADD(MONTH, ((DATEDIFF(MONTH, 0, @dt) / 3) + 1) * 3, 0)
                    WHEN 7 THEN DATEADD(MONTH, ((DATEDIFF(MONTH, 0, @dt) / 6) + 1) * 6, 0)
                    ELSE DATEFROMPARTS(YEAR(DATEADD(MONTH, ISNULL(a.intervalo, 1), @dt)), MONTH(DATEADD(MONTH, ISNULL(a.intervalo, 1), @dt)), 1)
                END
            FROM tb_cad_apartamento a
            WHERE a.codigo = @codigo_apartamento AND a.codigo_empresa = @codigo_empresa AND a.codigo_unidade = @codigo_unidade;

            -- status derivado pela data (Atrasado / Pendente / Em Dia)
            UPDATE tb_cad_apartamento SET
                status_tudo = CASE
                    WHEN data_proximo_tudo IS NULL THEN 2
                    WHEN DATEFROMPARTS(YEAR(data_proximo_tudo), MONTH(data_proximo_tudo), 1) < @mesAtual THEN 1
                    WHEN DATEFROMPARTS(YEAR(data_proximo_tudo), MONTH(data_proximo_tudo), 1) = @mesAtual THEN 2
                    ELSE 5
                END
            WHERE codigo = @codigo_apartamento AND codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade;
        END

    END

END
GO
