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
    public class UploadController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Upload oUpload = new Upload(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: PLANILHA :::

        // GET: INSERT
        public ActionResult UploadExcel()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", null);
                ViewBag.cadastro_basico = new SelectList(oCombo.CadastroBasicoUpload(), "codigo", "descricao", null);

                return View();
            }
        }

        // GET: INSERT
        public ActionResult UploadPMOC()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", null);
                ViewBag.cadastro_basico = new SelectList(oCombo.CadastroBasicoPMOC(), "codigo", "descricao", null);

                return View();
            }
        }

        // POST: INSERT
        public JsonResult UploadExcelPlanilha(int unidade, string cadastro_basico, string planilha)
            {
                if (Session["empresa"] == null)
                {
                    return Json("ERRO: Usuário Desconectado");
                }
                else
                {

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase oHttpPostedFileBase = Request.Files[i];
                        if (oHttpPostedFileBase.FileName != "")
                        {

                            string filename = DateTime.Now.ToString("yyMMdd_HHmmssfff") + Path.GetExtension(oHttpPostedFileBase.FileName);
                            string path = Path.Combine("C:\\SIM\\PCM\\SITE\\APP\\Content\\arq\\UPLOAD\\", Session["empresa"].ToString());
                            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                            if (System.IO.File.Exists(Path.Combine(path, filename))) System.IO.File.Delete(Path.Combine(path, filename));
                            oHttpPostedFileBase.SaveAs(Path.Combine(path, filename));

                            try
                            {
                                oUpload.UploadExcel(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: unidade,
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    sFile: Path.Combine(path, filename),
                                                    sPlanilha: planilha,
                                                    sQuery: cadastro_basico);
                            }
                            catch (Exception ex)
                            {
                                return Json(ex.Message.Trim());
                            }
                            finally
                            {
                                System.IO.File.Delete(Path.Combine(path, filename));
                            }
                        }
                    }
                    return Json("Arquivo importado com Sucesso!");
                }
            }

        #endregion

    }
}