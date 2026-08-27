#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Migra o manual do Supabase (PostgreSQL) para o banco PCM (SQL Server).

Precisa rodar numa maquina que enxergue os DOIS bancos — tipicamente o NOTE-KLEBER,
onde o SQL Server e local e o Supabase esta a um NAT de distancia.

    pip install pg8000            (obrigatorio: le o Supabase)
    pip install pyodbc            (opcional: grava direto no SQL Server)

Sem pyodbc o script gera um .sql com a carga, para voce rodar no SSMS — que costuma ser
o caminho mais simples, e deixa voce conferir tudo antes de gravar.

DE ONDE VEM O QUE
-----------------
    public.chapters (12)  -> manual de PROCESSO (tipo 'P'): as trilhas.
    public.articles (82)  -> manual de TELA (tipo 'S'): um manual por artigo.

O texto do artigo esta em Markdown e vira SECOES do manual, quebradas nos titulos de
nivel 2 ("## ..."). O que vem antes do primeiro "##" entra como a primeira secao. O
video_url do artigo vai para a primeira secao.

Cada artigo aponta para a trilha dele no rodape do painel ("Ver tambem").

QUAL TELA E CADA ARTIGO
-----------------------
O Supabase organiza o manual por TRILHA DE TREINAMENTO, e nao por tela do sistema —
articles nao tem controller/action. Como o botao "?" precisa saber a tela, o script
casa o titulo do artigo com o nome que a tela tem NO MENU do PCM (telas_pcm.csv, 159
telas extraidas do _Sidebar) e grava o palpite em mapa_telas.csv, para voce revisar.

    Artigo                      -> Tela                     confianca
    Categoria de Servico        -> CadastroBasico/CategoriaIndex   alta
    Plano de Acao               -> PlanoAcao/PlanoAcaoIndex        alta
    Rotinas e Rondas            -> CadastroBasico/RotinaIndex      media

Artigo sem tela definida entra como manual de processo — continua no cadastro, visivel
e editavel, e voce liga na tela pela tela de edicao quando quiser.

COMO USAR, NA ORDEM
-------------------
  1) Ver o que existe no Supabase (nao grava nada):

         python migrar_manual_supabase.py inspecionar

  2) Ver um artigo inteiro e como ele vai ficar dividido em secoes:

         python migrar_manual_supabase.py amostra
         python migrar_manual_supabase.py amostra 2-8-categoria-de-servico

  3) Gerar o de-para de telas para revisar (cria mapa_telas.csv):

         python migrar_manual_supabase.py mapear

     Abra o mapa_telas.csv no Excel, confira a coluna controller/action e corrija o que
     estiver errado. Linha com controller vazio vira manual de processo.

  4) Conferir o que seria migrado (nao grava nada):

         python migrar_manual_supabase.py previa

  5) Gerar o arquivo .sql da carga, para rodar no SSMS:

         python migrar_manual_supabase.py gerar-sql

     ou gravar direto no SQL Server (precisa de pyodbc):

         python migrar_manual_supabase.py migrar

