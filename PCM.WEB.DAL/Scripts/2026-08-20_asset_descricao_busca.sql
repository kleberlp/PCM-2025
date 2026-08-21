/**********************************************************************************************
    Script      : Inventário de Ativo Fixo - busca de nomes para cadastro de novo ativo
    Data        : 20/08/2026
    Descricao   : Etapa 2 do fluxo de cadastro de um equipamento não identificado no app
                  (PCM.WEB.OS): o usuário digita ao menos 3 caracteres e escolhe o nome em uma
                  lista vinda de um cadastro pré-definido.

    FONTE DOS NOMES: por ora, os nomes já usados no cadastro de ativos (DISTINCT descricao de
    tb_cad_asset), que na prática é o catálogo existente. Se houver//passar a haver uma tabela
    dedicada de nomes padronizados, basta trocar o FROM abaixo — o formato de saída
    (uma coluna "descricao") é o que a aplicação espera.

    Busca por unidade com fallback para a empresa: se a unidade ainda não tem aquele
    equipamento cadastrado, o nome continua aparecendo a partir das outras unidades.

    Chamada do código web (já publicada):
      - sp_select_asset_descricao_busca (@codigo_empresa, @codigo_unidade, @termo)
**********************************************************************************************/

IF OBJECT_ID('dbo.sp_select_asset_descricao_busca', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_asset_descricao_busca];
GO

CREATE PROCEDURE [dbo].[sp_select_asset_descricao_busca]
@codigo_empresa int,
@codigo_unidade int,
@termo          varchar(100)
AS
BEGIN

    SET NOCOUNT ON;

    -- A aplicação já barra termos com menos de 3 caracteres; aqui é apenas garantia
    IF LEN(ISNULL(@termo, '')) < 3
    BEGIN
        SELECT TOP 0 CONVERT(varchar(255), '') AS descricao;
        RETURN;
    END

    SELECT TOP 20
        a.descricao,
        -- Prioriza os nomes da própria unidade
        MIN(CASE WHEN a.codigo_unidade = @codigo_unidade THEN 0 ELSE 1 END) AS prioridade
    FROM
        tb_cad_asset a
    WHERE a.codigo_empresa = @codigo_empresa
    AND   ISNULL(a.descricao, '') <> ''
    AND   a.descricao LIKE '%' + @termo + '%'
    GROUP BY
        a.descricao
    ORDER BY
        prioridade,
        a.descricao;

END
GO
