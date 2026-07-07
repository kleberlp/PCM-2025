/**********************************************************************************************
    Script      : Governança - Metas e Parâmetros (AD07)
    Data        : 07/07/2026
    Descricao   : Persistência dos parâmetros gerais de governança (por empresa) e das metas
                  por unidade (overrides). Alimenta a tela AD07-MetasParametrosGovernanca e os
                  rankings das telas de Desempenho de Governança (D03).

    Objetos (novos):
        - Tabela    : tb_cfg_governanca_parametros
        - Tabela    : tb_cfg_governanca_meta_unidade
        - Procedure : sp_select_configuracao_governanca_parametros
        - Procedure : sp_update_configuracao_governanca_parametros
        - Procedure : sp_update_configuracao_governanca_meta_unidade
    (o sp_select_configuracao_governanca_meta_unidade fica no script _metas_unidade_pms.sql,
     pois depende da fonte PMS de "Aptos Totais")
**********************************************************************************************/

/*--------------------------------------------------------------------------------------------
    1) TABELAS
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.tb_cfg_governanca_parametros', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[tb_cfg_governanca_parametros](
        [codigo_empresa]                    [smallint]      NOT NULL,
        [pct_meta_atendimento]              [decimal](5,2)  NULL,
        [meta_aptos_dia_camareira]          [int]           NULL,
        [meta_pct_vistoria]                 [decimal](5,2)  NULL,
        [peso_nc_unidades]                  [decimal](5,2)  NULL,
        [peso_vistoria_unidades]            [decimal](5,2)  NULL,
        [peso_produtividade_unidades]       [decimal](5,2)  NULL,
        [peso_retrabalho_unidades]          [decimal](5,2)  NULL,
        [qtd_minima_elegivel_unidades]      [int]           NULL,
        [peso_produtividade_individual]     [decimal](5,2)  NULL,
        [peso_nc_individual]                [decimal](5,2)  NULL,
        [peso_retrabalho_individual]        [decimal](5,2)  NULL,
        [qtd_minima_elegivel_individual]    [int]           NULL,
        [pct_vistoriados_estimado]          [decimal](5,2)  NULL,
        [codigo_usuario_update]             [int]           NULL,
        [data_update]                       [datetime]      NULL,
     CONSTRAINT [PK_tb_cfg_governanca_parametros] PRIMARY KEY CLUSTERED ([codigo_empresa] ASC)
    ) ON [PRIMARY]
END
GO

IF OBJECT_ID('dbo.tb_cfg_governanca_meta_unidade', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[tb_cfg_governanca_meta_unidade](
        [codigo_empresa]                    [smallint]      NOT NULL,
        [codigo_unidade]                    [int]           NOT NULL,
        [pct_meta_custom]                   [decimal](5,2)  NULL,
        [meta_aptos_dia_camareira_custom]   [int]           NULL,
        [meta_pct_vistoria_custom]          [decimal](5,2)  NULL,
        [codigo_usuario_update]             [int]           NULL,
        [data_update]                       [datetime]      NULL,
     CONSTRAINT [PK_tb_cfg_governanca_meta_unidade] PRIMARY KEY CLUSTERED ([codigo_empresa] ASC, [codigo_unidade] ASC)
    ) ON [PRIMARY]
END
GO

/*--------------------------------------------------------------------------------------------
    2) SELECT PARÂMETROS GERAIS (retorna defaults quando ainda não há registro)
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_select_configuracao_governanca_parametros', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_configuracao_governanca_parametros]
GO

CREATE PROCEDURE [dbo].[sp_select_configuracao_governanca_parametros]
@codigo_empresa smallint
AS
BEGIN

    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM tb_cfg_governanca_parametros WHERE codigo_empresa = @codigo_empresa)
    BEGIN
        SELECT
            ISNULL(pct_meta_atendimento, 90)            AS pct_meta_atendimento,
            ISNULL(meta_aptos_dia_camareira, 18)        AS meta_aptos_dia_camareira,
            ISNULL(meta_pct_vistoria, 95)               AS meta_pct_vistoria,
            ISNULL(peso_nc_unidades, 30)                AS peso_nc_unidades,
            ISNULL(peso_vistoria_unidades, 30)          AS peso_vistoria_unidades,
            ISNULL(peso_produtividade_unidades, 30)     AS peso_produtividade_unidades,
            ISNULL(peso_retrabalho_unidades, 10)        AS peso_retrabalho_unidades,
            ISNULL(qtd_minima_elegivel_unidades, 100)   AS qtd_minima_elegivel_unidades,
            ISNULL(peso_produtividade_individual, 60)   AS peso_produtividade_individual,
            ISNULL(peso_nc_individual, 30)              AS peso_nc_individual,
            ISNULL(peso_retrabalho_individual, 10)      AS peso_retrabalho_individual,
            ISNULL(qtd_minima_elegivel_individual, 20)  AS qtd_minima_elegivel_individual,
            ISNULL(pct_vistoriados_estimado, 80)        AS pct_vistoriados_estimado
        FROM tb_cfg_governanca_parametros
        WHERE codigo_empresa = @codigo_empresa
    END
    ELSE
    BEGIN
        SELECT
            CAST(90 AS decimal(5,2))  AS pct_meta_atendimento,
            18                        AS meta_aptos_dia_camareira,
            CAST(95 AS decimal(5,2))  AS meta_pct_vistoria,
            CAST(30 AS decimal(5,2))  AS peso_nc_unidades,
            CAST(30 AS decimal(5,2))  AS peso_vistoria_unidades,
            CAST(30 AS decimal(5,2))  AS peso_produtividade_unidades,
            CAST(10 AS decimal(5,2))  AS peso_retrabalho_unidades,
            100                       AS qtd_minima_elegivel_unidades,
            CAST(60 AS decimal(5,2))  AS peso_produtividade_individual,
            CAST(30 AS decimal(5,2))  AS peso_nc_individual,
            CAST(10 AS decimal(5,2))  AS peso_retrabalho_individual,
            20                        AS qtd_minima_elegivel_individual,
            CAST(80 AS decimal(5,2))  AS pct_vistoriados_estimado
    END

END
GO

/*--------------------------------------------------------------------------------------------
    3) UPSERT PARÂMETROS GERAIS
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_update_configuracao_governanca_parametros', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_update_configuracao_governanca_parametros]
GO

CREATE PROCEDURE [dbo].[sp_update_configuracao_governanca_parametros]
@codigo_empresa                     smallint,
@codigo_usuario                     int,
@pct_meta_atendimento               decimal(5,2),
@meta_aptos_dia_camareira           int,
@meta_pct_vistoria                  decimal(5,2),
@peso_nc_unidades                   decimal(5,2),
@peso_vistoria_unidades             decimal(5,2),
@peso_produtividade_unidades        decimal(5,2),
@peso_retrabalho_unidades           decimal(5,2),
@qtd_minima_elegivel_unidades       int,
@peso_produtividade_individual      decimal(5,2),
@peso_nc_individual                 decimal(5,2),
@peso_retrabalho_individual         decimal(5,2),
@qtd_minima_elegivel_individual     int,
@pct_vistoriados_estimado           decimal(5,2)
AS
BEGIN

    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM tb_cfg_governanca_parametros WHERE codigo_empresa = @codigo_empresa)
    BEGIN
        UPDATE tb_cfg_governanca_parametros SET
            pct_meta_atendimento            = @pct_meta_atendimento,
            meta_aptos_dia_camareira        = @meta_aptos_dia_camareira,
            meta_pct_vistoria               = @meta_pct_vistoria,
            peso_nc_unidades                = @peso_nc_unidades,
            peso_vistoria_unidades          = @peso_vistoria_unidades,
            peso_produtividade_unidades     = @peso_produtividade_unidades,
            peso_retrabalho_unidades        = @peso_retrabalho_unidades,
            qtd_minima_elegivel_unidades    = @qtd_minima_elegivel_unidades,
            peso_produtividade_individual   = @peso_produtividade_individual,
            peso_nc_individual              = @peso_nc_individual,
            peso_retrabalho_individual      = @peso_retrabalho_individual,
            qtd_minima_elegivel_individual  = @qtd_minima_elegivel_individual,
            pct_vistoriados_estimado        = @pct_vistoriados_estimado,
            codigo_usuario_update           = @codigo_usuario,
            data_update                     = GETDATE()
        WHERE codigo_empresa = @codigo_empresa
    END
    ELSE
    BEGIN
        INSERT INTO tb_cfg_governanca_parametros(
            codigo_empresa, pct_meta_atendimento, meta_aptos_dia_camareira, meta_pct_vistoria,
            peso_nc_unidades, peso_vistoria_unidades, peso_produtividade_unidades, peso_retrabalho_unidades,
            qtd_minima_elegivel_unidades, peso_produtividade_individual, peso_nc_individual,
            peso_retrabalho_individual, qtd_minima_elegivel_individual, pct_vistoriados_estimado,
            codigo_usuario_update, data_update)
        VALUES(
            @codigo_empresa, @pct_meta_atendimento, @meta_aptos_dia_camareira, @meta_pct_vistoria,
            @peso_nc_unidades, @peso_vistoria_unidades, @peso_produtividade_unidades, @peso_retrabalho_unidades,
            @qtd_minima_elegivel_unidades, @peso_produtividade_individual, @peso_nc_individual,
            @peso_retrabalho_individual, @qtd_minima_elegivel_individual, @pct_vistoriados_estimado,
            @codigo_usuario, GETDATE())
    END

END
GO

/*--------------------------------------------------------------------------------------------
    4) UPSERT META POR UNIDADE (override). Campos custom podem ser NULL (= usa padrão).
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_update_configuracao_governanca_meta_unidade', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_update_configuracao_governanca_meta_unidade]
GO

CREATE PROCEDURE [dbo].[sp_update_configuracao_governanca_meta_unidade]
@codigo_empresa                     smallint,
@codigo_unidade                     int,
@codigo_usuario                     int,
@pct_meta_custom                    decimal(5,2) = NULL,
@meta_aptos_dia_camareira_custom    int          = NULL,
@meta_pct_vistoria_custom           decimal(5,2) = NULL
AS
BEGIN

    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM tb_cfg_governanca_meta_unidade WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade)
    BEGIN
        UPDATE tb_cfg_governanca_meta_unidade SET
            pct_meta_custom                 = @pct_meta_custom,
            meta_aptos_dia_camareira_custom = @meta_aptos_dia_camareira_custom,
            meta_pct_vistoria_custom        = @meta_pct_vistoria_custom,
            codigo_usuario_update           = @codigo_usuario,
            data_update                     = GETDATE()
        WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade
    END
    ELSE
    BEGIN
        INSERT INTO tb_cfg_governanca_meta_unidade(
            codigo_empresa, codigo_unidade, pct_meta_custom, meta_aptos_dia_camareira_custom,
            meta_pct_vistoria_custom, codigo_usuario_update, data_update)
        VALUES(
            @codigo_empresa, @codigo_unidade, @pct_meta_custom, @meta_aptos_dia_camareira_custom,
            @meta_pct_vistoria_custom, @codigo_usuario, GETDATE())
    END

END
GO
