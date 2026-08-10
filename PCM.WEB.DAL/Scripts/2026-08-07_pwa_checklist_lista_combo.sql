/**********************************************************************************************
    Script      : PWA - Checklist - listaCombo (itens de DISCREPÂNCIA / Governança)
    Data        : 07/08/2026
    Descricao   : Adiciona o 5º resultset em sp_pwa_select_pcm_checklist_full com as opções de
                  combo dos itens vindos da tb_cad_discrepancia_gov.

    COMO FUNCIONA:
      - A tb_cad_discrepancia_gov guarda em [procedure_lista] a QUERY (ou EXEC de SP) que
        devolve as opções daquele item. Ex.: "SELECT CODIGO, DESCRICAO FROM (VALUES ...) ..."
      - Os itens de discrepância entram no #itens com codigo = tb_cad_discrepancia_gov.codigo * -1
        (grupo "00 - DISCREPANCIA"). O 5º resultset devolve as opções já amarradas nesse
        codigo NEGATIVO, para a DAL apenas distribuir.
      - Linhas com procedure_lista NULL/vazio simplesmente não aparecem no resultset.
      - Cada query roda dentro de TRY/CATCH: configuração inválida (SP inexistente, erro de
        sintaxe, número de colunas errado) NÃO derruba o getChecklist — o item volta sem opções.

    CONTRATO DA QUERY CONFIGURADA em [procedure_lista]:
      - Deve devolver EXATAMENTE 2 colunas, nesta ordem: codigo, descricao.
      - "codigo" é tratado como TEXTO (varchar), porque já existem listas alfanuméricas
        (BAGAGEM = P / M / G, ver tb_gov_apontamento.bagagem varchar(1)).

    Objetos:
      - Procedure  : sp_pwa_select_pcm_checklist_full  (ALTER - acrescenta RESULTSET 5)
**********************************************************************************************/

/*
SELECT * FROM API.dbo.tb_pwa_log_api_debug WHERE codigo_empresa = 905 ORDER BY date_input DESC
http://www.simservices.com.br/api.pwa/api/pwa/getChecklist?codigoEmpresa=905&codigoUnidade=16&codigoChecklist=1379&tipo=PREVENTIVA&codigoDocumento=183
EXECUTE sp_pwa_select_pcm_checklist_full 905, 18, 1406, 'GOVERNANCA', -1, -1, -1

SELECT * FROM tb_cad_unidade WHERE codigo_empresa = 905
/api.pwa/api/pwa/getChecklist?codigoEmpresa=905&codigoUnidade=18&codigoChecklist=1406&tipo=GOVERNANCA&codigoDocumento=5
*/

