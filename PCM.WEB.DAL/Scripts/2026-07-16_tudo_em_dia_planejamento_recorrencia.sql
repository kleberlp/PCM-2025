/**********************************************************************************************
    Script      : Tudo em Dia (Fase 5.2) - Planejamento por periodicidade (recorrência)
    Data        : 16/07/2026
    Descricao   : Reescreve sp_gerar_agenda_tudo para montar a agenda do mês a partir da
                  PERIODICIDADE de cada local (não depende de data_proximo_tudo preenchida):

                    - DIÁRIA (1): ocorre em TODOS os dias conforme o intervalo (a cada N dias)
                                  a partir do início — datas fixas.
                    - SEMANAL (2): 1 ocorrência por semana elegível (a cada N semanas),
                                   distribuída dentro daquela semana.
                    - MENSAL e acima (3,5,6,7): 1 ocorrência no mês, distribuída pelos dias
                                   restantes do mês (ex.: 100 tarefas ÷ 21 dias ≈ 5/dia).

                  Início de cada local = data_proximo_tudo quando preenchida (e futura);
                  caso contrário HOJE (locais nunca executados / atrasados entram a partir de hoje).

                  Distribuição das flexíveis (semanal/mensal) respeita a capacidade/dia por
                  responsável: @capacidade > 0 = limite fixo; @capacidade = 0 = automático
                  (teto(qtde / dias da janela)).

    Objetos:
        - Procedure : sp_gerar_agenda_tudo (recriada)
**********************************************************************************************/

IF OBJECT_ID('dbo.sp_gerar_agenda_tudo', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_gerar_agenda_tudo]
GO

