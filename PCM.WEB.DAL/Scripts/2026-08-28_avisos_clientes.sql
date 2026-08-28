-- =============================================================================================
--  AVISOS AOS CLIENTES
--
--  Aviso com periodo de vigencia, alvo por empresa/unidade (NULL = todas), secoes 1:N em HTML
--  (cada secao e um passo do carrossel do popup exibido no login), auditoria opcional
--  (quem viu, quantas vezes, avaliacao e dispensa) e avaliacao opcional (5 estrelas).
--
--  O estado por usuario (tb_aviso_usuario) existe SEMPRE: sem ele nao ha como suprimir o
--  aviso dispensado nem guardar a avaliacao. O flag "auditado" controla a exibicao da tela
--  de auditoria e o aviso de transparencia no popup.
--
--  Permissao: formulario 'adm_aviso' no cadastro de perfis; enquanto nao existir, o sistema
--  usa como reserva o direito de 'adm_perfil' (mesmo padrao do manual integrado).
--
--  Idempotente: pode rodar mais de uma vez.
--  Autor: manutencao PCM · Data: 28/08/2026
-- =============================================================================================

-- ---- Aviso ----------------------------------------------------------------------------------
IF OBJECT_ID('dbo.tb_aviso', 'U') IS NULL
BEGIN

    CREATE TABLE dbo.tb_aviso (
        codigo             int IDENTITY(1,1) NOT NULL,
        codigo_empresa     smallint      NULL,               -- NULL = todas as empresas
        codigo_unidade     int           NULL,               -- NULL = todas as unidades
        titulo             nvarchar(200) NOT NULL,
        data_inicio        date          NOT NULL,
        data_termino       date          NOT NULL,
        auditado           bit           NOT NULL CONSTRAINT DF_tb_aviso_auditado DEFAULT (0),
        avaliado           bit           NOT NULL CONSTRAINT DF_tb_aviso_avaliado DEFAULT (0),
        ativo              bit           NOT NULL CONSTRAINT DF_tb_aviso_ativo DEFAULT (1),
        usuario            varchar(100)  NULL,
        data_inclusao      datetime      NOT NULL CONSTRAINT DF_tb_aviso_data DEFAULT (GETDATE()),
        usuario_alteracao  varchar(100)  NULL,
        data_alteracao     datetime      NULL,
        CONSTRAINT PK_tb_aviso PRIMARY KEY CLUSTERED (codigo),
        CONSTRAINT CK_tb_aviso_periodo CHECK (data_termino >= data_inicio)
    )

    CREATE INDEX IX_tb_aviso_vigencia ON dbo.tb_aviso (ativo, data_inicio, data_termino)

END
GO

-- ---- Secoes (passos do carrossel) -----------------------------------------------------------
IF OBJECT_ID('dbo.tb_aviso_secao', 'U') IS NULL
BEGIN

    CREATE TABLE dbo.tb_aviso_secao (
        codigo        int IDENTITY(1,1) NOT NULL,
        codigo_aviso  int            NOT NULL,
        sequencia     int            NOT NULL,
        titulo        nvarchar(200)  NOT NULL,
        conteudo      nvarchar(max)  NULL,     -- HTML (sanitizado na gravacao pela aplicacao)
        CONSTRAINT PK_tb_aviso_secao PRIMARY KEY CLUSTERED (codigo),
        CONSTRAINT FK_tb_aviso_secao_aviso FOREIGN KEY (codigo_aviso)
            REFERENCES dbo.tb_aviso (codigo) ON DELETE CASCADE
    )

    CREATE INDEX IX_tb_aviso_secao_aviso ON dbo.tb_aviso_secao (codigo_aviso, sequencia)

END
GO

