/**********************************************************************************************
    Script      : Tela Discrepancias (GH15) - Governança
    Data        : 11/08/2026
    Descricao   : SP da grade da nova tela Governanca/Discrepancias (cópia da tela Apontamento
                  + campos de discrepância de tb_gov_apontamento).

    IMPORTANTE: as SPs de governança vivem no BANCO. Esta SP é um TEMPLATE baseado na
    sp_select_governanca_apontamento (mesmos parâmetros/filtros) — o ideal é COPIAR o corpo
    real da sp_select_governanca_apontamento do banco e ACRESCENTAR as colunas marcadas
    abaixo. O FORMATO DE SAÍDA (aliases) já casa com a DAL — NÃO altere os aliases.
    Ajuste os nomes REAIS de tabelas/colunas marcados com  -- >>> AJUSTE .

    Colunas de discrepância (criadas pelo script 2026-08-10 em tb_gov_apontamento):
      status_uh_discrepancia / status_governanca_discrepancia / discrepancia /
      adultos / criancas1 / criancas2 / bagagem / observacao

    Objetos:
      - Procedure : sp_select_governanca_apontamento_discrepancia_lista
**********************************************************************************************/

IF OBJECT_ID('dbo.sp_select_governanca_apontamento_discrepancia_lista', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_governanca_apontamento_discrepancia_lista];
GO

CREATE PROCEDURE [dbo].[sp_select_governanca_apontamento_discrepancia_lista]
@codigo_empresa         smallint,
@codigo_unidade         int,
@data                   date,
@codigo_tipo_governanca smallint,
@bloco                  varchar(50),
@andar                  varchar(50),
@front_office_status    varchar(50),
@room_status            varchar(50),
@uhInicio               int,
@uhTermino              int
AS
BEGIN

    SET NOCOUNT ON;

    /* >>> AJUSTE: base = corpo da sp_select_governanca_apontamento (apartamentos da unidade +
       planejamento do dia). Abaixo, um esqueleto com os joins usados nas demais SPs. */
    SELECT
        a.codigo                                                        AS codigo_apartamento,
        ISNULL(ap.codigo, 0)                                            AS codigo_apontamento,   -- 0 = sem apontamento (controles desabilitados na tela)
        ISNULL(CONVERT(varchar(50), a.apartamento), '')                 AS apartamento,          -- >>> AJUSTE se a coluna não for 'apartamento'
        ISNULL(a.bloco, '')                                             AS bloco,                -- >>> AJUSTE
        ISNULL(a.andar, '')                                             AS andar,                -- >>> AJUSTE
        ISNULL(a.front_office_status, '')                               AS front_office_status,  -- >>> AJUSTE (status PMS/Front Office)
        ISNULL(a.room_status, '')                                       AS room_status,          -- >>> AJUSTE
        ISNULL(tg.descricao, '')                                        AS tipo_governanca,      -- Limpeza Planejada
        ISNULL(tg.css_class, '')                                        AS css_class_tipo_governanca,  -- >>> AJUSTE
        ISNULL(CONVERT(varchar(10), a.data_chegada, 103), '')           AS data_chegada,         -- >>> AJUSTE
        ISNULL(CONVERT(varchar(10), a.data_saida, 103), '')             AS data_saida,           -- >>> AJUSTE
        CASE WHEN a.data_chegada IS NOT NULL AND a.data_saida IS NOT NULL
             THEN CONVERT(varchar(10), DATEDIFF(DAY, a.data_chegada, a.data_saida))
             ELSE '' END                                                AS dias,
        ''                                                              AS tipo_hospede,         -- reservado: virá de API futura (OPER/PART/EXGU)
        ISNULL(fp.nome, '')                                             AS planejado_para,       -- >>> AJUSTE: camareira PLANEJADA
        ISNULL(fe.nome, '')                                             AS executado_por,        -- >>> AJUSTE: funcionário que executou
        ISNULL(CONVERT(varchar(5), ap.hora_termino, 108), '')           AS hora_termino,
        ISNULL(fv.nome, '')                                             AS vistoriado_por,       -- >>> AJUSTE: vistoriador
        ISNULL(CONVERT(varchar(10), ap.status_uh_discrepancia), '')     AS status_uh,            -- código gravado (a tela monta a combo)
        ISNULL(CONVERT(varchar(10), ap.status_governanca_discrepancia), '') AS status_gov,       -- código gravado
        ISNULL(ap.discrepancia, '')                                     AS divergencia,          -- OK / N/OK
        ISNULL(CONVERT(varchar(5), ap.adultos), '')                     AS adultos,
        ISNULL(CONVERT(varchar(5), ap.criancas1), '')                   AS criancas1,
        ISNULL(CONVERT(varchar(5), ap.criancas2), '')                   AS criancas2,
        ISNULL(ap.bagagem, '')                                          AS bagagem,
        ISNULL(ap.observacao, '')                                       AS observacao,
        ''                                                              AS selecionado
    FROM
        tb_cad_apartamento a                                                                     -- >>> AJUSTE: mesma base da sp_select_governanca_apontamento
        LEFT JOIN tb_gov_apontamento ap ON ap.codigo_apartamento = a.codigo
                                       AND ap.codigo_empresa = a.codigo_empresa
                                       AND ap.codigo_unidade = a.codigo_unidade
                                       AND ap.data = @data
        LEFT JOIN tb_stc_tipo_governanca tg ON tg.codigo = ap.codigo_tipo_governanca             -- >>> AJUSTE
        LEFT JOIN tb_gov_funcionario fe ON fe.codigo = ap.codigo_funcionario  AND fe.codigo_empresa = a.codigo_empresa  -- >>> AJUSTE
        LEFT JOIN tb_gov_funcionario fv ON fv.codigo = ap.codigo_vistoriador AND fv.codigo_empresa = a.codigo_empresa   -- >>> AJUSTE
        LEFT JOIN tb_gov_funcionario fp ON fp.codigo = ap.codigo_camareira_planejada AND fp.codigo_empresa = a.codigo_empresa  -- >>> AJUSTE
    WHERE a.codigo_empresa = @codigo_empresa
    AND   a.codigo_unidade = @codigo_unidade
    AND  (@codigo_tipo_governanca = -1 OR ap.codigo_tipo_governanca = @codigo_tipo_governanca)
    AND  (@bloco = ''                OR a.bloco = @bloco)                                        -- >>> AJUSTE
    AND  (@andar = ''                OR a.andar = @andar)                                        -- >>> AJUSTE
    AND  (@front_office_status = ''  OR a.front_office_status = @front_office_status)            -- >>> AJUSTE
    AND  (@room_status = ''          OR a.room_status = @room_status)                            -- >>> AJUSTE
    AND  (@uhInicio = -1 OR TRY_CONVERT(int, a.apartamento) >= @uhInicio)                        -- >>> AJUSTE
    AND  (@uhTermino = -1 OR TRY_CONVERT(int, a.apartamento) <= @uhTermino)                      -- >>> AJUSTE
    ORDER BY a.bloco, a.andar, a.apartamento;

END
GO
