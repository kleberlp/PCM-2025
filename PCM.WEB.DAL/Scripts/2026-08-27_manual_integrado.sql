/**********************************************************************************************
    Script      : Manual integrado (botao "?" do cabecalho e telas de cadastro do help)
    Data        : 27/08/2026
    Descricao   : Estrutura e procedures do manual, no banco PCM.

    Um botao "?" no cabecalho abre o manual DA TELA em que o usuario esta. A tela se
    identifica por controller + action, que e o que ela sabe de si mesma sem carregar nada.

    Duas granularidades, na mesma tabela:

      tipo = 'S' (tela)     -> controller/action preenchidos. Com action vazia, o manual vale
                               para o modulo inteiro: qualquer tela daquele controller que nao
                               tenha manual proprio cai nele, em vez de abrir vazio.
      tipo = 'P' (processo) -> manual avulso, sem tela. As telas apontam para ele por
                               codigo_manual_processo e ele aparece como "ver tambem" no rodape.

    Multiempresa: codigo_empresa NULL = manual do sistema, vale para todas as empresas. Uma
    empresa que queira o proprio texto de uma tela grava a linha com o codigo dela, e essa
    linha vence a global na busca — sem afetar as demais empresas.

    Cada secao tem texto, destaque opcional (dica/aviso), imagem e HYPERLINK DE VIDEO:
    YouTube e Vimeo abrem incorporados no painel, outras URLs viram link.

    Chamadas do codigo web:
      - sp_select_manual_tela           (@codigo_empresa, @controller, @action)
      - sp_select_manual                (@codigo_empresa, @codigo)
      - sp_select_manual_index          (@codigo_empresa, @titulo)
      - sp_select_manual_combo_processo (@codigo_empresa)
      - sp_save_manual                  (@codigo, @codigo_empresa, @tipo, @controller, @action,
                                         @codigo_manual_processo, @titulo, @subtitulo, @ativo,
                                         @itens, @usuario)
      - sp_delete_manual                (@codigo)
**********************************************************************************************/

-- =============================================================================================
-- 1. ESTRUTURA
-- =============================================================================================

-- ---- Cabecalho ------------------------------------------------------------------------------
IF OBJECT_ID('dbo.tb_manual', 'U') IS NULL
BEGIN

    CREATE TABLE dbo.tb_manual (
        codigo                  int IDENTITY(1,1) NOT NULL,
        codigo_empresa          smallint     NULL,               -- NULL = vale para todas
        tipo                    varchar(1)   NOT NULL CONSTRAINT DF_tb_manual_tipo DEFAULT ('S'),
        controller              varchar(100) NULL,
        [action]                varchar(100) NULL,
        codigo_manual_processo  int          NULL,
        titulo                  varchar(200) NOT NULL,
        subtitulo               varchar(300) NULL,
        ativo                   bit          NOT NULL CONSTRAINT DF_tb_manual_ativo DEFAULT (1),
        usuario                 varchar(100) NULL,
        data_inclusao           datetime     NOT NULL CONSTRAINT DF_tb_manual_data DEFAULT (GETDATE()),
        usuario_alteracao       varchar(100) NULL,
        data_alteracao          datetime     NULL,
        CONSTRAINT PK_tb_manual PRIMARY KEY CLUSTERED (codigo),
        CONSTRAINT CK_tb_manual_tipo CHECK (tipo IN ('S', 'P')),
        CONSTRAINT FK_tb_manual_processo FOREIGN KEY (codigo_manual_processo)
            REFERENCES dbo.tb_manual (codigo)
    )

    CREATE INDEX IX_tb_manual_tela ON dbo.tb_manual (controller, [action], ativo)

END
GO

-- ---- Secoes ---------------------------------------------------------------------------------
IF OBJECT_ID('dbo.tb_manual_item', 'U') IS NULL
BEGIN

    CREATE TABLE dbo.tb_manual_item (
        codigo         int IDENTITY(1,1) NOT NULL,
        codigo_manual  int            NOT NULL,
        sequencia      int            NOT NULL,
        titulo         varchar(200)   NOT NULL,
        conteudo       nvarchar(max)  NULL,
        -- Destaque opcional ao pe da secao: D = dica, A = aviso.
        tipo_nota      varchar(1)     NULL,
        nota           nvarchar(1000) NULL,
        imagem         varchar(500)   NULL,
        -- Hyperlink de video da secao (YouTube, Vimeo ou URL direta).
        video          varchar(500)   NULL,
        ativo          bit            NOT NULL CONSTRAINT DF_tb_manual_item_ativo DEFAULT (1),
        CONSTRAINT PK_tb_manual_item PRIMARY KEY CLUSTERED (codigo),
        CONSTRAINT FK_tb_manual_item_manual FOREIGN KEY (codigo_manual)
            REFERENCES dbo.tb_manual (codigo) ON DELETE CASCADE
    )

    CREATE INDEX IX_tb_manual_item_manual ON dbo.tb_manual_item (codigo_manual, sequencia)