-- ---- Estado/log por usuario -----------------------------------------------------------------
IF OBJECT_ID('dbo.tb_aviso_usuario', 'U') IS NULL
BEGIN

    CREATE TABLE dbo.tb_aviso_usuario (
        codigo                 int IDENTITY(1,1) NOT NULL,
        codigo_aviso           int      NOT NULL,
        codigo_empresa         smallint NOT NULL,   -- empresa do usuario (a chave de usuario e por empresa)
        codigo_usuario         int      NOT NULL,
        exibicoes              int      NOT NULL CONSTRAINT DF_tb_aviso_usuario_exib DEFAULT (0),
        primeira_visualizacao  datetime NULL,
        ultima_visualizacao    datetime NULL,
        avaliacao              tinyint  NULL,       -- 1 a 5
        data_avaliacao         datetime NULL,
        nao_ver_mais           bit      NOT NULL CONSTRAINT DF_tb_aviso_usuario_dispensa DEFAULT (0),
        data_dispensa          datetime NULL,
        CONSTRAINT PK_tb_aviso_usuario PRIMARY KEY CLUSTERED (codigo),
        CONSTRAINT FK_tb_aviso_usuario_aviso FOREIGN KEY (codigo_aviso)
            REFERENCES dbo.tb_aviso (codigo) ON DELETE CASCADE,
        CONSTRAINT UQ_tb_aviso_usuario UNIQUE (codigo_aviso, codigo_empresa, codigo_usuario),
        CONSTRAINT CK_tb_aviso_usuario_nota CHECK (avaliacao IS NULL OR avaliacao BETWEEN 1 AND 5)
    )

END
GO


-- =============================================================================================
-- 1. LISTA DA MANUTENCAO
-- =============================================================================================
IF OBJECT_ID('dbo.sp_select_aviso_index', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_aviso_index];
GO

CREATE PROCEDURE [dbo].[sp_select_aviso_index]
@codigo_empresa smallint
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        codigo          = a.codigo,
        titulo          = a.titulo,
        data_inicio     = FORMAT(a.data_inicio, 'dd/MM/yyyy'),
        data_termino    = FORMAT(a.data_termino, 'dd/MM/yyyy'),
        codigo_empresa  = ISNULL(a.codigo_empresa, -1),      -- -1 = todas (a tela resolve o nome)
        codigo_unidade  = ISNULL(a.codigo_unidade, -1),
        unidade         = ISNULL(u.nome_fantasia, ''),
        auditado        = a.auditado,
        avaliado        = a.avaliado,
        ativo           = a.ativo,
        -- situacao pelo calendario: 1 agendado, 2 ativo, 3 encerrado (0 = desativado)
        situacao        = CASE
                             WHEN a.ativo = 0 THEN 0
                             WHEN CONVERT(date, GETDATE()) < a.data_inicio  THEN 1
                             WHEN CONVERT(date, GETDATE()) > a.data_termino THEN 3
                             ELSE 2
                          END,
        visualizacoes   = ISNULL((SELECT SUM(l.exibicoes) FROM tb_aviso_usuario l WHERE l.codigo_aviso = a.codigo), 0),
        avaliacoes      = (SELECT COUNT(*) FROM tb_aviso_usuario l WHERE l.codigo_aviso = a.codigo AND l.avaliacao IS NOT NULL),
        media_avaliacao = ISNULL((SELECT AVG(CONVERT(decimal(4,2), l.avaliacao))
                                  FROM tb_aviso_usuario l
                                  WHERE l.codigo_aviso = a.codigo AND l.avaliacao IS NOT NULL), 0)
    FROM   tb_aviso a LEFT JOIN
           tb_cad_unidade u ON
           a.codigo_unidade = u.codigo AND
           a.codigo_empresa = u.codigo_empresa
    WHERE  (a.codigo_empresa IS NULL OR a.codigo_empresa = @codigo_empresa OR @codigo_empresa = -1)
    ORDER  BY a.data_inicio DESC, a.codigo DESC;

END
GO


-- =============================================================================================
-- 2. UM AVISO (edicao) — cabecalho + secoes
-- =============================================================================================
IF OBJECT_ID('dbo.sp_select_aviso', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_aviso];
GO

