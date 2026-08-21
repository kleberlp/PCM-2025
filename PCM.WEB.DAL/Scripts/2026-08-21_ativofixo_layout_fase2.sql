/**********************************************************************************************
    Script      : Ativo Fixo - dados que faltavam para o novo layout (Fase 2)
    Data        : 21/08/2026
    Descricao   : Acrescenta ao SELECT das procedures as informações que o layout novo mostra e
                  que hoje não são devolvidas. NENHUMA ALTERAÇÃO DE TABELA — só procedure.

                  Nas telas de Ativos e Inventários as colunas da grid são declaradas dentro da
                  própria procedure (segundo result set), então a coluna nova aparece na tela
                  sem mexer em C# ou JavaScript.

    Conteúdo:
      1. sp_select_asset_inventory_manager    -> contadores, contados/previstos e % de progresso
      2. sp_select_asset_inventario_opened    -> devolve também o código do inventário aberto
      3. sp_select_cadastro_basico_asset      -> valor de compra e última movimentação
      4. sp_select_asset_manager_movement     -> status_ok e foto_path na aba de inventariados

    ATENÇÃO (item 4): as colunas daquela grid vêm da tabela tb_stc_grid_column (codigo_grid
    1002/1003/1004), e não da procedure. Por isso o item 4 só acrescenta os DADOS — a tela usa
    esses campos para desenhar o chip de avaliação dentro da coluna de descrição. Para virarem
    colunas próprias, basta cadastrar as linhas correspondentes em tb_stc_grid_column.
**********************************************************************************************/

/*============================================================================================
    1) INVENTÁRIOS: contadores e progresso
============================================================================================*/
IF OBJECT_ID('dbo.sp_select_asset_inventory_manager', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_asset_inventory_manager];
GO

