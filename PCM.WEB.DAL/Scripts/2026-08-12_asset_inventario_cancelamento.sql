/**********************************************************************************************
    Script      : Inventário de Ativo Fixo - cancelamento (ponto 1)
    Data        : 12/08/2026  (corrigido 20/08/2026 com o schema real)
    Descricao   : Cancela o inventário ABERTO da unidade (botão "Cancelar inventário" na tela
                  assetInventoryClose).

    CORREÇÃO 20/08/2026 — conflito de status:
      A versão anterior gravava status = 4, mas a sp_select_asset_inventario_opened conta
      status IN (1, 4) como inventário ABERTO. Essa SP alimenta o HasInventoryOpened, que na
      tela de gerenciamento ESCONDE o botão "Novo inventário" quando a contagem é maior que
      zero. Resultado: ao cancelar, a unidade ficava travada — nunca mais era possível abrir
      um novo inventário, exatamente o oposto do que o cancelamento serve.

      Correção em duas partes, sem depender do significado do status 4:
        1. CANCELADO passa a ter status próprio, criado aqui se ainda não existir, e a SP de
           cancelamento resolve o número PELA DESCRIÇÃO (nunca fixa o número no código).
        2. sp_select_asset_inventario_opened passa a ignorar o inventário cancelado,
           mantendo intacta a regra atual (1, 4) para os demais status.

    OBSERVAÇÃO: a descrição precisa ser exatamente 'CANCELADO' — é por ela que a tela de
    gerenciamento esconde o botão de gerenciar inventário cancelado.

    Objetos:
      - Status    : 'CANCELADO' em tb_stc_status_inventario_asset
      - Procedure : sp_update_asset_inventory_cancel
      - Procedure : sp_select_asset_inventario_opened  (ajuste pontual)
**********************************************************************************************/

/*--------------------------------------------------------------------------------------------
    1) Status CANCELADO (criado com o próximo número livre, se ainda não existir)
--------------------------------------------------------------------------------------------*/
IF NOT EXISTS (SELECT 1 FROM tb_stc_status_inventario_asset WHERE descricao = 'CANCELADO')
BEGIN
    INSERT INTO tb_stc_status_inventario_asset (status, descricao)
    SELECT ISNULL(MAX(status), 0) + 1, 'CANCELADO'
    FROM   tb_stc_status_inventario_asset;
END
GO

/*--------------------------------------------------------------------------------------------
    1.1) Regulariza cancelamentos feitos pela versão anterior (status 4)
         Só alcança linhas com data_cancelamento preenchida, ou seja, que passaram mesmo pelo
         cancelamento — inventários legitimamente em status 4 não são tocados.
--------------------------------------------------------------------------------------------*/
UPDATE tb_ast_inventario
SET    status = (SELECT status FROM tb_stc_status_inventario_asset WHERE descricao = 'CANCELADO')
WHERE  status = 4
AND    data_cancelamento IS NOT NULL
AND    4 <> (SELECT status FROM tb_stc_status_inventario_asset WHERE descricao = 'CANCELADO');
GO

/*--------------------------------------------------------------------------------------------
    2) Cancelamento
--------------------------------------------------------------------------------------------*/
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

    DECLARE @status_cancelado smallint;

    SELECT @status_cancelado = status
    FROM   tb_stc_status_inventario_asset
    WHERE  descricao = 'CANCELADO';

    IF @status_cancelado IS NULL
        THROW 51001, 'Status CANCELADO não cadastrado em tb_stc_status_inventario_asset.', 1;

    UPDATE tb_ast_inventario
    SET    status = @status_cancelado,
           codigo_usuario_cancelamento = @codigo_usuario,
           data_cancelamento = GETDATE()
    WHERE  codigo_empresa = @codigo_empresa
    AND    codigo_unidade = @codigo_unidade
    AND    status IN (1, 2);

    IF @@ROWCOUNT = 0
        THROW 51000, 'Nenhum inventário aberto encontrado para esta unidade.', 1;

END
GO

/*--------------------------------------------------------------------------------------------
    3) Inventário aberto: cancelado NÃO conta
       (mantém a regra atual IN (1, 4) e apenas exclui o cancelado, para o botão
        "Novo inventário" voltar a aparecer depois de um cancelamento)
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_select_asset_inventario_opened', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_asset_inventario_opened];
GO

CREATE PROCEDURE [dbo].[sp_select_asset_inventario_opened]
(
    @codigo_empresa smallint,
    @codigo_unidade int
)
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        COUNT(*) AS quantidade
    FROM
        tb_ast_inventario INNER JOIN
        tb_stc_status_inventario_asset ON
        tb_ast_inventario.status = tb_stc_status_inventario_asset.status
    WHERE   (((tb_ast_inventario.codigo_empresa) = @codigo_empresa)
    AND     ((tb_ast_inventario.codigo_unidade) = @codigo_unidade)
    AND     ((tb_ast_inventario.status) IN (1, 4))
    AND     ((tb_stc_status_inventario_asset.descricao) <> 'CANCELADO'))

END
GO
