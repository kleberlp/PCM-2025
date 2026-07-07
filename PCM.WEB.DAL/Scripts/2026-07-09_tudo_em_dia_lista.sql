/**********************************************************************************************
    Script      : Tudo em Dia (Fase 1) - Estado por local + SPs de lista/status
    Data        : 09/07/2026
    Descricao   : Espelha o "UH em Dia", mas por LOCAL (tb_cad_apartamento com local <> NULL) e
                  puxando checklist/periodicidade/intervalo do PRÓPRIO registro do local
                  (colunas codigo_checklist / codigo_periodicidade / intervalo já existentes).

    Fase 1 (esta): colunas de estado + SPs de leitura (lista e contadores de status).
    (apontamento/execução e recálculo de próxima data ficam na Fase 2)

    Status (reusa tb_stc_status_uh_dia): 1 Atrasado · 2 Pendente · 3 Em Manutenção ·
                                         4 Nova Vistoria · 5 Em Dia
**********************************************************************************************/

/*--------------------------------------------------------------------------------------------
    1) COLUNAS DE ESTADO em tb_cad_apartamento (dedicadas ao Tudo em Dia)
--------------------------------------------------------------------------------------------*/
IF COL_LENGTH('dbo.tb_cad_apartamento', 'status_tudo') IS NULL
    ALTER TABLE [dbo].[tb_cad_apartamento] ADD [status_tudo] [smallint] NULL
GO
IF COL_LENGTH('dbo.tb_cad_apartamento', 'data_ultimo_tudo') IS NULL
    ALTER TABLE [dbo].[tb_cad_apartamento] ADD [data_ultimo_tudo] [date] NULL
GO
IF COL_LENGTH('dbo.tb_cad_apartamento', 'data_proximo_tudo') IS NULL
    ALTER TABLE [dbo].[tb_cad_apartamento] ADD [data_proximo_tudo] [date] NULL
GO
IF COL_LENGTH('dbo.tb_cad_apartamento', 'codigo_tudo_apontamento') IS NULL
    ALTER TABLE [dbo].[tb_cad_apartamento] ADD [codigo_tudo_apontamento] [bigint] NULL
GO

