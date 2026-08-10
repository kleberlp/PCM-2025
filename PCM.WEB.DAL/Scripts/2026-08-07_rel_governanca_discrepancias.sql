/**********************************************************************************************
    Script      : Relatório de Discrepâncias (RE18) - Governança
    Data        : 07/08/2026  (atualizado 10/08/2026)
    Descricao   : SPs do relatório (grade + KPIs) lendo das colunas REAIS apontadas na tela
                  (bloco Discrepância do ChecklistGovernancaApontamento), gravadas em
                  tb_gov_apontamento pelo script 2026-08-10:
                    status_uh_discrepancia / status_governanca_discrepancia / discrepancia /
                    adultos / criancas1 / criancas2 / bagagem / observacao

    IMPORTANTE: as SPs de governança vivem no BANCO. O FORMATO DE SAÍDA (aliases) já casa com
    a DAL/tela — NÃO altere os aliases. Ajuste apenas os nomes REAIS de tabelas/colunas de
    origem marcados com  -- >>> AJUSTE  (funcionário/planejamento/tipo governança).

    Divergência = apontamento com discrepancia = 'N/OK'.

    Objetos:
      - Procedure  : sp_report_governanca_discrepancias
      - Procedure  : sp_report_governanca_discrepancias_kpi
    (As colunas de discrepância são criadas pelo script 2026-08-10.)
**********************************************************************************************/

/*--------------------------------------------------------------------------------------------
    1) GRADE DO RELATÓRIO  (aliases de saída fixos)
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
        ISNULL(CONVERT(varchar(50), a.apartamento), CONVERT(varchar(50), ap.codigo_apartamento)) AS local,  -- Nº do apartamento  (>>> AJUSTE se a coluna não for 'apartamento')
        CONVERT(varchar(10), ap.data, 103)                              AS data,
        ISNULL(fp.nome, '')                                             AS planejado_para,   -- >>> AJUSTE: camareira PLANEJADA (planejamento)
        ISNULL(fe.nome, '')                                             AS executado_por,    -- >>> AJUSTE: nome do funcionário (executou)
        ISNULL(fv.nome, '')                                             AS vistoriado_por,   -- >>> AJUSTE: nome do vistoriador
        ISNULL(CONVERT(varchar(5), ap.hora_termino, 108), '')          AS hora_termino,
        ISNULL(suh.descricao, '')                                       AS status_uh,        -- status UH da discrepância
        ISNULL(sgd.descricao, '')                                       AS status_gov,       -- status governança da discrepância
        ISNULL(ap.discrepancia, '')                                     AS divergencia,      -- valor bruto enviado no app (OK / N/OK)
        CONCAT(ISNULL(ap.adultos,0), ' / ', ISNULL(ap.criancas1,0), ' / ', ISNULL(ap.criancas2,0)) AS ocupacao,
        ISNULL(ap.bagagem, '')                                          AS bagagem,
        ISNULL(ap.observacao, '')                                       AS observacao,
        CAST(CASE WHEN ap.codigo_vistoriador IS NULL THEN 1 ELSE 0 END AS bit) AS sem_vistoria,
        CAST(CASE WHEN ap.codigo_funcionario  IS NULL THEN 1 ELSE 0 END AS bit) AS sem_execucao
    FROM
        tb_gov_apontamento ap
        LEFT JOIN tb_cad_apartamento a ON a.codigo = ap.codigo_apartamento AND a.codigo_empresa = ap.codigo_empresa AND a.codigo_unidade = ap.codigo_unidade
        LEFT JOIN tb_gov_funcionario fe ON fe.codigo = ap.codigo_funcionario  AND fe.codigo_empresa = ap.codigo_empresa   -- >>> AJUSTE: tabela/colunas de funcionário de governança
        LEFT JOIN tb_gov_funcionario fv ON fv.codigo = ap.codigo_vistoriador AND fv.codigo_empresa = ap.codigo_empresa
        LEFT JOIN tb_gov_funcionario fp ON fp.codigo = ap.codigo_camareira_planejada AND fp.codigo_empresa = ap.codigo_empresa  -- >>> AJUSTE: camareira planejada
        LEFT JOIN tb_stc_status_uh_divergencia         suh ON suh.codigo = ap.status_uh_discrepancia
        LEFT JOIN tb_stc_status_governanca_divergencia sgd ON sgd.codigo = ap.status_governanca_discrepancia
    WHERE ap.codigo_empresa = @codigo_empresa
    AND   ap.codigo_unidade = @codigo_unidade
    AND   ap.data BETWEEN @data_inicio AND @data_termino
    ORDER BY ap.data, local;

END
GO

/*--------------------------------------------------------------------------------------------
    2) KPIs  (aliases de saída fixos)
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

    -- >>> AJUSTE os predicados de tipo/permanência/saída/N.Q.A conforme o seu schema.
    SELECT
        ISNULL(COUNT(*), 0)                                                                        AS total_planejado,
        ISNULL(SUM(CASE WHEN ap.codigo_funcionario IS NOT NULL THEN 1 ELSE 0 END), 0)              AS total_arrumado,
        ISNULL(SUM(CASE WHEN tg.descricao = 'PERMANÊNCIA' THEN 1 ELSE 0 END), 0)                   AS total_permanencia,   -- >>> AJUSTE
        ISNULL(SUM(CASE WHEN tg.descricao = 'SAÍDA'       THEN 1 ELSE 0 END), 0)                   AS total_saida,         -- >>> AJUSTE
        ISNULL(SUM(CASE WHEN ap.discrepancia = 'N/OK' THEN 1 ELSE 0 END), 0)                       AS divergencias,
        ISNULL(SUM(CASE WHEN ap.codigo_funcionario IS NULL THEN 1 ELSE 0 END), 0)                  AS planejado_sem_execucao,
        ISNULL(SUM(CASE WHEN ap.codigo_funcionario IS NOT NULL AND ap.codigo_vistoriador IS NULL THEN 1 ELSE 0 END), 0) AS executado_sem_vistoria,
        ISNULL(SUM(CASE WHEN sgd.descricao = 'N.Q.A' THEN 1 ELSE 0 END), 0)                        AS quartos_nqa,         -- status governança = N.Q.A
        ISNULL(SUM(CASE WHEN ISNULL(ap.nao_perturbe,0) = 1 THEN 1 ELSE 0 END), 0)                  AS quartos_nao_perturbe
    FROM
        tb_gov_apontamento ap
        LEFT JOIN tb_stc_tipo_governanca tg ON tg.codigo = ap.codigo_tipo_governanca                  -- >>> AJUSTE
        LEFT JOIN tb_stc_status_governanca_divergencia sgd ON sgd.codigo = ap.status_governanca_discrepancia
    WHERE ap.codigo_empresa = @codigo_empresa
    AND   ap.codigo_unidade = @codigo_unidade
    AND   ap.data BETWEEN @data_inicio AND @data_termino;

END
GO
