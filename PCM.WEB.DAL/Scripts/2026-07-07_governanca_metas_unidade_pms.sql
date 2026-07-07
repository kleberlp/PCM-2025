/**********************************************************************************************
    Script      : Governança - Metas por Unidade (SELECT com Aptos Totais)
    Data        : 07/07/2026
    Descricao   : Lista uma linha por unidade da empresa, trazendo os overrides cadastrados
                  (tb_cfg_governanca_meta_unidade) e o "Aptos Totais" do período de referência.

    ⚠️  FONTE DO "APTOS TOTAIS PMS":
        Não existe no schema atual uma tabela-fato mensal com a ocupação vinda do PMS
        (os dados do PMS entram só em staging transitório - tb_interface_uh_reservas_stg -
        truncado a cada importação; a ocupação corrente fica em tb_cad_apartamento.data_saida).
        Como PROXY PERSISTIDO e MENSAL, usamos a contagem de apartamentos distintos com
        apontamento de governança (arrumação) no mês de referência: tb_gov_apontamento.
        => Para refletir a ocupação real (check-outs + stayovers) do PMS, criar futuramente
           uma tabela-fato diária (codigo_empresa, codigo_unidade, data, qtde) alimentada pela
           importação e trocar apenas a subquery "aptos_totais_pms" abaixo.

    Objetos (novo):
        - Procedure : sp_select_configuracao_governanca_meta_unidade
**********************************************************************************************/

IF OBJECT_ID('dbo.sp_select_configuracao_governanca_meta_unidade', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_configuracao_governanca_meta_unidade]
GO

CREATE PROCEDURE [dbo].[sp_select_configuracao_governanca_meta_unidade]
@codigo_empresa smallint,
@mes            int,
@ano            int
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        u.codigo                                        AS codigo_unidade,
        ISNULL(u.nome_fantasia, '')                     AS nome_unidade,
        ISNULL((SELECT COUNT(DISTINCT g.codigo_apartamento)
                FROM   tb_gov_apontamento g
                WHERE  g.codigo_empresa = u.codigo_empresa
                AND    g.codigo_unidade = u.codigo
                AND    MONTH(g.data) = @mes
                AND    YEAR(g.data)  = @ano), 0)         AS aptos_totais_pms,
        m.pct_meta_custom,
        m.meta_aptos_dia_camareira_custom,
        m.meta_pct_vistoria_custom
    FROM
        tb_cad_unidade u
        LEFT JOIN tb_cfg_governanca_meta_unidade m ON
            m.codigo_empresa = u.codigo_empresa AND
            m.codigo_unidade = u.codigo
    WHERE   u.codigo_empresa = @codigo_empresa
    AND     u.ativo = 1
    ORDER BY
        u.nome_fantasia

END
GO