END
GO

-- Reexecucao em base que ja tinha a tabela sem o video (versao anterior do script).
IF COL_LENGTH('dbo.tb_manual_item', 'video') IS NULL
    ALTER TABLE dbo.tb_manual_item ADD video varchar(500) NULL
GO


-- =============================================================================================
-- 2. O MANUAL DA TELA ATUAL
--
-- Recebe controller e action porque e o que a tela sabe de si mesma. Devolve dois result sets:
-- o cabecalho e as secoes. Preferencia: manual da propria tela antes do manual do modulo, e
-- manual da empresa antes do manual do sistema.
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

    SELECT TOP 1 @codigo = m.codigo
    FROM   tb_manual m
    WHERE  m.ativo = 1
    AND    m.tipo  = 'S'
    AND    ISNULL(m.controller, '') = @controller
    AND    (ISNULL(m.[action], '') = @action OR ISNULL(m.[action], '') = '')
    AND    (m.codigo_empresa IS NULL OR m.codigo_empresa = @codigo_empresa)
    ORDER BY CASE WHEN ISNULL(m.[action], '') = @action THEN 0 ELSE 1 END,   -- a tela vence o modulo
             CASE WHEN m.codigo_empresa IS NOT NULL THEN 0 ELSE 1 END,       -- a empresa vence o global
             m.codigo;

    EXEC dbo.sp_select_manual @codigo_empresa = @codigo_empresa,
                              @codigo         = @codigo,
                              @somente_ativo  = 1;

END
GO


-- =============================================================================================
-- 3. UM MANUAL PELO CODIGO (tela de manutencao e link "ver tambem" do painel)
-- =============================================================================================
IF OBJECT_ID('dbo.sp_select_manual', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_manual];
GO

CREATE PROCEDURE [dbo].[sp_select_manual]
@codigo_empresa smallint,
@codigo         int = NULL,
@somente_ativo  bit = 0
AS
BEGIN

    SET NOCOUNT ON;

    -- ---- 1) Cabecalho ----
    -- Sem codigo (tela sem manual) o SELECT nao volta linha nenhuma, e o DAL devolve o objeto
    -- vazio: o painel mostra "esta tela ainda nao tem manual" em vez de erro.
    SELECT
        codigo                  = m.codigo,
        tipo                    = m.tipo,
        controller              = ISNULL(m.controller, ''),
        [action]                = ISNULL(m.[action], ''),
        codigo_manual_processo  = ISNULL(m.codigo_manual_processo, 0),
        processo_titulo         = ISNULL(p.titulo, ''),
        titulo                  = m.titulo,
        subtitulo               = ISNULL(m.subtitulo, ''),
        ativo                   = m.ativo
    FROM   tb_manual m
    LEFT   JOIN tb_manual p
           ON  p.codigo = m.codigo_manual_processo
           AND p.ativo  = 1
    WHERE  m.codigo = @codigo
    AND    (@somente_ativo = 0 OR m.ativo = 1)
    AND    (m.codigo_empresa IS NULL OR m.codigo_empresa = @codigo_empresa);

    -- ---- 2) Secoes ----
    SELECT
        codigo    = i.codigo,
        sequencia = i.sequencia,
        titulo    = i.titulo,
        conteudo  = ISNULL(i.conteudo, ''),
        tipo_nota = ISNULL(i.tipo_nota, ''),
        nota      = ISNULL(i.nota, ''),
        imagem    = ISNULL(i.imagem, ''),
        video     = ISNULL(i.video, '')
    FROM   tb_manual_item i
    WHERE  i.codigo_manual = @codigo
    AND    i.ativo = 1
    ORDER  BY i.sequencia, i.codigo;

END
GO


-- =============================================================================================
-- 4. GRADE DA MANUTENCAO
--
-- Mostra o que a tela precisa para escolher: de que tela (ou processo) e o manual e quantas
-- secoes tem. Uma coluna so para os dois casos — quem le a grade quer saber a que a ajuda
-- pertence, e nao se e tela ou processo.
-- =============================================================================================
IF OBJECT_ID('dbo.sp_select_manual_index', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_manual_index];
GO

