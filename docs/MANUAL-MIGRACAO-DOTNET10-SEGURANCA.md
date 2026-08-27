# Manual de Migração — PCM para .NET 10, CSP e Segurança

> Data do levantamento: 21/08/2026 · Base: branch `master`
> Números extraídos do próprio repositório (contagens reais, não estimadas).

Este manual consolida o plano para levar a solução PCM ao **.NET 10 (LTS)**, aplicar
**Content Security Policy (CSP)** e endurecer a segurança da aplicação. Está organizado
para ser executado em fases independentes — cada fase entrega valor sozinha e pode ser
publicada sem esperar as demais.

---

## 1. Inventário da solução

### 1.1 Já em .NET moderno (retarget trivial para net10)

| Projeto | Framework atual |
|---|---|
| PCM.WEB.OS (+ .DAL, .MODELS) | net8.0 |
| MESSAGE.SERVICE | net8.0 |
| PCM.INTERFACE.INTERCITY (+ .DAL, .MODELS) | net8.0 |
| PCM.API | net6.0 |
| PCM.WEB.INTERFACE | net6.0 |
| PCM.JSON | net6.0-windows |
| PCM.IMAGE.RESIZE | net6.0-windows |

### 1.2 Em .NET Framework 4.8 (o esforço real mora aqui)

| Projeto | Tipo | Observação |
|---|---|---|
| **PCM.WEB** | ASP.NET MVC5 | 469 views (133 mil linhas), 29 controllers (38 mil linhas) |
| PCM.ADM.WEB | ASP.NET MVC5 | 113 views |
| PCM.WEB.API / PCM.WEB.API.TOTVS | Web API 2 | menores |
| PCM.WEB.DAL | Class library VB.NET | ~80 mil linhas, ADO.NET puro |
| PCM.WEB.MODELS | Class library C# | ~7 mil linhas |
| PCM.SERVICE, .INTERCITY, .LAUDO, .MESSAGE, .TAG, .WISH, PCM.EMAIL.LAUDO, PCM.TAG.FORM | Serviços Windows VB.NET | 7 serviços + 1 WinForms |

### 1.3 Métricas que definem o tamanho do PCM.WEB

| Métrica | Valor | Impacto na migração |
|---|---|---|
| `Session[...]` | **6.561 usos** | Session do ASP.NET Core só guarda string/bytes → exige camada adaptadora |
| `HttpPostedFileBase` | 83 usos | vira `IFormFile` — mecânico |
| Identity 2 + OWIN | Startup.Auth | migrar para ASP.NET Core Identity (hashes de senha compatíveis) |
| **Crystal Reports** | 15 referências, **60 arquivos .rpt** | **bloqueador: Crystal não existe em .NET Core/5+** |
| Views com `<script>` inline | 392 | superfície do trabalho de CSP |
| Handlers inline (`onclick=` etc.) | 364 | precisam virar handlers registrados |
| `style=""` inline | 2.737 | terceiro degrau da CSP |

---

## 2. Fases e esforço

Estimativas para **1 desenvolvedor sênior dedicado**. Com 2 pessoas, o calendário cai
para ~6–8 meses porque as fases 1–3 paralelizam com a 5.

| Fase | Escopo | Esforço | Risco |
|---|---|---|---|
| 0 | Retarget dos projetos já-Core → net10 | ~1 semana | baixo |
| 1 | Bibliotecas: PCM.WEB.MODELS + PCM.WEB.DAL para net10 | 3–4 semanas | baixo |
| 2 | 7 serviços Windows → Worker Services | 4–6 semanas | baixo |
| 3 | PCM.WEB.API + TOTVS → ASP.NET Core | 3–4 semanas | médio |
| 4 | **PCM.WEB MVC5 → ASP.NET Core MVC** | **3–5 meses** | alto |
| 5 | **Substituição do Crystal Reports (60 relatórios)** | **2–4 meses** | **o maior da conta** |
| 6 | PCM.ADM.WEB (mesma receita da fase 4) | 1–1,5 mês | médio |

**Total realista: 9 a 14 meses·pessoa.**

### Notas por fase

