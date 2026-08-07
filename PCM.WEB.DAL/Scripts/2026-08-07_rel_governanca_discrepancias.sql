/**********************************************************************************************
    Script      : Relatório de Discrepâncias (RE18) - Governança
    Data        : 07/08/2026
    Descricao   : Colunas novas na tb_gov_apontamento + SPs do relatório (grade + KPIs).

    IMPORTANTE (LEIA): as SPs de governança vivem no BANCO (não no repositório), então este
    script é um TEMPLATE. O FORMATO DE SAÍDA (nomes das colunas retornadas) já está correto e
    casa com a DAL/tela — NÃO altere os aliases de saída. Ajuste apenas os nomes REAIS de
    tabelas/colunas de origem marcados com  -- >>> AJUSTE  conforme o seu schema.

    Origem confirmada pelo usuário:
      - AD/Cr1/Cr2, Bagagem e Divergência (SIM/NÃO) vêm da tb_gov_apontamento.
      - "Planejado Para" = camareira planejada (planejamento de governança).
      - "Executado Por" = quem executou (codigo_funcionario do apontamento).
      - "Vistoriado Por" = supervisora (codigo_vistoriador do apontamento).

    Objetos:
      - Colunas    : tb_gov_apontamento.divergencia / ad / cr1 / cr2 / bagagem
      - Procedure  : sp_report_governanca_discrepancias
      - Procedure  : sp_report_governanca_discrepancias_kpi
**********************************************************************************************/

/*--------------------------------------------------------------------------------------------
    1) COLUNAS NOVAS (idempotente)
--------------------------------------------------------------------------------------------*/
IF COL_LENGTH('dbo.tb_gov_apontamento', 'divergencia') IS NULL
    ALTER TABLE [dbo].[tb_gov_apontamento] ADD [divergencia] [bit] NULL;      -- 1 = SIM, 0 = NÃO
GO
IF COL_LENGTH('dbo.tb_gov_apontamento', 'ad') IS NULL
    ALTER TABLE [dbo].[tb_gov_apontamento] ADD [ad] [int] NULL;               -- adultos
GO
IF COL_LENGTH('dbo.tb_gov_apontamento', 'cr1') IS NULL
    ALTER TABLE [dbo].[tb_gov_apontamento] ADD [cr1] [int] NULL;              -- criança 1
GO
IF COL_LENGTH('dbo.tb_gov_apontamento', 'cr2') IS NULL
    ALTER TABLE [dbo].[tb_gov_apontamento] ADD [cr2] [int] NULL;              -- criança 2
GO
IF COL_LENGTH('dbo.tb_gov_apontamento', 'bagagem') IS NULL
    ALTER TABLE [dbo].[tb_gov_apontamento] ADD [bagagem] [varchar](1) NULL;   -- M / P / G
GO

