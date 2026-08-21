/**********************************************************************************************
    Script      : Inventário de Ativo Fixo - avaliação anterior e foto do ativo
    Data        : 13/08/2026  (revisto 20/08/2026 com o schema real)
    Descricao   : Usada pela bipagem do aplicativo (PCM.WEB.OS) ao encontrar o ativo na base:
                    - pré-classifica o modal com a ÚLTIMA avaliação (OK / N-OK + observação)
                    - informa se o ativo JÁ POSSUI FOTO, para aplicar a regra de foto
                      obrigatória do item bipado que ainda não tem foto

    Regra da foto no app (foto só é permitida nestes casos):
      1. ativo bipado que ainda não possui foto  -> possui_foto = 0 aqui
      2. novo cadastro (ativo fora da base)      -> fluxo próprio de cadastro
      3. avaliação Não OK                        -> sempre exige

    Escrita sobre o schema real: tb_ast_inventario_count (status_ok, observacao, foto_path).
    Se a foto oficial do ativo passar a ser guardada em tb_cad_asset, basta trocar o cálculo
    de possui_foto para olhar aquela coluna.

    Retorno: status_ok / observacao / possui_foto
             (nenhuma linha = ativo sem contagem anterior; o app trata como sem avaliação e
              sem foto, ou seja, vai exigir a foto)

    Chamada do código web (já publicada):
      - sp_select_asset_last_evaluation (@codigo_empresa, @codigo_unidade, @asset_code)
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

    -- Possui foto: qualquer contagem anterior do ativo com foto registrada
    DECLARE @possui_foto bit =
    (
        SELECT CASE WHEN EXISTS
        (
            SELECT 1
            FROM   tb_ast_inventario_count
            WHERE  codigo_empresa = @codigo_empresa
            AND    codigo_unidade = @codigo_unidade
            AND    asset_code = @asset_code
            AND    ISNULL(foto_path, '') <> ''
        )
        THEN 1 ELSE 0 END
    );

    -- Última avaliação registrada (a mais recente que teve status preenchido)
    SELECT TOP 1
        CAST(1 AS bit)                      AS possui_avaliacao,
        CAST(ISNULL(c.status_ok, 1) AS bit) AS status_ok,
        ISNULL(c.observacao, '')            AS observacao,
        @possui_foto                        AS possui_foto
    FROM
        tb_ast_inventario_count c
    WHERE c.codigo_empresa = @codigo_empresa
    AND   c.codigo_unidade = @codigo_unidade
    AND   c.asset_code = @asset_code
    AND   c.status_ok IS NOT NULL
    ORDER BY c.data DESC;

    -- Sem avaliação anterior: ainda assim devolve a linha, porque o app precisa saber
    -- se o ativo já tem foto (possui_avaliacao = 0 evita marcar como pré-classificado)
    IF @@ROWCOUNT = 0
    BEGIN
        SELECT
            CAST(0 AS bit)  AS possui_avaliacao,
            CAST(1 AS bit)  AS status_ok,
            ''              AS observacao,
            @possui_foto    AS possui_foto;
    END

END
GO
