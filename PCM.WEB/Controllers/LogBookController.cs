using System;
using System.Web;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PCM.WEB.MODELS;
using PCM.WEB.DAL;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace PCM.WEB.Controllers
{
    public class LogBookController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.LogBook oLogBook = new DAL.LogBook(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: JSON :::

            //JSON: /CATEGORIA/
            public JsonResult LoadLogBook(string data_inicio, string data_termino, int unidade)
            {
                return Json(oLogBook.LoadLogBook(iCodigoEmpresa: Convert.ToInt16(Session["empresa"].ToString()),
                                                 iCodigoUnidade: unidade,
                                                 sDataInicio: data_inicio,
                                                 sDataTermino: data_termino));
            }

            //JSON: /UNIDADE/
            public JsonResult LoadUnidade()
            {
                return Json(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                           iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                           bCadastro: false));
            }

        #endregion

        #region ::: LOGBOOK :::

            // GET: LOGBOOK
            public ActionResult LogBook()
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {

                    ViewBag.data_inicio = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString();
                    ViewBag.data_termino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());

                    return View();
                }
            }

            // GET: INSERT
            public ActionResult LogBookInsert()
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                    return View();
                }
            }

            // POST: INSERT
            [HttpPost]
            public ActionResult LogBookInsert(int unidade, string informacao)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    //Insere Registro no Banco de Dados
                    oLogBook.InsertLogBook(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                           iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                           iCodigoUnidade: unidade,
                                           sInformacao: informacao);

                    return RedirectToAction("Index", "Home");
                }
            }

        #endregion

    }
}