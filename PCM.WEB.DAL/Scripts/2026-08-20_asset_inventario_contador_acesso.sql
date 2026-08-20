/**********************************************************************************************
    Script      : Inventário de Ativo Fixo - envio do link de acesso ao contador
    Data        : 20/08/2026
    Descricao   : Quando o app (PCM.WEB.OS) é aberto sem uniqueId, o contador informa o e-mail
                  e o sistema ENVIA o link de acesso para esse e-mail (em vez de liberar a tela
                  direto). Assim o acesso continua dependendo da caixa postal do contador, do
                  mesmo jeito que o link enviado na criação do inventário.

    Escrita sobre o schema real:
      tb_ast_inventario_contador (codigo_inventario, uniqueId, nome, email, celular)
      tb_ast_inventario          (codigo, ..., descricao, data_inicio, status)

    A fila de e-mail é a tb_log_email (ordem_servico, para, body), a mesma usada pela
    sp_insert_asset_inventario ao enviar o link na criação do inventário.

    Retorno: 1 quando um e-mail foi enfileirado, 0 quando o e-mail não corresponde a nenhum
             contador de inventário em aberto (status 1 ou 2).

    Chamada do código web (já publicada):
      - sp_insert_asset_inventario_contador_acesso (@email, @link)
**********************************************************************************************/

IF OBJECT_ID('dbo.sp_insert_asset_inventario_contador_acesso', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_insert_asset_inventario_contador_acesso];
GO

CREATE PROCEDURE [dbo].[sp_insert_asset_inventario_contador_acesso]
@email varchar(150),
@link  varchar(500)
AS
BEGIN

    SET NOCOUNT ON;

    DECLARE @codigo_inventario bigint,
            @uniqueId          varchar(50),
            @nome              varchar(150),
            @descricao         varchar(150);

    -- Contador de inventário em aberto (o mais recente, se houver mais de um)
    SELECT TOP 1
        @codigo_inventario = c.codigo_inventario,
        @uniqueId          = c.uniqueId,
        @nome              = c.nome,
        @descricao         = i.descricao
    FROM
        tb_ast_inventario_contador c
        INNER JOIN tb_ast_inventario i ON i.codigo = c.codigo_inventario
    WHERE LTRIM(RTRIM(c.email)) = LTRIM(RTRIM(@email))
    AND   i.status IN (1, 2)
    ORDER BY i.data_inicio DESC, i.codigo DESC;

    IF @uniqueId IS NULL
    BEGIN
        SELECT 0;   -- e-mail não corresponde a inventário em aberto
        RETURN;
    END

    DECLARE @body varchar(max) =
        '<p>Olá, ' + ISNULL(@nome, '') + '.</p>' +
        '<p>Segue o link de acesso ao inventário <b>' + ISNULL(@descricao, '') + '</b>:</p>' +
        '<p><a href="' + @link + '">Abrir inventário</a></p>' +
        '<p style="font-size:12px;color:#6c757d">Se você não solicitou este acesso, ignore este e-mail.</p>';

    -- Enfileira na mesma tabela usada pela sp_insert_asset_inventario
    INSERT INTO tb_log_email
    (
        ordem_servico,
        para,
        body
    )
    VALUES
    (
        'INVENTARIO_ACESSO_' + CAST(@codigo_inventario AS varchar),
        @email,
        @body
    );

    SELECT 1;   -- e-mail enfileirado

END
GO
