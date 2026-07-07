/**********************************************************************************************
    Script      : Setor / Local - Checklist (Tudo em dia), Periodicidade e Intervalo
    Data        : 07/07/2026
    Descricao   : Adiciona a cada "local" do Setor (armazenado em tb_cad_apartamento) as
                  informacoes de checklist (tipo "Tudo em dia" = codigo_tipo_checklist 11),
                  periodicidade e intervalo, de forma semelhante ao TipoApartamento.

    Objetos afetados:
        - Tabela      : tb_cad_apartamento (3 colunas novas)
        - Procedure   : sp_insert_cadastro_basico_setor_local
        - Procedure   : sp_update_cadastro_basico_setor_local
        - Procedure   : sp_select_cadastro_basico_setor_local
**********************************************************************************************/

/*--------------------------------------------------------------------------------------------
    1) TABELA - Novas colunas em tb_cad_apartamento
--------------------------------------------------------------------------------------------*/
IF COL_LENGTH('dbo.tb_cad_apartamento', 'codigo_checklist') IS NULL
BEGIN
    ALTER TABLE [dbo].[tb_cad_apartamento] ADD [codigo_checklist] [bigint] NULL
END
GO

IF COL_LENGTH('dbo.tb_cad_apartamento', 'codigo_periodicidade') IS NULL
BEGIN
    ALTER TABLE [dbo].[tb_cad_apartamento] ADD [codigo_periodicidade] [smallint] NULL
END
GO

IF COL_LENGTH('dbo.tb_cad_apartamento', 'intervalo') IS NULL
BEGIN
    ALTER TABLE [dbo].[tb_cad_apartamento] ADD [intervalo] [smallint] NULL
END
GO

/*--------------------------------------------------------------------------------------------
    2) PROCEDURE - Insert do local do setor
--------------------------------------------------------------------------------------------*/
ALTER PROCEDURE [dbo].[sp_insert_cadastro_basico_setor_local]

@codigo_empresa			smallint,
@codigo_usuario			int,
@codigo_unidade			int,
@codigo_setor			int,
@local					varchar(100),
@codigo_checklist		bigint		= NULL,
@codigo_periodicidade	smallint	= NULL,
@intervalo				smallint	= NULL

AS

BEGIN

	DECLARE @codigo	AS int

	SET @codigo =	ISNULL((SELECT	MAX(codigo)
							FROM	tb_cad_apartamento
							WHERE	(((codigo_empresa) = @codigo_empresa))), 0) + 1

	INSERT INTO tb_cad_apartamento(
		codigo,
		codigo_empresa,
		codigo_unidade,
		codigo_setor,
		local,
		codigo_checklist,
		codigo_periodicidade,
		intervalo,
		ativo,
		codigo_usuario_input,
		data_input)
	VALUES(
		@codigo,
		@codigo_empresa,
		@codigo_unidade,
		@codigo_setor,
		@local,
		@codigo_checklist,
		@codigo_periodicidade,
		@intervalo,
		1,
		@codigo_usuario,
		GETDATE())

END
GO

/*--------------------------------------------------------------------------------------------
    3) PROCEDURE - Update do local do setor
--------------------------------------------------------------------------------------------*/
ALTER PROCEDURE [dbo].[sp_update_cadastro_basico_setor_local]

@codigo_empresa			smallint,
@codigo_usuario			int,
@codigo_unidade			int,
@codigo_setor			int,
@local					varchar(100),
@excluido				int,
@codigo					int,
@codigo_checklist		bigint		= NULL,
@codigo_periodicidade	smallint	= NULL,
@intervalo				smallint	= NULL

AS

BEGIN

	IF @excluido = 1 BEGIN

		UPDATE tb_cad_apartamento SET
			ativo = 0,
			codigo_usuario_update = @codigo_usuario,
			data_update = GETDATE()
		WHERE	(((codigo_empresa) = @codigo_empresa)
		AND		((codigo_unidade) = @codigo_unidade)
		AND		((codigo) = @codigo))

	END ELSE IF @codigo = 0 AND @excluido = 0 BEGIN

		SET @codigo =	ISNULL((SELECT	MAX(codigo)
								FROM	tb_cad_apartamento
								WHERE	(((codigo_empresa) = @codigo_empresa))), 0) + 1

		INSERT INTO tb_cad_apartamento(
			codigo,
			codigo_empresa,
			codigo_unidade,
			codigo_setor,
			local,
			codigo_checklist,
			codigo_periodicidade,
			intervalo,
			ativo,
			codigo_usuario_input,
			data_input)
		VALUES(
			@codigo,
			@codigo_empresa,
			@codigo_unidade,
			@codigo_setor,
			@local,
			@codigo_checklist,
			@codigo_periodicidade,
			@intervalo,
			1,
			@codigo_usuario,
			GETDATE())

	END ELSE BEGIN

		UPDATE tb_cad_apartamento SET
			local = @local,
			codigo_checklist = @codigo_checklist,
			codigo_periodicidade = @codigo_periodicidade,
			intervalo = @intervalo,
			codigo_usuario_update = @codigo_usuario,
			data_update = GETDATE()
		FROM
			tb_cad_apartamento
		WHERE	(((codigo_empresa) = @codigo_empresa)
		AND		((codigo_unidade) = @codigo_unidade)
		AND		((codigo) = @codigo))

	END

END
GO

/*--------------------------------------------------------------------------------------------
    4) PROCEDURE - Select dos locais do setor
--------------------------------------------------------------------------------------------*/
ALTER PROCEDURE [dbo].[sp_select_cadastro_basico_setor_local]

@codigo_empresa	smallint,
@codigo_unidade	int,
@codigo_setor	int

AS

BEGIN

	SELECT
		tb_cad_apartamento.local,
		tb_cad_apartamento.codigo,
		tb_cad_apartamento.codigo_checklist,
		tb_chk_checklist.descricao			AS checklist,
		tb_cad_apartamento.codigo_periodicidade,
		tb_stc_periodicidade.descricao		AS periodicidade,
		tb_cad_apartamento.intervalo
	FROM
		tb_cad_apartamento
		LEFT JOIN tb_chk_checklist ON
			tb_chk_checklist.codigo = tb_cad_apartamento.codigo_checklist AND
			tb_chk_checklist.codigo_empresa = tb_cad_apartamento.codigo_empresa
		LEFT JOIN tb_stc_periodicidade ON
			tb_stc_periodicidade.codigo = tb_cad_apartamento.codigo_periodicidade
	WHERE	(((tb_cad_apartamento.codigo_empresa) = @codigo_empresa)
	AND		((tb_cad_apartamento.codigo_unidade) = @codigo_unidade)
	AND		((tb_cad_apartamento.codigo_setor) = @codigo_setor)
	AND		((tb_cad_apartamento.ativo) = 1)
	AND		((tb_cad_apartamento.local) IS NOT NULL))
	ORDER BY
		tb_cad_apartamento.local

END
GO
