



--EXECUTE sp_report_000000035_picture 905, 4, 4, 'ROTINA'



-- =============================================
-- Author		: Kleber Leonardo Pinto
-- Create Date	: 26/02/2018
-- Description	: Seleciona dados da tb_aud_checklist
-- =============================================
CREATE PROCEDURE [dbo].[sp_report_000000035_picture]

@codigo_empresa			smallint,
@codigo_unidade			int,
@codigo					bigint,
@codigo_item_checklist	int = -1,
@tipo					varchar(20)

AS

BEGIN
	
	SELECT
		imagem AS foto,
		'' AS observacao,
		CONVERT(varchar(MAX), CONCAT(CONVERT(varchar(MAX), tb_pcm_programada_ordem_servico_checklist.checklist), ' - ', CONVERT(varchar(MAX), tb_pcm_programada_ordem_servico_checklist.descricao))) AS checklist
	FROM
		tb_pcm_picture INNER JOIN
		tb_pcm_programada_ordem_servico_checklist ON
		tb_pcm_picture.codigo_documento = tb_pcm_programada_ordem_servico_checklist.codigo_pcm_programada_ordem_servico AND
		tb_pcm_picture.codigo_empresa = tb_pcm_programada_ordem_servico_checklist.codigo_empresa AND
		tb_pcm_picture.codigo_unidade = tb_pcm_programada_ordem_servico_checklist.codigo_unidade AND
		tb_pcm_picture.codigo_item_checklist = tb_pcm_programada_ordem_servico_checklist.codigo
	WHERE	(((tb_pcm_picture.codigo_empresa) = @codigo_empresa)
	AND		((tb_pcm_picture.codigo_unidade) = @codigo_unidade)
	AND		((tb_pcm_picture.codigo_documento) = @codigo)
	AND		(((tb_pcm_picture.codigo_item_checklist) = @codigo_item_checklist) OR ((@codigo_item_checklist) = -1))
	AND		((ISNULL(tb_pcm_picture.ativo, 1)) = 1)
	AND		((tb_pcm_programada_ordem_servico_checklist.codigo_pcm_programada_ordem_servico) = @codigo)
	AND		((tb_pcm_programada_ordem_servico_checklist.codigo_empresa) = @codigo_empresa)
	AND		((tb_pcm_programada_ordem_servico_checklist.codigo_unidade) = @codigo_unidade))

END

