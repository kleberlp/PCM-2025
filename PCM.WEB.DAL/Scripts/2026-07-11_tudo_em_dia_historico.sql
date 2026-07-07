/**********************************************************************************************
    Script      : Tudo em Dia (Fase 3) - Histórico de execuções
    Data        : 11/07/2026
    Descricao   : Lista os apontamentos (execuções) do checklist do local num período,
                  com setor, checklist, responsável e tempo gasto. Espelha o histórico do UH.

    Objetos (novo):
        - Procedure : sp_select_tudo_checklist_historico
**********************************************************************************************/

IF OBJECT_ID('dbo.sp_select_tudo_checklist_historico', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_tudo_checklist_historico]
GO

CREATE PROCEDURE [dbo].[sp_select_tudo_checklist_historico]
@codigo_empresa     smallint,
@codigo_unidade     int,
@data_inicio        date,
@data_termino       date,
@codigo_apartamento int = -1
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        a.codigo_unidade,
        u.nome_fantasia                                 AS unidade,
        ISNULL(s.descricao, '')                         AS setor,
        a.codigo                                        AS codigo_apartamento,
        a.local,
        ap.codigo,
        ISNULL(c.descricao, '')                         AS checklist,
        ISNULL(f.nome, '')                              AS responsavel,
        CONVERT(varchar(10), ap.data_inicio, 103) + ' ' + LEFT(CONVERT(varchar(8), ISNULL(ap.hora_inicio, '00:00:00'), 108), 5)  AS data_inicio,
        CONVERT(varchar(10), ap.data_termino, 103) + ' ' + LEFT(CONVERT(varchar(8), ISNULL(ap.hora_termino, '00:00:00'), 108), 5) AS data_termino,
        dbo.fn_format_horas(DATEDIFF(MINUTE,
            DATETIMEFROMPARTS(YEAR(ap.data_inicio), MONTH(ap.data_inicio), DAY(ap.data_inicio), DATEPART(HOUR, ISNULL(ap.hora_inicio, '00:00:00')), DATEPART(MINUTE, ISNULL(ap.hora_inicio, '00:00:00')), 0, 0),
            DATETIMEFROMPARTS(YEAR(ap.data_termino), MONTH(ap.data_termino), DAY(ap.data_termino), DATEPART(HOUR, ISNULL(ap.hora_termino, '00:00:00')), DATEPART(MINUTE, ISNULL(ap.hora_termino, '00:00:00')), 0, 0))
        ) AS tempo
    FROM
        tb_tudo_apontamento ap
        INNER JOIN tb_cad_apartamento a ON a.codigo = ap.codigo_apartamento AND a.codigo_empresa = ap.codigo_empresa AND a.codigo_unidade = ap.codigo_unidade
        INNER JOIN tb_cad_unidade u ON u.codigo = a.codigo_unidade AND u.codigo_empresa = a.codigo_empresa
        LEFT  JOIN tb_cad_setor s ON s.codigo = a.codigo_setor AND s.codigo_empresa = a.codigo_empresa
        LEFT  JOIN tb_chk_checklist c ON c.codigo = ap.codigo_checklist AND c.codigo_empresa = ap.codigo_empresa
        LEFT  JOIN tb_cad_funcionario f ON f.codigo = ap.codigo_funcionario_responsavel AND f.codigo_empresa = ap.codigo_empresa
    WHERE ap.codigo_empresa = @codigo_empresa
    AND   ((ap.codigo_unidade = @codigo_unidade) OR (@codigo_unidade <= 0))
    AND   (CONVERT(date, ap.data_termino) BETWEEN @data_inicio AND @data_termino)
    AND   ((ap.codigo_apartamento = @codigo_apartamento) OR (@codigo_apartamento <= 0))
    ORDER BY ap.data_termino DESC, ap.codigo DESC

END
GO
