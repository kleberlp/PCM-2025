USE [PCM_dev]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author		: Kleber Leonardo Pinto
-- Create Date	: 26/02/2018
-- Description	: Seleciona dados da tb_aud_checklist
-- =============================================
-- Extraído de PCM_dev via OBJECT_DEFINITION() em 2026-08-25 para servir de referência
-- ao novo Report35Controller (mvc/Controllers/Report35Controller.cs). Lógica original
-- preservada; não editar este arquivo para alterar o comportamento do relatório.
--
--EXECUTE sp_report_000000035 907, 4, 4
CREATE PROCEDURE [dbo].[sp_report_000000035]

@codigo_empresa							smallint,
@codigo_unidade							int,
@codigo_pcm_programada_ordem_servico	bigint

AS

BEGIN

	SELECT DISTINCT
		tb_stc_tipo_ordem_servico.descricao AS tipo_ordem_servico,
		ISNULL(ISNULL(tb_cad_equipamento.tag, ' - ') + tb_cad_equipamento.descricao, 'N/A') AS equipamento,
		tb_pcm_programada.descricao AS programada,
		tb_cad_categoria.descricao AS categoria,
		tb_stc_tipo_servico.descricao AS tipo_servico,
		tb_pcm_programada_ordem_servico.descricao_solucao,
		tb_pcm_programada_ordem_servico.codigo_pcm_programada,
		FORMAT(tb_pcm_programada_ordem_servico.data, 'dd/MM/yyyy') AS data,
		tb_pcm_programada_ordem_servico_checklist.codigo_tipo_item_checklist,
		tb_pcm_programada_ordem_servico_checklist.codigo_empresa,
		tb_pcm_programada_ordem_servico_checklist.codigo_unidade,
		tb_pcm_programada_ordem_servico_checklist.codigo_pcm_programada_ordem_servico,
		tb_pcm_programada_ordem_servico_checklist.codigo AS codigo_item_checklist,
		tb_cad_unidade.nome_fantasia AS unidade,
		tb_pcm_programada_ordem_servico_checklist.grupo AS grupo_checklist,
		tb_pcm_programada_ordem_servico_checklist.subgrupo AS sub_grupo_checklist,
		tb_pcm_programada_ordem_servico_checklist.checklist AS codigo_checklist,
		tb_pcm_programada_ordem_servico_checklist.descricao,
		tb_pcm_programada_ordem_servico_checklist.resultado,
		tb_pcm_programada_ordem_servico_checklist.observacao,
		1 AS quantidade_foto,
		0 AS ok,
		0 AS nok,
		tb_subgrupo.media AS media_subgrupo,
		tb_pcm_programada_ordem_servico_checklist.peso,
		tb_grupo.media AS media_grupo
	FROM
		tb_pcm_programada_ordem_servico INNER JOIN
		tb_pcm_programada_ordem_servico_checklist ON
		tb_pcm_programada_ordem_servico.codigo = tb_pcm_programada_ordem_servico_checklist.codigo_pcm_programada_ordem_servico AND
		tb_pcm_programada_ordem_servico.codigo_empresa = tb_pcm_programada_ordem_servico_checklist.codigo_empresa AND
		tb_pcm_programada_ordem_servico.codigo_unidade = tb_pcm_programada_ordem_servico_checklist.codigo_unidade INNER JOIN
		tb_cad_unidade ON
		tb_pcm_programada_ordem_servico_checklist.codigo_unidade = tb_cad_unidade.codigo AND
		tb_pcm_programada_ordem_servico_checklist.codigo_empresa = tb_cad_unidade.codigo_empresa LEFT JOIN
		(SELECT
			grupo,
			subgrupo,
			CONVERT(numeric(15,2),
				CASE
					WHEN ISNULL(SUM(CASE WHEN codigo_tipo_item_checklist IN (1, 8) AND (ISNULL(tb_pcm_programada_ordem_servico_checklist.resultado, '') = 'SIM' OR ISNULL(tb_pcm_programada_ordem_servico_checklist.resultado, '') = 'N/A') THEN tb_pcm_programada_ordem_servico_checklist.peso ELSE 0 END), 0) = 0 THEN 0
					ELSE CONVERT(float, SUM(CASE WHEN codigo_tipo_item_checklist IN (1, 8) AND (ISNULL(tb_pcm_programada_ordem_servico_checklist.resultado, '') = 'SIM' OR ISNULL(tb_pcm_programada_ordem_servico_checklist.resultado, '') = 'N/A') THEN tb_pcm_programada_ordem_servico_checklist.peso ELSE 0 END)) /
						 CONVERT(float, SUM(CASE WHEN codigo_tipo_item_checklist IN (1, 8) THEN tb_pcm_programada_ordem_servico_checklist.peso ELSE 0 END))
				END * 100.0) AS media
		 FROM
			tb_pcm_programada_ordem_servico_checklist
		 WHERE	(((codigo_empresa) = @codigo_empresa)
		 AND	((codigo_unidade) = @codigo_unidade)
		 AND	((codigo_tipo_item_checklist) IN (1, 2, 8))
		 AND	((codigo_pcm_programada_ordem_servico) = @codigo_pcm_programada_ordem_servico))
		 GROUP BY
			grupo,
			subgrupo) tb_subgrupo ON
		ISNULL(tb_pcm_programada_ordem_servico_checklist.grupo, '') = ISNULL(tb_subgrupo.grupo, '') AND
		ISNULL(tb_pcm_programada_ordem_servico_checklist.subgrupo, '') = ISNULL(tb_subgrupo.subgrupo, '') LEFT JOIN
		(SELECT
			grupo,
			CONVERT(numeric(15,2),
				CASE
					WHEN ISNULL(SUM(CASE WHEN codigo_tipo_item_checklist IN (1, 8) AND (ISNULL(tb_pcm_programada_ordem_servico_checklist.resultado, '') = 'SIM' OR ISNULL(tb_pcm_programada_ordem_servico_checklist.resultado, '') = 'N/A') THEN tb_pcm_programada_ordem_servico_checklist.peso ELSE 0 END), 0) = 0 THEN 0
					ELSE CONVERT(float, SUM(CASE WHEN codigo_tipo_item_checklist IN (1, 8) AND (ISNULL(tb_pcm_programada_ordem_servico_checklist.resultado, '') = 'SIM' OR ISNULL(tb_pcm_programada_ordem_servico_checklist.resultado, '') = 'N/A') THEN tb_pcm_programada_ordem_servico_checklist.peso ELSE 0 END)) /
						 CONVERT(float, SUM(CASE WHEN codigo_tipo_item_checklist IN (1, 8) THEN tb_pcm_programada_ordem_servico_checklist.peso ELSE 0 END))
				END * 100.0) AS media
		 FROM
			tb_pcm_programada_ordem_servico_checklist
		 WHERE	(((codigo_empresa) = @codigo_empresa)
		 AND	((codigo_unidade) = @codigo_unidade)
		 AND	((codigo_tipo_item_checklist) IN (1, 8))
		 AND	((codigo_pcm_programada_ordem_servico) = @codigo_pcm_programada_ordem_servico))
		 GROUP BY
			grupo) tb_grupo ON
		ISNULL(tb_pcm_programada_ordem_servico_checklist.grupo, '') = ISNULL(tb_grupo.grupo, '') INNER JOIN
		tb_pcm_programada ON
		tb_pcm_programada_ordem_servico.codigo_pcm_programada = tb_pcm_programada.codigo AND
		tb_pcm_programada_ordem_servico.codigo_empresa = tb_pcm_programada.codigo_empresa AND
		tb_pcm_programada_ordem_servico.codigo_unidade = tb_pcm_programada.codigo_unidade INNER JOIN
		tb_stc_tipo_ordem_servico ON
		tb_pcm_programada.codigo_tipo_ordem_servico = tb_stc_tipo_ordem_servico.codigo AND
		tb_pcm_programada.codigo_empresa = tb_stc_tipo_ordem_servico.codigo_empresa INNER JOIN
		tb_cad_categoria ON
		tb_pcm_programada.codigo_categoria = tb_cad_categoria.codigo AND
		tb_pcm_programada.codigo_empresa = tb_cad_categoria.codigo_empresa AND
		tb_pcm_programada.codigo_unidade = tb_cad_categoria.codigo_unidade INNER JOIN
		tb_stc_tipo_servico ON
		tb_pcm_programada.codigo_tipo_servico = tb_stc_tipo_servico.codigo LEFT JOIN
		tb_cad_equipamento ON
		tb_pcm_programada.codigo_equipamento = tb_cad_equipamento.codigo AND
		tb_pcm_programada.codigo_empresa = tb_cad_equipamento.codigo_empresa
	WHERE	(((tb_pcm_programada_ordem_servico_checklist.codigo_empresa) = @codigo_empresa)
	AND		((tb_pcm_programada_ordem_servico_checklist.codigo_unidade) = @codigo_unidade)
	AND		((tb_pcm_programada_ordem_servico_checklist.codigo_pcm_programada_ordem_servico) = @codigo_pcm_programada_ordem_servico))
	GROUP BY
		tb_stc_tipo_ordem_servico.descricao,
		tb_pcm_programada.descricao,
		tb_pcm_programada_ordem_servico.data,
		ISNULL(ISNULL(tb_cad_equipamento.tag, ' - ') + tb_cad_equipamento.descricao, 'N/A'),
		tb_cad_categoria.descricao,
		tb_stc_tipo_servico.descricao,
		tb_pcm_programada_ordem_servico.descricao_solucao,
		tb_pcm_programada_ordem_servico.codigo_pcm_programada,
		tb_pcm_programada_ordem_servico_checklist.codigo_tipo_item_checklist,
		tb_pcm_programada_ordem_servico_checklist.codigo_empresa,
		tb_pcm_programada_ordem_servico_checklist.codigo_unidade,
		tb_pcm_programada_ordem_servico_checklist.codigo_pcm_programada_ordem_servico,
		tb_pcm_programada_ordem_servico_checklist.codigo,
		tb_cad_unidade.nome_fantasia,
		tb_pcm_programada_ordem_servico_checklist.grupo,
		tb_pcm_programada_ordem_servico_checklist.subgrupo,
		tb_pcm_programada_ordem_servico_checklist.checklist,
		tb_pcm_programada_ordem_servico_checklist.descricao,
		tb_pcm_programada_ordem_servico_checklist.resultado,
		tb_pcm_programada_ordem_servico_checklist.observacao,
		tb_grupo.media,
		tb_subgrupo.media,
		tb_pcm_programada_ordem_servico_checklist.peso
	ORDER BY
		tb_pcm_programada_ordem_servico_checklist.grupo,
		tb_pcm_programada_ordem_servico_checklist.subgrupo,
		tb_pcm_programada_ordem_servico_checklist.checklist

END
GO

-- NOTA (2026-08-25): "media_grupo"/"media_subgrupo" dependem da coluna "peso", que não é
-- preenchida para checklists do tipo ROTINA/PREVENTIVA (fica NULL em todas as linhas) — nesse
-- caso a fórmula cai no fallback "= 0" e a média SEMPRE retorna 0, por isso o relatório Crystal
-- original nunca exibia esse número. O Report35Controller ignora esses dois campos e calcula a
-- conformidade por grupo/subgrupo diretamente a partir de resultado='SIM' nos tipos 1 e 8.