CREATE PROCEDURE [dbo].[sp_select_asset_inventory_manager]
(
    @codigo_empresa smallint,
    @codigo_unidade int,
    @descricao      varchar(255) = '',
    @data_inicio    date,
    @data_termino   date,
    @status         smallint
)
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        tb_cad_unidade.nome_fantasia AS unidade,
        tb_ast_inventario.descricao,
        FORMAT(tb_ast_inventario.data_inicio, 'dd/MM/yyyy HH:mm:ss') AS data_inicio,
        tb_cad_usuario.nome AS usuario_inicio,
        FORMAT(tb_ast_inventario.data_termino, 'dd/MM/yyyy HH:mm:ss') AS data_termino,
        tb_cad_usuario_termino.nome AS usuario_termino,
        tb_stc_status_inventario_asset.descricao AS status,
        tb_ast_inventario.codigo AS codigoInventario,

        -- NOVO: quantos contadores foram designados
        (SELECT COUNT(*)
         FROM   tb_ast_inventario_contador c
         WHERE  c.codigo_inventario = tb_ast_inventario.codigo) AS contadores,

        -- NOVO: contados x previstos e o percentual, para a barra de progresso
        (SELECT COUNT(*)
         FROM   tb_ast_inventario_count n
         WHERE  n.codigo_inventario = tb_ast_inventario.codigo) AS contados,

        (SELECT COUNT(*)
         FROM   tb_cad_asset a
         WHERE  a.codigo_empresa = tb_ast_inventario.codigo_empresa
         AND    a.codigo_unidade = tb_ast_inventario.codigo_unidade
         AND    a.codigo_status = 1) AS previstos,

        CASE
            WHEN (SELECT COUNT(*) FROM tb_cad_asset a
                  WHERE a.codigo_empresa = tb_ast_inventario.codigo_empresa
                  AND   a.codigo_unidade = tb_ast_inventario.codigo_unidade
                  AND   a.codigo_status = 1) = 0 THEN 0
            ELSE
                CASE
                    WHEN (SELECT COUNT(*) FROM tb_ast_inventario_count n
                          WHERE n.codigo_inventario = tb_ast_inventario.codigo) * 100
                         / (SELECT COUNT(*) FROM tb_cad_asset a
                            WHERE a.codigo_empresa = tb_ast_inventario.codigo_empresa
                            AND   a.codigo_unidade = tb_ast_inventario.codigo_unidade
                            AND   a.codigo_status = 1) > 100 THEN 100
                    ELSE (SELECT COUNT(*) FROM tb_ast_inventario_count n
                          WHERE n.codigo_inventario = tb_ast_inventario.codigo) * 100
                         / (SELECT COUNT(*) FROM tb_cad_asset a
                            WHERE a.codigo_empresa = tb_ast_inventario.codigo_empresa
                            AND   a.codigo_unidade = tb_ast_inventario.codigo_unidade
                            AND   a.codigo_status = 1)
                END
        END AS progresso

    FROM
        tb_ast_inventario INNER JOIN
        tb_cad_unidade ON
        tb_ast_inventario.codigo_unidade = tb_cad_unidade.codigo AND
        tb_ast_inventario.codigo_empresa = tb_cad_unidade.codigo_empresa INNER JOIN
        tb_cad_usuario ON
        tb_ast_inventario.codigo_usuario = tb_cad_usuario.codigo AND
        tb_ast_inventario.codigo_empresa = tb_cad_usuario.codigo_empresa LEFT JOIN
        tb_cad_usuario tb_cad_usuario_termino ON
        tb_ast_inventario.codigo_usuario_termino = tb_cad_usuario_termino.codigo AND
        tb_ast_inventario.codigo_empresa = tb_cad_usuario_termino.codigo_empresa INNER JOIN
        tb_stc_status_inventario_asset ON
        tb_ast_inventario.status = tb_stc_status_inventario_asset.status
    WHERE   (((tb_ast_inventario.codigo_empresa) = @codigo_empresa)
    AND     (((tb_ast_inventario.codigo_unidade) = @codigo_unidade) OR ((@codigo_unidade) = -1))
    AND     ((tb_ast_inventario.descricao) LIKE '%' + @descricao + '%')
    AND     (((tb_ast_inventario.data_inicio) >= @data_inicio) OR ((@data_inicio) IS NULL))
    AND     (((tb_ast_inventario.data_inicio) <= @data_termino) OR ((@data_termino) IS NULL))
    AND     (((tb_ast_inventario.status) = @status) OR ((@status) = -1)))
    ORDER BY
        tb_ast_inventario.data_inicio DESC

    -- Colunas da grid (a tela monta a partir daqui)
    SELECT 'unidade' AS data, 'Unidade' AS title, CAST(1 AS bit) AS visible, CAST(1 AS bit) AS orderable, 'left' AS align WHERE @codigo_unidade = -1
    UNION ALL
    SELECT 'descricao', 'Descrição', CAST(1 AS bit), CAST(1 AS bit), 'left'
    UNION ALL
    SELECT 'data_inicio', 'Data início', CAST(1 AS bit), CAST(1 AS bit), 'center'
    UNION ALL
    SELECT 'usuario_inicio', 'Usuário Início', CAST(1 AS bit), CAST(1 AS bit), 'center'
    UNION ALL
    SELECT 'contadores', 'Contadores', CAST(1 AS bit), CAST(1 AS bit), 'center'          -- NOVO
    UNION ALL
    SELECT 'progresso', 'Progresso', CAST(1 AS bit), CAST(1 AS bit), 'left'              -- NOVO
    UNION ALL
    SELECT 'data_termino', 'Data Término', CAST(1 AS bit), CAST(1 AS bit), 'center'
    UNION ALL
    SELECT 'usuario_termino', 'Usuário Término', CAST(1 AS bit), CAST(1 AS bit), 'center'
    UNION ALL
    SELECT 'status', 'Status', CAST(1 AS bit), CAST(1 AS bit), 'center';

    SELECT
        'unidade' AS ColumnName,
        0 AS Level,
        CAST(1 AS bit) AS Collapsible,
        CAST(1 AS bit) AS ShowCount,
        '' AS CssClass
    WHERE   (((1) <> 1))

END
GO

