/**********************************************************************************************
    Script      : Governança - Tabela-fato de Ocupação (PMS) para "Aptos Totais"
    Data        : 08/07/2026
    Descricao   : Cria a tabela-fato diária de ocupação (aptos a arrumar) alimentada pela
                  importação de reservas do PMS, e passa o "Aptos Totais PMS" (AD07 / rankings
                  de Governança) a somar essa fato no mês de referência.

    Contexto da importação (PCM.INTERFACE.DAL/InterfaceApiOracle.vb):
        1) TRUNCATE tb_interface_uh_reservas_stg
        2) BULK INSERT das reservas IN-HOUSE HOJE do hotel (Oracle: SYSDATE BETWEEN
           chegada e partida) -> 1 linha por UH ocupada hoje (uh, data_chegada, data_saida)
        3) EXEC sp_update_interface_uh_reservas_stg (@codigo_empresa, @hotel_id)

    Como o snapshot só contém as UHs ocupadas HOJE, só é possível calcular com precisão a
    ocupação do DIA da importação. Por isso a fato acumula 1 linha por dia/unidade a cada
    importação diária; o total mensal é a soma dos dias.

    "Aptos a arrumar" no dia D:
        - Saídas       : data_saida = D            (limpeza de saída / check-out)
        - Permanências : data_chegada < D < data_saida (arrumação diária / stayover)
        (o dia de chegada não conta: a UH foi arrumada na saída do hóspede anterior)

    Objetos:
        - Tabela (nova)   : tb_fat_governanca_ocupacao_dia
        - Procedure (nova): sp_gerar_governanca_ocupacao_dia
        - Procedure (ALTER): sp_update_interface_uh_reservas_stg  (adiciona o hook no fim)
        - Procedure (subst): sp_select_configuracao_governanca_meta_unidade (usa a fato)
**********************************************************************************************/

