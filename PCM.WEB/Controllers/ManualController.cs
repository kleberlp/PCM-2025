using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PCM.WEB.MODELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManualDal = PCM.WEB.DAL.Manual;

namespace PCM.WEB.Controllers
{
    /// <summary>
    /// Manual integrado: telas de cadastro (HelpIndex / HelpInsert / HelpEdit) e
    /// o JSON que alimenta o painel do botão "?" do cabeçalho (ManualTela).
    /// Conteúdo e permissão vivem no banco PCM — a estrutura está em
    /// PCM.WEB.DAL\Scripts\2026-08-27_manual_integrado.sql.
    /// </summary>
    public class ManualController : Controller
    {
        private ManualDal oManual = new ManualDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private PCM.WEB.DAL.Account oAccount = new PCM.WEB.DAL.Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: APOIO :::

        private int codigoEmpresa
        {
            get { return Convert.ToInt32(Session["empresa"]); }
        }

        /// <summary>
        /// Direitos da manutenção do manual. Enquanto o formulário 'adm_manual'
        /// não existir no cadastro de perfis, vale o direito de 'adm_perfil':
        /// quem administra perfis também mantém o manual (ver o passo 8 de
        /// PCM.WEB.DAL\Scripts\2026-08-27_manual_integrado.sql).
        /// </summary>
        private void LoadPerfilManual(ref bool inserir, ref bool editar, ref bool excluir)
        {
            bool administrador = false;

            oAccount.LoadPerfil(iCodigoEmpresa: codigoEmpresa,
                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                sFormulario: "adm_manual",
                                bInserir: ref inserir,
                                bEditar: ref editar,
                                bExcluir: ref excluir,
                                bAdministrador: ref administrador);

            if (!inserir && !editar && !excluir && !administrador)
            {
                oAccount.LoadPerfil(iCodigoEmpresa: codigoEmpresa,
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "adm_perfil",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);
            }
        }

        private string UsuarioAtual()
        {
            string usuario = Convert.ToString(Session["email"]);
            if (string.IsNullOrEmpty(usuario)) usuario = Convert.ToString(Session["nome"]);
            if (string.IsNullOrEmpty(usuario)) usuario = User.Identity.GetUserName();
            return usuario;
        }

        /// <summary>
        /// Telas do sistema para o combo do cadastro, por reflexão sobre os
        /// controllers: um mapa paralelo em tabela seria cadastro para alguém
        /// esquecer de manter. Lista só GET que devolve página (ActionResult
        /// sem [HttpPost]); os JSON de apoio não são tela e só sujariam o combo.
        /// </summary>
        private Dictionary<string, List<string>> TelasDoSistema()
        {
            var telas = new Dictionary<string, List<string>>();

            Type[] tipos;
            try
            {
                tipos = typeof(ManualController).Assembly.GetTypes();
            }
            catch (System.Reflection.ReflectionTypeLoadException ex)
            {
                // Um tipo que não carrega não pode custar o combo inteiro.
                tipos = ex.Types.Where(t => t != null).ToArray();
            }

            foreach (var tipo in tipos.Where(t => !t.IsAbstract && typeof(Controller).IsAssignableFrom(t))
                                      .OrderBy(t => t.Name))
            {
                string nome = tipo.Name.EndsWith("Controller") ? tipo.Name.Substring(0, tipo.Name.Length - "Controller".Length) : tipo.Name;
                if (nome == "Base") continue;

                var acoes = tipo.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly)
                                .Where(m => typeof(ActionResult).IsAssignableFrom(m.ReturnType))
                                .Where(m => !typeof(JsonResult).IsAssignableFrom(m.ReturnType))
                                .Where(m => !m.GetCustomAttributes(typeof(HttpPostAttribute), false).Any())
                                .Where(m => !m.IsSpecialName)
                                .Select(m => m.Name)
                                .Distinct()
                                .OrderBy(a => a)
                                .ToList();

                if (acoes.Count > 0) telas[nome] = acoes;
            }

