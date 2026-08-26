-- =============================================================================
-- MANUAL INTEGRADO DO PCM.WEB  —  banco PostgreSQL (Supabase)
--
-- Mesmo conceito do manual do MEZ.WEB, com duas diferencas:
--
--   1. O PCM nao tem tabela de formularios (o menu e fixo no _Sidebar), entao a
--      tela e identificada direto por controller + action — que e o que o botao
--      "?" do cabecalho sabe de si mesmo sem carregar nada.
--   2. Cada secao ganhou, alem da imagem, um hyperlink de VIDEO (coluna video):
--      o painel reconhece YouTube/Vimeo e incorpora o player; qualquer outra
--      URL vira um link para abrir em outra aba.
--
-- Duas granularidades, na mesma tabela:
--
--   kind = 'S' (tela)     -> controller/action preenchidos. Se action ficar
--                            vazio, o manual vale para o modulo inteiro
--                            (qualquer tela daquele controller sem manual
--                            proprio cai nele).
--   kind = 'P' (processo) -> manual avulso, sem tela. As telas apontam para ele
--                            por processo_help_id e ele aparece como link
--                            "ver tambem" no rodape do painel.
--
-- Rode este script no SQL Editor do Supabase (ou via psql). Ele nao apaga nada
-- do que ja existe no banco: apenas cria as tabelas novas do padrao.
-- =============================================================================

-- ---- Cabecalho --------------------------------------------------------------
CREATE TABLE IF NOT EXISTS tb_pcm_help (
    help_id          serial       NOT NULL,
    kind             varchar(1)   NOT NULL DEFAULT 'S',      -- S = tela, P = processo
    controller       varchar(100) NULL,
    action           varchar(100) NULL,
    processo_help_id int          NULL,                      -- manual de processo ligado a tela
    language         varchar(10)  NOT NULL DEFAULT 'pt-BR',
    title            varchar(200) NOT NULL,
    subtitle         varchar(300) NULL,
    active           boolean      NOT NULL DEFAULT true,
    username         varchar(100) NULL,
    date_input       timestamp    NOT NULL DEFAULT now(),
    username_update  varchar(100) NULL,
    date_update      timestamp    NULL,
    CONSTRAINT pk_tb_pcm_help PRIMARY KEY (help_id),
    CONSTRAINT ck_tb_pcm_help_kind CHECK (kind IN ('S', 'P')),
    CONSTRAINT fk_tb_pcm_help_processo FOREIGN KEY (processo_help_id)
        REFERENCES tb_pcm_help (help_id) ON DELETE SET NULL
);

CREATE INDEX IF NOT EXISTS ix_tb_pcm_help_tela
    ON tb_pcm_help (lower(controller), lower(action));

-- ---- Secoes -----------------------------------------------------------------
CREATE TABLE IF NOT EXISTS tb_pcm_help_item (
    item_id   serial        NOT NULL,
    help_id   int           NOT NULL,
    sequence  int           NOT NULL,
    title     varchar(200)  NOT NULL,
    content   text          NULL,
    -- Destaque opcional ao pe da secao: D = dica, A = aviso.
    note_type varchar(1)    NULL,
    note      varchar(1000) NULL,
    image     varchar(500)  NULL,
    -- Hyperlink de video da secao (YouTube, Vimeo ou URL direta).
    video     varchar(500)  NULL,
    active    boolean       NOT NULL DEFAULT true,
    CONSTRAINT pk_tb_pcm_help_item PRIMARY KEY (item_id),
    CONSTRAINT fk_tb_pcm_help_item_help FOREIGN KEY (help_id)
        REFERENCES tb_pcm_help (help_id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS ix_tb_pcm_help_item_help
    ON tb_pcm_help_item (help_id, sequence);

-- =============================================================================
-- MIGRACAO DO CONTEUDO QUE JA EXISTE NO BANCO
--
-- A base atual do manual nao esta neste padrao. Como cada estrutura de origem
-- e diferente, o de-para fica aqui como modelo: ajuste os nomes de tabela e
-- coluna da SUA estrutura atual e rode uma vez. Exemplo:
--
-- INSERT INTO tb_pcm_help (kind, controller, action, title, subtitle, active, username)
-- SELECT 'S',
--        m.controller,                -- ou a coluna que identifica a tela
--        m.action,
--        m.titulo,
--        m.descricao,
--        true,
--        'migracao'
-- FROM   minha_tabela_manual m;
--
-- INSERT INTO tb_pcm_help_item (help_id, sequence, title, content, image, video, active)
-- SELECT h.help_id,
--        i.ordem,
--        i.titulo,
--        i.texto,
--        i.imagem,
--        i.link_video,                -- se a base atual ja tiver video
--        true
-- FROM   minha_tabela_manual_item i
-- JOIN   tb_pcm_help h ON h.title = (SELECT titulo FROM minha_tabela_manual m WHERE m.id = i.id_manual);
--
-- Se preferir, me passe a estrutura atual (\d das tabelas) que eu escrevo o
-- de-para exato.
-- =============================================================================