/*--------------------------------------------------------------------------------------------
    2) LISTA de locais a executar (agrupável por setor)
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_select_tudo_checklist', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_tudo_checklist]
GO

CREATE PROCEDURE [dbo].[sp_select_tudo_checklist]
@codigo_empresa smallint,
@codigo_unidade int,
@codigo_setor   int = -1,
@status         varchar(50) = ''
AS
BEGIN

    SET NOCOUNT ON;

    DECLARE @mesAtual date = DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1);

    ;WITH base AS (
        SELECT
            a.codigo,
            a.codigo_unidade,
            a.codigo_setor,
            a.local,
            a.codigo_checklist,
            a.codigo_periodicidade,
            a.intervalo,
            a.data_proximo_tudo,
            a.codigo_tudo_apontamento,
            status_calc = CASE
                WHEN a.status_tudo = 3 THEN 3
                WHEN a.status_tudo = 4 THEN 4
                WHEN a.data_proximo_tudo IS NULL THEN 2
                WHEN DATEFROMPARTS(YEAR(a.data_proximo_tudo), MONTH(a.data_proximo_tudo), 1) < @mesAtual THEN 1
                WHEN DATEFROMPARTS(YEAR(a.data_proximo_tudo), MONTH(a.data_proximo_tudo), 1) = @mesAtual THEN 2
                ELSE 5
            END
        FROM tb_cad_apartamento a
        WHERE a.codigo_empresa = @codigo_empresa
        AND   ((a.codigo_unidade = @codigo_unidade) OR (@codigo_unidade <= 0))
        AND   ((a.codigo_setor = @codigo_setor) OR (@codigo_setor <= 0))
        AND   a.ativo = 1
        AND   a.local IS NOT NULL
        AND   a.codigo_checklist IS NOT NULL
    )
    SELECT
        base.codigo_unidade,
        ISNULL(u.nome_fantasia, '')                     AS unidade,
        base.codigo_setor,
        ISNULL(s.descricao, '')                         AS setor,
        base.codigo,
        base.local,
        base.codigo_checklist,
        ISNULL(c.descricao, '')                         AS checklist,
        base.codigo_periodicidade,
        ISNULL(p.descricao, '')                         AS periodicidade,
        ISNULL(base.intervalo, 0)                       AS intervalo,
        ISNULL(CONVERT(varchar(10), base.data_proximo_tudo, 103), '') AS data_proxima,
        base.status_calc                                AS status,
        st.css_class,
        st.color,
        st.bg_color,
        ISNULL(base.codigo_tudo_apontamento, 0)         AS codigo_apontamento
    FROM
        base
        INNER JOIN tb_cad_unidade u ON u.codigo_empresa = @codigo_empresa AND u.codigo = base.codigo_unidade
        LEFT  JOIN tb_cad_setor   s ON s.codigo_empresa = @codigo_empresa AND s.codigo = base.codigo_setor
        LEFT  JOIN tb_chk_checklist c ON c.codigo_empresa = @codigo_empresa AND c.codigo = base.codigo_checklist
        LEFT  JOIN tb_stc_periodicidade p ON p.codigo = base.codigo_periodicidade
        LEFT  JOIN tb_stc_status_uh_dia st ON st.status = base.status_calc
    WHERE (@status = '' OR CHARINDEX(',' + CAST(base.status_calc AS varchar(3)) + ',', ',' + @status + ',') > 0)
    ORDER BY setor, base.local

END
GO

/*--------------------------------------------------------------------------------------------
    3) CONTADORES por status (torre de KPIs)
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_select_tudo_checklist_status', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_tudo_checklist_status]
GO

CREATE PROCEDURE [dbo].[sp_select_tudo_checklist_status]
@codigo_empresa smallint,
@codigo_unidade int,
@codigo_setor   int = -1
AS
BEGIN

    SET NOCOUNT ON;

    DECLARE @mesAtual date = DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1);

    ;WITH base AS (
        SELECT
            status_calc = CASE
                WHEN a.status_tudo = 3 THEN 3
                WHEN a.status_tudo = 4 THEN 4
                WHEN a.data_proximo_tudo IS NULL THEN 2
                WHEN DATEFROMPARTS(YEAR(a.data_proximo_tudo), MONTH(a.data_proximo_tudo), 1) < @mesAtual THEN 1
                WHEN DATEFROMPARTS(YEAR(a.data_proximo_tudo), MONTH(a.data_proximo_tudo), 1) = @mesAtual THEN 2
                ELSE 5
            END
        FROM tb_cad_apartamento a
        WHERE a.codigo_empresa = @codigo_empresa
        AND   ((a.codigo_unidade = @codigo_unidade) OR (@codigo_unidade <= 0))
        AND   ((a.codigo_setor = @codigo_setor) OR (@codigo_setor <= 0))
        AND   a.ativo = 1
        AND   a.local IS NOT NULL
        AND   a.codigo_checklist IS NOT NULL
    )
    SELECT
        SUM(CASE WHEN status_calc = 1 THEN 1 ELSE 0 END) AS quantidade_atrasado,
        SUM(CASE WHEN status_calc = 2 THEN 1 ELSE 0 END) AS quantidade_pendente,
        SUM(CASE WHEN status_calc = 3 THEN 1 ELSE 0 END) AS quantidade_manutencao,
        SUM(CASE WHEN status_calc = 4 THEN 1 ELSE 0 END) AS quantidade_nova_vistoria,
        SUM(CASE WHEN status_calc = 5 THEN 1 ELSE 0 END) AS quantidade_realizada
    FROM base

END
GO
