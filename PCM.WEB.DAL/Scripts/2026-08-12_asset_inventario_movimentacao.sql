/**********************************************************************************************
    Script      : Inventário de Ativo Fixo - contagem duplicada (ponto 5)
    Data        : 12/08/2026  (corrigido 20/08/2026 com o schema real)
    Descricao   : Verifica se o ativo já foi contado no inventário e em qual local, para a
                  bipagem perguntar ao usuário se deseja movimentar.

    CORREÇÃO 20/08/2026 — a versão anterior deste script foi escrita sem acesso ao schema e
    tinha 3 erros que faziam a SP falhar em tempo de execução (a verificação de duplicidade
    ficava desligada em silêncio, porque a aplicação trata falha aqui como "não contado"):
      1. tabela  tb_asset_inventario_count  -> o nome real é  tb_ast_inventario_count
      2. ORDER BY c.codigo                  -> a tabela não tem coluna codigo; ordena por data
      3. joins de setor/apartamento sem codigo_empresa (as chaves são compostas)

    NOTA sobre o ponto 8 (movimentação do ativo): NÃO é preciso mexer na
    sp_insert_asset_inventory_count. A sp_update_asset_inventory_close já atualiza
    tb_cad_asset (codigo_setor / codigo_apartamento) a partir de tb_ast_inventario_count no
    fechamento do inventário. A aplicação NÃO envia mais o parâmetro @movimentar.

    Objetos:
      - Procedure : sp_select_asset_inventory_count_location
**********************************************************************************************/

IF OBJECT_ID('dbo.sp_select_asset_inventory_count_location', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_asset_inventory_count_location];
GO

CREATE PROCEDURE [dbo].[sp_select_asset_inventory_count_location]
@codigo_inventario  bigint,
@asset_code         varchar(50),
@codigo_setor       int,
@codigo_apartamento int = NULL
AS
BEGIN

    SET NOCOUNT ON;

    -- Nenhuma linha = ainda não contado (a aplicação trata reader vazio como "não contado")
    SELECT TOP 1
        CAST(1 AS bit)                                              AS already_counted,
        CAST(CASE WHEN c.codigo_setor = @codigo_setor
                   AND ISNULL(c.codigo_apartamento, -1) = ISNULL(@codigo_apartamento, -1)
                  THEN 1 ELSE 0 END AS bit)                         AS same_location,
        CONCAT(ISNULL(s.descricao, ''),
               CASE WHEN a.codigo IS NULL
                    THEN ''
                    ELSE CONCAT(' / ', ISNULL(CONVERT(varchar(100), a.apartamento), a.local))
               END)                                                 AS local_atual
    FROM
        tb_ast_inventario_count c
        LEFT JOIN tb_cad_setor s ON s.codigo = c.codigo_setor
                                AND s.codigo_empresa = c.codigo_empresa
        LEFT JOIN tb_cad_apartamento a ON a.codigo = c.codigo_apartamento
                                      AND a.codigo_empresa = c.codigo_empresa
    WHERE c.codigo_inventario = @codigo_inventario
    AND   c.asset_code = @asset_code
    ORDER BY c.data DESC;

END
GO