/*============================================================================================
    2) INVENTÁRIO ABERTO: devolve também o código, para a tela levar direto ao inventário
       A quantidade continua sendo a PRIMEIRA coluna (o código lê por ExecuteScalar).
       Cancelado não conta — ver script 2026-08-12_asset_inventario_cancelamento.sql
============================================================================================*/
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
        COUNT(*)                    AS quantidade,
        MAX(tb_ast_inventario.codigo) AS codigo_inventario
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

/*============================================================================================
    3) ATIVOS: valor de compra e última movimentação
============================================================================================*/
IF OBJECT_ID('dbo.sp_select_cadastro_basico_asset', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_cadastro_basico_asset];
GO

CREATE PROCEDURE [dbo].[sp_select_cadastro_basico_asset]
(
    @codigo_empresa smallint,
    @codigo_unidade int,
    @codigo         varchar(50) = '',
    @descricao      varchar(255) = '',
    @status         varchar(10) = -1,
    @localizacao    varchar(100) = ''
)
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        u.nome_fantasia AS unidade,
        a.codigo,
        a.asset_code AS assetCode,
        a.descricao,
        s.descricao AS status,
        a.local,
        a.codigo_setor,
        a.codigo_apartamento,
        a.setor,

        -- NOVO: última movimentação registrada do ativo
        ISNULL(FORMAT((SELECT MAX(m.data_movimentacao)
                       FROM   tb_ast_movimentacao m
                       WHERE  m.codigo_asset = a.codigo
                       AND    m.codigo_empresa = a.codigo_empresa), 'dd/MM/yyyy'), '—') AS ultimaMovimentacao,

        -- NOVO: valor de compra (a coluna já existia na tabela, mas não era devolvida)
        ISNULL(FORMAT(a.valor_compra, 'N2', 'pt-BR'), '—') AS valorCompra

    FROM
        tb_cad_asset a LEFT JOIN
        tb_stc_status_asset s ON
        s.status = a.codigo_status INNER JOIN
        tb_cad_unidade u ON
        a.codigo_empresa = u.codigo_empresa AND
        a.codigo_unidade = u.codigo
    WHERE
        a.codigo_empresa = @codigo_empresa
        AND (a.codigo_unidade = @codigo_unidade OR @codigo_unidade = -1)
        AND (@codigo = '' OR a.asset_code LIKE '%' + @codigo + '%')
        AND (@descricao = '' OR a.descricao LIKE '%' + @descricao + '%')
        AND (@status = -1 OR a.codigo_status = @status)
        AND (
            @localizacao = '' OR
            (
                (a.codigo_setor IS NOT NULL AND CAST(a.codigo_setor AS varchar) LIKE '%' + @localizacao + '%')
                OR
                (a.codigo_apartamento IS NOT NULL AND CAST(a.codigo_apartamento AS varchar) LIKE '%' + @localizacao + '%')
            )
        )
    ORDER BY a.codigo DESC;

    SELECT
        'unidade' AS Data,
        'Unidade' AS Title,
        CAST(1 AS bit) AS Visible,
        CAST(1 AS bit) AS Orderable,
        'left' AS Align
    UNION ALL
    SELECT 'assetCode', 'Código', CAST(1 AS bit), CAST(1 AS bit), 'center'
    UNION ALL
    SELECT 'descricao', 'Descrição', CAST(1 AS bit), CAST(1 AS bit), 'left'
    UNION ALL
    SELECT 'status', 'Status', CAST(1 AS bit), CAST(1 AS bit), 'center'
    UNION ALL
    SELECT 'local', 'Local', CAST(1 AS bit), CAST(1 AS bit), 'left'
    UNION ALL
    SELECT 'ultimaMovimentacao', 'Últ. movimentação', CAST(1 AS bit), CAST(1 AS bit), 'center'   -- NOVO
    UNION ALL
    SELECT 'valorCompra', 'Valor', CAST(1 AS bit), CAST(1 AS bit), 'right';                      -- NOVO

    SELECT
        'unidade' AS ColumnName,
        0 AS Level,
        CAST(1 AS bit) AS Collapsible,
        CAST(1 AS bit) AS ShowCount,
        '' AS CssClass
    WHERE   (((1) <> 1))

END
GO

