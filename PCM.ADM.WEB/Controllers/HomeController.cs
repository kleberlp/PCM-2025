using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using PCM.WEB.DAL;
using PCM.WEB.MODELS;
using Microsoft.AspNet.Identity;
using System.Globalization;
using System;
using System.Web;
using System.Net;
using System.Collections.Generic;

namespace PCM.ADM.WEB.Controllers
{
    
    public class HomeController : Controller
    {
        private Home oHome = new Home(ConfigurationManager.ConnectionStrings["DefaultConnectionAdm"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnectionAdm"].ConnectionString);
        private SIM oSIM = new SIM(ConfigurationManager.ConnectionStrings["DefaultConnectionAdm"].ConnectionString);
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnectionAdm"].ConnectionString);

        #region ::: INDEX :::

            public ActionResult Index()
            {
                if (@Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {                 
                    FormularioVisualizar formulario_visualizar = null;

                    oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        oFormularioVisualizar: ref formulario_visualizar);

                    ViewBag.data_inicio = System.DateTime.Now.Date.AddYears(-1).ToShortDateString();
                    ViewBag.formulario_visualizar = formulario_visualizar;
                    ViewBag.resumo_empresa = oSIM.ResumoEmpresa(iPeriodo: 3);
                
                    return View();
                }
            }

            public JsonResult ResumoEmpresa(int periodo)
            {
                return Json(oSIM.ResumoEmpresa(iPeriodo: periodo));
            }
            
            public JsonResult chartFaturamentoSeguimento()
            {
                return Json(oSIM.ChartFaturamentoSegmento());
            }

            public JsonResult chartEvolucaoFaturamento()
            {
                return Json(oSIM.ChartEvolucaoFaturamento());
            }

        #endregion

        #region ::: LANGUAGE :::

        public ActionResult ChangeCulture(string lang, string returnUrl)
        {
            //Atualiza Banco de Dados
            //oUser.Update(User.Identity.GetUserName(), lang);
            Session["language"] = new CultureInfo(lang);
            return Redirect(returnUrl);
        }

        #endregion

    }
}