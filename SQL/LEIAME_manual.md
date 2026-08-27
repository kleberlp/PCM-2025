# Manual integrado — como publicar

O manual (botão "?" do cabeçalho + telas de cadastro) fica **no banco PCM, no SQL Server**.
O conteúdo que está no Supabase entra por uma migração de uma vez só.

## 1. Criar a estrutura

No SSMS, no banco **PCM**, execute:

```
PCM.WEB.DAL\Scripts\2026-08-27_manual_integrado.sql
```

Cria `tb_manual` e `tb_manual_item` e as procedures `sp_select_manual_tela`,
`sp_select_manual`, `sp_select_manual_index`, `sp_select_manual_combo_processo`,
`sp_save_manual` e `sp_delete_manual`. Rodar de novo é seguro: as tabelas só nascem
se não existirem e as procedures são recriadas.

No fim, o script lista as tabelas que têm coluna `formulario` — é ali que entra o
`adm_manual`, se você quiser controlar o direito do manual separadamente. **Sem isso o
manual já funciona**: quem tem `adm_perfil` mantém o manual.

## 2. Trazer o conteúdo do Supabase

```bash
pip install pg8000          # obrigatório, lê o Supabase
pip install pyodbc          # opcional, grava direto no SQL Server
```

### De onde vem o quê

| No Supabase | Vira no PCM |
|---|---|
| `public.chapters` (12 trilhas) | Manual de **processo** — aparece como "Ver também" no rodapé do painel |
| `public.articles` (82 artigos) | Manual de **tela** — um por artigo, com o botão "?" da tela |
| `articles.content` (Markdown) | **Seções** do manual, quebradas nos títulos `##` |
| `articles.video_url` | Vídeo da primeira seção (YouTube, Vimeo e Drive abrem incorporados) |

### Qual tela é cada artigo

O Supabase organiza o manual **por trilha de treinamento**, não por tela — `articles` não
tem controller/action. Como o "?" precisa saber a tela, o script casa o título do artigo
com o nome que a tela tem **no menu do PCM** (`telas_pcm.csv`, 159 telas extraídas do
`_Sidebar`) e grava o palpite num CSV para você revisar.

```bash
python migrar_manual_supabase.py mapear
```

Abra o **`mapa_telas.csv`** no Excel. A coluna `confianca` diz o quanto confiar:

- **alta** — bateu quase exato (`Categoria de Serviço` → `CadastroBasico/CategoriaIndex`)
- **media** / **baixa** — palpite, confira
- **sem** — não achou tela; o artigo entra como manual de processo

Corrija o que estiver errado e salve. Linha com `controller` vazio vira manual de
processo — continua no cadastro, visível e editável, e você liga na tela depois.

### Rodar

```bash
python migrar_manual_supabase.py amostra    # vê um artigo inteiro e como fica dividido
python migrar_manual_supabase.py previa     # o que seria migrado, sem gravar
python migrar_manual_supabase.py gerar-sql  # gera carga_manual.sql para o SSMS
python migrar_manual_supabase.py migrar     # ou grava direto (precisa de pyodbc)
```

Comece pelo `amostra`: ele mostra o Markdown original de um artigo e as seções que vão
sair dele. É a maneira mais rápida de ver se a quebra ficou boa antes de migrar os 82.

Rodar a migração de novo é seguro — a carga apaga só o que veio de uma execução anterior
(os registros com `usuario = 'supabase'`).

## 3. Conferir na aplicação

- **Administração → Manual** lista o que foi migrado.
- O **"?"** no cabeçalho abre o manual da tela em que você está.

Se o "?" abrir vazio numa tela que deveria ter manual, o `controller`/`action` do registro
não bate com a URL. A grade mostra a rota de cada manual; a tela de edição tem os combos
com as rotas válidas do sistema.

## Detalhes que valem saber

**O conteúdo é Markdown.** O painel renderiza títulos, **negrito**, *itálico*, `código`,
listas, **tabelas**, citação, imagem e link. Tudo é escapado antes de virar marcação, e
link só passa se for `http(s)` ou caminho do próprio site — texto do manual não vira
script na tela de quem lê. Tabela larga rola dentro dela mesma, sem esticar o painel.

**Tela, módulo e processo.** `tipo = 'S'` com `action` preenchida é o manual daquela tela;
com `action` vazia vale para o módulo inteiro (qualquer tela do mesmo controller que não
tenha manual próprio cai nele). `tipo = 'P'` é manual de processo: não tem tela, e aparece
como "ver também" no rodapé do painel das telas que apontam para ele.

**Multiempresa.** `codigo_empresa` nulo = manual do sistema, vale para todas as empresas —
é o que a tela de cadastro grava, porque o manual descreve o PCM e não os dados de um
cliente. Uma empresa que precise do próprio texto de uma tela recebe a linha com o código
dela por script, e essa linha vence a global na leitura, sem afetar as demais.

**Imagens.** As imagens dos artigos ficam embutidas no Markdown, na posição em que o autor
colocou. Se apontam para um servidor que vai sair do ar, publique os arquivos em
`~/Files/Manual` e ajuste as URLs no texto. O campo `imagem` da seção continua existindo
para quem cadastra pela tela, com upload direto.

**Vídeos.** Abrem incorporados no painel, com o player carregando só quando a seção é
aberta:

| Link | Como aparece |
|---|---|
| YouTube (`youtu.be/…`, `watch?v=`, `/shorts/`) | player incorporado |
| Vimeo | player incorporado |
| Google Drive (`/file/d/…/view`, `/preview`, `open?id=`) | player incorporado |
| Arquivo de vídeo (`.mp4`, `.webm`, `.mov`…) | player do próprio navegador |
| Qualquer outra URL | link para abrir em outra aba |

No Drive, o link que a pessoa copia costuma ser o `/view` — não precisa converter para
`/preview` na mão, o painel faz isso. O arquivo tem que estar compartilhado como
"qualquer pessoa com o link", senão o player pede login.

## Se os títulos aparecerem com "??"

Emoji e símbolo não cabem na code page do `varchar`, e viram `?` **ao gravar**. A versão
atual do script de estrutura já cria `titulo` e `subtitulo` como `nvarchar` e conserta
quem criou antes — mas o texto já gravado com `?` não volta sozinho:

1. Rode de novo o `2026-08-27_manual_integrado.sql` (ele tem o `ALTER COLUMN`).
2. Rode de novo a migração — a carga refaz o que veio do Supabase.
