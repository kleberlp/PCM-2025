/**********************************************************************************************
    Script      : Governança - Campos de Discrepância no Apontamento
    Data        : 10/08/2026
    Descricao   : Bloco "Discrepância" no ChecklistGovernancaApontamento (abaixo do Enxoval).
                  Os campos são montados a partir de tb_cad_discrepancia_gov (por empresa):
                    - codigo_tipo_item_checklist = 10 => combo (opções da procedure_lista)
                    - codigo_tipo_item_checklist = 1  => texto
                  Os valores são gravados em colunas de tb_gov_apontamento, mapeadas por 'codigo':
                    1 -> status_uh_discrepancia         5 -> criancas1
                    2 -> status_governanca_discrepancia 6 -> criancas2
                    3 -> discrepancia                   7 -> bagagem
                    4 -> adultos                        8 -> observacao

    Objetos:
      - Colunas    : tb_gov_apontamento.status_uh_discrepancia / status_governanca_discrepancia /
                     discrepancia / adultos / criancas1 / criancas2 / bagagem / observacao
      - Procedure  : sp_select_governanca_discrepancia_campos
      - Procedure  : sp_select_governanca_apontamento_discrepancia
      - Procedure  : sp_update_governanca_apontamento_discrepancia
**********************************************************************************************/

/*--------------------------------------------------------------------------------------------
    1) COLUNAS (idempotente)
--------------------------------------------------------------------------------------------*/
IF COL_LENGTH('dbo.tb_gov_apontamento', 'status_uh_discrepancia') IS NULL
    ALTER TABLE [dbo].[tb_gov_apontamento] ADD [status_uh_discrepancia] [int] NULL;
GO
IF COL_LENGTH('dbo.tb_gov_apontamento', 'status_governanca_discrepancia') IS NULL
    ALTER TABLE [dbo].[tb_gov_apontamento] ADD [status_governanca_discrepancia] [int] NULL;
GO
IF COL_LENGTH('dbo.tb_gov_apontamento', 'discrepancia') IS NULL
    ALTER TABLE [dbo].[tb_gov_apontamento] ADD [discrepancia] [varchar](10) NULL;
GO
IF COL_LENGTH('dbo.tb_gov_apontamento', 'adultos') IS NULL
    ALTER TABLE [dbo].[tb_gov_apontamento] ADD [adultos] [int] NULL;
GO
IF COL_LENGTH('dbo.tb_gov_apontamento', 'criancas1') IS NULL
    ALTER TABLE [dbo].[tb_gov_apontamento] ADD [criancas1] [int] NULL;
GO
IF COL_LENGTH('dbo.tb_gov_apontamento', 'criancas2') IS NULL
    ALTER TABLE [dbo].[tb_gov_apontamento] ADD [criancas2] [int] NULL;
GO
IF COL_LENGTH('dbo.tb_gov_apontamento', 'bagagem') IS NULL
    ALTER TABLE [dbo].[tb_gov_apontamento] ADD [bagagem] [varchar](1) NULL;
GO
IF COL_LENGTH('dbo.tb_gov_apontamento', 'observacao') IS NULL
    ALTER TABLE [dbo].[tb_gov_apontamento] ADD [observacao] [varchar](200) NULL;
GO

/*--------------------------------------------------------------------------------------------
    2) CAMPOS de discrepância cadastrados (por empresa)
       campo_apontamento = coluna destino em tb_gov_apontamento (mapeada por 'codigo')
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_select_governanca_discrepancia_campos', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_governanca_discrepancia_campos];
GO

CREATE PROCEDURE [dbo].[sp_select_governanca_discrepancia_campos]
@codigo_empresa smallint
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        d.codigo,
        d.descricao,
        d.codigo_tipo_item_checklist,
        d.procedure_lista,
        CASE d.codigo
            WHEN 1 THEN 'status_uh_discrepancia'
            WHEN 2 THEN 'status_governanca_discrepancia'
            WHEN 3 THEN 'discrepancia'
            WHEN 4 THEN 'adultos'
            WHEN 5 THEN 'criancas1'
            WHEN 6 THEN 'criancas2'
            WHEN 7 THEN 'bagagem'
            WHEN 8 THEN 'observacao'
            ELSE '' END AS campo_apontamento
    FROM tb_cad_discrepancia_gov d
    WHERE d.codigo_empresa = @codigo_empresa
    ORDER BY d.codigo;

END
GO

/*--------------------------------------------------------------------------------------------
    3) Valores de discrepância de um apontamento (edição)
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_select_governanca_apontamento_discrepancia', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_governanca_apontamento_discrepancia];
GO

CREATE PROCEDURE [dbo].[sp_select_governanca_apontamento_discrepancia]
@codigo_empresa     smallint,
@codigo_unidade     int,
@codigo_apontamento bigint
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        CONVERT(varchar(20), status_uh_discrepancia)         AS status_uh_discrepancia,
        CONVERT(varchar(20), status_governanca_discrepancia) AS status_governanca_discrepancia,
        ISNULL(discrepancia, '')                             AS discrepancia,
        CONVERT(varchar(20), adultos)                        AS adultos,
        CONVERT(varchar(20), criancas1)                      AS criancas1,
        CONVERT(varchar(20), criancas2)                      AS criancas2,
        ISNULL(bagagem, '')                                  AS bagagem,
        ISNULL(observacao, '')                               AS observacao
    FROM tb_gov_apontamento
    WHERE codigo = @codigo_apontamento
    AND   codigo_empresa = @codigo_empresa
    AND   codigo_unidade = @codigo_unidade;

END
GO

/*--------------------------------------------------------------------------------------------
    4) Gravação da discrepância no apontamento
       COALESCE mantém o valor atual quando o parâmetro vem NULL (campo não exibido).
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_update_governanca_apontamento_discrepancia', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_update_governanca_apontamento_discrepancia];
GO

CREATE PROCEDURE [dbo].[sp_update_governanca_apontamento_discrepancia]
@codigo_empresa     smallint,
@codigo_unidade     int,
@codigo_apontamento bigint,
@status_uh          int = NULL,
@status_gov         int = NULL,
@discrepancia       varchar(10) = NULL,
@adultos            int = NULL,
@criancas1          int = NULL,
@criancas2          int = NULL,
@bagagem            varchar(1) = NULL,
@observacao         varchar(200) = NULL
AS
BEGIN

    SET NOCOUNT ON;

    UPDATE tb_gov_apontamento SET
        status_uh_discrepancia         = COALESCE(@status_uh,   status_uh_discrepancia),
        status_governanca_discrepancia = COALESCE(@status_gov,  status_governanca_discrepancia),
        discrepancia                   = COALESCE(@discrepancia, discrepancia),
        adultos                        = COALESCE(@adultos,     adultos),
        criancas1                      = COALESCE(@criancas1,   criancas1),
        criancas2                      = COALESCE(@criancas2,   criancas2),
        bagagem                        = COALESCE(@bagagem,     bagagem),
        observacao                     = COALESCE(@observacao,  observacao)
    WHERE codigo = @codigo_apontamento
    AND   codigo_empresa = @codigo_empresa
    AND   codigo_unidade = @codigo_unidade;

END
GO
