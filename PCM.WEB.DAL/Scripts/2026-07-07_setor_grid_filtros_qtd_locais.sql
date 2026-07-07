/**********************************************************************************************
    Script      : Setor - Grid dinamico (loadGridMain) com filtros e Qtd. Locais
    Data        : 07/07/2026
    Descricao   : Procedure para alimentar o grid dinamico (LoadDynamicGrid) da tela
                  SetorIndex. Retorna 3 result sets: dados, colunas e groupBy.
                  Filtros por unidade e descricao. Coluna "Qtd. Locais" = quantidade
                  de registros em tb_cad_apartamento do setor com local <> NULL e ativo = 1.

    Objetos afetados:
        - Procedure (nova) : sp_select_cadastro_basico_setor_grid
**********************************************************************************************/

IF OBJECT_ID('dbo.sp_select_cadastro_basico_setor_grid', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_cadastro_basico_setor_grid]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author      : Kleber Leonardo Pinto
-- Create Date : 07/07/2026
-- Description : Grid dinamico de Setor (filtros + Qtd. Locais)
-- =============================================
CREATE PROCEDURE [dbo].[sp_select_cadastro_basico_setor_grid]
@codigo_empresa	smallint,
@codigo_unidade	int	= -1,
@descricao		varchar(255) = ''
AS
BEGIN

	SET NOCOUNT ON;

	-- ============================
	-- 1) DADOS
	-- ============================
	SELECT
		tb_cad_setor.codigo,
		tb_cad_setor.codigo_unidade,
		ISNULL(tb_cad_unidade.nome_fantasia, '')	AS unidade,
		ISNULL(tb_cad_setor.descricao, '')			AS descricao,
		(SELECT COUNT(*)
		 FROM	tb_cad_apartamento
		 WHERE	tb_cad_apartamento.codigo_empresa = tb_cad_setor.codigo_empresa
		 AND	tb_cad_apartamento.codigo_unidade = tb_cad_setor.codigo_unidade
		 AND	tb_cad_apartamento.codigo_setor   = tb_cad_setor.codigo
		 AND	tb_cad_apartamento.local IS NOT NULL
		 AND	tb_cad_apartamento.ativo = 1)		AS qtd_locais,
		CASE WHEN tb_cad_setor.ativo = 1 THEN 'Sim' ELSE 'Não' END AS ativo
	FROM
		tb_cad_setor INNER JOIN
		tb_cad_unidade ON
		tb_cad_setor.codigo_unidade = tb_cad_unidade.codigo AND
		tb_cad_setor.codigo_empresa = tb_cad_unidade.codigo_empresa
	WHERE	(((tb_cad_setor.codigo_empresa) = @codigo_empresa)
	AND		(((tb_cad_setor.codigo_unidade) = @codigo_unidade) OR ((@codigo_unidade) <= 0))
	AND		((@descricao = '') OR (tb_cad_setor.descricao LIKE '%' + @descricao + '%')))
	ORDER BY
		tb_cad_setor.descricao

	-- ============================
	-- 2) COLUNAS
	-- ============================
	SELECT
		'unidade'	AS Data,
		'Unidade'	AS Title,
		CAST(1 AS bit)	AS Visible,
		CAST(1 AS bit)	AS Orderable,
		'left'		AS Align
	UNION ALL
	SELECT 'descricao',		'Descrição',	CAST(1 AS bit), CAST(1 AS bit), 'left'
	UNION ALL
	SELECT 'qtd_locais',	'Qtd. Locais',	CAST(1 AS bit), CAST(1 AS bit), 'center'
	UNION ALL
	SELECT 'ativo',			'Ativo',		CAST(1 AS bit), CAST(1 AS bit), 'center';

	-- ============================
	-- 3) GROUP BY (vazio)
	-- ============================
	SELECT
		'unidade'	AS ColumnName,
		0			AS Level,
		CAST(1 AS bit)	AS Collapsible,
		CAST(1 AS bit)	AS ShowCount,
		''			AS CssClass
	WHERE	(((1) <> 1))

END
GO
