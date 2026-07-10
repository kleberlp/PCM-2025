/**********************************************************************************************
    Script      : Tudo em Dia (Fase 5.1) - Agenda estilo calendário + planejamento uniforme
    Data        : 15/07/2026
    Descricao   : - Novo planejamento (sp_gerar_agenda_tudo): pega tudo que ainda falta no
                    período (mês), conta os dias disponíveis e DISTRIBUI UNIFORMEMENTE por dia
                    por responsável (capacidade automática = teto(qtde / dias)), sem passar do
                    vencimento. Um limite fixo/dia continua possível passando @capacidade > 0.
                  - Nova SP para o calendário (sp_select_tudo_agenda_calendario): eventos por
                    dia no intervalo visível do calendário.

    Objetos:
        - Procedure : sp_gerar_agenda_tudo              (recriada - distribuição uniforme)
        - Procedure : sp_select_tudo_agenda_calendario  (nova)
**********************************************************************************************/

/*--------------------------------------------------------------------------------------------
    1) PLANEJAMENTO com distribuição UNIFORME no período
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_gerar_agenda_tudo', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_gerar_agenda_tudo]
GO

CREATE PROCEDURE [dbo].[sp_gerar_agenda_tudo]
@codigo_empresa smallint,
@codigo_unidade int,
@data_inicio    date,
@data_fim       date,
@capacidade     int = 0,     -- 0 = automático (divide a demanda pelos dias do período)
@codigo_usuario int
AS
BEGIN

    SET NOCOUNT ON;

    DECLARE @hoje date = CONVERT(date, GETDATE());
    DECLARE @base date = CASE WHEN @data_inicio > @hoje THEN @data_inicio ELSE @hoje END;

    -- dias disponíveis no período (a partir de hoje/início até o fim)
    DECLARE @dias int = DATEDIFF(DAY, @base, @data_fim) + 1;
    IF @dias < 1 SET @dias = 1;

    -- regenera: remove o planejamento futuro ainda não concluído dentro do período
    DELETE FROM tb_tudo_agenda
    WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade
    AND   status = 0 AND data_planejada BETWEEN @data_inicio AND @data_fim;

    -- pool de responsáveis (round-robin) = funcionários ativos da unidade
    SELECT ROW_NUMBER() OVER (ORDER BY codigo) - 1 AS rn, codigo
    INTO #pool
    FROM tb_cad_funcionario
    WHERE codigo_empresa = @codigo_empresa
    AND   (codigo_unidade = @codigo_unidade OR codigo_unidade IS NULL)
    AND   ativo = 1;

    DECLARE @npool int = (SELECT COUNT(*) FROM #pool);

    -- candidatos: locais a vencer no período, ativos, com checklist, atrasados/pendentes e sem agenda ativa
    SELECT
        a.codigo AS codigo_apartamento,
        CASE WHEN a.data_proximo_tudo < @base THEN @base
             WHEN a.data_proximo_tudo > @data_fim THEN @data_fim
             ELSE a.data_proximo_tudo END          AS data_alvo,
        s.codigo_funcionario_responsavel           AS resp_setor
    INTO #cand
    FROM tb_cad_apartamento a
        LEFT JOIN tb_cad_setor s ON s.codigo = a.codigo_setor AND s.codigo_empresa = a.codigo_empresa AND s.codigo_unidade = a.codigo_unidade
    WHERE a.codigo_empresa = @codigo_empresa
    AND   a.codigo_unidade = @codigo_unidade
    AND   ISNULL(a.ativo, 1) = 1
    AND   a.local IS NOT NULL
    AND   a.codigo_checklist IS NOT NULL
    AND   a.data_proximo_tudo IS NOT NULL
    AND   a.data_proximo_tudo <= @data_fim
    AND   ISNULL(a.status_tudo, 2) IN (1, 2)
    AND   NOT EXISTS (SELECT 1 FROM tb_tudo_agenda ag
                      WHERE ag.codigo_empresa = a.codigo_empresa AND ag.codigo_unidade = a.codigo_unidade
                      AND   ag.codigo_apartamento = a.codigo AND ag.status = 0);

    -- resolve o responsável (setor -> round-robin)
    SELECT
        c.codigo_apartamento,
        c.data_alvo,
        CASE
            WHEN c.resp_setor IS NOT NULL THEN c.resp_setor
            WHEN @npool > 0 THEN (SELECT codigo FROM #pool WHERE rn = (c.rr % @npool))
            ELSE NULL
        END AS resp
    INTO #resp
    FROM (
        SELECT
            codigo_apartamento, data_alvo, resp_setor,
            CASE WHEN resp_setor IS NULL
                 THEN ROW_NUMBER() OVER (PARTITION BY CASE WHEN resp_setor IS NULL THEN 1 ELSE 0 END ORDER BY data_alvo, codigo_apartamento) - 1
                 ELSE 0 END AS rr
        FROM #cand
    ) c;

    -- capacidade efetiva por responsável:
    --   @capacidade > 0  => limite fixo por dia
    --   @capacidade = 0  => automático = teto(qtde do responsável / dias do período)
    SELECT
        r.codigo_apartamento,
        r.resp,
        r.data_alvo,
        CASE WHEN @capacidade > 0 THEN @capacidade
             ELSE CAST(CEILING(COUNT(*) OVER (PARTITION BY r.resp) * 1.0 / @dias) AS int)
        END AS cap
    INTO #respc
    FROM #resp r;

    -- distribui uniformemente por dia (a partir de @base), sem furar o vencimento
    SELECT
        codigo_apartamento,
        resp,
        CASE
            WHEN DATEADD(DAY, (ROW_NUMBER() OVER (PARTITION BY resp ORDER BY data_alvo, codigo_apartamento) - 1) / cap, @base) > data_alvo
                THEN data_alvo
            ELSE DATEADD(DAY, (ROW_NUMBER() OVER (PARTITION BY resp ORDER BY data_alvo, codigo_apartamento) - 1) / cap, @base)
        END AS planned
    INTO #plan
    FROM #respc;

    DECLARE @base_codigo bigint = ISNULL((SELECT MAX(codigo) FROM tb_tudo_agenda
                                          WHERE codigo_empresa = @codigo_empresa AND codigo_unidade = @codigo_unidade), 0);

    INSERT INTO tb_tudo_agenda(
        codigo, codigo_empresa, codigo_unidade, codigo_apartamento, data_planejada,
        codigo_funcionario, status, codigo_tudo_apontamento, codigo_usuario_input, data_input)
    SELECT
        @base_codigo + ROW_NUMBER() OVER (ORDER BY planned, codigo_apartamento),
        @codigo_empresa, @codigo_unidade, codigo_apartamento, planned,
        NULLIF(ISNULL(resp, 0), 0), 0, NULL, @codigo_usuario, GETDATE()
    FROM #plan;

    SELECT @@ROWCOUNT AS total;

    DROP TABLE #pool; DROP TABLE #cand; DROP TABLE #resp; DROP TABLE #respc; DROP TABLE #plan;

END
GO

/*--------------------------------------------------------------------------------------------
    2) EVENTOS DO CALENDÁRIO (intervalo visível)
--------------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.sp_select_tudo_agenda_calendario', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_tudo_agenda_calendario]
GO

CREATE PROCEDURE [dbo].[sp_select_tudo_agenda_calendario]
@codigo_empresa     smallint,
@codigo_unidade     int,
@data_inicio        date,
@data_fim           date,
@codigo_funcionario int = -1
AS
BEGIN

    SET NOCOUNT ON;

    DECLARE @hoje date = CONVERT(date, GETDATE());

    SELECT
        ag.codigo,
        ag.codigo_unidade,
        ag.codigo_apartamento,
        CONVERT(varchar(10), ag.data_planejada, 23)                 AS data,          -- yyyy-MM-dd
        ISNULL(a.local, '')                                         AS local,
        ISNULL(s.descricao, '')                                     AS setor,
        ISNULL(f.nome, 'Sem responsável')                           AS responsavel,
        ag.status                                                   AS status_codigo,
        CASE ag.status WHEN 0 THEN 'Planejado' WHEN 1 THEN 'Concluído' WHEN 2 THEN 'Cancelado' ELSE '' END AS situacao,
        CONVERT(bit, CASE WHEN ag.status = 0 AND ag.data_planejada < @hoje THEN 1 ELSE 0 END) AS atrasado
    FROM
        tb_tudo_agenda ag
        INNER JOIN tb_cad_apartamento a ON a.codigo = ag.codigo_apartamento AND a.codigo_empresa = ag.codigo_empresa AND a.codigo_unidade = ag.codigo_unidade
        LEFT  JOIN tb_cad_setor s ON s.codigo = a.codigo_setor AND s.codigo_empresa = a.codigo_empresa
        LEFT  JOIN tb_cad_funcionario f ON f.codigo = ag.codigo_funcionario AND f.codigo_empresa = ag.codigo_empresa
    WHERE ag.codigo_empresa = @codigo_empresa
    AND   ag.codigo_unidade = @codigo_unidade
    AND   ag.data_planejada BETWEEN @data_inicio AND @data_fim
    AND   (@codigo_funcionario <= 0 OR ag.codigo_funcionario = @codigo_funcionario)
    ORDER BY ag.data_planejada, f.nome, a.local

END
GO
