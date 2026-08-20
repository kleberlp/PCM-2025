/**********************************************************************************************
    Script      : Inventário de Ativo Fixo - identificação do contador por e-mail
    Data        : 20/08/2026
    Descricao   : Usada pelo app (PCM.WEB.OS) quando a tela de inventário é aberta SEM uniqueId
                  — caso do atalho do PWA na tela inicial ou de um link expirado.
                  O usuário informa o e-mail e, se ele estiver vinculado a um contador de um
                  inventário em aberto (status 1 ou 2), o app entra com o uniqueId encontrado.

    Escrita sobre o schema real:
      tb_ast_inventario_contador (codigo_inventario, uniqueId, nome, email, celular)
      tb_ast_inventario          (codigo, ..., data_inicio, status)

    Chamada do código web (já publicada):
      - sp_select_asset_inventario_contador_email (@email) -> uniqueId (nenhuma linha = não achou)
**********************************************************************************************/

IF OBJECT_ID('dbo.sp_select_asset_inventario_contador_email', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_asset_inventario_contador_email];
GO

CREATE PROCEDURE [dbo].[sp_select_asset_inventario_contador_email]
@email varchar(150)
AS
BEGIN

    SET NOCOUNT ON;

    -- Havendo mais de um inventário em aberto para o mesmo e-mail, entra no mais recente
    SELECT TOP 1
        c.uniqueId
    FROM
        tb_ast_inventario_contador c
        INNER JOIN tb_ast_inventario i ON i.codigo = c.codigo_inventario
    WHERE LTRIM(RTRIM(c.email)) = LTRIM(RTRIM(@email))
    AND   i.status IN (1, 2)
    ORDER BY i.data_inicio DESC, i.codigo DESC;

END
GO

-- Busca por e-mail em tabela sem índice: acelera a consulta acima
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_ast_inventario_contador_email' AND object_id = OBJECT_ID('dbo.tb_ast_inventario_contador'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_tb_ast_inventario_contador_email]
        ON [dbo].[tb_ast_inventario_contador] ([email]) INCLUDE ([codigo_inventario], [uniqueId]);
END
GO
