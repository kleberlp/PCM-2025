-- =============================================================================================
--  COR DA PRIORIDADE
--
--  A cor do chip de prioridade (Kanban de O.S. e telas futuras) passa a vir do cadastro, em vez
--  de regra fixa no JavaScript: mudar a paleta vira manutencao de dados, nao deploy.
--
--  Le-se por uma procedure propria — a sp_select_pcm_ordem_servico NAO e tocada, para nao
--  arriscar a query principal da O.S. O Kanban busca o mapa (codigo -> cor) uma vez por carga.
--
--  Idempotente: pode rodar mais de uma vez.
--  Autor: manutencao PCM · Data: 28/08/2026
-- =============================================================================================

-- ---- Coluna ---------------------------------------------------------------------------------
IF COL_LENGTH('dbo.tb_cad_prioridade', 'cor') IS NULL
    ALTER TABLE dbo.tb_cad_prioridade ADD cor varchar(20) NULL
GO

-- ---- Semente: paleta combinada (so onde ainda esta vazio) -----------------------------------
--      0 critico vermelho · 1 alta laranja · 2 media mostarda · 3 baixa amarelo claro
--      A descricao cadastrada carrega o numero do negocio ("0- CRITICA", "1- ALTA"...).
UPDATE dbo.tb_cad_prioridade
SET    cor = CASE
                WHEN LTRIM(descricao) LIKE '0%' THEN '#d32f2f'
                WHEN LTRIM(descricao) LIKE '1%' THEN '#f57c00'
                WHEN LTRIM(descricao) LIKE '2%' THEN '#c9a227'
                WHEN LTRIM(descricao) LIKE '3%' THEN '#f7e08a'
                WHEN UPPER(descricao) LIKE '%CRITIC%' OR UPPER(descricao) LIKE '%CRÍTIC%' THEN '#d32f2f'
                WHEN UPPER(descricao) LIKE '%ALTA%'   OR UPPER(descricao) LIKE '%URGEN%'  THEN '#f57c00'
                WHEN UPPER(descricao) LIKE '%MEDIA%'  OR UPPER(descricao) LIKE '%MÉDIA%'  THEN '#c9a227'
                WHEN UPPER(descricao) LIKE '%BAIXA%'                                       THEN '#f7e08a'
             END
WHERE  ISNULL(cor, '') = ''
AND    (LTRIM(descricao) LIKE '[0-3]%'
        OR UPPER(descricao) LIKE '%CRITIC%' OR UPPER(descricao) LIKE '%CRÍTIC%'
        OR UPPER(descricao) LIKE '%ALTA%'   OR UPPER(descricao) LIKE '%URGEN%'
        OR UPPER(descricao) LIKE '%MEDIA%'  OR UPPER(descricao) LIKE '%MÉDIA%'
        OR UPPER(descricao) LIKE '%BAIXA%')
GO


-- =============================================================================================
--  MAPA codigo -> cor, para as telas pintarem o chip
-- =============================================================================================
IF OBJECT_ID('dbo.sp_select_cadastro_basico_prioridade_cor', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_cadastro_basico_prioridade_cor];
GO

CREATE PROCEDURE [dbo].[sp_select_cadastro_basico_prioridade_cor]
@codigo_empresa smallint
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        codigo    = p.codigo,
        descricao = p.descricao,
        cor       = ISNULL(p.cor, '')
    FROM   tb_cad_prioridade p
    WHERE  p.codigo_empresa = @codigo_empresa
    ORDER  BY p.descricao;

END
GO