CREATE PROCEDURE [dbo].[sp_select_aviso]
@codigo int
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        codigo         = a.codigo,
        titulo         = a.titulo,
        data_inicio    = FORMAT(a.data_inicio, 'yyyy-MM-dd'),
        data_termino   = FORMAT(a.data_termino, 'yyyy-MM-dd'),
        codigo_empresa = ISNULL(a.codigo_empresa, -1),
        codigo_unidade = ISNULL(a.codigo_unidade, -1),
        auditado       = a.auditado,
        avaliado       = a.avaliado,
        ativo          = a.ativo
    FROM   tb_aviso a
    WHERE  a.codigo = @codigo;

    SELECT
        codigo    = s.codigo,
        sequencia = s.sequencia,
        titulo    = s.titulo,
        conteudo  = ISNULL(s.conteudo, '')
    FROM   tb_aviso_secao s
    WHERE  s.codigo_aviso = @codigo
    ORDER  BY s.sequencia;

END
GO


-- =============================================================================================
-- 3. GRAVA O AVISO — cabecalho e secoes (JSON) numa transacao, como o sp_save_manual
-- =============================================================================================
IF OBJECT_ID('dbo.sp_save_aviso', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_save_aviso];
GO

CREATE PROCEDURE [dbo].[sp_save_aviso]
@codigo         int,
@codigo_empresa smallint,       -- -1 = todas
@codigo_unidade int,            -- -1 = todas
@titulo         nvarchar(200),
@data_inicio    date,
@data_termino   date,
@auditado       bit,
@avaliado       bit,
@ativo          bit,
@secoes         nvarchar(max),  -- [{"sequencia":1,"titulo":"...","conteudo":"<p>...</p>"}]
@usuario        varchar(100)
AS
BEGIN

    SET NOCOUNT ON;

    DECLARE @empresaOk smallint = NULLIF(@codigo_empresa, -1);
    -- unidade so faz sentido com empresa definida
    DECLARE @unidadeOk int      = CASE WHEN @empresaOk IS NULL THEN NULL ELSE NULLIF(@codigo_unidade, -1) END;

    IF LTRIM(RTRIM(ISNULL(@titulo, ''))) = ''
    BEGIN
        RAISERROR('Informe o titulo do aviso.', 16, 1);
        RETURN;
    END

    IF @data_inicio IS NULL OR @data_termino IS NULL OR @data_termino < @data_inicio
    BEGIN
        RAISERROR('Periodo invalido: a data de termino deve ser igual ou posterior a de inicio.', 16, 1);
        RETURN;
    END

    BEGIN TRANSACTION;

    BEGIN TRY

        IF ISNULL(@codigo, 0) = 0
        BEGIN

            INSERT INTO tb_aviso
                (codigo_empresa, codigo_unidade, titulo, data_inicio, data_termino,
                 auditado, avaliado, ativo, usuario, data_inclusao)
            VALUES
                (@empresaOk, @unidadeOk, @titulo, @data_inicio, @data_termino,
                 @auditado, @avaliado, @ativo, @usuario, GETDATE());

            SET @codigo = SCOPE_IDENTITY();

        END
        ELSE
        BEGIN

            UPDATE tb_aviso
            SET    codigo_empresa    = @empresaOk,
                   codigo_unidade    = @unidadeOk,
                   titulo            = @titulo,
                   data_inicio       = @data_inicio,
                   data_termino      = @data_termino,
                   auditado          = @auditado,
                   avaliado          = @avaliado,
                   ativo             = @ativo,
                   usuario_alteracao = @usuario,
                   data_alteracao    = GETDATE()
            WHERE  codigo = @codigo;

            DELETE FROM tb_aviso_secao WHERE codigo_aviso = @codigo;

        END

        IF ISNULL(@secoes, '') <> ''
            INSERT INTO tb_aviso_secao (codigo_aviso, sequencia, titulo, conteudo)
            SELECT
                @codigo,
                ROW_NUMBER() OVER (ORDER BY j.sequencia),
                j.titulo,
                NULLIF(j.conteudo, '')
            FROM OPENJSON(@secoes)
            WITH (
                sequencia int           '$.sequencia',
                titulo    nvarchar(200) '$.titulo',
                conteudo  nvarchar(max) '$.conteudo'
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


-- =============================================================================================
-- 4. EXCLUI O AVISO (secoes e log caem pelo CASCADE)
-- =============================================================================================
IF OBJECT_ID('dbo.sp_delete_aviso', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_delete_aviso];
GO

CREATE PROCEDURE [dbo].[sp_delete_aviso]
@codigo int
AS
BEGIN

    SET NOCOUNT ON;

    DELETE FROM tb_aviso WHERE codigo = @codigo;

END
GO


-- =============================================================================================
-- 5. AVISOS PENDENTES DO LOGIN — vigentes, do alvo do usuario e ainda nao dispensados
--    Result sets: 1) avisos  2) secoes (com codigo_aviso, para o popup montar cada carrossel)
-- =============================================================================================
IF OBJECT_ID('dbo.sp_select_aviso_login', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_aviso_login];
GO