Rodar de novo e seguro: a carga apaga so os manuais marcados com a origem 'supabase'
antes de inserir (ver LIMPAR_ANTES).
"""

from __future__ import annotations

import csv
import os
import re
import sys
import unicodedata
from datetime import datetime
from difflib import SequenceMatcher

# O console do Windows nao usa UTF-8 por padrao, e o relatorio tem acento — sem isto,
# "python ... > saida.txt" morre com UnicodeEncodeError no primeiro titulo.
try:
    sys.stdout.reconfigure(encoding="utf-8", errors="replace")
except (AttributeError, ValueError):
    pass


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

# Traz tambem os artigos com is_published = false.
INCLUIR_NAO_PUBLICADOS = False

PASTA = os.path.dirname(os.path.abspath(__file__))
ARQUIVO_SQL = os.path.join(PASTA, "carga_manual.sql")
ARQUIVO_MAPA = os.path.join(PASTA, "mapa_telas.csv")
ARQUIVO_TELAS = os.path.join(PASTA, "telas_pcm.csv")

# Marca gravada em tb_manual.usuario para reconhecer o que veio daqui.
ORIGEM = "supabase"


# ─────────────────────────────────────────────────────────────────────────────────────────
# Postgres
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
            "db.<ref>.supabase.co so resolve IPv6; em rede IPv4 use o pooler."
            % (PG["user"], PG["host"], e)
        )


def texto(v) -> str:
    return "" if v is None else str(v).strip()


def ler_chapters(conn) -> list[dict]:
    return [
        {"id": texto(r[0]), "titulo": texto(r[1]), "descricao": texto(r[2]), "ordem": r[3] or 0}
        for r in conn.run("SELECT id, title, description, ordem FROM public.chapters ORDER BY ordem, title")
    ]


def ler_articles(conn) -> list[dict]:
    sql = ("SELECT id, title, content, chapter_id, ordem, video_url, section_number, is_published "
           "FROM public.articles ")
    if not INCLUIR_NAO_PUBLICADOS:
        sql += "WHERE COALESCE(is_published, true) = true "
    sql += "ORDER BY section_number, ordem, title"

    return [
        {
            "id": texto(r[0]),
            "titulo": texto(r[1]),
            "conteudo": r[2] or "",
            "chapter_id": texto(r[3]),
            "ordem": r[4] or 0,
            "video": url_valida(texto(r[5])),
            "secao_numero": texto(r[6]),
            "publicado": bool(r[7]) if r[7] is not None else True,
        }
        for r in conn.run(sql)
    ]


def url_valida(u: str) -> str:
    """Link de video e hyperlink, nao texto solto: sem http(s), nao migra."""
    return u[:500] if u.lower().startswith(("http://", "https://")) else ""


# ─────────────────────────────────────────────────────────────────────────────────────────
# Markdown do artigo -> secoes do manual
#
# O painel do PCM renderiza Markdown, entao o texto vai como esta. O que o script faz e
# quebrar o artigo em secoes: cada "## titulo" vira uma secao recolhivel no painel.
# ─────────────────────────────────────────────────────────────────────────────────────────

def dividir_em_secoes(conteudo: str, titulo_artigo: str) -> list[dict]:

    txt = (conteudo or "").replace("\r\n", "\n").replace("\r", "\n")

    # O H1 do topo repete o titulo do artigo, que ja e o titulo do manual.
    txt = re.sub(r"\A\s*#\s+[^\n]*\n", "", txt)

    # Quebra nos titulos de nivel 2, preservando o texto que vem antes do primeiro.
    partes = re.split(r"^##\s+(.+)$", txt, flags=re.M)

    secoes = []
    abertura = partes[0].strip()
    if abertura:
        secoes.append({"titulo": "Visão geral", "conteudo": abertura})

    for i in range(1, len(partes) - 1, 2):
        titulo = partes[i].strip()
        corpo = partes[i + 1].strip()
        if titulo or corpo:
            secoes.append({"titulo": titulo[:200] or "Passo %d" % (len(secoes) + 1), "conteudo": corpo})

    # Artigo sem "##" nenhum: o texto inteiro e uma secao so.
    if not secoes:
        corpo = txt.strip()
        if corpo:
            secoes.append({"titulo": titulo_artigo[:200] or "Conteúdo", "conteudo": corpo})

    # Uma secao unica chamada "Visão geral" nao diz nada: usa o titulo do artigo.
    if len(secoes) == 1 and secoes[0]["titulo"] == "Visão geral":
        secoes[0]["titulo"] = titulo_artigo[:200] or "Visão geral"

    for n, s in enumerate(secoes, start=1):
        s["sequencia"] = n
        s["tipo_nota"] = ""
        s["nota"] = ""
        s["imagem"] = ""     # imagens do artigo ficam embutidas no Markdown, na posicao certa
        s["video"] = ""

    return secoes


# ─────────────────────────────────────────────────────────────────────────────────────────
# De-para de telas
# ─────────────────────────────────────────────────────────────────────────────────────────

RUIDO = {
    "de", "do", "da", "dos", "das", "e", "a", "o", "as", "os", "em", "no", "na", "para",
    "com", "por", "cadastro", "cadastros", "gestao", "gerenciamento", "controle",
    "manual", "como", "usar", "modulo", "tela", "sistema", "pcm",
}


# O menu fala a lingua do sistema e o artigo fala a lingua de quem escreve o treinamento.
SINONIMOS = {
    "colaborador": "funcionario",
    "ronda": "rotina",
    "chamado": "requisicao",
    "os": "ordem",
    "insumo": "produto",
    "material": "produto",
    "peca": "produto",
    "equipe": "funcionario",
    "predio": "unidade",
    "apartamento": "uh",
    "quarto": "uh",
}

# O controller Excel sao os botoes de exportacao, e Relatorio sao as consultas: quando o
# titulo bate igual numa tela e num deles, o manual e da tela.
PENALIDADE = {"Excel": 0.35, "Relatorio": 0.12}


def normalizar(s: str) -> str:
    s = unicodedata.normalize("NFKD", s or "").encode("ascii", "ignore").decode()
    return re.sub(r"[^a-z0-9 ]+", " ", s.lower()).strip()


def radical(p: str) -> str:
    """Plural fora do caminho: 'rotinas' e 'rotina' precisam bater, e 'ordens'/'ordem'."""
    p = SINONIMOS.get(p, p)
    if len(p) > 4:
        if p.endswith("oes") or p.endswith("aes"):
            return p[:-3] + "ao"
        if p.endswith("ns"):
            return p[:-2] + "m"
        if p.endswith("eis"):
            return p[:-3] + "el"
        if p.endswith("es") and not p.endswith("ses"):
            return p[:-2]
        if p.endswith("s"):
            return p[:-1]
    return p


def palavras(s: str) -> set:
    return {radical(p) for p in normalizar(s).split() if p and p not in RUIDO and len(p) > 2}


def carregar_telas() -> list[tuple[str, str, str]]:
    if not os.path.exists(ARQUIVO_TELAS):
        sys.exit("Nao achei %s — ele vem junto no repositorio, na mesma pasta deste script."
                 % os.path.basename(ARQUIVO_TELAS))

    with open(ARQUIVO_TELAS, encoding="utf-8-sig", newline="") as f:
        return [(l["nome_no_menu"], l["controller"], l["action"])
                for l in csv.DictReader(f, delimiter=";")]


def casar_tela(titulo: str, telas: list) -> tuple[str, str, float]:
    """Melhor tela para o titulo do artigo, com a nota de 0 a 1."""
    pa = palavras(titulo)
    # "Logbook" no artigo e "Log Book" no menu: sem espaco, as duas viram a mesma coisa.
    junto_artigo = normalizar(titulo).replace(" ", "")
    melhor, nota_melhor = None, 0.0

    for nome, controller, action in telas:
        pt = palavras(nome)
        if not pt:
            continue

        # Quanto do nome da tela aparece no titulo do artigo, com desempate por
        # semelhanca das strings inteiras (pega genero e abreviacao).
        cobertura = len(pa & pt) / len(pt)
        semelhanca = SequenceMatcher(None, normalizar(titulo), normalizar(nome)).ratio()
        nota = cobertura * 0.75 + semelhanca * 0.25

        junto_tela = normalizar(nome).replace(" ", "")
        if junto_tela and junto_tela in junto_artigo:
            nota = max(nota, 0.80)

        nota -= PENALIDADE.get(controller, 0.0)

        if nota > nota_melhor:
            melhor, nota_melhor = (controller, action), nota

    if not melhor or nota_melhor < 0.45:
        return "", "", nota_melhor

    return melhor[0], melhor[1], nota_melhor


def confianca(nota: float) -> str:
    return "alta" if nota >= 0.75 else "media" if nota >= 0.55 else "baixa"


def gerar_mapa(artigos: list[dict], chapters: dict) -> None:
    telas = carregar_telas()

    with open(ARQUIVO_MAPA, "w", encoding="utf-8-sig", newline="") as f:
        w = csv.writer(f, delimiter=";")
        w.writerow(["artigo_id", "titulo", "trilha", "controller", "action", "confianca"])

        contagem = {"alta": 0, "media": 0, "baixa": 0, "sem": 0}
        for a in artigos:
            controller, action, nota = casar_tela(a["titulo"], telas)
            marca = confianca(nota) if controller else "sem"
            contagem[marca] += 1
            w.writerow([a["id"], a["titulo"], chapters.get(a["chapter_id"], {}).get("titulo", ""),
                        controller, action, marca])

    print("Gerado %s com %d artigos:" % (os.path.basename(ARQUIVO_MAPA), len(artigos)))
    print("   %d alta, %d media, %d baixa, %d sem tela"
          % (contagem["alta"], contagem["media"], contagem["baixa"], contagem["sem"]))
    print("\nAbra no Excel e revise a coluna controller/action. As de confianca 'media' e")
    print("'baixa' sao palpites — confira. Linha com controller vazio vira manual de processo.")
    print("Depois rode:  python %s previa" % os.path.basename(__file__))


def carregar_mapa() -> dict:
    if not os.path.exists(ARQUIVO_MAPA):
        return {}
    with open(ARQUIVO_MAPA, encoding="utf-8-sig", newline="") as f:
        return {l["artigo_id"]: (l["controller"].strip(), l["action"].strip())
                for l in csv.DictReader(f, delimiter=";")}


# ─────────────────────────────────────────────────────────────────────────────────────────
# Monta os manuais no formato do destino
# ─────────────────────────────────────────────────────────────────────────────────────────

def montar(conn) -> tuple[list[dict], list[dict]]:
    chapters = {c["id"]: c for c in ler_chapters(conn)}
    artigos = ler_articles(conn)

    mapa = carregar_mapa()
    telas = carregar_telas() if not mapa else None

    # ---- trilhas: manuais de processo ----
    processos = []
    usados = {a["chapter_id"] for a in artigos}
    for c in sorted(chapters.values(), key=lambda x: (x["ordem"], x["titulo"])):
        if c["id"] not in usados:
            continue
        processos.append({
            "chave": "trilha:" + c["id"],
            "tipo": "P",
            "controller": "", "action": "",
            "titulo": c["titulo"][:200],
            "subtitulo": c["descricao"][:300],
            "ativo": True,
            "processo": "",
            "itens": [{
                "sequencia": 1,
                "titulo": "Sobre esta trilha",
                "conteudo": c["descricao"] or c["titulo"],
                "tipo_nota": "", "nota": "", "imagem": "", "video": "",
            }] if c["descricao"] else [],
        })

    # ---- artigos: manuais de tela ----
    manuais = []
    for a in artigos:
        if a["id"] in mapa:
            controller, action = mapa[a["id"]]
        else:
            controller, action, _ = casar_tela(a["titulo"], telas)

        secoes = dividir_em_secoes(a["conteudo"], a["titulo"])
        if a["video"] and secoes:
            secoes[0]["video"] = a["video"]

        manuais.append({
            "chave": "artigo:" + a["id"],
            "tipo": "S" if controller else "P",
            "controller": controller,
            "action": action,
            "titulo": a["titulo"][:200] or a["id"],
            "subtitulo": (("%s — " % a["secao_numero"]) if a["secao_numero"] else "")
                         + chapters.get(a["chapter_id"], {}).get("titulo", ""),
            "ativo": a["publicado"],
            "processo": ("trilha:" + a["chapter_id"]) if a["chapter_id"] in chapters else "",
            "itens": secoes,
        })

    return processos, manuais


# ─────────────────────────────────────────────────────────────────────────────────────────
# Escrita no SQL Server
# ─────────────────────────────────────────────────────────────────────────────────────────

def lit(v) -> str:
    """Literal T-SQL. N'' para preservar acento e emoji."""
    if v is None or v == "":
        return "NULL"
    return "N'" + str(v).replace("'", "''") + "'"


def gerar_sql(processos: list[dict], manuais: list[dict]) -> str:
    L = []
    A = L.append
    A("/* Carga do manual — gerada por migrar_manual_supabase.py em %s */"
      % datetime.now().strftime("%d/%m/%Y %H:%M"))
    A("/* Rode DEPOIS de 2026-08-27_manual_integrado.sql, que cria as tabelas.        */")
    A("")
    A("SET NOCOUNT ON;")
    A("SET XACT_ABORT ON;")
    A("BEGIN TRANSACTION;")
    A("")

    if LIMPAR_ANTES:
        A("-- Refaz a carga: sai o que veio de uma execucao anterior deste script.")
        A("DELETE FROM tb_manual_item WHERE codigo_manual IN (SELECT codigo FROM tb_manual WHERE usuario = %s);" % lit(ORIGEM))
        A("UPDATE tb_manual SET codigo_manual_processo = NULL WHERE usuario = %s;" % lit(ORIGEM))
        A("DELETE FROM tb_manual WHERE usuario = %s;" % lit(ORIGEM))
        A("")

    # As trilhas entram primeiro: os artigos apontam para elas.
    A("DECLARE @codigo int;")
    A("DECLARE @trilha TABLE (chave varchar(200) PRIMARY KEY, codigo int);")
    A("")

    def inserir(m, guarda_trilha=False):
        A("-- %s%s" % (m["titulo"], (" [%s/%s]" % (m["controller"], m["action"])).replace("/]", "]")
                       if m["controller"] else " [processo]"))
        A("INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, subtitulo, ativo, usuario, data_inclusao)")
        A("VALUES (%s, %s, %s, %s, %s, %s, %d, %s, GETDATE());" % (
            "NULL" if not CODIGO_EMPRESA else str(CODIGO_EMPRESA),
            lit(m["tipo"]), lit(m["controller"]), lit(m["action"]),
            lit(m["titulo"]), lit(m["subtitulo"]), 1 if m["ativo"] else 0, lit(ORIGEM)))
        A("SET @codigo = SCOPE_IDENTITY();")
        if guarda_trilha:
            A("INSERT INTO @trilha (chave, codigo) VALUES (%s, @codigo);" % lit(m["chave"]))
        if m.get("processo"):
            A("UPDATE tb_manual SET codigo_manual_processo = "
              "(SELECT codigo FROM @trilha WHERE chave = %s) WHERE codigo = @codigo;" % lit(m["processo"]))

        for s in m["itens"]:
            A("INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, tipo_nota, nota, imagem, video, ativo)")
            A("VALUES (@codigo, %d, %s, %s, %s, %s, %s, %s, 1);" % (
                s["sequencia"], lit(s["titulo"]), lit(s["conteudo"]),
                lit(s["tipo_nota"] if s["tipo_nota"] in ("D", "A") else ""),
                lit(s["nota"]), lit(s["imagem"]), lit(s["video"])))
        A("")

    A("-- ---- Trilhas (manuais de processo) ----")
    A("")
    for p in processos:
        inserir(p, guarda_trilha=True)

    A("-- ---- Artigos ----")
    A("")
    for m in manuais:
        inserir(m)

    A("COMMIT TRANSACTION;")
    A("")
    A("SELECT manuais = (SELECT COUNT(*) FROM tb_manual WHERE usuario = %s)," % lit(ORIGEM))
    A("       secoes  = (SELECT COUNT(*) FROM tb_manual_item i")
    A("                  JOIN tb_manual m ON m.codigo = i.codigo_manual WHERE m.usuario = %s);" % lit(ORIGEM))
    return "\n".join(L)


def gravar_sqlserver(processos: list[dict], manuais: list[dict]) -> None:
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

    cn = pyodbc.connect(cs, autocommit=False)
    try:
        cur = cn.cursor()

        if LIMPAR_ANTES:
            cur.execute("DELETE FROM tb_manual_item WHERE codigo_manual IN "
                        "(SELECT codigo FROM tb_manual WHERE usuario = ?)", ORIGEM)
            cur.execute("UPDATE tb_manual SET codigo_manual_processo = NULL WHERE usuario = ?", ORIGEM)
            cur.execute("DELETE FROM tb_manual WHERE usuario = ?", ORIGEM)

        codigos = {}
        secoes = 0

        for m in processos + manuais:
            cur.execute(
                "INSERT INTO tb_manual (codigo_empresa, tipo, controller, [action], titulo, "
                "subtitulo, ativo, usuario, data_inclusao) "
                "OUTPUT INSERTED.codigo VALUES (?, ?, ?, ?, ?, ?, ?, ?, GETDATE())",
                (CODIGO_EMPRESA or None), m["tipo"], m["controller"] or None, m["action"] or None,
                m["titulo"], m["subtitulo"] or None, 1 if m["ativo"] else 0, ORIGEM)
            codigo = cur.fetchone()[0]
            codigos[m["chave"]] = codigo

            if m.get("processo") and m["processo"] in codigos:
                cur.execute("UPDATE tb_manual SET codigo_manual_processo = ? WHERE codigo = ?",
                            codigos[m["processo"]], codigo)

            for s in m["itens"]:
                cur.execute(
                    "INSERT INTO tb_manual_item (codigo_manual, sequencia, titulo, conteudo, "
                    "tipo_nota, nota, imagem, video, ativo) VALUES (?, ?, ?, ?, ?, ?, ?, ?, 1)",
                    codigo, s["sequencia"], s["titulo"], s["conteudo"] or None,
                    (s["tipo_nota"] if s["tipo_nota"] in ("D", "A") else None),
                    s["nota"] or None, s["imagem"] or None, s["video"] or None)
                secoes += 1

        cn.commit()
    except Exception:
        cn.rollback()
        raise
    finally:
        cn.close()

    print("Gravado no SQL Server: %d manuais, %d secoes." % (len(processos) + len(manuais), secoes))


# ─────────────────────────────────────────────────────────────────────────────────────────
# Comandos
# ─────────────────────────────────────────────────────────────────────────────────────────

def cmd_inspecionar(conn) -> None:
    print("Tabelas no Supabase\n" + "=" * 70)
    for schema, tabela in conn.run(
        "SELECT table_schema, table_name FROM information_schema.tables "
        "WHERE table_type = 'BASE TABLE' AND table_schema NOT IN ('pg_catalog', 'information_schema') "
        "ORDER BY 1, 2"
    ):
        cols = [r[0] for r in conn.run(
            "SELECT column_name FROM information_schema.columns "
            "WHERE table_schema = :s AND table_name = :t ORDER BY ordinal_position", s=schema, t=tabela)]
        n = conn.run('SELECT COUNT(*) FROM "%s"."%s"' % (schema, tabela))[0][0]
        print("\n%s.%s  (%d linhas)" % (schema, tabela, n))
        print("  colunas: " + ", ".join(cols))


def cmd_amostra(conn, artigo_id: str = "") -> None:
    artigos = ler_articles(conn)
    if not artigos:
        sys.exit("Nenhum artigo publicado em public.articles.")

    a = next((x for x in artigos if x["id"] == artigo_id), None) if artigo_id else artigos[0]
    if a is None:
        print("Artigo '%s' nao encontrado. Alguns ids:" % artigo_id)
        for x in artigos[:15]:
            print("   %s" % x["id"])
        return

    print("=" * 74)
    print("%s   (%s)" % (a["titulo"], a["id"]))
    print("trilha: %s | secao: %s | video: %s" % (a["chapter_id"], a["secao_numero"], a["video"] or "—"))
    print("=" * 74)
    print("\n----- CONTEUDO ORIGINAL (Markdown) -----\n")
    print(a["conteudo"][:4000] + ("\n[... cortado]" if len(a["conteudo"]) > 4000 else ""))

    secoes = dividir_em_secoes(a["conteudo"], a["titulo"])
    print("\n\n----- VIRA %d SECAO(OES) NO PCM -----" % len(secoes))
    for s in secoes:
        corpo = s["conteudo"]
        print("\n  %d. %s   (%d caracteres)" % (s["sequencia"], s["titulo"], len(corpo)))
        for linha in corpo.splitlines()[:4]:
            print("       | " + linha[:90])
        if len(corpo.splitlines()) > 4:
            print("       | ...")


def cmd_previa(processos: list[dict], manuais: list[dict]) -> None:
    total = sum(len(m["itens"]) for m in processos + manuais)
    videos = sum(1 for m in manuais for s in m["itens"] if s["video"])
    telas = sum(1 for m in manuais if m["tipo"] == "S")
    sem = [m for m in manuais if m["tipo"] == "P"]

    print("%d trilhas + %d artigos = %d manuais, %d secoes (%d com video)"
          % (len(processos), len(manuais), len(processos) + len(manuais), total, videos))
    print("%d artigos ligados a uma tela, %d ainda sem tela (entram como processo)\n"
          % (telas, len(sem)))

    for p in processos:
        print("  [trilha]  %s" % p["titulo"])
    print()
    for m in manuais:
        if m["tipo"] != "S":
            continue
        print("  %-46s -> %s/%s  (%d secoes)"
              % (m["titulo"][:46], m["controller"], m["action"], len(m["itens"])))

    if sem:
        print("\n  Sem tela definida (viram manual de processo):")
        for m in sem:
            print("     %s" % m["titulo"])
        print("\n  Para ligar a uma tela, edite %s e rode de novo." % os.path.basename(ARQUIVO_MAPA))


def main() -> None:
    cmd = sys.argv[1] if len(sys.argv) > 1 else "previa"
    arg = sys.argv[2] if len(sys.argv) > 2 else ""

    if cmd not in ("inspecionar", "amostra", "mapear", "previa", "gerar-sql", "migrar"):
        sys.exit(__doc__)

    conn = conectar_pg()
    try:
        if cmd == "inspecionar":
            cmd_inspecionar(conn)
            return

        if cmd == "amostra":
            cmd_amostra(conn, arg)
            return

        if cmd == "mapear":
            chapters = {c["id"]: c for c in ler_chapters(conn)}
            gerar_mapa(ler_articles(conn), chapters)
            return

        processos, manuais = montar(conn)
    finally:
        conn.close()

    if not manuais:
        sys.exit("Nada encontrado para migrar em public.articles.")

    if not os.path.exists(ARQUIVO_MAPA):
        print("(sem %s — usando o palpite automatico de telas; rode 'mapear' para revisar)\n"
              % os.path.basename(ARQUIVO_MAPA))

    if cmd == "previa":
        cmd_previa(processos, manuais)
    elif cmd == "gerar-sql":
        # utf-8-sig: sem o BOM, o SSMS abre o arquivo como ANSI e "Ordens de Serviço"
        # chega no banco com os acentos trocados.
        with open(ARQUIVO_SQL, "w", encoding="utf-8-sig") as f:
            f.write(gerar_sql(processos, manuais))
        cmd_previa(processos, manuais)
        print("\nGerado %s — abra no SSMS (banco PCM) e execute." % os.path.basename(ARQUIVO_SQL))
    elif cmd == "migrar":
        cmd_previa(processos, manuais)
        gravar_sqlserver(processos, manuais)


if __name__ == "__main__":
    main()