**Fase 0** — trocar `<TargetFramework>` para `net10.0`, atualizar pacotes, rodar testes.
Projetos `net6.0-windows` (PCM.JSON, PCM.IMAGE.RESIZE) mantêm o sufixo `-windows`.

> **ATENÇÃO — alvo interino net9.0 (2026-08-27):** o Visual Studio instalado não
> compila net10 (NETSDK1209 exige VS ≥ 17.16), então todos os retargets abaixo
> estão temporariamente em **net9.0** (STS, fim de suporte em maio/2026). Assim
> que o VS for atualizado, trocar `net9.0` → `net10.0` nos .csproj/.vbproj/.pubxml
> (busca e substituição direta; o JwtBearer 9.0.4 vira 10.x junto).
>
> **EXECUTADA em 2026-08-27** — 12 projetos retargetados para net10.0 (os 12 da
> tabela 1.1 mais PCM.INTERFACE.ATRIO/DAL/MODELS, que não existiam no levantamento),
> incluindo os perfis de publicação (.pubxml). Único pacote atualizado:
> `Microsoft.AspNetCore.Authentication.JwtBearer` 6.0.28 → 10.0.0 nas duas APIs
> (série 6.x em fim de vida, com CVEs conhecidas). Pendências da fase:
> 1. compilar com o SDK .NET 10 (VS atualizado) e rodar smoke tests;
> 2. instalar o runtime/hosting bundle do .NET 10 nos servidores ANTES do
>    próximo deploy desses projetos (PCM.WEB.OS em produção é o crítico);
> 3. com acesso ao NuGet, elevar EF Core 9→10, Microsoft.Extensions.* 8.x→10.x,
>    Microsoft.Windows.Compatibility 9→10 e Swashbuckle nas APIs.

**Fase 1** — VB.NET é suportado em class libraries .NET 10; o PCM.WEB.OS.DAL (net8, VB)
prova o padrão dentro do próprio repositório. Trabalho principal: converter os `.vbproj`
para SDK-style e trocar `System.Data.SqlClient` → `Microsoft.Data.SqlClient`
(atenção: o novo driver liga `Encrypt=True` por padrão — validar certificado do SQL Server
ou ajustar a connection string conscientemente).

**Fase 2** — o padrão Worker Service já existe no repo (MESSAGE.SERVICE). Cada serviço
4.8 é pequeno; a conversão é repetitiva. Aproveitar para tirar segredos do fonte
(ver §4.1 — há senha de SMTP hardcoded hoje).

**Fase 4** — os pontos de atenção do PCM.WEB, em ordem de dor:
1. **Session (6.561 usos)** — criar extensão `Session.GetObject<T>()/SetObject<T>()`
   serializando JSON, e substituir por busca-e-troca. Não tentar redesenhar o uso de
   sessão durante a migração — é escopo separado.
2. **Identity/OWIN → ASP.NET Core Identity** — o hash de senha do Identity 2 é
   compatível com o Core; usuários não precisam trocar senha.
3. **Bundling** (`@Scripts.Render`/`@Styles.Render`) → referências diretas + minificação
   no build (o projeto já serve muita coisa sem bundle).
4. **Views** — Razor é majoritariamente compatível; `Views/Web.config` vira
   `_ViewImports.cshtml`; `Session["..."]` em view passa por `@Context.Session`.
5. **`HttpPostedFileBase` → `IFormFile`** — mecânico (83 pontos).

**Fase 5 — Crystal Reports** é decisão de produto, não só técnica:
- Opção A: reescrever os 60 relatórios em outra engine (FastReport, DevExpress,
  Stimulsoft, QuestPDF). Custo alto, resolve de vez.
- Opção B: manter um "servidor de relatórios" mínimo em 4.8 só com o Crystal,
  chamado por HTTP pela aplicação nova. Custo baixo agora, dívida permanente.
- Recomendação: decidir esta fase ANTES de começar a fase 4 — ela muda a conta em meses.

---

## 3. CSP — independente da migração

CSP é trabalho de front-end: **pode começar hoje, no MVC5**, e tudo que for feito
sobrevive à migração. Três degraus:

