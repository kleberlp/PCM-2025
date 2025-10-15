using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.IO;
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
    public class TreinamentoController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private CadastroBasico oCadastroBasico = new CadastroBasico(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: TREINAMENTO :::

            // GET: INDEX
            public ActionResult TreinamentoIndex()
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {

                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                    ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                  iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Session["codigo_modulo"].ToString());

                    return View(oCadastroBasico.IndexTreinamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                 iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                 iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"]),
                                                                 iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])));
                }
            }

            // POST: INDEX
            [HttpPost]
            public ActionResult TreinamentoIndex(int unidade = -1, int modulo = -1)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {

                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: false), "codigo", "descricao", unidade);
                    ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                  iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", modulo);

                    return View(oCadastroBasico.IndexTreinamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                 iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                 iCodigoUnidade: Convert.ToInt32(unidade),
                                                                 iCodigoModulo: Convert.ToInt32(modulo)));
                }
            }

        #endregion

    }
}