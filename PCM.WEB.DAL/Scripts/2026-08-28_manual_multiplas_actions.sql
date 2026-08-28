-- =============================================================================================
--  MANUAL — UM MANUAL PARA VARIAS TELAS
--
--  Ate aqui a associacao era 1 manual = 1 action (colunas controller/[action] da tb_manual).
--  Telas irmas (ex.: OrdemServicoIndex2, OrdemServicoIndex3 e OrdemServicoKanban) obrigavam a
--  duplicar o texto do manual.
--
--  Passa a existir a tb_manual_tela: N telas por manual. As colunas controller/[action] da
--  tb_manual continuam existindo como a tela PRINCIPAL (compatibilidade e ordenacao do index);
--  as telas adicionais moram na tabela filha.
--
--  Ordem de precedencia na busca (sp_select_manual_tela), da mais especifica para a mais geral:
--    1. manual da propria action (principal ou adicional) da empresa
--    2. manual da propria action global
--    3. manual do modulo (action vazia) da empresa
--    4. manual do modulo global
--
--  Idempotente: pode rodar mais de uma vez.
--  Autor: manutencao PCM · Data: 28/08/2026
-- =============================================================================================

-- ---- Telas adicionais do manual --------------------------------------------------------------
IF OBJECT_ID('dbo.tb_manual_tela', 'U') IS NULL
BEGIN

    CREATE TABLE dbo.tb_manual_tela (
        codigo         int IDENTITY(1,1) NOT NULL,
        codigo_manual  int          NOT NULL,
        controller     varchar(100) NOT NULL,
        -- NULL/vazio = modulo inteiro daquele controller
        [action]       varchar(100) NULL,
        CONSTRAINT PK_tb_manual_tela PRIMARY KEY CLUSTERED (codigo),
        CONSTRAINT FK_tb_manual_tela_manual FOREIGN KEY (codigo_manual)
            REFERENCES dbo.tb_manual (codigo) ON DELETE CASCADE
    )

    CREATE INDEX IX_tb_manual_tela_busca ON dbo.tb_manual_tela (controller, [action], codigo_manual)

END
GO


-- =============================================================================================
-- 1. MANUAL DA TELA — agora olha tambem as telas adicionais
-- =============================================================================================
IF OBJECT_ID('dbo.sp_select_manual_tela', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_manual_tela];
GO

CREATE PROCEDURE [dbo].[sp_select_manual_tela]
@codigo_empresa smallint,
@controller     varchar(100),
@action         varchar(100) = ''
AS
BEGIN

    SET NOCOUNT ON;

    DECLARE @codigo int;

    -- Candidatos: a tela principal do manual + cada tela adicional.
    ;WITH telas AS (
        SELECT  m.codigo,
                m.codigo_empresa,
                controller = ISNULL(m.controller, ''),
                [action]   = ISNULL(m.[action], '')
        FROM    tb_manual m
        WHERE   m.ativo = 1
        AND     m.tipo  = 'S'

        UNION ALL

        SELECT  m.codigo,
                m.codigo_empresa,
                controller = ISNULL(t.controller, ''),
                [action]   = ISNULL(t.[action], '')
        FROM    tb_manual m
                INNER JOIN tb_manual_tela t ON t.codigo_manual = m.codigo
        WHERE   m.ativo = 1
        AND     m.tipo  = 'S'
    )
    SELECT TOP 1 @codigo = c.codigo
    FROM   telas c
    WHERE  c.controller = @controller
    AND    (c.[action] = @action OR c.[action] = '')
    AND    (c.codigo_empresa IS NULL OR c.codigo_empresa = @codigo_empresa)
    ORDER BY CASE WHEN c.[action] = @action THEN 0 ELSE 1 END,          -- a tela vence o modulo
             CASE WHEN c.codigo_empresa IS NOT NULL THEN 0 ELSE 1 END,  -- a empresa vence o global
             c.codigo;

    EXEC dbo.sp_select_manual @codigo_empresa = @codigo_empresa,
                              @codigo         = @codigo,
                              @somente_ativo  = 1;

END
GO


-- =============================================================================================
-- 2. LISTA DE MANUAIS — a coluna "tela" mostra todas as telas atendidas
-- =============================================================================================
IF OBJECT_ID('dbo.sp_select_manual_index', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_manual_index];
GO