ALTER   PROCEDURE [dbo].[sp_pwa_select_pcm_checklist_full]
(
    @codigo_empresa           smallint,
    @codigo_unidade           int,
    @codigo_checklist         bigint,
    @tipo                     varchar(50),
    @codigo_documento         bigint,
    @intervalo                smallint = -1,
    @codigo_equipamento       bigint   = -1
)
AS
BEGIN
    SET NOCOUNT ON;

    -------------------------------------------------------------------------
    -- 1) Contexto GOVERNANCA (status/apto/tipo) quando houver documento
    -------------------------------------------------------------------------
    DECLARE
        @status                  smallint = -1,
        @codigo_apartamento      int      = -1,
        @codigo_tipo_governanca  smallint = -1;

    IF (@tipo = 'GOVERNANCA' AND @codigo_documento > 0)
    BEGIN
        SELECT
            @codigo_tipo_governanca = ga.codigo_tipo_governanca,
            @codigo_apartamento     = ga.codigo_apartamento,
            @status                 = ca.status_governanca
        FROM tb_gov_apontamento ga
        JOIN tb_cad_apartamento ca
          ON ca.codigo = ga.codigo_apartamento
         AND ca.codigo_empresa = ga.codigo_empresa
        WHERE ga.codigo_empresa = @codigo_empresa
          AND ga.codigo_unidade = @codigo_unidade
          AND ga.codigo         = @codigo_documento;
    END

    -------------------------------------------------------------------------
    -- 2) Tabela única normalizada de itens (1 leitura “grande”)
    -------------------------------------------------------------------------
    CREATE TABLE #itens
    (
        grupo                   varchar(100) NOT NULL,
        subgrupo                varchar(100) NOT NULL,
        codigo_tipo_checklist   int          NOT NULL,
        codigo                  int          NOT NULL,
        checklist               varchar(200) NULL,
        descricao               varchar(500) NULL,
        numero_digitos          int          NOT NULL,
        allow_picture           int          NOT NULL,
        uom                     varchar(50)  NULL,
        resultado               varchar(max) NULL,
        observacao              varchar(max) NULL,
        ordem_servico           bit          NOT NULL,
        color                   varchar(20)  NOT NULL,
		prazo					date,
		associar_equipamento	int DEFAULT 0
    );

    -------------------------------------------------------------------------
    -- 2.1) Fontes com @codigo_documento > 0 (PREVENTIVA/ROTINA, PMOC, UH, TAREFA, AUDITORIA, GOVERNANCA)
    -------------------------------------------------------------------------
    IF (@codigo_documento > 0)
    BEGIN
        IF (@tipo IN ('PREVENTIVA','ROTINA'))
        BEGIN
            INSERT INTO #itens
            SELECT
                ISNULL(grupo,'GRUPO'),
                ISNULL(subgrupo,''),
                codigo_tipo_item_checklist,
                codigo,
                checklist,
                descricao,
                ISNULL(numero_digitos,0),
                CASE WHEN ISNULL(allow_picture,1)=1 THEN 1 ELSE 0 END,
                ISNULL(unidade_medida,''),
                ISNULL(resultado,''),
                ISNULL(observacao,''),
                CONVERT(bit,0),
                '#000000',
				NULL,
				0
            FROM tb_pcm_programada_ordem_servico_checklist
            WHERE codigo_empresa = @codigo_empresa
              AND codigo_unidade = @codigo_unidade
              AND codigo_checklist = @codigo_checklist
              AND codigo_pcm_programada_ordem_servico = @codigo_documento;
        END
        ELSE IF (@tipo = 'PMOC')
        BEGIN
            INSERT INTO #itens
            SELECT
                ISNULL(grupo,'GRUPO'),
                ISNULL(subgrupo,''),
                codigo_tipo_item_checklist,
                codigo,
                checklist,
                descricao,
                ISNULL(numero_digitos,0),
                CASE WHEN ISNULL(allow_picture,1)=1 THEN 1 ELSE 0 END,
                ISNULL(unidade_medida,''),
                ISNULL(resultado,''),
                ISNULL(observacao,''),
                CONVERT(bit,0),
                '#000000',
				NULL
            FROM tb_pmoc_ordem_servico_checklist
            WHERE codigo_empresa = @codigo_empresa
              AND codigo_unidade = @codigo_unidade
              AND codigo_checklist = @codigo_checklist
              AND codigo_pmoc_ordem_servico = @codigo_documento;
        END
        ELSE IF (@tipo = 'UH')
        BEGIN
            INSERT INTO #itens
            SELECT
                ISNULL(grupo,'GRUPO'),
                ISNULL(subgrupo,''),
                1,
                codigo_checklist_item,
                checklist,
                descricao,
                ISNULL(numero_digitos,0),
                CASE WHEN ISNULL(allow_picture,1)=1 THEN 1 ELSE 0 END,
                ISNULL(unidade_medida,''),
                ISNULL(opcao,''),
                ISNULL(observacao,''),
                CONVERT(bit,0),
                '#000000',
				NULL,
				1
            FROM tb_uh_apontamento_checklist
            WHERE codigo_empresa = @codigo_empresa
              AND codigo_unidade = @codigo_unidade
              AND codigo_checklist = @codigo_checklist
              AND codigo_uh_apontamento = @codigo_documento;
        END
        ELSE IF (@tipo = 'TAREFA')
        BEGIN
            INSERT INTO #itens
            SELECT
                ISNULL(grupo,'GRUPO'),
                ISNULL(subgrupo,''),
                codigo_tipo_item_checklist,
                codigo,
                checklist,
                descricao,
                ISNULL(numero_digitos,0),
                CASE WHEN ISNULL(allow_picture,1)=1 THEN 1 ELSE 0 END,
                ISNULL(unidade_medida,''),
                ISNULL(resultado,''),
                ISNULL(observacao,''),
                CONVERT(bit,0),
                '#000000',
				NULL
            FROM tb_qa_tarefa_ordem_servico_checklist
            WHERE codigo_empresa = @codigo_empresa
              AND codigo_unidade = @codigo_unidade
              AND codigo_checklist = @codigo_checklist
              AND codigo_qa_tarefa_ordem_servico = @codigo_documento;
        END
        ELSE IF (@tipo IN ('AUDITORIA_QUALIDADE','AUDITORIA_CORPORATIVO'))
        BEGIN
            INSERT INTO #itens
            SELECT DISTINCT
                ISNULL(grupo,'GRUPO'),
                ISNULL(subgrupo,''),
                codigo_tipo_item_checklist,
                codigo_item_checklist,
                checklist,
                descricao,
                ISNULL(numero_digitos,0),
                CASE WHEN ISNULL(allow_picture,1)=1 THEN 1 ELSE 0 END,
                ISNULL(unidade_medida,''),
                ISNULL(resultado,''),
                ISNULL(observacao,''),
                CONVERT(bit,1),
                '#000000',
				NULL, NULL
            FROM tb_aud_auditoria_checklist
            WHERE codigo_empresa = @codigo_empresa
              AND codigo_unidade = @codigo_unidade
              AND codigo_checklist = @codigo_checklist
              AND codigo_auditoria = @codigo_documento;
        END
        ELSE IF (@tipo = 'GOVERNANCA')
        BEGIN

            -- Base do checklist (tb_chk_checklist_item) com filtro de intervalo
            INSERT INTO #itens
            SELECT
                ISNULL(grupo,''),
                ISNULL(subgrupo,''),
                9,
                codigo,
                checklist,
                descricao,
                ISNULL(numero_digitos,0),
                CASE WHEN ISNULL(allow_picture,1)=1 THEN 1 ELSE 0 END,
                ISNULL(unidade_medida,''),
                CASE WHEN ISNULL((SELECT checklist_preenchido_gov FROM tb_cad_empresa WHERE codigo=@codigo_empresa),0)=1 THEN 'OK' ELSE '' END,
                '' ,
                CONVERT(bit,ISNULL(ordem_servico,0)),
                '#000000',
				NULL,
				0
            FROM tb_chk_checklist_item
            WHERE codigo_empresa = @codigo_empresa
              AND codigo_checklist = @codigo_checklist
              AND ((intervalo <= @intervalo) OR (@intervalo = -1))
              AND @status <> 4;

            -- NC - CAMAREIRA (status=3)
            IF (@status = 3)
            BEGIN
                INSERT INTO #itens
                SELECT
                    'NC - CAMAREIRA',
                    ISNULL(subgrupo,''),
                    9,
                    codigo_checklist_item,
                    checklist,
                    descricao,
                    ISNULL(numero_digitos,0),
                    CASE WHEN ISNULL(allow_picture,1)=1 THEN 1 ELSE 0 END,
                    ISNULL(unidade_medida,''),
                    'OK',
                    ISNULL(observacao,''),
                    CONVERT(bit,0),
                    '#000000',
					NULL,
					0
                FROM tb_gov_apontamento_checklist
                WHERE codigo_empresa = @codigo_empresa
                  AND codigo_unidade = @codigo_unidade
                  AND codigo_governanca_apontamento = @codigo_documento
                  AND tipo_apontamento_governanca = 'C'
                  AND resultado IN ('N','NAO','NÃO');
            END

            -- NC - VISTORIA (status=4) usando revisao max como no seu código
            IF (@status = 4)
            BEGIN
                DECLARE @revisao int;

                SELECT @revisao = MAX(revisao)
                FROM tb_gov_apontamento
                WHERE codigo_empresa = @codigo_empresa
                  AND codigo_unidade = @codigo_unidade
                  AND tipo_apontamento_governanca = 'V'
                  AND codigo = @codigo_documento;

                INSERT INTO #itens
                SELECT
                    'NC - VISTORIA',
                    ISNULL(subgrupo,''),
                    9,
                    codigo_checklist_item,
                    checklist,
                    descricao,
                    ISNULL(numero_digitos,0),
                    CASE WHEN ISNULL(allow_picture,1)=1 THEN 1 ELSE 0 END,
                    ISNULL(unidade_medida,''),
                    'OK',
                    ISNULL(observacao,''),
                    CONVERT(bit,0),
                    '#000000',
					NULL
                FROM tb_gov_apontamento_checklist
                WHERE codigo_empresa = @codigo_empresa
                  AND codigo_unidade = @codigo_unidade
                  AND codigo_governanca_apontamento = @codigo_documento
                  AND tipo_apontamento_governanca = 'V'
                  AND codigo = @revisao
                  AND resultado IN ('NOK');
            END
        END
    END
    ELSE
    BEGIN
        ---------------------------------------------------------------------
        -- 2.2) Sem documento (templates): tb_chk_checklist_item (+ enxoval)
        ---------------------------------------------------------------------
        IF (@tipo = 'GOVERNANCA')
        BEGIN
            DECLARE @apontamento_camareira bit = 1;

            SELECT @apontamento_camareira = ISNULL(apontamento_camareira,1)
            FROM tb_cad_unidade
            WHERE codigo_empresa = @codigo_empresa
              AND codigo = @codigo_unidade;

            INSERT INTO #itens
            SELECT
                ISNULL(grupo,'GRUPO'),
                ISNULL(subgrupo,''),
                CASE WHEN @apontamento_camareira = 1 THEN 8 ELSE 9 END,
                codigo,
                checklist,
                descricao,
                ISNULL(numero_digitos,0),
                CASE WHEN ISNULL(allow_picture,1)=1 THEN 1 ELSE 0 END,
                ISNULL(unidade_medida,''),
                CASE WHEN ISNULL((SELECT checklist_preenchido_gov FROM tb_cad_empresa WHERE codigo=@codigo_empresa),0)=1 THEN
                        CASE WHEN @apontamento_camareira=1 THEN 'SIM' ELSE 'OK' END
                     ELSE '' END,
                '',
                CONVERT(bit,ISNULL(ordem_servico,0)),
                '#000000',
				NULL,
				0
            FROM tb_chk_checklist_item
            WHERE codigo_empresa = @codigo_empresa
              AND codigo_checklist = @codigo_checklist
              AND ((intervalo <= @intervalo) OR (@intervalo = -1));


            -- Base do checklist (tb_chk_checklist_item) com filtro de intervalo
            INSERT INTO #itens
            SELECT
                '00 - DISCREPANCIA',
                '',
                tb_cad_discrepancia_gov.codigo_tipo_item_checklist,
                tb_cad_discrepancia_gov.codigo * -1 AS codigo,
                '' AS checklist,
                tb_cad_discrepancia_gov.descricao,
                0,
                0,
                '',
                '',
                '' ,
                0,
                '#000000',
				NULL,
				0
            FROM tb_cad_discrepancia_gov
            WHERE codigo_empresa = @codigo_empresa
              AND codigo_unidade = @codigo_unidade;


            -- Grupo "00 - ENXOVAL" (igual ao seu UNION antigo)
            IF (@apontamento_camareira=1)
            BEGIN
                INSERT INTO #itens
                SELECT
                    '00 - ENXOVAL',
                    '',
                    2,
                    e.codigo,
                    e.descricao,
                    e.descricao,
                    0,
                    0,
                    '',
                    '',
                    '',
                    CONVERT(bit,0),
                    '#000000',
					NULL,
					0
                FROM tb_cad_enxoval e
                WHERE e.codigo_empresa = @codigo_empresa
                  AND e.codigo_unidade = @codigo_unidade;
            END
        END
        ELSE IF (@tipo = 'PMOC' AND @intervalo = -1)
		BEGIN

			DECLARE @MesAtual date = DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1);

			CREATE TABLE #tb_result
			(
				codigo_empresa              smallint NOT NULL,
				codigo_unidade              int      NOT NULL,
				codigo_equipamento          bigint   NOT NULL,
				codigo_checklist			bigint	 NOT NULL,
				codigo_tipo_ar_condicionado int      NOT NULL,
				codigo_checklist_item       int      NOT NULL,
				data_prevista               date     NOT NULL,
				data_ultima_manutencao      date     NULL
			);

			CREATE INDEX IX_tb_result_key
			ON #tb_result (codigo_empresa, codigo_unidade, codigo_equipamento, codigo_checklist_item)
			INCLUDE (codigo_tipo_ar_condicionado, data_prevista, data_ultima_manutencao);


			-------------------------------------------------------------------------
			-- Parte 2: itens previstos do checklist do tipo de ar
			-------------------------------------------------------------------------
			INSERT INTO #tb_result
			(
				codigo_empresa, codigo_unidade, codigo_equipamento, codigo_checklist,
				codigo_tipo_ar_condicionado, codigo_checklist_item,
				data_prevista, data_ultima_manutencao
			)
			SELECT
				e.codigo_empresa,
				e.codigo_unidade,
				e.codigo,
				i.codigo_checklist,
				e.codigo_tipo_ar_condicionado,
				i.codigo AS codigo_checklist_item,
				CAST(
					CASE ta.codigo_periodicidade
						WHEN 1 THEN DATEADD(DAY, ISNULL(i.intervalo, 1) , ISNULL(os_last.data, e.data_ultima_manutencao))
						WHEN 2 THEN DATEADD(DAY, -DATEPART(WEEKDAY, DATEADD(WEEK, ISNULL(i.intervalo, 1), ISNULL(os_last.data, e.data_ultima_manutencao))) + 1, DATEADD(WEEK, ISNULL(i.intervalo, 1), ISNULL(os_last.data, e.data_ultima_manutencao)))
						ELSE DATEFROMPARTS(DATEPART(YEAR, DATEADD(MONTH, i.intervalo, ISNULL(os_last.data, e.data_ultima_manutencao))),
											DATEPART(MONTH, DATEADD(MONTH, i.intervalo, ISNULL(os_last.data, e.data_ultima_manutencao))),
											1)
					END
				AS date) AS data_prevista,
				CAST(ISNULL(ISNULL(os_last.data, i.data_ultima_manutencao), e.data_ultima_manutencao) AS date) AS data_ultima_manutencao
			FROM tb_cad_equipamento e
			INNER JOIN tb_cad_tipo_ar_condicionado ta
				ON ta.codigo = e.codigo_tipo_ar_condicionado
			   AND ta.codigo_empresa = e.codigo_empresa
			INNER JOIN tb_chk_checklist_item i
				ON i.codigo_checklist = ta.codigo_checklist
			   AND i.codigo_empresa = ta.codigo_empresa
			OUTER APPLY
			(
				SELECT TOP (1) CAST(os.data AS date) AS data
				FROM tb_pmoc_ordem_servico os
				LEFT JOIN tb_pmoc_ordem_servico_checklist osc
					ON osc.codigo_pmoc_ordem_servico = os.codigo
				   AND osc.codigo_empresa = os.codigo_empresa
				   AND osc.codigo_unidade = os.codigo_unidade
				WHERE os.codigo_empresa     = @codigo_empresa
				  AND os.codigo_unidade     = @codigo_unidade
				  AND os.codigo_equipamento = @codigo_equipamento
				  AND os.status IN (2,4)
				  AND osc.codigo = i.codigo
				ORDER BY os.data DESC
			) os_last
			WHERE e.codigo_empresa = @codigo_empresa
			  AND e.codigo_unidade = @codigo_unidade
			  AND e.codigo         = @codigo_equipamento
			  AND NOT EXISTS
			  (
				  SELECT 1
				  FROM #tb_result r
				  WHERE r.codigo_empresa     = e.codigo_empresa
					AND r.codigo_unidade     = e.codigo_unidade
					AND r.codigo_equipamento = e.codigo
					AND r.codigo_checklist_item = i.codigo
			  );

			-------------------------------------------------------------------------
			-- Inserir no #itens no formato da sua API (grupo/subgrupo/item)
			-- (filtra até o fim do mês atual, como sua SP original)
			-------------------------------------------------------------------------
			INSERT INTO #itens
			(
				grupo, subgrupo, codigo_tipo_checklist, codigo, checklist, descricao,
				numero_digitos, allow_picture, uom, resultado, observacao,
				ordem_servico, prazo, color, associar_equipamento
			)
			SELECT
				ISNULL(i.grupo, 'GRUPO') AS grupo,
				ISNULL(i.subgrupo, '')   AS subgrupo,
				i.codigo_tipo_item_checklist AS codigo_tipo_checklist,
				i.codigo AS codigo,
				i.checklist,
				i.descricao,
				ISNULL(i.numero_digitos, 0),
				CASE WHEN ISNULL(i.allow_picture, 1) = 1 THEN 1 ELSE 0 END,
				ISNULL(um.sigla, ISNULL(i.unidade_medida, '')) AS uom,
				'' AS resultado,
				'' AS observacao,
				CONVERT(bit, 0) AS ordem_servico,
				CONVERT(varchar(10), MIN(r.data_prevista), 103) AS prazo, -- dd/MM/yyyy
				'#000000' AS color,
				1 AS associar_equipamento
			FROM #tb_result r
			INNER JOIN tb_chk_checklist_item i
				ON i.codigo_empresa = r.codigo_empresa
			   AND i.codigo = r.codigo_checklist_item
			   AND i.codigo_checklist = r.codigo_checklist
			LEFT JOIN tb_cad_unidade_medida um
				ON um.codigo_empresa = i.codigo_empresa
			   AND um.codigo = i.codigo_unidade_medida
			WHERE r.data_prevista < DATEADD(MONTH, 1, @MesAtual)
			GROUP BY
				ISNULL(i.grupo,'GRUPO'),
				ISNULL(i.subgrupo,''),
				i.codigo_tipo_item_checklist,
				i.codigo,
				i.checklist,
				i.descricao,
				ISNULL(i.numero_digitos, 0),
				CASE WHEN ISNULL(i.allow_picture, 1) = 1 THEN 1 ELSE 0 END,
				ISNULL(um.sigla, ISNULL(i.unidade_medida, ''))
			;
		END
		ELSE
		BEGIN
            INSERT INTO #itens
            SELECT
                ISNULL(grupo,'GRUPO'),
                ISNULL(subgrupo,''),
                codigo_tipo_item_checklist,
                codigo,
                checklist,
                descricao,
                ISNULL(numero_digitos,0),
                CASE WHEN ISNULL(allow_picture,1)=1 THEN 1 ELSE 0 END,
                ISNULL(unidade_medida,''),
                CASE WHEN ISNULL((SELECT checklist_preenchido FROM tb_cad_empresa WHERE codigo=@codigo_empresa),0)=1
                          AND codigo_tipo_item_checklist IN (1,8) THEN 'SIM' ELSE '' END,
                '',
                CONVERT(bit,1),
                '#000000',
				NULL,
				1 AS associar_equipamento
            FROM tb_chk_checklist_item
            WHERE codigo_empresa = @codigo_empresa
              AND codigo_checklist = @codigo_checklist
              AND ((intervalo <= @intervalo) OR (@intervalo = -1));
        END
    END

    -------------------------------------------------------------------------
    -- 3) Arquivos: 1 leitura só (sem chamar SP por item)
    -------------------------------------------------------------------------
    CREATE TABLE #arquivos
    (
        codigo_item  int NOT NULL,
        url          varchar(2000) NULL,
        extensao     varchar(20) NULL
    );

    IF (@codigo_documento <> -1)
    BEGIN
        INSERT INTO #arquivos (codigo_item, url, extensao)
        SELECT
            p.codigo_item_checklist,
            REPLACE(REPLACE(p.imagem, 'C:\SIM\PCM\SITE\', 'https://www.simservices.com.br/'), '\', '/') AS url,
            REVERSE(LEFT(REVERSE(p.imagem), CHARINDEX('.', REVERSE(p.imagem)) - 1)) AS extensao
        FROM tb_pcm_picture p
        WHERE p.codigo_empresa = @codigo_empresa
          AND p.codigo_unidade = @codigo_unidade
          AND p.codigo_documento = @codigo_documento
          AND (
                ( @tipo IN ('QUALIDADE','AUDITORIA_QUALIDADE') AND p.tipo IN ('QUALIDADE','AUDITORIA_QUALIDADE') )
                OR
                ( @tipo NOT IN ('QUALIDADE','AUDITORIA_QUALIDADE') AND p.tipo = @tipo )
              );
    END

    -------------------------------------------------------------------------
    -- 3.1) Listas de combo dos itens de DISCREPANCIA (tb_cad_discrepancia_gov)
    --      A query de cada item vive em [procedure_lista] e devolve (codigo, descricao).
    --      Só entra aqui item de discrepância que REALMENTE foi para o #itens.
    -------------------------------------------------------------------------
    CREATE TABLE #combo
    (
        codigo_item int          NOT NULL,   -- = tb_cad_discrepancia_gov.codigo * -1
        ordem       int          NOT NULL,
        codigo      varchar(100) NULL,
        descricao   varchar(500) NULL
    );

    CREATE TABLE #combo_raw
    (
        seq       int IDENTITY(1,1) NOT NULL,
        codigo    varchar(100) NULL,
        descricao varchar(500) NULL
    );

    DECLARE @disc_codigo int,
            @disc_sql    nvarchar(max);

    DECLARE cur_discrepancia CURSOR LOCAL FAST_FORWARD FOR
        SELECT d.codigo,
               CAST(d.procedure_lista AS nvarchar(max))
        FROM tb_cad_discrepancia_gov d
        WHERE d.codigo_empresa = @codigo_empresa
          AND d.codigo_unidade = @codigo_unidade
          AND NULLIF(LTRIM(RTRIM(d.procedure_lista)),'') IS NOT NULL
          AND EXISTS (SELECT 1 FROM #itens i WHERE i.codigo = d.codigo * -1);

    OPEN cur_discrepancia;
    FETCH NEXT FROM cur_discrepancia INTO @disc_codigo, @disc_sql;

    WHILE (@@FETCH_STATUS = 0)
    BEGIN
        BEGIN TRY
            DELETE FROM #combo_raw;

            -- A query configurada TEM que devolver 2 colunas: codigo, descricao
            INSERT INTO #combo_raw (codigo, descricao)
            EXEC sp_executesql @disc_sql;

            INSERT INTO #combo (codigo_item, ordem, codigo, descricao)
            SELECT @disc_codigo * -1, seq, codigo, descricao
            FROM #combo_raw;
        END TRY
        BEGIN CATCH
            -- Configuração inválida (SP inexistente, erro de sintaxe, nº de colunas errado):
            -- ignora este item. O checklist continua respondendo normalmente.
        END CATCH

        FETCH NEXT FROM cur_discrepancia INTO @disc_codigo, @disc_sql;
    END

    CLOSE cur_discrepancia;
    DEALLOCATE cur_discrepancia;

    -------------------------------------------------------------------------
    -- 4) RESULTSET 1: Grupos
    -------------------------------------------------------------------------
    SELECT
        g.grupo AS descricao,
        SUM(CASE WHEN ISNULL(NULLIF(i.subgrupo,''),'') <> '' THEN 1 ELSE 0 END) AS possui_subgrupo,
        SUM(CASE WHEN ISNULL(i.resultado,'') <> '' THEN 1 ELSE 0 END) AS totalOk,
        COUNT(*) AS total,
        @status AS status,
        @codigo_apartamento AS codigo_apartamento,
        @codigo_tipo_governanca AS codigo_tipo_governanca
    FROM #itens i
    CROSS APPLY (SELECT i.grupo) g
    GROUP BY g.grupo
    ORDER BY g.grupo;

    -------------------------------------------------------------------------
    -- 5) RESULTSET 2: Subgrupos (somente onde existir subgrupo)
    -------------------------------------------------------------------------
    SELECT
        grupo,
        subgrupo,
        SUM(CASE WHEN ISNULL(resultado,'') <> '' THEN 1 ELSE 0 END) AS totalOk,
        COUNT(*) AS total
    FROM #itens
    WHERE ISNULL(subgrupo,'') <> ''
    GROUP BY grupo, subgrupo
    ORDER BY grupo, subgrupo;

    -------------------------------------------------------------------------
    -- 6) RESULTSET 3: Itens
    -------------------------------------------------------------------------
    SELECT
        grupo,
        subgrupo,
        codigo_tipo_checklist,
        codigo,
        checklist,
        descricao,
        2 AS numero_digitos,
        allow_picture,
        uom,
        resultado,
        observacao,
        ordem_servico,
        '' AS prazo,
        color,
		1 AS associar_equipamento,
		NULL AS codigo_equipamento
    FROM #itens
    ORDER BY grupo, subgrupo, checklist;

    -------------------------------------------------------------------------
    -- 7) RESULTSET 4: Arquivos
    -------------------------------------------------------------------------
    SELECT
        codigo_item,
        url,
        extensao
    FROM #arquivos
    ORDER BY codigo_item, url;

    -------------------------------------------------------------------------
    -- 8) RESULTSET 5: listaCombo (itens de discrepância)
    -------------------------------------------------------------------------
    SELECT
        codigo_item,
        codigo,
        descricao
    FROM #combo
    ORDER BY codigo_item DESC, ordem;
END
