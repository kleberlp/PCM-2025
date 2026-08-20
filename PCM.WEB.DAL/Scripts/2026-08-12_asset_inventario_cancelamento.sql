/**********************************************************************************************
    Script      : Inventário de Ativo Fixo - cancelamento (ponto 1)
    Data        : 12/08/2026
    Descricao   : Cancela o inventário ABERTO da unidade (botão "Cancelar inventário" na tela
                  assetInventoryClose). Espelha a sp_update_asset_inventory_close, mas leva o
                  inventário ao status CANCELADO.

    IMPORTANTE: as SPs vivem no BANCO. Este é um TEMPLATE — confira os nomes reais marcados
    com  -- >>> AJUSTE  (tabela do inventário, tabela de status e colunas de auditoria),
    usando a sp_update_asset_inventory_close real como referência.

    Chamada do código web (já publicada):
      - sp_update_asset_inventory_cancel (@codigo_empresa, @codigo_unidade, @codigo_usuario)
**********************************************************************************************/

-- 1) Status CANCELADO no domínio de status do inventário
--    (a tela de gerenciamento esconde o botão de gerenciar quando status = 'CANCELADO')
IF NOT EXISTS (SELECT 1 FROM tb_stc_status_inventario_asset WHERE descricao = 'CANCELADO')   -- >>> AJUSTE tabela/coluna
BEGIN
    INSERT INTO tb_stc_status_inventario_asset (descricao)                                    -- >>> AJUSTE colunas
    VALUES ('CANCELADO');
END
GO

-- 2) Procedure de cancelamento
IF OBJECT_ID('dbo.sp_update_asset_inventory_cancel', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_update_asset_inventory_cancel];
GO

CREATE PROCEDURE [dbo].[sp_update_asset_inventory_cancel]
@codigo_empresa int,
@codigo_unidade int,
@codigo_usuario int
AS
BEGIN

    SET NOCOUNT ON;

    UPDATE tb_asset_inventario                                                                -- >>> AJUSTE: mesma tabela usada pela sp_update_asset_inventory_close
    SET    codigo_status = (SELECT codigo FROM tb_stc_status_inventario_asset WHERE descricao = 'CANCELADO'),  -- >>> AJUSTE
           codigo_usuario_update = @codigo_usuario,                                           -- >>> AJUSTE colunas de auditoria
           data_update = GETDATE()                                                            -- >>> AJUSTE
    WHERE  codigo_empresa = @codigo_empresa
    AND    codigo_unidade = @codigo_unidade
    AND    codigo_status = (SELECT codigo FROM tb_stc_status_inventario_asset WHERE descricao = 'ABERTO');     -- >>> AJUSTE: mesmo critério de "aberto" da sp de fechamento

    IF @@ROWCOUNT = 0
        THROW 51000, 'Nenhum inventário aberto encontrado para esta unidade.', 1;

END
GO