CREATE PROCEDURE [dbo].[sp_select_manual_index]
@codigo_empresa smallint,
@titulo         nvarchar(200) = ''
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        codigo    = m.codigo,
        tipo      = m.tipo,
        titulo    = m.titulo,
        subtitulo = ISNULL(m.subtitulo, ''),
        tela      = CASE WHEN m.tipo = 'P' THEN ''
                         ELSE STUFF((
                                SELECT ', ' + x.tela
                                FROM (
                                    SELECT tela = ISNULL(m.controller, '')
                                                + CASE WHEN ISNULL(m.[action], '') = '' THEN ''
                                                       ELSE '/' + m.[action] END
                                    UNION ALL
                                    SELECT tela = t.controller
                                                + CASE WHEN ISNULL(t.[action], '') = '' THEN ''
                                                       ELSE '/' + t.[action] END
                                    FROM   tb_manual_tela t
                                    WHERE  t.codigo_manual = m.codigo
                                ) x
                                ORDER BY x.tela
                                FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)'), 1, 2, '')
                    END,
        secoes    = (SELECT COUNT(*) FROM tb_manual_item i
                     WHERE i.codigo_manual = m.codigo AND i.ativo = 1),
        ativo     = m.ativo
    FROM   tb_manual m
    WHERE  (m.codigo_empresa IS NULL OR m.codigo_empresa = @codigo_empresa)
    AND    (ISNULL(@titulo, '') = '' OR m.titulo LIKE '%' + @titulo + '%')
    ORDER  BY m.titulo;

END
GO


-- =============================================================================================
-- 3. UM MANUAL PELO CODIGO — devolve tambem as telas adicionais (3o result set)
--
--    Result sets: 1) cabecalho  2) secoes  3) telas adicionais
--    O DAL le o 3o quando existir; base antiga sem a tabela continua com 2.
-- =============================================================================================
IF OBJECT_ID('dbo.sp_select_manual_tela_lista', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_manual_tela_lista];
GO

CREATE PROCEDURE [dbo].[sp_select_manual_tela_lista]
@codigo int
AS
BEGIN

    SET NOCOUNT ON;

    SELECT  controller = t.controller,
            [action]   = ISNULL(t.[action], '')
    FROM    tb_manual_tela t
    WHERE   t.codigo_manual = @codigo
    ORDER   BY t.controller, ISNULL(t.[action], '');

END
GO


-- =============================================================================================
-- 4. GRAVA O MANUAL — telas adicionais em JSON ('[]' limpa; NULL/vazio preserva as gravadas)
-- =============================================================================================
IF OBJECT_ID('dbo.sp_save_manual', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_save_manual];
GO