CREATE PROCEDURE [dbo].[sp_select_aviso_login]
@codigo_empresa smallint,
@codigo_unidade int,
@codigo_usuario int
AS
BEGIN

    SET NOCOUNT ON;

    DECLARE @pendentes AS table (codigo int PRIMARY KEY);

    INSERT INTO @pendentes
    SELECT a.codigo
    FROM   tb_aviso a
    WHERE  a.ativo = 1
    AND    CONVERT(date, GETDATE()) BETWEEN a.data_inicio AND a.data_termino
    AND    (a.codigo_empresa IS NULL OR a.codigo_empresa = @codigo_empresa)
    AND    (a.codigo_unidade IS NULL OR a.codigo_unidade = @codigo_unidade)
    AND    NOT EXISTS (SELECT 1
                       FROM tb_aviso_usuario l
                       WHERE l.codigo_aviso   = a.codigo
                       AND   l.codigo_empresa = @codigo_empresa
                       AND   l.codigo_usuario = @codigo_usuario
                       AND   l.nao_ver_mais   = 1);

    SELECT
        codigo       = a.codigo,
        titulo       = a.titulo,
        data_termino = FORMAT(a.data_termino, 'dd/MM/yyyy'),
        auditado     = a.auditado,
        avaliado     = a.avaliado,
        -- avaliacao ja dada em login anterior (popup mostra as estrelas preenchidas)
        avaliacao    = ISNULL((SELECT l.avaliacao FROM tb_aviso_usuario l
                               WHERE l.codigo_aviso = a.codigo
                               AND   l.codigo_empresa = @codigo_empresa
                               AND   l.codigo_usuario = @codigo_usuario), 0)
    FROM   tb_aviso a INNER JOIN
           @pendentes p ON a.codigo = p.codigo
    ORDER  BY a.data_inicio, a.codigo;

    SELECT
        codigo_aviso = s.codigo_aviso,
        sequencia    = s.sequencia,
        titulo       = s.titulo,
        conteudo     = ISNULL(s.conteudo, '')
    FROM   tb_aviso_secao s INNER JOIN
           @pendentes p ON s.codigo_aviso = p.codigo
    ORDER  BY s.codigo_aviso, s.sequencia;

END
GO


-- =============================================================================================
-- 6. REGISTROS DO USUARIO (upsert por aviso+empresa+usuario)
-- =============================================================================================
IF OBJECT_ID('dbo.sp_update_aviso_visualizacao', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_update_aviso_visualizacao];
GO

CREATE PROCEDURE [dbo].[sp_update_aviso_visualizacao]
@codigo_aviso   int,
@codigo_empresa smallint,
@codigo_usuario int
AS
BEGIN

    SET NOCOUNT ON;

    UPDATE tb_aviso_usuario
    SET    exibicoes = exibicoes + 1,
           primeira_visualizacao = ISNULL(primeira_visualizacao, GETDATE()),
           ultima_visualizacao   = GETDATE()
    WHERE  codigo_aviso = @codigo_aviso AND codigo_empresa = @codigo_empresa AND codigo_usuario = @codigo_usuario;

    IF @@ROWCOUNT = 0
        INSERT INTO tb_aviso_usuario
            (codigo_aviso, codigo_empresa, codigo_usuario, exibicoes, primeira_visualizacao, ultima_visualizacao)
        VALUES
            (@codigo_aviso, @codigo_empresa, @codigo_usuario, 1, GETDATE(), GETDATE());

END
GO

IF OBJECT_ID('dbo.sp_update_aviso_avaliacao', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_update_aviso_avaliacao];
GO

