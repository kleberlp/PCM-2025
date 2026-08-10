using Microsoft.AspNet.Identity;
using PCM.WEB.DAL;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace PCM.WEB.Controllers
{
    public class ConfiguracaoController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Configuracao oConfig = new Configuracao(sCon: ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(sCon: ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: INTERFACE OPERA :::

        // JSON
        public JsonResult LoadInterfaceOpera(int codigoEmpresa, int codigoUnidade)
        {            
            return Json(oConfig.LoadConfiguracaoOpera(codigoEmpresa: codigoEmpresa,
                                                      codigoUnidade: codigoUnidade));
        }

        // GET
        public ActionResult InterfaceOpera()
        {
            if (Session["language"] == null || User.Identity.GetUserName() == "")
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;
                bool imprimir = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "cfg_opera",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);
                //Seta o Perfil
                ViewBag.edutar = editar;
                ViewBag.codigoEmpresa = Session["empresa"].ToString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());

                return View(oConfig.LoadConfiguracaoOpera(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                          codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
            }
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InterfaceOpera(string hostname, string username, string password, string appKey, string clientId, string clientSecret, int intervalo = 0, int unidade = -1)
        {
            if (Session["language"] == null || User.Identity.GetUserName() == "")
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Atualiza Registro no Banco de Dados
                oConfig.UpdateConfiguracaoOpera(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                codigoUnidade: unidade,
                                                codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                hostname: hostname,
                                                username: username,
                                                password: password,
                                                appKey: appKey,
                                                clientId: clientId,
                                                clientSecret: clientSecret,
                                                intervalo: intervalo);

                //Redireciona para Index
                return RedirectToAction("InterfaceOpera");
            }
        }

        #endregion

    }
}