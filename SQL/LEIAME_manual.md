# Manual integrado — como publicar

O manual (botão "?" do cabeçalho + telas de cadastro) fica **no banco PCM, no SQL Server**.
O conteúdo que hoje está no Supabase entra por uma migração de uma vez só.

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

`migrar_manual_supabase.py` lê o Postgres e grava no SQL Server. Rode numa máquina que
enxergue os dois bancos (o NOTE-KLEBER serve).

```bash
pip install pg8000          # obrigatório, lê o Supabase
pip install pyodbc          # opcional, grava direto no SQL Server

python migrar_manual_supabase.py inspecionar   # 1. mostra tabelas, colunas e amostra
python migrar_manual_supabase.py previa        # 2. mostra o que viria, sem gravar
python migrar_manual_supabase.py gerar-sql     # 3. gera carga_manual.sql para o SSMS
python migrar_manual_supabase.py migrar        #    (ou grava direto, se tiver pyodbc)
```

A estrutura de origem **não precisa** estar no padrão novo: o script deduz o de-para pelos
nomes das colunas (`titulo`/`title`, `conteudo`/`texto`/`content`, `ordem`/`sequence`,
`imagem`, `video`/`link`…). Se errar o palpite, o `inspecionar` mostra os nomes reais e
você corrige `MAPA_MANUAL` / `MAPA_ITEM` no fim do arquivo.

O que ele faz de bônus: conteúdo que veio como HTML vira o texto simples que o painel
formata (`**negrito**`, listas com `- `), e link de vídeo sem `http(s)` não migra.

Rodar a migração de novo é seguro — a carga apaga só o que veio de uma execução anterior
(os registros com `usuario = 'supabase'`).

## 3. Conferir na aplicação

- **Administração → Manual** lista o que foi migrado.
- O **"?"** no cabeçalho abre o manual da tela em que você está.

Se o "?" abrir vazio numa tela que deveria ter manual, é porque `controller`/`action` do
registro não batem com a URL. A grade mostra a rota de cada manual; a tela de edição tem
os combos com as rotas válidas do sistema.

## Detalhes que valem saber

**Tela, módulo e processo.** `tipo = 'S'` com `action` preenchida é o manual daquela tela;
com `action` vazia vale para o módulo inteiro (qualquer tela do mesmo controller que não
tenha manual próprio cai nele). `tipo = 'P'` é manual de processo: não tem tela, e aparece
como "ver também" no rodapé do painel das telas que apontam para ele.

**Multiempresa.** `codigo_empresa` nulo = manual do sistema, vale para todas as empresas —
é o que a tela de cadastro grava, porque o manual descreve o PCM e não os dados de um
cliente. Uma empresa que precise do próprio texto de uma tela recebe a linha com o código
dela por script, e essa linha vence a global na leitura, sem afetar as demais.

**Imagens.** Sobem para `~/Files/Manual` e a seção guarda o caminho relativo. Se a base
antiga tinha imagens em outro lugar, o caminho vem como está: publique os arquivos nesse
mesmo caminho ou ajuste a coluna `imagem` depois da carga.

**Vídeos.** YouTube e Vimeo abrem incorporados no painel (o player só carrega quando a
seção é aberta); qualquer outra URL vira link para abrir em outra aba.