CREATE PROCEDURE [dbo].[sp_gerar_agenda_tudo]
@codigo_empresa smallint,
@codigo_unidade int,
@data_inicio    date,
@data_fim       date,
@capacidade     int = 0,     -- 0 = automático (divide a demanda flexível pelos dias da janela)
@codigo_usuario int
AS
BEGIN

    SET NOCOUNT ON;

    DECLARE @hoje date = CONVERT(date, GETDATE());
    DECLARE @base date = CASE WHEN @data_inicio > @hoje THEN @data_inicio ELSE @hoje END;

    IF @data_fim < @base
        SET @data_fim = @base;

    -- regenera: remove o planejamento em aberto do período
    DELETE FROM tb_tudo_agenda
    WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade
    AND   status = 0 AND data_planejada BETWEEN @data_inicio AND @data_fim;

    -- dias do período (a partir de hoje/início até o fim)
    ;WITH datas AS (
        SELECT @base AS d
        UNION ALL
        SELECT DATEADD(DAY, 1, d) FROM datas WHERE d < @data_fim
    )
    SELECT
        d,
        DATEDIFF(DAY, @base, d)  AS didx,
        DATEDIFF(WEEK, @base, d) AS widx
    INTO #datas
    FROM datas
    OPTION (MAXRECURSION 1000);

    -- janelas de semana (para distribuir as semanais dentro da semana)
    SELECT widx, MIN(d) AS win_ini, MAX(d) AS win_fim
    INTO #weeks
    FROM #datas
    GROUP BY widx;

    -- pool de responsáveis (round-robin) = funcionários ativos da unidade
    SELECT ROW_NUMBER() OVER (ORDER BY codigo) - 1 AS rn, codigo
    INTO #pool
    FROM tb_cad_funcionario
    WHERE codigo_empresa = @codigo_empresa
    AND   (codigo_unidade = @codigo_unidade OR codigo_unidade IS NULL)
    AND   ativo = 1;

    DECLARE @npool int = (SELECT COUNT(*) FROM #pool);

    -- candidatos: todos os locais ativos com checklist e periodicidade
    SELECT
        a.codigo AS codigo_apartamento,
        ISNULL(a.codigo_periodicidade, 3)                                   AS periodicidade,
        CASE WHEN ISNULL(a.intervalo, 0) < 1 THEN 1 ELSE a.intervalo END    AS intervalo,
        CASE WHEN a.data_proximo_tudo IS NULL OR a.data_proximo_tudo < @base
             THEN @base ELSE a.data_proximo_tudo END                        AS due_start,
        s.codigo_funcionario_responsavel                                    AS resp_setor
    INTO #cand
    FROM tb_cad_apartamento a
        LEFT JOIN tb_cad_setor s ON s.codigo = a.codigo_setor AND s.codigo_empresa = a.codigo_empresa AND s.codigo_unidade = a.codigo_unidade
    WHERE a.codigo_empresa = @codigo_empresa
    AND   a.codigo_unidade = @codigo_unidade
    AND   ISNULL(a.ativo, 1) = 1
    AND   a.local IS NOT NULL
    AND   a.codigo_checklist IS NOT NULL
    AND   a.codigo_periodicidade IS NOT NULL;

    -- resolve o responsável (setor -> round-robin)
    SELECT
        c.codigo_apartamento, c.periodicidade, c.intervalo, c.due_start,
        CASE
            WHEN c.resp_setor IS NOT NULL THEN c.resp_setor
            WHEN @npool > 0 THEN (SELECT codigo FROM #pool WHERE rn = (c.rr % @npool))
            ELSE NULL
        END AS resp
    INTO #candr
    FROM (
        SELECT *,
            CASE WHEN resp_setor IS NULL
                 THEN ROW_NUMBER() OVER (PARTITION BY CASE WHEN resp_setor IS NULL THEN 1 ELSE 0 END ORDER BY codigo_apartamento) - 1
                 ELSE 0 END AS rr
        FROM #cand
    ) c;

    /* =====================================================================
       OCORRÊNCIAS
       ===================================================================== */

    -- (A) DIÁRIA (1): datas FIXAS a cada 'intervalo' dias a partir do início
    SELECT c.codigo_apartamento, c.resp, dt.d AS data_planejada
    INTO #occ
    FROM #candr c
        INNER JOIN #datas dt ON dt.d >= c.due_start
            AND (DATEDIFF(DAY, c.due_start, dt.d) % c.intervalo) = 0
    WHERE c.periodicidade = 1;

    -- (B) SEMANAL (2): 1 ocorrência por semana elegível; janela = dias daquela semana (>= início)
    SELECT
        c.codigo_apartamento, c.resp,
        CASE WHEN c.due_start > w.win_ini THEN c.due_start ELSE w.win_ini END AS win_ini,
        w.win_fim
    INTO #flex
    FROM #candr c
        INNER JOIN #weeks w ON (w.widx % c.intervalo) = 0 AND w.win_fim >= c.due_start
    WHERE c.periodicidade = 2;

    -- (C) MENSAL e acima (3,5,6,7): 1 ocorrência no período; janela = [início .. fim do mês]
    INSERT INTO #flex (codigo_apartamento, resp, win_ini, win_fim)
    SELECT c.codigo_apartamento, c.resp, c.due_start, @data_fim
    FROM #candr c
    WHERE c.periodicidade NOT IN (1, 2) AND c.due_start <= @data_fim;

    /* =====================================================================
       DISTRIBUIÇÃO por dia — CAPACIDADE DA UNIDADE (global), contando as DIÁRIAS
         A capacidade é o quanto a unidade faz por dia (independe de nº de
         funcionários). As DIÁRIAS já foram fixadas em #occ e OCUPAM capacidade;
         as flexíveis (semanal/mensal) preenchem o que sobra:
           - cada flexível vai no PRIMEIRO dia com vaga dentro da sua janela
             (semanal = a semana; mensal = o mês);
           - o que não couber até o fim vai para o ÚLTIMO dia da janela (mostra
             o que atrasaria);
           @capacidade > 0 => N por dia;
           @capacidade = 0 => automático = teto((diárias + flexíveis) / dias).
       ===================================================================== */

    -- carga das diárias por dia (já estão em #occ)
    SELECT data_planejada AS d, COUNT(*) AS dcount
    INTO #dload
    FROM #occ
    GROUP BY data_planejada;

    DECLARE @cnt_daily int = (SELECT COUNT(*) FROM #occ);
    DECLARE @cnt_flex  int = (SELECT COUNT(*) FROM #flex);
    DECLARE @dias_win  int = (SELECT COUNT(*) FROM #datas);
    IF @dias_win < 1 SET @dias_win = 1;

    DECLARE @cap_auto int = CAST(CEILING((@cnt_daily + @cnt_flex) * 1.0 / @dias_win) AS int);
    IF @cap_auto < 1 SET @cap_auto = 1;

    -- capacidade restante por dia = capacidade do dia menos as diárias já alocadas
    SELECT
        dt.d,
        (CASE WHEN @capacidade > 0 THEN @capacidade ELSE @cap_auto END) - ISNULL(dl.dcount, 0) AS restante
    INTO #cap
    FROM #datas dt
        LEFT JOIN #dload dl ON dl.d = dt.d;

    -- aloca as flexíveis: janelas menores primeiro (semanais antes das mensais),
    -- cada uma no primeiro dia com vaga dentro da janela; sem vaga -> último dia
    DECLARE @ca int, @rp int, @wi date, @wf date, @dd date;

    DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
        SELECT codigo_apartamento, resp, win_ini, win_fim
        FROM #flex
        ORDER BY DATEDIFF(DAY, win_ini, win_fim), win_ini, codigo_apartamento;

    OPEN cur;
    FETCH NEXT FROM cur INTO @ca, @rp, @wi, @wf;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @dd = NULL;

        SELECT TOP 1 @dd = d
        FROM #cap
        WHERE d BETWEEN @wi AND @wf AND restante > 0
        ORDER BY d;

        IF @dd IS NULL SET @dd = @wf;  -- sem vaga na janela -> último dia (atraso)

        INSERT INTO #occ (codigo_apartamento, resp, data_planejada) VALUES (@ca, @rp, @dd);
        UPDATE #cap SET restante = restante - 1 WHERE d = @dd;

        FETCH NEXT FROM cur INTO @ca, @rp, @wi, @wf;
    END

    CLOSE cur;
    DEALLOCATE cur;

    DROP TABLE #dload; DROP TABLE #cap;

    /* =====================================================================
       GRAVA
       ===================================================================== */
    DECLARE @base_codigo bigint = ISNULL((SELECT MAX(codigo) FROM tb_tudo_agenda
                                          WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade), 0);

    INSERT INTO tb_tudo_agenda(
        codigo, codigo_empresa, codigo_unidade, codigo_apartamento, data_planejada,
        codigo_funcionario, status, codigo_tudo_apontamento, codigo_usuario_input, data_input)
    SELECT
        @base_codigo + ROW_NUMBER() OVER (ORDER BY data_planejada, codigo_apartamento),
        @codigo_empresa, @codigo_unidade, codigo_apartamento, data_planejada,
        NULLIF(ISNULL(resp, 0), 0), 0, NULL, @codigo_usuario, GETDATE()
    FROM #occ;

    SELECT @@ROWCOUNT AS total;

    DROP TABLE #datas; DROP TABLE #weeks; DROP TABLE #pool; DROP TABLE #cand; DROP TABLE #candr; DROP TABLE #occ; DROP TABLE #flex;

END
GO