CREATE PROCEDURE [dbo].[sp_select_manual_index]
@codigo_empresa smallint,
@titulo         varchar(200) = ''
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        codigo    = m.codigo,
        tipo      = m.tipo,
        titulo    = m.titulo,
        subtitulo = ISNULL(m.subtitulo, ''),
        tela      = CASE WHEN m.tipo = 'P' THEN ''
                         ELSE ISNULL(m.controller, '')
                              + CASE WHEN ISNULL(m.[action], '') = '' THEN ''
                                     ELSE '/' + m.[action] END END,
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
-- 5. MANUAIS DE PROCESSO (combo "ver tambem" da tela de cadastro)
-- =============================================================================================
IF OBJECT_ID('dbo.sp_select_manual_combo_processo', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_manual_combo_processo];
GO

CREATE PROCEDURE [dbo].[sp_select_manual_combo_processo]
@codigo_empresa smallint
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        codigo    = m.codigo,
        descricao = m.titulo
    FROM   tb_manual m
    WHERE  m.tipo  = 'P'
    AND    m.ativo = 1
    AND    (m.codigo_empresa IS NULL OR m.codigo_empresa = @codigo_empresa)
    ORDER  BY m.titulo;

END
GO


-- =============================================================================================
-- 6. GRAVA O MANUAL INTEIRO — CABECALHO E SECOES
--
-- As secoes vem em JSON e substituem as que existiam. Editar manual e mexer na ordem e no texto
-- das secoes ao mesmo tempo; salvar tudo de uma vez evita meio-caminho gravado quando o
-- navegador cai no meio da edicao.
--
-- OPENJSON exige compatibility level 130+ (SQL Server 2016). Em base mais antiga, troque o
-- INSERT ... SELECT FROM OPENJSON por um loop de INSERTs a partir do DAL.
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
@titulo                 varchar(200),
@subtitulo              varchar(300),
@ativo                  bit,
@itens                  nvarchar(max),
@usuario                varchar(100)
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
                titulo    varchar(200)   '$.titulo',
                conteudo  nvarchar(max)  '$.conteudo',
                tipo_nota varchar(1)     '$.tipo_nota',
                nota      nvarchar(1000) '$.nota',
                imagem    varchar(500)   '$.imagem',
                video     varchar(500)   '$.video'
            ) j
            WHERE LTRIM(RTRIM(ISNULL(j.titulo, ''))) <> '';

        COMMIT TRANSACTION;

        SELECT codigo = @codigo;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH

END
GO


-- =============================================================================================
-- 7. EXCLUSAO
-- =============================================================================================
IF OBJECT_ID('dbo.sp_delete_manual', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_delete_manual];
GO

CREATE PROCEDURE [dbo].[sp_delete_manual]
@codigo int
AS
BEGIN

    SET NOCOUNT ON;

    -- Telas que apontavam para este processo perdem so o link do rodape.
    UPDATE tb_manual SET codigo_manual_processo = NULL WHERE codigo_manual_processo = @codigo;

    -- As secoes saem junto pelo ON DELETE CASCADE.
    DELETE FROM tb_manual WHERE codigo = @codigo;

END
GO


-- =============================================================================================
-- 8. PERMISSAO DA MANUTENCAO
--
-- A permissao segue o padrao do PCM: um formulario no cadastro de perfis, lido por
-- sp_select_cadastro_basico_perfil_direito_dados com formulario = 'adm_manual'.
--
-- Enquanto 'adm_manual' nao existir, o sistema usa como reserva o direito de 'adm_perfil'
-- (quem administra perfis tambem mantem o manual) — ou seja, o manual ja funciona sem esta
-- parte. Confira na sua base o nome real da tabela de formularios antes de rodar: o SELECT
-- abaixo so mostra onde 'adm_perfil' esta cadastrado, sem alterar nada.
-- =============================================================================================

SELECT  t.name AS tabela_de_formularios, c.name AS coluna
FROM    sys.columns c
JOIN    sys.tables  t ON t.object_id = c.object_id
WHERE   c.name = 'formulario'
ORDER   BY t.name;
GO

-- Depois de identificar a tabela acima, descomente e ajuste:
--
-- IF NOT EXISTS (SELECT 1 FROM <tabela_de_formularios> WHERE formulario = 'adm_manual')
--     INSERT INTO <tabela_de_formularios> (formulario, descricao, ativo)
--     VALUES ('adm_manual', 'Manual', 1);
