using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PCM.WEB.MODELS;
using AvisoDal = PCM.WEB.DAL.Aviso;

namespace PCM.WEB.Controllers
{
    /// <summary>
    /// Avisos aos Clientes: manutenção (lista/cadastro/auditoria) e os endpoints
    /// do popup exibido no login (avisos pendentes, visualização, avaliação e
    /// dispensa). Estrutura no banco: PCM.WEB.DAL\Scripts\2026-08-28_avisos_clientes.sql.
    /// </summary>
    public class AvisoController : Controller
    {
        private AvisoDal oAviso = new AvisoDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private PCM.WEB.DAL.Account oAccount = new PCM.WEB.DAL.Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private PCM.WEB.DAL.Combo oCombo = new PCM.WEB.DAL.Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: APOIO :::

        private int codigoEmpresa
        {
            get { return Convert.ToInt32(Session["empresa"]); }
        }

        private int codigoUsuario
        {
            get { return Convert.ToInt32(User.Identity.GetUserName()); }
        }

        /// <summary>
        /// Direitos da manutenção de avisos: formulário 'cad_aviso' no cadastro
        /// de perfis (registrado no banco pela administração).
        /// </summary>
        private void LoadPerfilAviso(ref bool inserir, ref bool editar, ref bool excluir)
        {
            bool administrador = false;

            oAccount.LoadPerfil(iCodigoEmpresa: codigoEmpresa,
                                iCodigoUsuario: codigoUsuario,
                                sFormulario: "cad_aviso",
                                bInserir: ref inserir,
                                bEditar: ref editar,
                                bExcluir: ref excluir,
                                bAdministrador: ref administrador);
        }

        /// <summary>
        /// Sanitiza o HTML das seções: remove script/iframe/object/embed/form,
        /// atributos de evento (onclick etc.) e URLs javascript:. O conteúdo é
        /// digitado por administradores, mas será renderizado no navegador de
        /// todos os usuários — defesa em profundidade além da CSP.
        /// Aceita um documento inteiro colado (export de outra ferramenta):
        /// o invólucro (doctype/html/head/body) e o CSS global saem, porque o
        /// conteúdo vai para DENTRO da página do PCM — um style com "body{...}"
        /// restilizaria o sistema todo enquanto o popup existisse no DOM.
        /// </summary>
        private static string SanitizarHtml(string html)
        {
            if (string.IsNullOrEmpty(html)) return html;

            var opts = RegexOptions.IgnoreCase | RegexOptions.Singleline;

            html = Regex.Replace(html, @"<!--.*?-->", "", opts);
            html = Regex.Replace(html, @"<!DOCTYPE[^>]*>", "", opts);
            html = Regex.Replace(html, @"<\s*(script|iframe|object|embed|form|style|title)\b[^>]*>.*?<\s*/\s*\1\s*>", "", opts);
            html = Regex.Replace(html, @"<\s*(script|iframe|object|embed|form)\b[^>]*/?\s*>", "", opts);
            html = Regex.Replace(html, @"<\s*/?\s*(html|head|body)\b[^>]*>", "", opts);
            html = Regex.Replace(html, @"<\s*(meta|link|base)\b[^>]*/?\s*>", "", opts);
            html = Regex.Replace(html, @"\son\w+\s*=\s*""[^""]*""", "", opts);
            html = Regex.Replace(html, @"\son\w+\s*=\s*'[^']*'", "", opts);
            html = Regex.Replace(html, @"\son\w+\s*=\s*[^\s>]+", "", opts);
            html = Regex.Replace(html, @"(href|src)\s*=\s*(""|')\s*javascript:[^""']*\2", "$1=$2#$2", opts);

            return html;
        }

        #endregion

        #region ::: MANUTENCAO :::

        // GET: INDEX
        public ActionResult AvisoIndex()
        {
            if (Session["empresa"] == null)
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });

            bool inserir = false, editar = false, excluir = false;
            LoadPerfilAviso(ref inserir, ref editar, ref excluir);

            if (!inserir && !editar && !excluir)
                return RedirectToAction(Session["dashboard"].ToString(), "Home");