/*--------------------------------------------------------------------------------------------
    1) TABELA-FATO
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.tb_fat_governanca_ocupacao_dia', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[tb_fat_governanca_ocupacao_dia](
        [codigo_empresa]    [smallint]  NOT NULL,
        [codigo_unidade]    [int]       NOT NULL,
        [data]              [date]      NOT NULL,
        [qtde_saida]        [int]       NOT NULL,
        [qtde_permanencia]  [int]       NOT NULL,
        [qtde_total]        [int]       NOT NULL,
        [data_atualizacao]  [datetime]  NULL,
     CONSTRAINT [PK_tb_fat_governanca_ocupacao_dia] PRIMARY KEY CLUSTERED
        ([codigo_empresa] ASC, [codigo_unidade] ASC, [data] ASC)
    ) ON [PRIMARY]
END
GO

/*--------------------------------------------------------------------------------------------
    2) GERA/ATUALIZA a ocupação de um dia a partir do staging de reservas (snapshot atual)
       @data default = hoje (único dia confiável no snapshot in-house)
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_gerar_governanca_ocupacao_dia', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_gerar_governanca_ocupacao_dia]
GO

CREATE PROCEDURE [dbo].[sp_gerar_governanca_ocupacao_dia]
@codigo_empresa smallint,
@codigo_unidade int,
@hotel_id       varchar(20),
@data           date = NULL
AS
BEGIN

    SET NOCOUNT ON;

    IF @data IS NULL SET @data = CAST(GETDATE() AS date);

    DECLARE @saida int, @perm int;

    SELECT
        @saida = SUM(CASE WHEN data_saida = @data THEN 1 ELSE 0 END),
        @perm  = SUM(CASE WHEN data_chegada < @data AND data_saida > @data THEN 1 ELSE 0 END)
    FROM tb_interface_uh_reservas_stg
    WHERE codigo_empresa = @codigo_empresa
    AND   hotel_id = @hotel_id;

    SET @saida = ISNULL(@saida, 0);
    SET @perm  = ISNULL(@perm, 0);

    IF EXISTS (SELECT 1 FROM tb_fat_governanca_ocupacao_dia
               WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade AND data = @data)
    BEGIN
        UPDATE tb_fat_governanca_ocupacao_dia SET
            qtde_saida       = @saida,
            qtde_permanencia = @perm,
            qtde_total       = @saida + @perm,
            data_atualizacao = GETDATE()
        WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade AND data = @data
    END
    ELSE
    BEGIN
        INSERT INTO tb_fat_governanca_ocupacao_dia(
            codigo_empresa, codigo_unidade, data, qtde_saida, qtde_permanencia, qtde_total, data_atualizacao)
        VALUES(
            @codigo_empresa, @codigo_unidade, @data, @saida, @perm, @saida + @perm, GETDATE())
    END

END
GO

/*--------------------------------------------------------------------------------------------
    3) HOOK: recria sp_update_interface_uh_reservas_stg preservando o corpo e chamando a
       geração da ocupação do dia no final.
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_update_interface_uh_reservas_stg', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_update_interface_uh_reservas_stg]
GO

CREATE PROCEDURE [dbo].[sp_update_interface_uh_reservas_stg]

@codigo_empresa	smallint,
@hotel_id		varchar(20)

AS

BEGIN

	DECLARE @codigo_unidade	AS int

	SET @codigo_unidade =  (SELECT	codigo
							FROM	tb_cad_unidade
							WHERE	(((codigo_empresa) = @codigo_empresa)
							AND		((hotel_opera) = @hotel_id)))

	UPDATE tb_interface_uh_reservas_stg SET
		uh = TRIM(uh)

	UPDATE tb_cad_apartamento SET
		data_chegada = tb_interface_uh_reservas_stg.data_chegada,
		data_saida = tb_interface_uh_reservas_stg.data_saida,
		data_planejamento = GETDATE()
	FROM
		tb_cad_apartamento INNER JOIN
		tb_interface_uh_reservas_stg ON
		tb_cad_apartamento.codigo_empresa = tb_interface_uh_reservas_stg.codigo_empresa AND
		@hotel_id = tb_interface_uh_reservas_stg.hotel_id AND
		tb_cad_apartamento.apartamento = tb_interface_uh_reservas_stg.uh
	WHERE	(((tb_cad_apartamento.codigo_empresa) = @codigo_empresa)
	AND		((tb_cad_apartamento.codigo_unidade) = @codigo_unidade))

	-- Alimenta a ocupação do dia (aptos a arrumar) para o "Aptos Totais PMS" da governança
	IF @codigo_unidade IS NOT NULL
		EXEC sp_gerar_governanca_ocupacao_dia @codigo_empresa = @codigo_empresa,
											  @codigo_unidade = @codigo_unidade,
											  @hotel_id = @hotel_id

END
GO

/*--------------------------------------------------------------------------------------------
    4) SELECT de metas por unidade: "Aptos Totais" passa a somar a fato de ocupação no mês.
       Fallback (enquanto a fato não tiver dados no mês): contagem de apartamentos distintos
       com apontamento de governança (proxy anterior).
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_select_configuracao_governanca_meta_unidade', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_configuracao_governanca_meta_unidade]
GO

CREATE PROCEDURE [dbo].[sp_select_configuracao_governanca_meta_unidade]
@codigo_empresa smallint,
@mes            int,
@ano            int
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        u.codigo                                        AS codigo_unidade,
        ISNULL(u.nome_fantasia, '')                     AS nome_unidade,
        ISNULL(
            COALESCE(
                NULLIF((SELECT SUM(f.qtde_total)
                        FROM   tb_fat_governanca_ocupacao_dia f
                        WHERE  f.codigo_empresa = u.codigo_empresa
                        AND    f.codigo_unidade = u.codigo
                        AND    MONTH(f.data) = @mes
                        AND    YEAR(f.data)  = @ano), 0),
                NULLIF((SELECT COUNT(DISTINCT g.codigo_apartamento)
                        FROM   tb_gov_apontamento g
                        WHERE  g.codigo_empresa = u.codigo_empresa
                        AND    g.codigo_unidade = u.codigo
                        AND    MONTH(g.data) = @mes
                        AND    YEAR(g.data)  = @ano), 0)
            ), 0)                                        AS aptos_totais_pms,
        m.pct_meta_custom,
        m.meta_aptos_dia_camareira_custom,
        m.meta_pct_vistoria_custom
    FROM
        tb_cad_unidade u
        LEFT JOIN tb_cfg_governanca_meta_unidade m ON
            m.codigo_empresa = u.codigo_empresa AND
            m.codigo_unidade = u.codigo
    WHERE   u.codigo_empresa = @codigo_empresa
    AND     u.ativo = 1
    ORDER BY
        u.nome_fantasia

END
GO
