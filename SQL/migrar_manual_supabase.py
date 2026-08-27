#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Migra o manual do Supabase (PostgreSQL) para o banco PCM (SQL Server).

Precisa rodar numa maquina que enxergue os DOIS bancos — tipicamente o NOTE-KLEBER,
onde o SQL Server e local e o Supabase esta a um NAT de distancia.

    pip install pg8000            (obrigatorio: le o Postgres)
    pip install pyodbc            (opcional: grava direto no SQL Server)

Sem pyodbc, o script gera um .sql com os INSERTs para voce rodar no SSMS — que costuma
ser o caminho mais simples, e deixa voce conferir tudo antes de gravar.

Como usar, na ordem:

  1) Ver o que existe no Supabase (nao grava nada, so mostra tabelas, colunas e amostra):

         python migrar_manual_supabase.py inspecionar

  2) Conferir o de-para que o script deduziu e o que ele traria (nao grava nada):

         python migrar_manual_supabase.py previa

  3) Gerar o arquivo .sql com a carga, para rodar no SSMS:

         python migrar_manual_supabase.py gerar-sql

  4) Ou gravar direto no SQL Server (precisa de pyodbc):

         python migrar_manual_supabase.py migrar

A estrutura de origem NAO precisa estar no padrao novo: o script procura, entre as tabelas
do Supabase, quais parecem o cabecalho e a secao do manual, olhando os nomes das colunas
(titulo/title, conteudo/content/texto, ordem/sequence, imagem/image, video/link...). Se ele
errar o palpite, corrija o dicionario MAPA_MANUAL/MAPA_ITEM no fim do arquivo — o
"inspecionar" mostra exatamente os nomes reais para voce preencher.

