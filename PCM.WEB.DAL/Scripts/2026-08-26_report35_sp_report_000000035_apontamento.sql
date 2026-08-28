







--SELECT 240.0/260.0


--EXECUTE sp_report_000000035 905, 4, 4

-- =============================================
-- Author		: Kleber Leonardo Pinto
-- Create Date	: 26/02/2018
-- Description	: Seleciona dados da tb_aud_checklist
-- =============================================
CREATE PROCEDURE [dbo].[sp_report_000000035_apontamento]

@codigo_empresa							smallint,
@codigo_unidade							int,
@codigo_pcm_programada_ordem_servico	bigint

AS

BEGIN
	
	SELECT 
		ISNULL(tb_cad_funcionario.nome, tb_cad_fornecedor.nome_fantasia) AS executor,
		tb_pcm_apontamento.data_hora_inicio,
		tb_pcm_apontamento.data_hora_termino,
		DATEDIFF(MINUTE, tb_pcm_apontamento.data_hora_inicio, tb_pcm_apontamento.data_hora_termino) AS tempo_gasto,
		tb_pcm_apontamento.descricao_solucao
	FROM
		tb_pcm_apontamento LEFT JOIN
		tb_cad_funcionario ON
		tb_pcm_apontamento.codigo_empresa = tb_cad_funcionario.codigo_empresa AND
		tb_pcm_apontamento.codigo_funcionario = tb_cad_funcionario.codigo LEFT JOIN
		tb_cad_fornecedor ON
		tb_pcm_apontamento.codigo_empresa = tb_cad_fornecedor.codigo_empresa AND
		tb_pcm_apontamento.codigo_fornecedor = tb_cad_fornecedor.codigo
	WHERE	(((tb_pcm_apontamento.codigo_empresa) = @codigo_empresa)
	AND		((tb_pcm_apontamento.codigo_unidade) = @codigo_unidade)	
	AND		((tb_pcm_apontamento.codigo_pcm_programada_ordem_servico) = @codigo_pcm_programada_ordem_servico))
	ORDER BY
		tb_pcm_apontamento.data_hora_inicio

END

