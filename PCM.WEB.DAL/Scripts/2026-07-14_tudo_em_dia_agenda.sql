/**********************************************************************************************
    Script      : Tudo em Dia (Fase 5) - Agenda de vencimentos + auto-planejamento
    Data        : 14/07/2026
    Descricao   : Painel/agenda das execuções a fazer, com geração automática do planejamento
                  distribuindo os locais por RESPONSÁVEL e por DATA:
                    - Responsável = responsável padrão do setor (novo campo) quando houver;
                      caso contrário, round-robin entre os funcionários ativos da unidade.
                    - Data = data de vencimento (data_proximo_tudo), com atrasados no início
                      do período; capacidade máxima por responsável/dia (padrão 10) com
                      antecipação dos excedentes para dias anteriores sem furar o vencimento.
                      (@capacidade = 0 => sem limite.)

    Objetos:
        - Coluna    : tb_cad_setor.codigo_funcionario_responsavel
        - Tabela    : tb_tudo_agenda
        - Procedure : sp_gerar_agenda_tudo
        - Procedure : sp_select_tudo_agenda_grid
        - Procedure : sp_select_tudo_agenda_kpi
        - Procedure : sp_concluir_agenda_tudo
        - Procedure : sp_delete_tudo_agenda
**********************************************************************************************/

/*--------------------------------------------------------------------------------------------
    1) COLUNA + TABELA
--------------------------------------------------------------------------------------------*/
IF COL_LENGTH('dbo.tb_cad_setor', 'codigo_funcionario_responsavel') IS NULL
    ALTER TABLE [dbo].[tb_cad_setor] ADD [codigo_funcionario_responsavel] [int] NULL
GO