Rodar de novo e seguro: a carga apaga so os manuais marcados com a origem 'supabase' antes
de inserir (ver LIMPAR_ANTES).
"""

from __future__ import annotations

import json
import re
import sys
from datetime import datetime

# ─────────────────────────────────────────────────────────────────────────────────────────
# CONEXOES — ajuste aqui
# ─────────────────────────────────────────────────────────────────────────────────────────

# Supabase. O host direto (db.<ref>.supabase.co) so tem IPv6; se a sua rede for IPv4,
# use o Session Pooler, cujo usuario e sempre postgres.<ref do projeto>.
# O host/porta exatos estao em: Supabase > Settings > Database > Connection pooling.
PG = {
    "host": "aws-0-sa-east-1.pooler.supabase.com",
    "port": 5432,
    "database": "postgres",
    "user": "postgres.lyqpcipnuywziwijemux",
    "password": "22pJWfkB7Y8MsLrf",
}

# Alternativa direta, para rede com IPv6:
# PG = {"host": "db.lyqpcipnuywziwijemux.supabase.co", "port": 5432,
#       "database": "postgres", "user": "postgres", "password": "..."}

# SQL Server do PCM.
MSSQL = {
    "server": "NOTE-KLEBER",
    "database": "PCM",
    "user": "sa",
    "password": "p@ssw0rd013459",
}

# Empresa dona do manual migrado. 0 = manual do sistema, vale para todas as empresas.
CODIGO_EMPRESA = 0

# Antes de inserir, apaga os manuais que vieram de uma carga anterior deste script.
LIMPAR_ANTES = True

ARQUIVO_SQL = "carga_manual.sql"

# Marca gravada em tb_manual.usuario para reconhecer o que veio daqui.
ORIGEM = "supabase"


# ─────────────────────────────────────────────────────────────────────────────────────────
# Leitura do Postgres
# ─────────────────────────────────────────────────────────────────────────────────────────

def conectar_pg():
    try:
        import pg8000.native
    except ImportError:
        sys.exit("Falta o driver do Postgres. Rode:  pip install pg8000")

    try:
        return pg8000.native.Connection(
            PG["user"], host=PG["host"], port=PG["port"], database=PG["database"],
            password=PG["password"], ssl_context=True, timeout=30,
        )
    except Exception as e:
        sys.exit(
            "Nao consegui conectar no Supabase (%s@%s).\n  %r\n\n"
            "Confira host/usuario em Settings > Database > Connection pooling. O host direto\n"
            "db.<ref>.supabase.co so resolve IPv6; em rede IPv4 use o pooler." % (PG["user"], PG["host"], e)
        )


def tabelas(conn) -> list[tuple[str, str]]:
    return [
        (r[0], r[1])
        for r in conn.run(
            "SELECT table_schema, table_name FROM information_schema.tables "
            "WHERE table_type = 'BASE TABLE' "
            "AND table_schema NOT IN ('pg_catalog', 'information_schema') "
            "ORDER BY 1, 2"
        )
    ]


def colunas(conn, schema: str, tabela: str) -> list[str]:
    return [
        r[0]
        for r in conn.run(
            "SELECT column_name FROM information_schema.columns "
            "WHERE table_schema = :s AND table_name = :t ORDER BY ordinal_position",
            s=schema, t=tabela,
        )
    ]


def contar(conn, schema: str, tabela: str) -> int:
    return conn.run('SELECT COUNT(*) FROM "%s"."%s"' % (schema, tabela))[0][0]


# ─────────────────────────────────────────────────────────────────────────────────────────
# Descoberta do de-para
#
# Cada campo do destino tem uma lista de nomes prováveis na origem, do mais para o menos
# específico. O primeiro que existir na tabela vence.
# ─────────────────────────────────────────────────────────────────────────────────────────

SINONIMOS_MANUAL = {
    "titulo":     ["titulo", "title", "nome", "name", "assunto", "descricao_manual"],
    "subtitulo":  ["subtitulo", "subtitle", "descricao", "description", "resumo", "subtexto"],
    "controller": ["controller", "tela_controller", "modulo", "module", "tela", "screen", "form", "formulario"],
    "action":     ["action", "tela_action", "acao", "pagina", "page", "metodo"],
    "tipo":       ["tipo", "kind", "type", "categoria"],
    "ativo":      ["ativo", "active", "habilitado", "enabled", "publicado", "published"],
    "id":         ["help_id", "manual_id", "id", "codigo"],
}

SINONIMOS_ITEM = {
    "titulo":    ["titulo", "title", "nome", "name", "cabecalho"],
    "conteudo":  ["conteudo", "content", "texto", "text", "descricao", "description", "corpo", "body", "html"],
    "sequencia": ["sequencia", "sequence", "ordem", "order", "posicao", "position", "seq", "indice"],
    "tipo_nota": ["tipo_nota", "note_type", "tipo_destaque", "destaque_tipo"],
    "nota":      ["nota", "note", "observacao", "destaque", "dica", "aviso"],
    "imagem":    ["imagem", "image", "img", "foto", "picture", "url_imagem", "image_url", "figura"],
    "video":     ["video", "video_url", "url_video", "link_video", "youtube", "midia", "media_url", "link"],
    "pai":       ["help_id", "manual_id", "id_manual", "codigo_manual", "parent_id", "id_pai", "fk_manual"],
    "id":        ["item_id", "id", "codigo"],
}


def casar(cols: list[str], sinonimos: list[str]) -> str | None:
    baixa = {c.lower(): c for c in cols}
    for s in sinonimos:
        if s in baixa:
            return baixa[s]
    # Segunda passada: o nome do sinonimo dentro de um nome maior (ex.: "url_do_video").
    for s in sinonimos:
        for c in cols:
            if s in c.lower():
                return c
    return None


def mapear(cols: list[str], sinonimos: dict) -> dict:
    return {destino: casar(cols, nomes) for destino, nomes in sinonimos.items()}


def adivinhar(conn) -> tuple[tuple[str, str, dict] | None, tuple[str, str, dict] | None]:
    """Escolhe a tabela de cabecalho e a de secoes, pela cara das colunas."""
    cabecalho = None
    item = None
    melhor_c = melhor_i = 0

    for schema, tabela in tabelas(conn):
        cols = colunas(conn, schema, tabela)
        mc = mapear(cols, SINONIMOS_MANUAL)
        mi = mapear(cols, SINONIMOS_ITEM)

        # Secao: tem chave para o pai E texto longo. Cabecalho: tem titulo E identificacao de tela.
        nota_i = sum(1 for k in ("pai", "conteudo", "sequencia") if mi.get(k)) + (2 if mi.get("pai") else 0)
        nota_c = sum(1 for k in ("titulo", "controller", "subtitulo") if mc.get(k))

        if mi.get("pai") and mi.get("conteudo") and nota_i > melhor_i:
            melhor_i, item = nota_i, (schema, tabela, mi)
        elif mc.get("titulo") and nota_c > melhor_c:
            melhor_c, cabecalho = nota_c, (schema, tabela, mc)

    return cabecalho, item


# ─────────────────────────────────────────────────────────────────────────────────────────
# Extracao
# ─────────────────────────────────────────────────────────────────────────────────────────

def texto(v) -> str:
    if v is None:
        return ""
    if isinstance(v, (dict, list)):
        return json.dumps(v, ensure_ascii=False)
    return str(v).strip()


def booleano(v, padrao=True) -> bool:
    if v is None:
        return padrao
    if isinstance(v, bool):
        return v
    return str(v).strip().lower() in ("1", "true", "t", "s", "sim", "y", "yes", "ativo")


def extrair(conn) -> list[dict]:
    """Devolve os manuais ja no formato do destino."""
    palpite_cab, palpite_itm = (None, None) if (MAPA_MANUAL and MAPA_ITEM) else adivinhar(conn)
    cab = MAPA_MANUAL or palpite_cab
    itm = MAPA_ITEM or palpite_itm

    if not cab:
        sys.exit("Nao identifiquei a tabela do manual no Supabase.\n"
                 "Rode 'inspecionar' e preencha MAPA_MANUAL/MAPA_ITEM no fim deste arquivo.")

    schema_c, tab_c, mc = cab
    manuais = []

    campos = [c for c in mc.values() if c]
    sel = ", ".join('"%s"' % c for c in campos)
    linhas = conn.run('SELECT %s FROM "%s"."%s"' % (sel, schema_c, tab_c))
    idx = {c: i for i, c in enumerate(campos)}

    def val(linha, destino):
        col = mc.get(destino)
        return linha[idx[col]] if col else None

    for linha in linhas:
        controller = texto(val(linha, "controller"))
        action = texto(val(linha, "action"))

        # Alguns cadastros guardam a tela como "Controller/Action" numa coluna so.
        if "/" in controller and not action:
            controller, action = controller.split("/", 1)

        tipo = texto(val(linha, "tipo")).upper()[:1]
        if tipo not in ("S", "P"):
            tipo = "S" if controller else "P"

        manuais.append({
            "origem_id": val(linha, "id"),
            "tipo": tipo,
            "controller": controller if tipo == "S" else "",
            "action": action if tipo == "S" else "",
            "titulo": texto(val(linha, "titulo"))[:200] or "(sem titulo)",
            "subtitulo": texto(val(linha, "subtitulo"))[:300],
            "ativo": booleano(val(linha, "ativo")),
            "itens": [],
        })

    if itm:
        schema_i, tab_i, mi = itm
        campos_i = [c for c in mi.values() if c]
        sel_i = ", ".join('"%s"' % c for c in campos_i)
        ordem = mi.get("sequencia") or mi.get("id")
        sql = 'SELECT %s FROM "%s"."%s"' % (sel_i, schema_i, tab_i)
        if ordem:
            sql += ' ORDER BY "%s"' % ordem
        linhas_i = conn.run(sql)
        idx_i = {c: i for i, c in enumerate(campos_i)}

        por_pai: dict[str, list] = {}
        for linha in linhas_i:
            def vi(destino):
                col = mi.get(destino)
                return linha[idx_i[col]] if col else None

            pai = texto(vi("pai"))
            por_pai.setdefault(pai, []).append({
                "titulo": texto(vi("titulo"))[:200],
                "conteudo": limpar_html(texto(vi("conteudo"))),
                "tipo_nota": texto(vi("tipo_nota")).upper()[:1],
                "nota": texto(vi("nota"))[:1000],
                "imagem": texto(vi("imagem"))[:500],
                "video": url_valida(texto(vi("video"))),
            })

        for m in manuais:
            secoes = por_pai.get(texto(m["origem_id"]), [])
            for n, s in enumerate(secoes, start=1):
                s["sequencia"] = n
                if not s["titulo"]:
                    s["titulo"] = "Passo %d" % n
            m["itens"] = secoes

    return manuais


def limpar_html(t: str) -> str:
    """O painel formata texto simples (negrito com ** e listas com '- '); conteudo que veio
    como HTML vira esse texto, em vez de aparecer com as tags na tela."""
    if "<" not in t:
        return t
    t = re.sub(r"(?i)<br\s*/?>", "\n", t)
    t = re.sub(r"(?i)</p>", "\n\n", t)
    t = re.sub(r"(?i)<li[^>]*>", "- ", t)
    t = re.sub(r"(?i)</li>", "\n", t)
    t = re.sub(r"(?i)<(b|strong)>(.*?)</\1>", r"**\2**", t, flags=re.S)
    t = re.sub(r"<[^>]+>", "", t)
    t = (t.replace("&nbsp;", " ").replace("&amp;", "&")
          .replace("&lt;", "<").replace("&gt;", ">").replace("&quot;", '"'))
    return re.sub(r"\n{3,}", "\n\n", t).strip()


def url_valida(u: str) -> str:
    """Link de video e hyperlink, nao texto solto: sem http(s), nao migra."""
    return u[:500] if u.lower().startswith(("http://", "https://")) else ""


# ─────────────────────────────────────────────────────────────────────────────────────────
# Escrita no SQL Server
# ─────────────────────────────────────────────────────────────────────────────────────────

def lit(v) -> str:
    """Literal T-SQL. N'' para preservar acento em nvarchar."""
    if v is None or v == "":
        return "NULL"
    return "N'" + str(v).replace("'", "''") + "'"


def gerar_sql(manuais: list[dict]) -> str:
    L = []
    A = L.append
    A("/* Carga do manual — gerada por migrar_manual_supabase.py em %s */"
      % datetime.now().strftime("%d/%m/%Y %H:%M"))
    A("/* Rode DEPOIS de 2026-08-27_manual_integrado.sql, que cria as tabelas. */")
    A("")
    A("SET NOCOUNT ON;")
    A("BEGIN TRANSACTION;")
    A("")

    if LIMPAR_ANTES:
        A("-- Refaz a carga: sai o que veio de uma execucao anterior deste script.")
        A("DELETE FROM tb_manual_item WHERE codigo_manual IN (SELECT codigo FROM tb_manual WHERE usuario = %s);" % lit(ORIGEM))
        A("UPDATE tb_manual SET codigo_manual_processo = NULL WHERE usuario = %s;" % lit(ORIGEM))
        A("DELETE FROM tb_manual WHERE usuario = %s;" % lit(ORIGEM))
        A("")

    A("DECLARE @codigo int;")
    A("")

    for m in manuais:
        A("-- %s%s" % (m["titulo"], (" (%s/%s)" % (m["controller"], m["action"])) if m["controller"] else ""))
        A("INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)")
        A("VALUES (%s, %s, %s, %s, %s, %s, %d, %s, GETDATE());" % (
            "NULL" if not CODIGO_EMPRESA else str(CODIGO_EMPRESA),
            lit(m["tipo"]), lit(m["controller"]), lit(m["action"]),
            lit(m["titulo"]), lit(m["subtitulo"]), 1 if m["ativo"] else 0, lit(ORIGEM)))
        A("SET @codigo = SCOPE_IDENTITY();")

        for s in m["itens"]:
            A("INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)")
            A("VALUES (@codigo, %d, %s, %s, %s, %s, %s, %s, 1);" % (
                s["sequencia"], lit(s["titulo"]), lit(s["conteudo"]),
                lit(s["tipo_nota"] if s["tipo_nota"] in ("D", "A") else ""),
                lit(s["nota"]), lit(s["imagem"]), lit(s["video"])))
        A("")

    A("COMMIT TRANSACTION;")
    A("")
    A("SELECT manuais = (SELECT COUNT(*) FROM tb_manual WHERE usuario = %s)," % lit(ORIGEM))
    A("       secoes  = (SELECT COUNT(*) FROM tb_manual_item i")
    A("                  JOIN tb_manual m ON m.codigo = i.codigo_manual WHERE m.usuario = %s);" % lit(ORIGEM))
    return "\n".join(L)


def gravar_sqlserver(manuais: list[dict]) -> None:
    try:
        import pyodbc
    except ImportError:
        sys.exit("Falta o pyodbc para gravar direto. Rode 'pip install pyodbc',\n"
                 "ou use 'gerar-sql' e rode o arquivo no SSMS.")

    driver = next((d for d in pyodbc.drivers() if "SQL Server" in d), None)
    if not driver:
        sys.exit("Nenhum driver ODBC de SQL Server instalado. Use 'gerar-sql' e rode no SSMS.")

    cs = "DRIVER={%s};SERVER=%s;DATABASE=%s;UID=%s;PWD=%s;TrustServerCertificate=yes" % (
        driver, MSSQL["server"], MSSQL["database"], MSSQL["user"], MSSQL["password"])

    with pyodbc.connect(cs, autocommit=False) as cn:
        cur = cn.cursor()

        if LIMPAR_ANTES:
            cur.execute("DELETE FROM tb_manual_item WHERE codigo_manual IN "
                        "(SELECT codigo FROM tb_manual WHERE usuario = ?)", ORIGEM)
            cur.execute("UPDATE tb_manual SET codigo_manual_processo = NULL WHERE usuario = ?", ORIGEM)
            cur.execute("DELETE FROM tb_manual WHERE usuario = ?", ORIGEM)

        secoes = 0
        for m in manuais:
            cur.execute(
                "INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, "
                "subtitulo, ativo, usuario, data_inclusao) "
                "OUTPUT INSERTED.codigo VALUES (?, ?, ?, ?, ?, ?, ?, ?, GETDATE())",
                (CODIGO_EMPRESA or None), m["tipo"], m["controller"] or None, m["action"] or None,
                m["titulo"], m["subtitulo"] or None, 1 if m["ativo"] else 0, ORIGEM)
            codigo = cur.fetchone()[0]

            for s in m["itens"]:
                cur.execute(
                    "INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, "
                    "tipo_nota, nota, imagem, video, ativo) VALUES (?, ?, ?, ?, ?, ?, ?, ?, 1)",
                    codigo, s["sequencia"], s["titulo"], s["conteudo"] or None,
                    (s["tipo_nota"] if s["tipo_nota"] in ("D", "A") else None),
                    s["nota"] or None, s["imagem"] or None, s["video"] or None)
                secoes += 1

        cn.commit()

    print("Gravado no SQL Server: %d manuais, %d secoes." % (len(manuais), secoes))


# ─────────────────────────────────────────────────────────────────────────────────────────
# De-para manual — preencha SO se o palpite automatico errar
#
#   MAPA_MANUAL = ("public", "minha_tabela_manual", {
#       "id": "id", "titulo": "titulo", "subtitulo": "descricao",
#       "controller": "modulo", "action": "tela", "tipo": None, "ativo": "ativo"})
#   MAPA_ITEM = ("public", "minha_tabela_secao", {
#       "id": "id", "pai": "id_manual", "titulo": "titulo", "conteudo": "texto",
#       "sequencia": "ordem", "tipo_nota": None, "nota": None,
#       "imagem": "imagem", "video": "link_video"})
# ─────────────────────────────────────────────────────────────────────────────────────────

MAPA_MANUAL = None
MAPA_ITEM = None


# ─────────────────────────────────────────────────────────────────────────────────────────

def cmd_inspecionar(conn) -> None:
    print("Tabelas no Supabase\n" + "=" * 70)
    for schema, tabela in tabelas(conn):
        cols = colunas(conn, schema, tabela)
        print("\n%s.%s  (%d linhas)" % (schema, tabela, contar(conn, schema, tabela)))
        print("  colunas: " + ", ".join(cols))
        amostra = conn.run('SELECT * FROM "%s"."%s" LIMIT 2' % (schema, tabela))
        for linha in amostra:
            campos = ["%s=%s" % (c, texto(v)[:60]) for c, v in zip(cols, linha) if texto(v)]
            print("    - " + " | ".join(campos[:8]))

    cab, itm = adivinhar(conn)
    print("\n\nPalpite do de-para\n" + "=" * 70)
    print("cabecalho: %s" % ("%s.%s -> %s" % (cab[0], cab[1], cab[2]) if cab else "NAO IDENTIFICADO"))
    print("secoes:    %s" % ("%s.%s -> %s" % (itm[0], itm[1], itm[2]) if itm else "NAO IDENTIFICADO"))
    print("\nSe algo estiver errado, preencha MAPA_MANUAL/MAPA_ITEM no fim do script.")


def cmd_previa(manuais: list[dict]) -> None:
    total = sum(len(m["itens"]) for m in manuais)
    videos = sum(1 for m in manuais for s in m["itens"] if s["video"])
    imagens = sum(1 for m in manuais for s in m["itens"] if s["imagem"])
    print("%d manuais, %d secoes (%d com imagem, %d com video)\n" % (len(manuais), total, imagens, videos))
    for m in manuais:
        alvo = ("%s/%s" % (m["controller"], m["action"])).rstrip("/") if m["controller"] else "(processo)"
        print("- [%s] %s  ->  %s  (%d secoes)" % (m["tipo"], m["titulo"], alvo, len(m["itens"])))
        for s in m["itens"][:3]:
            marca = ("  video" if s["video"] else "") + ("  imagem" if s["imagem"] else "")
            print("     %d. %s%s" % (s["sequencia"], s["titulo"][:60], marca))
        if len(m["itens"]) > 3:
            print("     ... mais %d" % (len(m["itens"]) - 3))


def main() -> None:
    cmd = sys.argv[1] if len(sys.argv) > 1 else "previa"

    if cmd not in ("inspecionar", "previa", "gerar-sql", "migrar"):
        sys.exit(__doc__)

    conn = conectar_pg()
    try:
        if cmd == "inspecionar":
            cmd_inspecionar(conn)
            return

        manuais = extrair(conn)
    finally:
        conn.close()

    if not manuais:
        sys.exit("Nada encontrado para migrar. Rode 'inspecionar' e confira o de-para.")

    if cmd == "previa":
        cmd_previa(manuais)
    elif cmd == "gerar-sql":
        with open(ARQUIVO_SQL, "w", encoding="utf-8") as f:
            f.write(gerar_sql(manuais))
        print("Gerado %s — abra no SSMS (banco PCM) e execute." % ARQUIVO_SQL)
        cmd_previa(manuais)
    elif cmd == "migrar":
        cmd_previa(manuais)
        gravar_sqlserver(manuais)


if __name__ == "__main__":
    main()