/*============================================================================================
    4) INVENTÁRIO ABERTO (aba Inventariados): status_ok e foto_path junto dos dados
       As colunas dessa grid vêm de tb_stc_grid_column; aqui só acrescentamos os campos ao
       retorno, e a tela usa status_ok para desenhar o chip de avaliação.
       Somente o bloco @type = 0 muda; 1 e 2 seguem como estão.
============================================================================================*/
IF OBJECT_ID('dbo.sp_select_asset_manager_movement', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_asset_manager_movement];
GO

CREATE PROCEDURE [dbo].[sp_select_asset_manager_movement]

@codigo bigint,
@type   smallint

AS

BEGIN

    IF @type = 0 BEGIN

        SELECT
            tb_cad_asset.asset_code,
            tb_cad_asset.descricao,
            tb_ast_inventario_count.origem,
            tb_ast_inventario_count.destino,
            FORMAT(tb_ast_inventario_count.data, 'dd/MM/yyyy HH:mm:ss') AS data,
            tb_cad_usuario.nome AS usuario,
            tb_stc_tipo_movimentacao_asset.descricao AS tipo_movimentacao,
            tb_ast_inventario_count.status_ok,                      -- NOVO
            tb_ast_inventario_count.foto_path                       -- NOVO
        FROM
            tb_ast_inventario_count INNER JOIN
            tb_cad_asset ON
            tb_ast_inventario_count.codigo_asset = tb_cad_asset.codigo AND
            tb_ast_inventario_count.codigo_empresa = tb_cad_asset.codigo_empresa LEFT JOIN
            tb_cad_setor ON
            tb_cad_asset.codigo_setor = tb_cad_setor.codigo AND
            tb_cad_asset.codigo_empresa = tb_cad_setor.codigo_empresa LEFT JOIN
            tb_cad_apartamento ON
            tb_cad_asset.codigo_apartamento = tb_cad_apartamento.codigo AND
            tb_cad_asset.codigo_empresa = tb_cad_apartamento.codigo_empresa LEFT JOIN
            tb_cad_fornecedor ON
            tb_cad_asset.codigo_fornecedor = tb_cad_fornecedor.codigo AND
            tb_cad_asset.codigo_empresa = tb_cad_fornecedor.codigo_empresa LEFT JOIN
            tb_cad_setor tb_cad_setor_destino ON
            tb_ast_inventario_count.codigo_setor = tb_cad_setor_destino.codigo AND
            tb_ast_inventario_count.codigo_empresa = tb_cad_setor_destino.codigo_empresa LEFT JOIN
            tb_cad_apartamento tb_cad_apartamento_destino ON
            tb_ast_inventario_count.codigo_apartamento = tb_cad_apartamento_destino.codigo AND
            tb_ast_inventario_count.codigo_empresa = tb_cad_apartamento_destino.codigo_empresa LEFT JOIN
            tb_cad_usuario ON
            tb_ast_inventario_count.codigo_usuario = tb_cad_usuario.codigo AND
            tb_ast_inventario_count.codigo_empresa = tb_cad_usuario.codigo_empresa LEFT JOIN
            tb_stc_tipo_movimentacao_asset ON
            CASE
                WHEN tb_ast_inventario_count.origem <> tb_ast_inventario_count.destino THEN 1
                ELSE NULL
            END = tb_stc_tipo_movimentacao_asset.codigo
        WHERE   (((tb_ast_inventario_count.codigo_inventario) = @codigo))

        UNION

        SELECT
            tb_cad_asset.asset_code,
            tb_cad_asset.descricao,
            tb_ast_inventario_movimentacao.origem,
            tb_ast_inventario_movimentacao.destino,
            FORMAT(tb_ast_inventario_movimentacao.data_movimentacao, 'dd/MM/yyyy HH:mm:ss') AS data,
            tb_cad_usuario.nome AS usuario,
            tb_stc_tipo_movimentacao_asset.descricao AS tipo_movimentacao,
            NULL,                                                   -- NOVO: ajuste manual não tem avaliação
            NULL                                                    -- NOVO
        FROM
            tb_ast_inventario_movimentacao INNER JOIN
            tb_cad_asset ON
            tb_ast_inventario_movimentacao.codigo_asset = tb_cad_asset.codigo AND
            tb_ast_inventario_movimentacao.codigo_empresa = tb_cad_asset.codigo_empresa LEFT JOIN
            tb_cad_usuario ON
            tb_ast_inventario_movimentacao.codigo_usuario = tb_cad_usuario.codigo AND
            tb_ast_inventario_movimentacao.codigo_empresa = tb_cad_usuario.codigo_empresa INNER JOIN
            tb_stc_tipo_movimentacao_asset ON
            tb_ast_inventario_movimentacao.codigo_tipo_movimentacao = tb_stc_tipo_movimentacao_asset.codigo
        WHERE   (((tb_ast_inventario_movimentacao.codigo_inventario) = @codigo))

        ORDER BY
            data

        SELECT * FROM tb_stc_grid_column WHERE (((codigo_grid) = 1002))
        SELECT * FROM tb_stc_grid_group  WHERE (((codigo_grid) = 1002))

    END
    ELSE IF @type = 1 BEGIN

        DECLARE @codigo_unidade AS int
        DECLARE @codigo_empresa AS int

        SELECT
            @codigo_empresa = codigo_empresa,
            @codigo_unidade = codigo_unidade
        FROM
            tb_ast_inventario
        WHERE   (((codigo) = @codigo))

        SELECT
            tb_cad_asset.asset_code,
            tb_cad_asset.descricao,
            tb_cad_asset.setor,
            tb_cad_asset.apartamento
        FROM
            tb_cad_asset LEFT JOIN
            tb_ast_inventario_count ON
            tb_cad_asset.codigo = tb_ast_inventario_count.codigo_asset AND
            tb_cad_asset.codigo_empresa = tb_ast_inventario_count.codigo_empresa AND
            tb_ast_inventario_count.codigo_inventario = @codigo LEFT JOIN
            tb_ast_inventario_movimentacao ON
            tb_cad_asset.codigo = tb_ast_inventario_movimentacao.codigo_asset AND
            tb_cad_asset.codigo_empresa = tb_ast_inventario_movimentacao.codigo_empresa AND
            tb_ast_inventario_movimentacao.codigo_inventario = @codigo
        WHERE   (((tb_cad_asset.codigo_empresa) = @codigo_empresa)
        AND     ((tb_cad_asset.codigo_unidade) = @codigo_unidade)
        AND     ((tb_cad_asset.codigo_status) = 1)
        AND     ((tb_ast_inventario_count.codigo_inventario) IS NULL)
        AND     ((tb_ast_inventario_movimentacao.codigo_inventario) IS NULL))
        ORDER BY
            tb_ast_inventario_count.data

        SELECT * FROM tb_stc_grid_column WHERE (((codigo_grid) = 1003))
        SELECT * FROM tb_stc_grid_group  WHERE (((codigo_grid) = 1003))

    END
    ELSE IF @type = 2 BEGIN

        SELECT
            tb_ast_inventario_count.asset_code,
            tb_ast_inventario_count.descricao,
            tb_ast_inventario_count.setor,
            tb_ast_inventario_count.apartamento,
            tb_cad_usuario.nome AS usuario,
            FORMAT(tb_ast_inventario_count.data, 'dd/MM/yyyy HH:mm:ss') AS data
        FROM
            tb_ast_inventario_count INNER JOIN
            tb_cad_usuario ON
            tb_ast_inventario_count.codigo_usuario = tb_cad_usuario.codigo AND
            tb_ast_inventario_count.codigo_empresa = tb_cad_usuario.codigo_empresa
        WHERE   (((tb_ast_inventario_count.codigo_inventario) = @codigo)
        AND     ((tb_ast_inventario_count.ativo_cadastrado) = 0))
        ORDER BY
            tb_ast_inventario_count.data

        SELECT * FROM tb_stc_grid_column WHERE (((codigo_grid) = 1004))
        SELECT * FROM tb_stc_grid_group  WHERE (((codigo_grid) = 1004))

    END

END
GO