            return telas;
        }

        #endregion

        #region ::: PAINEL ("?") :::

        /// <summary>
        /// Manual da tela em que o usuário está. Vive num controller próprio
        /// porque o botão "?" existe em toda tela — quem responde precisa estar
        /// acessível de qualquer uma.
        /// </summary>
        /// <remarks>
        /// Os parâmetros NÃO podem se chamar controller e action: esses nomes
        /// são valores de rota da própria requisição, e o binder os preencheria
        /// com "Manual"/"ManualTela" antes de olhar a query string.
        /// </remarks>
        [HttpGet]
        public JsonResult ManualTela(string screenController, string screenAction, int codigo = 0)
        {
            try
            {
                if (Session["empresa"] == null)
                    return Json(new { found = false }, JsonRequestBehavior.AllowGet);

                // codigo preenchido = o leitor pediu um manual pelo link "ver
                // também" do rodapé; sem ele, é o manual da tela em que está.
                Manual manual = codigo > 0
                    ? oManual.InfoManual(iCodigoEmpresa: codigoEmpresa, iCodigo: codigo, bSomenteAtivo: true)
                    : oManual.ManualTela(iCodigoEmpresa: codigoEmpresa,
                                         sController: screenController ?? "",
                                         sAction: screenAction ?? "");

                // Quem pode manter o manual vê o atalho de edição no rodapé.
                bool inserir = false, editar = false, excluir = false;
                LoadPerfilManual(ref inserir, ref editar, ref excluir);

                return Json(new
                {
                    found = manual.codigo > 0,
                    manual,
                    canEdit = inserir || editar
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                // O manual é apoio: falhou (banco fora, rede), o painel mostra
                // "sem conteúdo" em vez de derrubar a tela de quem trabalha.
                return Json(new { found = false }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region ::: CADASTRO :::

        // GET: INDEX
        public ActionResult HelpIndex()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                bool inserir = false, editar = false, excluir = false;
                LoadPerfilManual(ref inserir, ref editar, ref excluir);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                return View(oManual.IndexManual(iCodigoEmpresa: codigoEmpresa));
            }
        }

        // GET: INSERT — controller/action chegam preenchidos quando o autor vem
        // pelo atalho "criar manual desta tela" do painel.
        public ActionResult HelpInsert(string tela_controller = "", string tela_action = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                bool inserir = false, editar = false, excluir = false;
                LoadPerfilManual(ref inserir, ref editar, ref excluir);

                if (!inserir) return RedirectToAction("HelpIndex");

                ViewBag.processos = oManual.ComboProcesso(iCodigoEmpresa: codigoEmpresa);
                ViewBag.telas = TelasDoSistema();

                return View("HelpInsert", new Manual
                {
                    tipo = "S",
                    controller = tela_controller ?? "",
                    action = tela_action ?? "",
                    ativo = true
                });
            }
        }

        // GET: EDIT
        public ActionResult HelpEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                bool inserir = false, editar = false, excluir = false;
                LoadPerfilManual(ref inserir, ref editar, ref excluir);

                if (!editar) return RedirectToAction("HelpIndex");

                Manual manual = oManual.InfoManual(iCodigoEmpresa: codigoEmpresa, iCodigo: codigo);
                if (manual.codigo == 0) return HttpNotFound();

                ViewBag.processos = oManual.ComboProcesso(iCodigoEmpresa: codigoEmpresa);
                ViewBag.telas = TelasDoSistema();

                return View("HelpEdit", manual);
            }
        }