CREATE PROCEDURE [dbo].[sp_update_aviso_avaliacao]
@codigo_aviso   int,
@codigo_empresa smallint,
@codigo_usuario int,
@avaliacao      tinyint
AS
BEGIN

    SET NOCOUNT ON;

    IF @avaliacao IS NULL OR @avaliacao < 1 OR @avaliacao > 5 RETURN;

    UPDATE tb_aviso_usuario
    SET    avaliacao = @avaliacao,
           data_avaliacao = GETDATE()
    WHERE  codigo_aviso = @codigo_aviso AND codigo_empresa = @codigo_empresa AND codigo_usuario = @codigo_usuario;

    IF @@ROWCOUNT = 0
        INSERT INTO tb_aviso_usuario
            (codigo_aviso, codigo_empresa, codigo_usuario, avaliacao, data_avaliacao)
        VALUES
            (@codigo_aviso, @codigo_empresa, @codigo_usuario, @avaliacao, GETDATE());

END
GO

IF OBJECT_ID('dbo.sp_update_aviso_dispensa', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_update_aviso_dispensa];
GO

CREATE PROCEDURE [dbo].[sp_update_aviso_dispensa]
@codigo_aviso   int,
@codigo_empresa smallint,
@codigo_usuario int
AS
BEGIN

    SET NOCOUNT ON;

    UPDATE tb_aviso_usuario
    SET    nao_ver_mais = 1,
           data_dispensa = GETDATE()
    WHERE  codigo_aviso = @codigo_aviso AND codigo_empresa = @codigo_empresa AND codigo_usuario = @codigo_usuario;

    IF @@ROWCOUNT = 0
        INSERT INTO tb_aviso_usuario
            (codigo_aviso, codigo_empresa, codigo_usuario, nao_ver_mais, data_dispensa)
        VALUES
            (@codigo_aviso, @codigo_empresa, @codigo_usuario, 1, GETDATE());

END
GO


-- =============================================================================================
-- 7. AUDITORIA — resumo + linhas por usuario
-- =============================================================================================
IF OBJECT_ID('dbo.sp_select_aviso_auditoria', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_select_aviso_auditoria];
GO

CREATE PROCEDURE [dbo].[sp_select_aviso_auditoria]
@codigo int
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        titulo          = a.titulo,
        visualizacoes   = ISNULL(SUM(l.exibicoes), 0),
        usuarios        = COUNT(l.codigo),
        dispensaram     = ISNULL(SUM(CONVERT(int, l.nao_ver_mais)), 0),
        avaliaram       = SUM(CASE WHEN l.avaliacao IS NOT NULL THEN 1 ELSE 0 END),
        media_avaliacao = ISNULL(AVG(CONVERT(decimal(4,2), l.avaliacao)), 0)
    FROM   tb_aviso a LEFT JOIN
           tb_aviso_usuario l ON a.codigo = l.codigo_aviso
    WHERE  a.codigo = @codigo
    GROUP  BY a.titulo;

    SELECT
        usuario             = ISNULL(u.nome, 'Usuário ' + CONVERT(varchar(10), l.codigo_usuario)),
        ultima_visualizacao = ISNULL(FORMAT(l.ultima_visualizacao, 'dd/MM/yyyy HH:mm'), ''),
        exibicoes           = l.exibicoes,
        avaliacao           = ISNULL(l.avaliacao, 0),
        nao_ver_mais        = l.nao_ver_mais,
        data_dispensa       = ISNULL(FORMAT(l.data_dispensa, 'dd/MM/yyyy HH:mm'), '')
    FROM   tb_aviso_usuario l LEFT JOIN
           tb_cad_usuario u ON
           l.codigo_usuario = u.codigo AND
           l.codigo_empresa = u.codigo_empresa
    WHERE  l.codigo_aviso = @codigo
    ORDER  BY l.ultima_visualizacao DESC;

END
GO


-- =============================================================================================
-- 8. PERMISSAO (documentacao — mesmo padrao do manual integrado)
--
--  A tela de manutencao usa o formulario 'adm_aviso'; enquanto ele nao for cadastrado no
--  perfil, o sistema usa como reserva o direito de 'adm_perfil' (quem administra perfis
--  tambem mantem os avisos). O popup do login nao exige direito nenhum: e para todos.
-- =============================================================================================
