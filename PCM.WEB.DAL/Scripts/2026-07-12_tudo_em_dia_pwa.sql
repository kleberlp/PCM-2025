/**********************************************************************************************
    Script      : Tudo em Dia (Fase 4) - PWA (app)
    Data        : 12/07/2026
    Descricao   : SPs consumidas pelo app (PWA) para listar os locais a executar e registrar
                  o apontamento do checklist do Tudo em Dia. Espelham as sp_pwa_* do UH em Dia,
                  mas por LOCAL e usando checklist/periodicidade do próprio local.

    Objetos (novos):
        - Procedure : sp_pwa_select_pcm_tudo_dia
        - Procedure : sp_pwa_select_pcm_tudo_dia_list
        - Procedure : sp_pwa_insert_pcm_tudo_dia_apontamento
**********************************************************************************************/

/*--------------------------------------------------------------------------------------------
    1) LISTA de locais a executar (status Atrasado/Pendente)
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_pwa_select_pcm_tudo_dia', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_pwa_select_pcm_tudo_dia]
GO

CREATE PROCEDURE [dbo].[sp_pwa_select_pcm_tudo_dia]
@codigo_empresa smallint,
@codigo_unidade int,
@page           smallint
AS
BEGIN

    SET NOCOUNT ON;

    DECLARE @mesAtual date = DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1);

    SELECT
        a.codigo                                        AS codigo_apartamento,
        a.codigo_checklist,
        a.local,
        ISNULL(s.descricao, '')                         AS setor,
        ISNULL(FORMAT(a.data_ultimo_tudo, 'dd/MM/yyyy'), '')            AS data_ultimo_tudo,
        FORMAT(ISNULL(a.data_proximo_tudo, GETDATE()), 'dd/MM/yyyy')    AS data_proximo_tudo,
        st.status                                       AS status_codigo,
        st.descricao                                    AS status_descricao,
        ISNULL(st.css_class, '')                        AS status_css_class
    FROM
        tb_cad_apartamento a
        INNER JOIN tb_cad_unidade un ON un.codigo = a.codigo_unidade AND un.codigo_empresa = a.codigo_empresa
        LEFT  JOIN tb_cad_setor s ON s.codigo = a.codigo_setor AND s.codigo_empresa = a.codigo_empresa
        INNER JOIN tb_stc_status_uh_dia st ON st.status = CASE
            WHEN a.status_tudo = 3 THEN 3
            WHEN a.status_tudo = 4 THEN 4
            WHEN a.data_proximo_tudo IS NULL THEN 2
            WHEN DATEFROMPARTS(YEAR(a.data_proximo_tudo), MONTH(a.data_proximo_tudo), 1) < @mesAtual THEN 1
            WHEN DATEFROMPARTS(YEAR(a.data_proximo_tudo), MONTH(a.data_proximo_tudo), 1) = @mesAtual THEN 2
            ELSE 5
        END
    WHERE a.codigo_empresa = @codigo_empresa
    AND   a.codigo_unidade = @codigo_unidade
    AND   un.ativo = 1
    AND   a.ativo = 1
    AND   a.local IS NOT NULL
    AND   a.codigo_checklist IS NOT NULL
    AND   st.status IN (1, 2)
    ORDER BY s.descricao, a.local

END
GO

/*--------------------------------------------------------------------------------------------
    2) CONTAGEM / paginação
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_pwa_select_pcm_tudo_dia_list', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_pwa_select_pcm_tudo_dia_list]
GO

CREATE PROCEDURE [dbo].[sp_pwa_select_pcm_tudo_dia_list]
@codigo_empresa smallint,
@codigo_unidade int,
@page           smallint
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        CEILING(COUNT(*) / 200.0)   AS total_pages,
        COUNT(*)                    AS total_results,
        @page                       AS page
    FROM tb_cad_apartamento
    WHERE codigo_empresa = @codigo_empresa
    AND   codigo_unidade = @codigo_unidade
    AND   ativo = 1
    AND   local IS NOT NULL
    AND   codigo_checklist IS NOT NULL
    AND   (data_proximo_tudo IS NULL OR data_proximo_tudo <= CONVERT(date, GETDATE()))

END
GO

/*--------------------------------------------------------------------------------------------
    3) INSERT do apontamento pelo app (cabeçalho + itens do template + recálculo)
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_pwa_insert_pcm_tudo_dia_apontamento', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_pwa_insert_pcm_tudo_dia_apontamento]
GO

CREATE PROCEDURE [dbo].[sp_pwa_insert_pcm_tudo_dia_apontamento]
@codigo_empresa     smallint,
@codigo_unidade     int,
@codigo_usuario     int,
@codigo_funcionario int,
@codigo_apartamento bigint,
@data_inicio        datetime,
@data_termino       datetime,
@concluido          bit,
@observacao         varchar(250),
@codigo             bigint OUTPUT
AS
BEGIN

    SET NOCOUNT ON;

    SET @codigo = ISNULL((SELECT MAX(codigo) FROM tb_tudo_apontamento
                          WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade), 0) + 1;

    DECLARE @codigo_checklist bigint = (SELECT codigo_checklist FROM tb_cad_apartamento
                                        WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade AND codigo = @codigo_apartamento);

    -- cabeçalho
    INSERT INTO tb_tudo_apontamento(
        codigo, codigo_empresa, codigo_unidade, codigo_apartamento, codigo_checklist,
        codigo_funcionario_responsavel, data_inicio, data_termino, hora_inicio, hora_termino,
        nova_vistoria, origem, codigo_usuario_input, data_input)
    VALUES(
        @codigo, @codigo_empresa, @codigo_unidade, @codigo_apartamento, @codigo_checklist,
        @codigo_funcionario, CONVERT(date, @data_inicio), CONVERT(date, @data_termino), CONVERT(time(0), @data_inicio), CONVERT(time(0), @data_termino),
        0, 'PWA', @codigo_usuario, GETDATE());

    -- itens do template do checklist do local (respostas padrão SIM)
    INSERT INTO tb_tudo_apontamento_checklist(
        codigo_tudo_apontamento, codigo_empresa, codigo_unidade, codigo_checklist, codigo_checklist_item,
        grupo, checklist, descricao, opcao, observacao, nova_vistoria)
    SELECT
        @codigo, @codigo_empresa, @codigo_unidade, ci.codigo_checklist, ci.codigo,
        ci.grupo, ci.checklist, ci.descricao, 'SIM', NULL, 0
    FROM
        tb_cad_apartamento a
        INNER JOIN tb_chk_checklist_item ci ON ci.codigo_checklist = a.codigo_checklist AND ci.codigo_empresa = a.codigo_empresa
    WHERE a.codigo_empresa = @codigo_empresa AND a.codigo = @codigo_apartamento;

    -- recálculo: status = 5 (Em Dia) + próxima data pela periodicidade do local
    UPDATE tb_cad_apartamento SET
        data_ultimo_tudo = CONVERT(date, @data_termino),
        status_tudo = 5,
        codigo_tudo_apontamento = @codigo,
        data_proximo_tudo = CASE a.codigo_periodicidade
            WHEN 1 THEN DATEADD(DAY, ISNULL(a.intervalo, 1), @data_termino)
            WHEN 2 THEN DATEADD(DAY, -DATEPART(WEEKDAY, DATEADD(WEEK, ISNULL(a.intervalo, 1), @data_termino)) + 1, DATEADD(WEEK, ISNULL(a.intervalo, 1), @data_termino))
            WHEN 3 THEN DATEFROMPARTS(YEAR(DATEADD(MONTH, ISNULL(a.intervalo, 1), @data_termino)), MONTH(DATEADD(MONTH, ISNULL(a.intervalo, 1), @data_termino)), 1)
            WHEN 5 THEN DATEADD(MONTH, ((DATEDIFF(MONTH, 0, @data_termino) / 2) + 1) * 2, 0)
            WHEN 6 THEN DATEADD(MONTH, ((DATEDIFF(MONTH, 0, @data_termino) / 3) + 1) * 3, 0)
            WHEN 7 THEN DATEADD(MONTH, ((DATEDIFF(MONTH, 0, @data_termino) / 6) + 1) * 6, 0)
            ELSE DATEFROMPARTS(YEAR(DATEADD(MONTH, ISNULL(a.intervalo, 1), @data_termino)), MONTH(DATEADD(MONTH, ISNULL(a.intervalo, 1), @data_termino)), 1)
        END
    FROM tb_cad_apartamento a
    WHERE a.codigo = @codigo_apartamento AND a.codigo_empresa = @codigo_empresa

END
GO
