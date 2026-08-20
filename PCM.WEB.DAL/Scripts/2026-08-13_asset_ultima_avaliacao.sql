/**********************************************************************************************
    Script      : Inventário de Ativo Fixo - última avaliação do ativo (ponto 4)
    Data        : 13/08/2026
    Descricao   : SP usada pela bipagem do aplicativo (PCM.WEB.OS): ao bipar um ativo
                  encontrado na base, o modal já abre pré-classificado com a última avaliação
                  de estado de conservação (OK / N-OK + observação); a edição é opcional.

    IMPORTANTE: as SPs vivem no BANCO. TEMPLATE — confira os nomes marcados com  -- >>> AJUSTE
    (tabela de contagem do inventário e colunas status_ok/observacao/auditoria).

    Chamada do código web (já publicada):
      - sp_select_asset_last_evaluation (@codigo_empresa, @codigo_unidade, @asset_code)
        -> status_ok / observacao (0 linhas = sem avaliação anterior; o app segue com OK)
        Obs.: se a SP não existir, o app não quebra — apenas abre o modal sem pré-classificação.
**********************************************************************************************/

IF OBJECT_ID('dbo.sp_select_asset_last_evaluation', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_asset_last_evaluation];
GO

CREATE PROCEDURE [dbo].[sp_select_asset_last_evaluation]
@codigo_empresa int,
@codigo_unidade int,
@asset_code     varchar(50)
AS
BEGIN

    SET NOCOUNT ON;

    SELECT TOP 1
        CAST(ISNULL(c.status_ok, 1) AS bit) AS status_ok,
        ISNULL(c.observacao, '')            AS observacao
    FROM
        tb_asset_inventario_count c                                  -- >>> AJUSTE: tabela de contagem (a mesma onde a bipagem grava status_ok/observacao/foto_path)
    WHERE c.codigo_empresa = @codigo_empresa
    AND   c.codigo_unidade = @codigo_unidade
    AND   c.asset_code = @asset_code                                 -- >>> AJUSTE se a coluna tiver outro nome
    AND   c.status_ok IS NOT NULL                                    -- só contagens que tiveram avaliação
    ORDER BY c.codigo DESC;                                          -- >>> AJUSTE: ou data_input DESC — a avaliação MAIS RECENTE

END
GO