        // POST: SAVE — cabeçalho e seções (JSON) de uma vez, para não ficar
        // meio-caminho gravado se o navegador cair no meio da edição.
        [HttpPost]
        public JsonResult SaveHelp(int codigo, string tipo, string tela_controller, string tela_action,
                                   int processo, string titulo, string subtitulo, bool ativo, string itensJson)
        {
            try
            {
                if (Session["empresa"] == null)
                    return Json(new { success = false, message = "Sessão expirada. Entre novamente." });

                bool inserir = false, editar = false, excluir = false;
                LoadPerfilManual(ref inserir, ref editar, ref excluir);

                if ((codigo == 0 && !inserir) || (codigo > 0 && !editar))
                    return Json(new { success = false, message = "Você não tem permissão para manter o manual." });

                var manual = new Manual
                {
                    codigo = codigo,
                    tipo = tipo == "P" ? "P" : "S",
                    controller = tela_controller ?? "",
                    action = tela_action ?? "",
                    processo_codigo = processo,
                    titulo = titulo ?? "",
                    subtitulo = subtitulo ?? "",
                    ativo = ativo,
                    itens = JsonConvert.DeserializeObject<List<ManualItem>>(itensJson ?? "[]") ?? new List<ManualItem>()
                };

                if (string.IsNullOrWhiteSpace(manual.titulo))
                    return Json(new { success = false, message = "Informe o título do manual." });

                // Link de vídeo é hyperlink, não texto solto: sem http(s), não grava.
                foreach (var item in manual.itens)
                {
                    if (!string.IsNullOrEmpty(item.video) &&
                        !item.video.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                        !item.video.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    {
                        return Json(new { success = false, message = "O link de vídeo da seção \"" + item.titulo + "\" precisa começar com http:// ou https://." });
                    }
                }

                // Empresa 0 = manual do sistema, que é o que a tela cadastra: o manual
                // descreve o PCM, e não os dados de um cliente. A coluna codigo_empresa
                // continua servindo para uma empresa que precise do próprio texto de
                // uma tela — essa exceção entra por script, e vence a global na leitura.
                int novoCodigo = oManual.SaveManual(iCodigoEmpresa: 0,
                                                    oManual: manual,
                                                    sUsuario: UsuarioAtual());

                return Json(new { success = true, codigo = novoCodigo });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: DELETE
        [HttpPost]
        public JsonResult DeleteHelp(int codigo)
        {
            try
            {
                if (Session["empresa"] == null)
                    return Json(new { success = false, message = "Sessão expirada. Entre novamente." });

                bool inserir = false, editar = false, excluir = false;
                LoadPerfilManual(ref inserir, ref editar, ref excluir);

                if (!excluir)
                    return Json(new { success = false, message = "Você não tem permissão para excluir o manual." });

                oManual.DeleteManual(iCodigo: codigo);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Imagem de uma seção. Sobe na hora de escolher, para o autor ver a
        /// imagem antes de gravar o manual. Guarda em ~/Files/Manual e devolve o
        /// caminho relativo, que é o que fica gravado na seção.
        /// </summary>
        [HttpPost]
        public JsonResult UploadHelpImage()
        {
            try
            {
                if (Session["empresa"] == null)
                    return Json(new { success = false, message = "Sessão expirada. Entre novamente." });

                bool inserir = false, editar = false, excluir = false;
                LoadPerfilManual(ref inserir, ref editar, ref excluir);

                if (!inserir && !editar)
                    return Json(new { success = false, message = "Você não tem permissão para manter o manual." });

                HttpPostedFileBase imagem = Request.Files.Count > 0 ? Request.Files[0] : null;

                if (imagem == null || imagem.ContentLength == 0)
                    return Json(new { success = false, message = "Selecione uma imagem." });

                string extensao = Path.GetExtension(imagem.FileName).ToLowerInvariant();

                // Só imagem: a seção ilustra um passo, não carrega anexo.
                if (extensao != ".jpg" && extensao != ".jpeg" && extensao != ".png" && extensao != ".gif")
                    return Json(new { success = false, message = "A imagem precisa ser JPG, PNG ou GIF." });

                string pasta = Server.MapPath("~/Files/Manual");
                if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);

                string nome = Guid.NewGuid().ToString("N") + extensao;
                imagem.SaveAs(Path.Combine(pasta, nome));

                return Json(new { success = true, path = Url.Content("~/Files/Manual/" + nome) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

    }
}
