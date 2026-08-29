# Migração PCM → .NET 10 (PCMCore)

Branch: `pcm_NetCore` (baseada na master). Os projetos antigos **continuam
intactos** — a produção segue saindo deles até a virada. Os novos convivem
lado a lado no mesmo repositório:

| Antigo (net48)   | Novo (net10)        | Estado |
|------------------|---------------------|--------|
| PCM.WEB.MODELS   | PCMCore.WEB.MODELS  | ✅ Convertido por inteiro (33 arquivos) |
| PCM.WEB.DAL      | PCMCore.WEB.DAL     | ✅ Convertido por inteiro (40 arquivos) |
| PCM.WEB          | PCMCore.Web         | 🚧 Esqueleto pronto; telas migram por módulo |

Solution nova: `PCMCore.sln` (a `PCM.sln` não foi tocada).

## Decisões de desenho

**Namespaces não mudam.** Os assemblies chamam `PCMCore.*`, mas o namespace
raiz segue `PCM.WEB`, `PCM.WEB.DAL` e `PCM.WEB.MODELS`. É o que faz um
controller ou classe do DAL migrar quase sem diff — só o csproj sabe que o
mundo mudou.

**BaseController.** Todo controller migrado herda de
`PCM.WEB.Controllers.BaseController` (PCMCore.Web/Controllers/), que
concentra o que o MVC5 repetia 32 vezes:

- `codigoEmpresa` — Session["empresa"]
- `codigoUnidade` — Session["codigo_unidade"]
- `codigoUsuario` — código no cookie de autenticação (User.Identity.Name)
- `nomeUsuario` — Session["nome"]
- `Logado` — substitui `if (Session["empresa"] == null)`
- `RedirecionaLogin()` — redirect para o login com returnURL

**VB continua VB.** O DAL não foi traduzido para C# — .NET 10 compila VB em
class library normalmente. Traduzir seria risco sem ganho.

## O que já foi ajustado nos fontes copiados

- 56 `Imports` que nunca foram usados (lixo de auto-complete do VS) saíram:
  `System.DirectoryServices*`, `MS.Internal.*`, `System.Windows`,
  `OracleInternal.*`, `System.Security.Policy`, `System.Net.WebRequestMethods`,
  `System.Data.Sql`, `OfficeOpenXml.FormulaParsing.*`. Nenhum existe no Core.
- `using System.Web` removido dos MODELS (não era usado).

## Pacotes do PCMCore.WEB.DAL

| Pacote | Versão | Observação |
|---|---|---|
| EPPlus | 8.4.2 | mesma versão do net48 — zero mudança de API |
| Oracle.ManagedDataAccess.**Core** | 23.6.1 | a clássica é só net48 |
| System.Data.SqlClient | 4.9.0 | mantém o código como está; migrar p/ Microsoft.Data.SqlClient é passo futuro (muda default de Encrypt) |
| System.Data.OleDb | 9.0.4 | import de Excel do Stock.vb; só Windows + ACE instalado no servidor |
| System.Drawing.Common | 9.0.4 | só Windows (ok — o PCM roda em IIS) |
| System.Configuration.ConfigurationManager | 9.0.4 | EmailSender lê AppSettings — ver abaixo |
| Newtonsoft.Json | 13.0.3 | |

## Se o VS reclamar do net10 (NETSDK1209)

.NET 10 pede VS ≥ 17.16 (ou VS 2026) + SDK 10.x. Se a máquina ainda não
tiver, busca-e-troca `net10.0` → `net9.0` nos 3 projetos e siga; a volta é a
mesma troca ao contrário. Nada no código depende da versão.

## Receita de migração de um controller (MVC5 → Core)

1. Copiar o `.cs` de PCM.WEB/Controllers para PCMCore.Web/Controllers.
2. Herdar de `BaseController`; apagar as propriedades locais
   `codigoEmpresa`/`codigoUsuario` e os `using System.Web.*`.
3. Traduções mecânicas:

| MVC5 | Core |
|---|---|
| `Session["empresa"] == null` | `!Logado` (BaseController) |
| `Session["chave"] = valor` | `HttpContext.Session.SetInt32/SetString/SetBool` |
| `(bool)Session["cad_x"]` | `HttpContext.Session.GetBool("cad_x")` (Infra/SessionExtensions) |
| `User.Identity.GetUserName()` | `codigoUsuario` (BaseController) |
| `Json(x, JsonRequestBehavior.AllowGet)` | `Json(x)` (GET liberado por padrão) |
| `JsonResult` de POST | `Json(new { ... })` igual — só o AllowGet some |
| `HttpPostedFileBase` / `Request.Files` | `IFormFile` / `Request.Form.Files` |
| `Server.MapPath("~/Files/X")` | `IWebHostEnvironment.WebRootPath` + Path.Combine |
| `Request.RawUrl` | `Request.Path + Request.QueryString` |
| `[ValidateInput(false)]` | apagar — o Core não tem request validation |
| `ConfigurationManager.ConnectionStrings` | `IConfiguration.GetConnectionString("DefaultConnection")` injetado no construtor |
| `RedirectToAction(Session["dashboard"]...)` | igual, lendo `Session.GetString("dashboard")` |
| `HttpNotFound()` | `NotFound()` |
| `ActionResult` | `IActionResult` (o tipo antigo também compila) |

4. Views: Razor é ~igual. `@Scripts.Render`/`@Styles.Render` viram tags
   `<script>`/`<link>` apontando para `wwwroot` (o Content/ e os js atuais
   copiam para `wwwroot/` na migração do layout). `@Html.Partial` ok;
   `@PCM.WEB.Properties.Resources.x` volta a funcionar quando o
   `Resources.resx` for copiado para o PCMCore.Web (mesmo namespace).

## Ordem sugerida dos módulos

1. **Account** (login/logout — pivot de permissões para Session, cookie auth) — destrava tudo.
2. **Layout** (_Layout/_Header/_Sidebar + Content→wwwroot + manual/avisos globais).
3. **Home** (dashboards) — maior visibilidade.
4. OrdemServico (inclui Kanban), Manual, Aviso — recém-mexidos, código na cabeça.
5. Demais módulos por uso: Governanca, PMOC, Estoque, AtivoFixo, ...

## Pontos de atenção

- **EmailSender**: lê `ConfigurationManager.AppSettings("Email:...")`. No host
  Core isso NÃO lê appsettings.json. Na migração do Account, trocar por
  IConfiguration ou publicar um `PCMCore.Web.dll.config` com as chaves.
- **OleDb/ACE** (import de Excel no Stock.vb): exige Access Database Engine no
  servidor — igual hoje, mas lembrar na provisão do servidor novo.
- **Crystal Reports não existe no Core.** O PCM.WEB referencia Crystal? Os
  laudos que dependem dele precisam de decisão (manter serviço legado net48
  para gerar, ou trocar o gerador) ANTES de migrar o módulo que os usa.
- **481 views**: a conversão é por módulo, com o sistema antigo no ar. Não é
  big-bang.