CREATE PROCEDURE [dbo].[sp_save_manual]
@codigo                 int,
@codigo_empresa         smallint,
@tipo                   varchar(1),
@controller             varchar(100),
@action                 varchar(100),
@codigo_manual_processo int,
@titulo                 nvarchar(200),
@subtitulo              nvarchar(300),
@ativo                  bit,
@itens                  nvarchar(max),
@usuario                varchar(100),
-- [{"controller":"OrdemServico","action":"OrdemServicoKanban"}, ...] — telas ALEM da principal
@telas                  nvarchar(max) = NULL
AS
BEGIN

    SET NOCOUNT ON;

    -- Um manual e de uma coisa so: processo nao tem tela nem "ver tambem"; e vazio vira NULL,
    -- para o indice e a leitura nao terem que tratar string vazia como "sem tela".
    DECLARE @tipoOk       varchar(1)   = CASE WHEN @tipo = 'P' THEN 'P' ELSE 'S' END;
    DECLARE @controllerOk varchar(100) = CASE WHEN @tipo = 'P' THEN NULL
                                              ELSE NULLIF(LTRIM(RTRIM(ISNULL(@controller, ''))), '') END;
    DECLARE @actionOk     varchar(100) = CASE WHEN @tipo = 'P' THEN NULL
                                              ELSE NULLIF(LTRIM(RTRIM(ISNULL(@action, ''))), '') END;
    DECLARE @processoOk   int          = CASE WHEN @tipo = 'P' THEN NULL
                                              ELSE NULLIF(ISNULL(@codigo_manual_processo, 0), 0) END;

    IF @tipoOk = 'S' AND @controllerOk IS NULL
    BEGIN
        RAISERROR('Informe a tela (controller) do manual.', 16, 1);
        RETURN;
    END

    IF LTRIM(RTRIM(ISNULL(@titulo, ''))) = ''
    BEGIN
        RAISERROR('Informe o titulo do manual.', 16, 1);
        RETURN;
    END

    -- Um manual apontando para si mesmo faria o rodape do painel oferecer a propria pagina.
    IF @processoOk = @codigo SET @processoOk = NULL;

    BEGIN TRANSACTION;

    BEGIN TRY

        IF ISNULL(@codigo, 0) = 0
        BEGIN

            INSERT INTO tb_manual
                (codigo_empresa, tipo, controller, [action], codigo_manual_processo,
                 titulo, subtitulo, ativo, usuario, data_inclusao)
            VALUES
                (NULLIF(@codigo_empresa, 0), @tipoOk, @controllerOk, @actionOk, @processoOk,
                 @titulo, NULLIF(@subtitulo, ''), @ativo, @usuario, GETDATE());

            SET @codigo = SCOPE_IDENTITY();

        END
        ELSE
        BEGIN

            UPDATE tb_manual
            SET    tipo                   = @tipoOk,
                   controller             = @controllerOk,
                   [action]               = @actionOk,
                   codigo_manual_processo = @processoOk,
                   titulo                 = @titulo,
                   subtitulo              = NULLIF(@subtitulo, ''),
                   ativo                  = @ativo,
                   usuario_alteracao      = @usuario,
                   data_alteracao         = GETDATE()
            WHERE  codigo = @codigo;

            DELETE FROM tb_manual_item WHERE codigo_manual = @codigo;

        END

        -- Telas adicionais: @telas NULL ou vazio PRESERVA o que esta gravado — e um chamador
        -- que nao carregou a lista (DLL/JS antigos no ar), e salvar o texto do manual nao pode
        -- apagar associacoes por tabela. JSON presente (mesmo '[]') substitui por inteiro; e
        -- manual de processo nao tem tela, entao sempre limpa.
        IF @tipoOk = 'P' OR ISNULL(@telas, '') <> ''
            DELETE FROM tb_manual_tela WHERE codigo_manual = @codigo;

        IF @tipoOk = 'S' AND ISNULL(@telas, '') <> ''
            INSERT INTO tb_manual_tela (codigo_manual, controller, [action])
            SELECT DISTINCT
                @codigo,
                LTRIM(RTRIM(j.controller)),
                NULLIF(LTRIM(RTRIM(ISNULL(j.[action], ''))), '')
            FROM OPENJSON(@telas)
            WITH (
                controller varchar(100) '$.controller',
                [action]   varchar(100) '$.action'
            ) j
            WHERE LTRIM(RTRIM(ISNULL(j.controller, ''))) <> ''
            -- a tela principal ja esta na tb_manual: nao duplicar
            AND   NOT (LTRIM(RTRIM(j.controller)) = ISNULL(@controllerOk, '')
                       AND ISNULL(NULLIF(LTRIM(RTRIM(ISNULL(j.[action], ''))), ''), '') = ISNULL(@actionOk, ''));

        IF ISNULL(@itens, '') <> ''
            INSERT INTO tb_manual_item
                (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)
            SELECT
                @codigo,
                ROW_NUMBER() OVER (ORDER BY j.sequencia),
                j.titulo,
                NULLIF(j.conteudo, ''),
                NULLIF(j.tipo_nota, ''),
                NULLIF(j.nota, ''),
                NULLIF(j.imagem, ''),
                NULLIF(j.video, ''),
                1
            FROM OPENJSON(@itens)
            WITH (
                sequencia int            '$.sequencia',
                titulo    nvarchar(200)  '$.titulo',
                conteudo  nvarchar(max)  '$.conteudo',
                tipo_nota varchar(1)     '$.tipo_nota',
                nota      nvarchar(1000) '$.nota',
                imagem    varchar(500)   '$.imagem',
                video     varchar(500)   '$.video'
            ) j
            WHERE LTRIM(RTRIM(ISNULL(j.titulo, ''))) <> '';

        COMMIT TRANSACTION;

        SELECT @codigo;

    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;

        DECLARE @erro nvarchar(4000) = ERROR_MESSAGE();
        RAISERROR(@erro, 16, 1);

    END CATCH

END
GO