            ViewBag.inserir = inserir;
            ViewBag.editar = editar;
            ViewBag.excluir = excluir;
            ViewBag.empresas = oCombo.Empresa();

            return View(oAviso.IndexAviso(iCodigoEmpresa: codigoEmpresa));
        }

        // GET: INSERT
        public ActionResult AvisoInsert()
        {
            if (Session["empresa"] == null)
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });

            bool inserir = false, editar = false, excluir = false;
            LoadPerfilAviso(ref inserir, ref editar, ref excluir);

            if (!inserir) return RedirectToAction("AvisoIndex");

            ViewBag.empresas = oCombo.Empresa();

            return View("AvisoInsert", new Aviso
            {
                data_inicio = DateTime.Today.ToString("yyyy-MM-dd"),
                data_termino = DateTime.Today.AddDays(30).ToString("yyyy-MM-dd"),
                ativo = true
            });
        }

        // GET: EDIT
        public ActionResult AvisoEdit(int codigo)
        {
            if (Session["empresa"] == null)
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });

            bool inserir = false, editar = false, excluir = false;
            LoadPerfilAviso(ref inserir, ref editar, ref excluir);

            if (!editar) return RedirectToAction("AvisoIndex");

            Aviso aviso = oAviso.InfoAviso(iCodigo: codigo);
            if (aviso.codigo == 0) return HttpNotFound();

            ViewBag.empresas = oCombo.Empresa();

            return View("AvisoEdit", aviso);
        }

        // JSON: unidades da empresa escolhida (para o combo do cadastro)
        public JsonResult LoadUnidadeAviso(int empresa)
        {
            if (Session["empresa"] == null || empresa <= 0)
                return Json(new List<ListCombo>(), JsonRequestBehavior.AllowGet);

            return Json(oCombo.Unidade(iCodigoEmpresa: empresa,
                                       iCodigoUsuario: codigoUsuario,
                                       bCadastro: false), JsonRequestBehavior.AllowGet);
        }

        // POST: SAVE — cabeçalho e seções (JSON) de uma vez.
        // ValidateInput(false): a seção É HTML — sem isso a validação de
        // requisição do ASP.NET derruba o POST com erro 500 antes do controller.
        // A defesa fica no SanitizarHtml (e na CSP, na exibição).
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveAviso(int codigo, string titulo, string data_inicio, string data_termino,
                                    int empresa, int unidade, bool auditado, bool avaliado, bool ativo,
                                    string secoesJson)
        {
            try
            {
                if (Session["empresa"] == null)
                    return Json(new { success = false, message = "Sessão expirada. Entre novamente." });

                bool inserir = false, editar = false, excluir = false;
                LoadPerfilAviso(ref inserir, ref editar, ref excluir);

                if ((codigo == 0 && !inserir) || (codigo > 0 && !editar))
                    return Json(new { success = false, message = "Você não tem permissão para manter avisos." });

                var aviso = new Aviso
                {
                    codigo = codigo,
                    titulo = (titulo ?? "").Trim(),
                    data_inicio = data_inicio,
                    data_termino = data_termino,
                    codigo_empresa = empresa,
                    codigo_unidade = unidade,
                    auditado = auditado,
                    avaliado = avaliado,
                    ativo = ativo,
                    secoes = JsonConvert.DeserializeObject<List<AvisoSecao>>(secoesJson ?? "[]") ?? new List<AvisoSecao>()
                };

                if (string.IsNullOrWhiteSpace(aviso.titulo))
                    return Json(new { success = false, message = "Informe o título do aviso." });

                if (aviso.secoes.Count == 0)
                    return Json(new { success = false, message = "Inclua ao menos uma seção no aviso." });

                foreach (var secao in aviso.secoes)
                    secao.conteudo = SanitizarHtml(secao.conteudo);

                int novoCodigo = oAviso.SaveAviso(oAviso: aviso, sUsuario: Session["nome"] + " (" + codigoUsuario + ")");

                return Json(new { success = true, codigo = novoCodigo });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: DELETE
        [HttpPost]
        public JsonResult DeleteAviso(int codigo)
        {
            try
            {
                if (Session["empresa"] == null)
                    return Json(new { success = false, message = "Sessão expirada. Entre novamente." });

                bool inserir = false, editar = false, excluir = false;
                LoadPerfilAviso(ref inserir, ref editar, ref excluir);

                if (!excluir)
                    return Json(new { success = false, message = "Você não tem permissão para excluir avisos." });

                oAviso.DeleteAviso(iCodigo: codigo);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: AUDITORIA
        public ActionResult AvisoAuditoria(int codigo)
        {
            if (Session["empresa"] == null)
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });

            bool inserir = false, editar = false, excluir = false;
            LoadPerfilAviso(ref inserir, ref editar, ref excluir);

            if (!inserir && !editar && !excluir)
                return RedirectToAction(Session["dashboard"].ToString(), "Home");

            ViewBag.codigo = codigo;

            return View(oAviso.AuditoriaAviso(iCodigo: codigo));
        }

        #endregion

        #region ::: POPUP DO LOGIN :::

        // JSON: avisos pendentes do usuário logado. Devolvidos uma vez por sessão
        // ("no momento do login"): depois da primeira entrega, as demais páginas
        // não reabrem o popup até o próximo login.
        public JsonResult AvisosLogin()
        {
            if (Session["empresa"] == null)
                return Json(new List<Aviso>(), JsonRequestBehavior.AllowGet);

            if (Session["avisos_exibidos"] != null)
                return Json(new List<Aviso>(), JsonRequestBehavior.AllowGet);

            var avisos = oAviso.AvisosPendentes(iCodigoEmpresa: codigoEmpresa,
                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"]),
                                                iCodigoUsuario: codigoUsuario);

            Session["avisos_exibidos"] = true;

            var jsonResult = Json(avisos, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        // JSON: quantos avisos vigentes o usuário ainda não dispensou — acende a
        // lâmpada do header. Só a contagem: o conteúdo (que pode pesar) fica
        // para o clique.
        public JsonResult AvisosResumo()
        {
            if (Session["empresa"] == null)
                return Json(new { quantidade = 0 }, JsonRequestBehavior.AllowGet);

            var avisos = oAviso.AvisosPendentes(iCodigoEmpresa: codigoEmpresa,
                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"]),
                                                iCodigoUsuario: codigoUsuario);

            return Json(new { quantidade = avisos.Count }, JsonRequestBehavior.AllowGet);
        }

        // JSON: avisos vigentes para abrir sob demanda (clique na lâmpada) —
        // sem a trava de uma-vez-por-sessão do AvisosLogin.
        public JsonResult AvisosAbrir()
        {
            if (Session["empresa"] == null)
                return Json(new List<Aviso>(), JsonRequestBehavior.AllowGet);

            var avisos = oAviso.AvisosPendentes(iCodigoEmpresa: codigoEmpresa,
                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"]),
                                                iCodigoUsuario: codigoUsuario);

            var jsonResult = Json(avisos, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        // POST: registra que o aviso foi exibido (log de auditoria)
        [HttpPost]
        public JsonResult AvisoVisualizado(int codigo)
        {
            if (Session["empresa"] == null) return Json(false);

            oAviso.RegistrarVisualizacao(iCodigoAviso: codigo,
                                         iCodigoEmpresa: codigoEmpresa,
                                         iCodigoUsuario: codigoUsuario);
            return Json(true);
        }

        // POST: avaliação de 1 a 5 estrelas
        [HttpPost]
        public JsonResult AvisoAvaliar(int codigo, int avaliacao)
        {
            if (Session["empresa"] == null) return Json(false);

            oAviso.RegistrarAvaliacao(iCodigoAviso: codigo,
                                      iCodigoEmpresa: codigoEmpresa,
                                      iCodigoUsuario: codigoUsuario,
                                      iAvaliacao: avaliacao);
            return Json(true);
        }

        // POST: "não mostrar este aviso novamente"
        [HttpPost]
        public JsonResult AvisoDispensar(int codigo)
        {
            if (Session["empresa"] == null) return Json(false);

            oAviso.RegistrarDispensa(iCodigoAviso: codigo,
                                     iCodigoEmpresa: codigoEmpresa,
                                     iCodigoUsuario: codigoUsuario);
            return Json(true);
        }

        #endregion
    }
}
