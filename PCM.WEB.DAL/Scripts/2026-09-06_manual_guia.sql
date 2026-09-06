-- =============================================================================================
--  MANUAL — GUIA COMPLETO (menu proprio no PCM)
--
--  O painel "?" mostra o manual de UMA tela. O Guia do PCM e a visao inteira, no estilo da
--  plataforma antiga (Guia by SIM): navegacao por trilha (manuais de processo) com os
--  artigos (manuais de tela) agrupados embaixo, todos vindos das mesmas tb_manual /
--  tb_manual_item ja carregadas pela migracao do Supabase.
--
--  Esta procedure devolve a lista enxuta para montar a navegacao — o conteudo de cada
--  manual continua vindo da sp_select_manual (via ManualTela, que o painel ja usa).
--
--  Idempotente: pode rodar mais de uma vez.
--  Autor: manutencao PCM · Data: 06/09/2026
-- =============================================================================================

IF OBJECT_ID('dbo.sp_select_manual_guia', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_manual_guia];
GO

CREATE PROCEDURE [dbo].[sp_select_manual_guia]
@codigo_empresa smallint
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        codigo          = m.codigo,
        tipo            = m.tipo,                                   -- 'P' trilha | 'S' artigo de tela
        titulo          = m.titulo,
        subtitulo       = ISNULL(m.subtitulo, ''),
        processo_codigo = ISNULL(m.codigo_manual_processo, 0),      -- trilha do artigo (0 = sem trilha)
        secoes          = (SELECT COUNT(*) FROM tb_manual_item i
                           WHERE i.codigo_manual = m.codigo AND i.ativo = 1)
    FROM   tb_manual m
    WHERE  m.ativo = 1
    AND    (m.codigo_empresa IS NULL OR m.codigo_empresa = @codigo_empresa)
    ORDER  BY CASE WHEN m.tipo = 'P' THEN 0 ELSE 1 END, m.titulo;

END
GO