/*--------------------------------------------------------------------------------------------
    2) GRADE DO RELATÓRIO
       Formato de saída FIXO (não alterar os aliases): codigo_apartamento, codigo_unidade,
       local, data, planejado_para, executado_por, vistoriado_por, hora_termino, status_gov,
       divergencia, ocupacao, bagagem, observacao, sem_vistoria, sem_execucao
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_report_governanca_discrepancias', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_report_governanca_discrepancias];
GO

CREATE PROCEDURE [dbo].[sp_report_governanca_discrepancias]
@codigo_empresa smallint,
@codigo_unidade int,
@data_inicio    date,
@data_termino   date
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        ap.codigo_apartamento                                           AS codigo_apartamento,
        ap.codigo_unidade                                               AS codigo_unidade,
        ISNULL(a.local, ap.codigo_apartamento)                          AS local,            -- >>> AJUSTE: coluna do local/UH em tb_cad_apartamento
        CONVERT(varchar(10), ap.data, 103)                              AS data,
        ISNULL(fp.nome, '')                                             AS planejado_para,   -- >>> AJUSTE: camareira PLANEJADA (join planejamento)
        ISNULL(fe.nome, '')                                             AS executado_por,    -- >>> AJUSTE: nome do funcionário (executou)
        ISNULL(fv.nome, '')                                             AS vistoriado_por,   -- >>> AJUSTE: nome do vistoriador
        ISNULL(CONVERT(varchar(5), ap.hora_termino, 108), '')          AS hora_termino,
        ISNULL(tg.descricao, '')                                        AS status_gov,       -- >>> AJUSTE: status/tipo governança
        CASE WHEN ISNULL(ap.divergencia, 0) = 1 THEN 'SIM' ELSE 'NÃO' END AS divergencia,
        CONCAT(ISNULL(ap.ad,0), ' / ', ISNULL(ap.cr1,0), ' / ', ISNULL(ap.cr2,0)) AS ocupacao,
        ISNULL(ap.bagagem, '')                                          AS bagagem,
        ISNULL(ap.observacao, '')                                       AS observacao,       -- >>> AJUSTE: coluna de observação (se houver)
        CAST(CASE WHEN ap.codigo_vistoriador IS NULL THEN 1 ELSE 0 END AS bit) AS sem_vistoria,
        CAST(CASE WHEN ap.codigo_funcionario IS NULL THEN 1 ELSE 0 END AS bit) AS sem_execucao
    FROM
        tb_gov_apontamento ap
        LEFT JOIN tb_cad_apartamento a ON a.codigo = ap.codigo_apartamento AND a.codigo_empresa = ap.codigo_empresa AND a.codigo_unidade = ap.codigo_unidade
        LEFT JOIN tb_gov_funcionario fe ON fe.codigo = ap.codigo_funcionario  AND fe.codigo_empresa = ap.codigo_empresa   -- >>> AJUSTE: tabela/colunas de funcionário de governança
        LEFT JOIN tb_gov_funcionario fv ON fv.codigo = ap.codigo_vistoriador AND fv.codigo_empresa = ap.codigo_empresa
        LEFT JOIN tb_gov_funcionario fp ON fp.codigo = ap.codigo_camareira_planejada AND fp.codigo_empresa = ap.codigo_empresa  -- >>> AJUSTE: camareira planejada (planejamento)
        LEFT JOIN tb_stc_tipo_governanca tg ON tg.codigo = ap.codigo_tipo_governanca   -- >>> AJUSTE: tabela do status/tipo governança
    WHERE ap.codigo_empresa = @codigo_empresa
    AND   ap.codigo_unidade = @codigo_unidade
    AND   ap.data BETWEEN @data_inicio AND @data_termino
    -- Regra confirmada: aparece o que foi marcado como divergente na tela de apontamento.
    -- (Se quiser incluir também planejado-sem-execução/sem-vistoria, ampliar o filtro abaixo.)
    AND   ISNULL(ap.divergencia, 0) = 1
    ORDER BY ap.data, local;

END
GO

/*--------------------------------------------------------------------------------------------
    3) KPIs
       Formato de saída FIXO: total_planejado, total_arrumado, total_permanencia, total_saida,
       divergencias, planejado_sem_execucao, executado_sem_vistoria, quartos_nqa, quartos_nao_perturbe
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_report_governanca_discrepancias_kpi', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_report_governanca_discrepancias_kpi];
GO

CREATE PROCEDURE [dbo].[sp_report_governanca_discrepancias_kpi]
@codigo_empresa smallint,
@codigo_unidade int,
@data_inicio    date,
@data_termino   date
AS
BEGIN

    SET NOCOUNT ON;

    -- >>> AJUSTE os predicados conforme o significado real de cada status/tipo no seu schema.
    --     Mantidos com nomes de coluna assumidos; troque pelos reais se diferentes.
    SELECT
        ISNULL(SUM(1), 0)                                                                          AS total_planejado,
        ISNULL(SUM(CASE WHEN ap.codigo_funcionario IS NOT NULL THEN 1 ELSE 0 END), 0)              AS total_arrumado,
        ISNULL(SUM(CASE WHEN tg.descricao = 'PERMANÊNCIA' THEN 1 ELSE 0 END), 0)                   AS total_permanencia,
        ISNULL(SUM(CASE WHEN tg.descricao = 'SAÍDA'       THEN 1 ELSE 0 END), 0)                   AS total_saida,
        ISNULL(SUM(CASE WHEN ISNULL(ap.divergencia,0) = 1 THEN 1 ELSE 0 END), 0)                   AS divergencias,
        ISNULL(SUM(CASE WHEN ap.codigo_funcionario IS NULL THEN 1 ELSE 0 END), 0)                  AS planejado_sem_execucao,
        ISNULL(SUM(CASE WHEN ap.codigo_funcionario IS NOT NULL AND ap.codigo_vistoriador IS NULL THEN 1 ELSE 0 END), 0) AS executado_sem_vistoria,
        ISNULL(SUM(CASE WHEN tg.descricao = 'N.Q.A' THEN 1 ELSE 0 END), 0)                         AS quartos_nqa,
        ISNULL(SUM(CASE WHEN ISNULL(ap.nao_perturbe,0) = 1 THEN 1 ELSE 0 END), 0)                  AS quartos_nao_perturbe
    FROM
        tb_gov_apontamento ap
        LEFT JOIN tb_stc_tipo_governanca tg ON tg.codigo = ap.codigo_tipo_governanca
    WHERE ap.codigo_empresa = @codigo_empresa
    AND   ap.codigo_unidade = @codigo_unidade
    AND   ap.data BETWEEN @data_inicio AND @data_termino;

END
GO