IF OBJECT_ID('dbo.tb_tudo_agenda', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[tb_tudo_agenda](
        [codigo]                    [bigint]    NOT NULL,
        [codigo_empresa]            [smallint]  NOT NULL,
        [codigo_unidade]            [int]       NOT NULL,
        [codigo_apartamento]        [int]       NOT NULL,
        [data_planejada]            [date]      NOT NULL,
        [codigo_funcionario]        [int]       NULL,
        [status]                    [smallint]  NOT NULL,   -- 0 planejado, 1 concluído, 2 cancelado
        [codigo_tudo_apontamento]   [bigint]    NULL,
        [codigo_usuario_input]      [int]       NULL,
        [data_input]                [datetime]  NULL,
     CONSTRAINT [PK_tb_tudo_agenda] PRIMARY KEY CLUSTERED
        ([codigo] ASC, [codigo_empresa] ASC, [codigo_unidade] ASC)
    ) ON [PRIMARY]
END
GO

/*--------------------------------------------------------------------------------------------
    2) GERAÇÃO AUTOMÁTICA DO PLANEJAMENTO
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_gerar_agenda_tudo', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_gerar_agenda_tudo]
GO

CREATE PROCEDURE [dbo].[sp_gerar_agenda_tudo]
@codigo_empresa smallint,
@codigo_unidade int,
@data_inicio    date,
@data_fim       date,
@capacidade     int = 10,
@codigo_usuario int
AS
BEGIN

    SET NOCOUNT ON;

    DECLARE @cap  int  = CASE WHEN @capacidade <= 0 THEN 1000000 ELSE @capacidade END;
    DECLARE @hoje date = CONVERT(date, GETDATE());
    DECLARE @base date = CASE WHEN @data_inicio > @hoje THEN @data_inicio ELSE @hoje END;

    -- regenera: remove o planejamento futuro ainda não concluído dentro do período
    DELETE FROM tb_tudo_agenda
    WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade
    AND   status = 0 AND data_planejada BETWEEN @data_inicio AND @data_fim;

    -- pool de responsáveis (round-robin) = funcionários ativos da unidade
    SELECT ROW_NUMBER() OVER (ORDER BY codigo) - 1 AS rn, codigo
    INTO #pool
    FROM tb_cad_funcionario
    WHERE codigo_empresa = @codigo_empresa
    AND   (codigo_unidade = @codigo_unidade OR codigo_unidade IS NULL)
    AND   ativo = 1;

    DECLARE @npool int = (SELECT COUNT(*) FROM #pool);

    -- candidatos: locais a vencer no período, ativos, com checklist, atrasados/pendentes e sem agenda ativa
    SELECT
        a.codigo AS codigo_apartamento,
        CASE WHEN a.data_proximo_tudo < @base THEN @base
             WHEN a.data_proximo_tudo > @data_fim THEN @data_fim
             ELSE a.data_proximo_tudo END          AS data_alvo,
        s.codigo_funcionario_responsavel           AS resp_setor
    INTO #cand
    FROM tb_cad_apartamento a
        LEFT JOIN tb_cad_setor s ON s.codigo = a.codigo_setor AND s.codigo_empresa = a.codigo_empresa AND s.codigo_unidade = a.codigo_unidade
    WHERE a.codigo_empresa = @codigo_empresa
    AND   a.codigo_unidade = @codigo_unidade
    AND   ISNULL(a.ativo, 1) = 1
    AND   a.local IS NOT NULL
    AND   a.codigo_checklist IS NOT NULL
    AND   a.data_proximo_tudo IS NOT NULL
    AND   a.data_proximo_tudo <= @data_fim
    AND   ISNULL(a.status_tudo, 2) IN (1, 2)
    AND   NOT EXISTS (SELECT 1 FROM tb_tudo_agenda ag
                      WHERE ag.codigo_empresa = a.codigo_empresa AND ag.codigo_unidade = a.codigo_unidade
                      AND   ag.codigo_apartamento = a.codigo AND ag.status = 0);

    -- resolve o responsável (setor -> round-robin)
    SELECT
        c.codigo_apartamento,
        c.data_alvo,
        CASE
            WHEN c.resp_setor IS NOT NULL THEN c.resp_setor
            WHEN @npool > 0 THEN (SELECT codigo FROM #pool WHERE rn = (c.rr % @npool))
            ELSE NULL
        END AS resp
    INTO #resp
    FROM (
        SELECT
            codigo_apartamento, data_alvo, resp_setor,
            CASE WHEN resp_setor IS NULL
                 THEN ROW_NUMBER() OVER (PARTITION BY CASE WHEN resp_setor IS NULL THEN 1 ELSE 0 END ORDER BY data_alvo, codigo_apartamento) - 1
                 ELSE 0 END AS rr
        FROM #cand
    ) c;

    -- distribui por capacidade/dia por responsável, sem furar o vencimento
    SELECT
        codigo_apartamento,
        resp,
        CASE
            WHEN DATEADD(DAY, (ROW_NUMBER() OVER (PARTITION BY resp ORDER BY data_alvo, codigo_apartamento) - 1) / @cap, @base) > data_alvo
                THEN data_alvo
            ELSE DATEADD(DAY, (ROW_NUMBER() OVER (PARTITION BY resp ORDER BY data_alvo, codigo_apartamento) - 1) / @cap, @base)
        END AS planned
    INTO #plan
    FROM #resp;

    DECLARE @base_codigo bigint = ISNULL((SELECT MAX(codigo) FROM tb_tudo_agenda
                                          WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade), 0);

    INSERT INTO tb_tudo_agenda(
        codigo, codigo_empresa, codigo_unidade, codigo_apartamento, data_planejada,
        codigo_funcionario, status, codigo_tudo_apontamento, codigo_usuario_input, data_input)
    SELECT
        @base_codigo + ROW_NUMBER() OVER (ORDER BY planned, codigo_apartamento),
        @codigo_empresa, @codigo_unidade, codigo_apartamento, planned,
        NULLIF(ISNULL(resp, 0), 0), 0, NULL, @codigo_usuario, GETDATE()
    FROM #plan;

    SELECT @@ROWCOUNT AS total;

    DROP TABLE #pool; DROP TABLE #cand; DROP TABLE #resp; DROP TABLE #plan;

END
GO

/*--------------------------------------------------------------------------------------------
    3) GRID DA AGENDA (loadGridMain) - agrupado por dia
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_select_tudo_agenda_grid', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_tudo_agenda_grid]
GO

CREATE PROCEDURE [dbo].[sp_select_tudo_agenda_grid]
@codigo_empresa     smallint,
@codigo_unidade     int,
@data_inicio        date,
@data_fim           date,
@codigo_funcionario int = -1,
@status             smallint = -1
AS
BEGIN

    SET NOCOUNT ON;

    DECLARE @hoje date = CONVERT(date, GETDATE());

    -- 1) DADOS
    SELECT
        ag.codigo,
        ag.codigo_unidade,
        ag.codigo_apartamento,
        FORMAT(ag.data_planejada, 'dd/MM/yyyy')                     AS dia,
        ISNULL(s.descricao, '')                                     AS setor,
        ISNULL(a.local, '')                                         AS local,
        ISNULL(c.descricao, '')                                     AS checklist,
        ISNULL(f.nome, 'Sem responsável')                           AS responsavel,
        ISNULL(p.descricao, '')                                     AS periodicidade,
        CASE ag.status WHEN 0 THEN 'Planejado' WHEN 1 THEN 'Concluído' WHEN 2 THEN 'Cancelado' ELSE '' END AS situacao,
        CONVERT(bit, CASE WHEN ag.status = 0 AND ag.data_planejada < @hoje THEN 1 ELSE 0 END) AS atrasado
    FROM
        tb_tudo_agenda ag
        INNER JOIN tb_cad_apartamento a ON a.codigo = ag.codigo_apartamento AND a.codigo_empresa = ag.codigo_empresa AND a.codigo_unidade = ag.codigo_unidade
        LEFT  JOIN tb_cad_setor s ON s.codigo = a.codigo_setor AND s.codigo_empresa = a.codigo_empresa
        LEFT  JOIN tb_chk_checklist c ON c.codigo = a.codigo_checklist AND c.codigo_empresa = a.codigo_empresa
        LEFT  JOIN tb_stc_periodicidade p ON p.codigo = a.codigo_periodicidade
        LEFT  JOIN tb_cad_funcionario f ON f.codigo = ag.codigo_funcionario AND f.codigo_empresa = ag.codigo_empresa
    WHERE ag.codigo_empresa = @codigo_empresa
    AND   ag.codigo_unidade = @codigo_unidade
    AND   ag.data_planejada BETWEEN @data_inicio AND @data_fim
    AND   (@codigo_funcionario <= 0 OR ag.codigo_funcionario = @codigo_funcionario)
    AND   (@status < 0 OR ag.status = @status)
    ORDER BY ag.data_planejada, f.nome, s.descricao, a.local

    -- 2) COLUNAS ('dia' fica oculta pois é usada como cabeçalho do agrupamento)
    SELECT 'dia'            AS Data, 'Dia'          AS Title, CAST(0 AS bit) AS Visible, CAST(0 AS bit) AS Orderable, 'center' AS Align
    UNION ALL SELECT 'responsavel',  'Responsável', CAST(1 AS bit), CAST(0 AS bit), 'left'
    UNION ALL SELECT 'setor',        'Setor',       CAST(1 AS bit), CAST(0 AS bit), 'left'
    UNION ALL SELECT 'local',        'Local',       CAST(1 AS bit), CAST(0 AS bit), 'left'
    UNION ALL SELECT 'checklist',    'Checklist',   CAST(1 AS bit), CAST(0 AS bit), 'left'
    UNION ALL SELECT 'periodicidade','Periodicidade',CAST(1 AS bit), CAST(0 AS bit), 'left'
    UNION ALL SELECT 'situacao',     'Situação',    CAST(1 AS bit), CAST(0 AS bit), 'center';

    -- 3) GROUP BY (por dia)
    SELECT 'dia' AS ColumnName, 0 AS Level, CAST(1 AS bit) AS Collapsible, CAST(1 AS bit) AS ShowCount, '' AS CssClass

END
GO

/*--------------------------------------------------------------------------------------------
    4) KPIs da agenda
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_select_tudo_agenda_kpi', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_tudo_agenda_kpi]
GO

CREATE PROCEDURE [dbo].[sp_select_tudo_agenda_kpi]
@codigo_empresa     smallint,
@codigo_unidade     int,
@data_inicio        date,
@data_fim           date,
@codigo_funcionario int = -1
AS
BEGIN

    SET NOCOUNT ON;

    DECLARE @hoje date = CONVERT(date, GETDATE());

    SELECT
        ISNULL(SUM(CASE WHEN ag.status = 0 THEN 1 ELSE 0 END), 0)                               AS planejados,
        ISNULL(SUM(CASE WHEN ag.status = 0 AND ag.data_planejada < @hoje THEN 1 ELSE 0 END), 0) AS atrasados,
        ISNULL(SUM(CASE WHEN ag.status = 0 AND ag.data_planejada = @hoje THEN 1 ELSE 0 END), 0) AS hoje,
        ISNULL(SUM(CASE WHEN ag.status = 1 THEN 1 ELSE 0 END), 0)                               AS concluidos
    FROM tb_tudo_agenda ag
    WHERE ag.codigo_empresa = @codigo_empresa
    AND   ag.codigo_unidade = @codigo_unidade
    AND   ag.data_planejada BETWEEN @data_inicio AND @data_fim
    AND   (@codigo_funcionario <= 0 OR ag.codigo_funcionario = @codigo_funcionario)

END
GO

/*--------------------------------------------------------------------------------------------
    5) CONCLUIR item da agenda (chamado ao finalizar o apontamento do local)
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_concluir_agenda_tudo', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_concluir_agenda_tudo]
GO

CREATE PROCEDURE [dbo].[sp_concluir_agenda_tudo]
@codigo_empresa             smallint,
@codigo_unidade             int,
@codigo_apartamento         int,
@codigo_tudo_apontamento    bigint
AS
BEGIN

    SET NOCOUNT ON;

    -- marca como concluído o item planejado mais antigo em aberto para este local
    ;WITH x AS (
        SELECT TOP 1 *
        FROM tb_tudo_agenda
        WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade
        AND   codigo_apartamento = @codigo_apartamento AND status = 0
        ORDER BY data_planejada, codigo
    )
    UPDATE x SET status = 1, codigo_tudo_apontamento = @codigo_tudo_apontamento;

END
GO

/*--------------------------------------------------------------------------------------------
    6) LIMPAR planejamento em aberto do período
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_delete_tudo_agenda', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_delete_tudo_agenda]
GO

CREATE PROCEDURE [dbo].[sp_delete_tudo_agenda]
@codigo_empresa smallint,
@codigo_unidade int,
@data_inicio    date,
@data_fim       date
AS
BEGIN

    SET NOCOUNT ON;

    DELETE FROM tb_tudo_agenda
    WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade
    AND   status = 0 AND data_planejada BETWEEN @data_inicio AND @data_fim

END
GO