### Degrau 1 — nonce nos scripts (mata o `unsafe-inline` de script)
- Gerar nonce por request (em 4.8: `HttpContext.Items`; um helper Razor
  `@Html.CspScriptNonce()` imprime `nonce="..."`).
- Aplicar nos `<script>` inline das 392 views (busca-e-troca assistida).
- Header: `script-src 'self' 'nonce-{...}'`. XSS por injeção de script morre aqui.
- `style-src 'unsafe-inline'` permanece temporariamente (risco muito menor).

### Degrau 2 — remover os 364 handlers inline
- `onclick="fn()"` → `id`/`data-*` + `$(document).on('click', ...)`.
- Padrão já aplicado nas telas novas do módulo Ativo Fixo — usar como referência.

### Degrau 3 — estilos inline (2.737 ocorrências)
- Tela a tela, junto da revisão de views da fase 4. Só então remover
  `unsafe-inline` de `style-src`.

**Regra para código novo (vale desde já):** JS em arquivo externo, sem `onclick`/`style`
inline, dados do servidor via `data-*` ou endpoint JSON — nunca concatenados no script.
O PCM.WEB.OS já segue este padrão e tem CSP ativa como referência de política.

---

## 4. Hardening que não espera a migração (~2–3 semanas)

Entrega mais segurança imediata do que a migração em si.

### 4.1 Segredos
- [ ] **Senha de SMTP hardcoded** (PCM.SERVICE.MESSAGE/Main.vb e SendEmailLaudo):
      mover para configuração e **trocar a senha** — ela está no histórico do Git.
- [ ] Connection strings e chaves (Firebase etc.) fora do fonte
      (variáveis de ambiente / cofre); `secrets.json` do PCM.SERVICE.MESSAGE fora do repo.

### 4.2 Transporte e cookies
- [ ] HSTS; redirecionamento HTTPS já no IIS.
- [ ] Cookies com `Secure; HttpOnly; SameSite=Lax` (auth e sessão).

### 4.3 Cabeçalhos
- [ ] `X-Content-Type-Options: nosniff`, `Referrer-Policy: strict-origin-when-cross-origin`,
      `X-Frame-Options: DENY` (ou `frame-ancestors` na CSP).
- [ ] Remover `Server`, `X-Powered-By`, `X-AspNet-Version`, `X-AspNetMvc-Version`.

### 4.4 Aplicação
- [ ] Auditar `[ValidateAntiForgeryToken]` em todos os POSTs (há endpoints sem).
- [ ] Upload: validar extensão, conteúdo e tamanho; gravar fora da webroot
      (padrão já usado no PCM.WEB.OS com lista de extensões permitidas).
- [ ] Endpoint que sirva arquivo por caminho: validar que o caminho pertence à pasta
      esperada (evitar leitura arbitrária de disco).
- [ ] Autorização: `LoadPerfil` presente em toda action de página; endpoints JSON
      também precisam checar sessão/perfil, não só as actions de view.
- [ ] Dependências: atualizar jQuery/plugins e pacotes NuGet com CVE conhecido
      (Newtonsoft.Json antigo, etc.).
- [ ] Acesso a dados: manter 100% via stored procedure parametrizada (padrão atual do
      DAL — preservar; nunca concatenar SQL).

---

## 5. Ordem recomendada de execução

1. **Hardening (§4) + CSP degrau 1** — agora, sem dependências.
2. **Fases 0–3** — baixo risco; destrava .NET 10 em tudo que não é web.
3. **Decisão do Crystal Reports (fase 5)** — antes de tocar o PCM.WEB.
4. **Fase 4 (PCM.WEB)** — por último, com o padrão provado pelo PCM.WEB.OS.
5. **Fase 6 (ADM.WEB)** — repete a receita.

---

## 6. Referências internas

- Padrão Worker Service: `MESSAGE.SERVICE/`
- Padrão web .NET Core + CSP ativa: `PCM.WEB.OS/Program.cs` (middleware de CSP)
- Padrão DAL VB em .NET moderno: `PCM.WEB.OS.DAL/`
- Padrão JS sem inline (delegação + arquivos externos): `PCM.WEB/Content/js/pages/AtivoFixo/`
