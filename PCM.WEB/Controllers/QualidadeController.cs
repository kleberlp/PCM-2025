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
using System.Drawing;
using System.Drawing.Drawing2D;
using NPOI.SS.Formula.Functions;
using static NPOI.HSSF.Util.HSSFColor;

namespace PCM.WEB.Controllers
{
    public class QualidadeController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Qualidade oQualidade = new Qualidade(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Picture oPicture = new Picture(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.PCM oPCM = new DAL.PCM(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.OrdemServico oOrdemServico = new DAL.OrdemServico(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: JSON :::

        //JSON: /AUDITORIA QUALIDADE/
        public JsonResult LoadAuditoriaQualidade(int unidade)
        {
            return Json(oCombo.AuditoriaQualidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                    iCodigoUnidade: unidade));
        }

        //JSON: /DEPARTAMENTO/
        public JsonResult LoadDepartamento()
            {
                return Json(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())));
        }
        
        //JSON: /PRIORIDADE/
        public JsonResult LoadPrioridade(int unidade)
        {
            return Json(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: unidade));
        }

        //JSON: /SOLICITANTE/
        public JsonResult LoadSolicitante(int unidade)
        {
            return Json(oCombo.Solicitante(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                            iCodigoUnidade: unidade));
        }

        //JSON: /JUSTIFICATIVA DE APONTAMENTO/
        public JsonResult LoadJustificativaApontamento(int unidade)
        {
            return Json(oCombo.JustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: unidade));
        }

        //JSON: /CHECKLIST/
        public JsonResult LoadChecklist(int unidade, int codigo_tipo_checklist = -1)
                {
                    return Json(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: unidade,
                                                    iCodigoTipoChecklist: codigo_tipo_checklist));
                }

        //JSON: /ATUALIZA DATA DA ORDEM DE SERVIÇO/
        public bool UpdateDataOrdemServico(long codigo, int unidade, string data = "")
        {
            return oOrdemServico.UpdateOrdemServicoDataNecessidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    lCodigo: codigo,
                                                                    iCodigoUnidade: unidade,
                                                                    sDataNecessidade: data);
        }

        //JSON: /PRIORIDADE/
        public JsonResult LoadComboTarefa(int unidade)
        {
            return Json(oCombo.Tarefa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                      iCodigoUnidade: unidade));
        }

        #endregion

        #region ::: CADASTRO AUDITORIA :::

        // GET: INDEX
        public ActionResult AuditoriaIndex()
            {
                if (Session["empresa"] == null)
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

                    oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        sFormulario: "qa_cad_auditoria",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;


                    return View(oQualidade.IndexAuditoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"]),
                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])));
                }
            }

        // GET: INSERT
        public ActionResult AuditoriaInsert()
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
                ViewBag.checklist = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                    iCodigoTipoChecklist: 6), "codigo", "descricao", null);
                ViewBag.periodicidade = new SelectList(oCombo.Periodicidade(bChecklist: false), "codigo", "descricao", null);

                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult AuditoriaInsert(int unidade, string descricao, long checklist, int periodicidade, int intervalo, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oQualidade.InsertAuditoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: unidade,
                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"]),
                                            sDescricao: descricao,
                                            lCodigoChecklist: checklist,
                                            iCodigoPeriodicidade: periodicidade,
                                            iIntervalo: intervalo,
                                            bAtivo: ativo,
                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()));


                return RedirectToAction("AuditoriaInsert");
            }
        }

        // GET: EDIT
        public ActionResult AuditoriaEdit(int codigo, int unidade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                AuditoriaQualidade auditoria = null;

                oQualidade.InfoAuditoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: unidade,
                                            iCodigo: codigo,
                                            oAuditoriaQualidade: ref auditoria);

                if (auditoria == null)
                {
                    return HttpNotFound();
                }

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: true), "codigo", "descricao", auditoria.codigo_unidade);
                ViewBag.checklist = new SelectList(oCombo.Checklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                    iCodigoTipoChecklist: 6), "codigo", "descricao", auditoria.codigo_checklist);
                ViewBag.periodicidade = new SelectList(oCombo.Periodicidade(bChecklist: false), "codigo", "descricao", auditoria.codigo_periodicidade);
                ViewBag.ativo = (auditoria.ativo) ? "checked" : "";

                return View(auditoria);
            }
        }

        // POST: EDIT
        [HttpPost]
        public ActionResult AuditoriaEdit(int unidade, string descricao, long checklist, int periodicidade, int intervalo, int codigo, bool ativo = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
            //Insere Registro no Banco de Dados
            oQualidade.UpdateAuditoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUnidade: unidade,
                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"]),
                                        sDescricao: descricao,
                                        lCodigoChecklist: checklist,
                                        iCodigoPeriodicidade: periodicidade,
                                        iIntervalo: intervalo,
                                        bAtivo: ativo,
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        iCodigo: codigo);


                return RedirectToAction("AuditoriaIndex");
            }
        }

        // GET: /DELETE
        public ActionResult AuditoriaDelete(int unidade, int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                AuditoriaQualidade auditoria = null;

                oQualidade.InfoAuditoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: unidade,
                                            iCodigo: codigo,
                                            oAuditoriaQualidade: ref auditoria);

                if (auditoria == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(auditoria);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuditoriaDelete(int unidade, int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oQualidade.DeleteAuditoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigoUnidade: unidade,
                                                iCodigo: codigo);

                    //Redireciona para Index
                    return RedirectToAction("AuditoriaIndex");
                }
                catch
                {
                    return AuditoriaDelete(unidade: unidade,
                                                    codigo: codigo,
                                                    erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA
        public JsonResult ValidaAuditoria(int unidade, int codigo, string descricao)
        {

            return Json(oQualidade.ValidaAuditoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: unidade,
                                                    sDescricao: descricao,
                                                    iCodigo: codigo));

        }

        #endregion

        #region ::: AUDITORIA :::

        // POST: /DELETE
        public JsonResult AuditoriaQualidadeReabrir(int codigo_empresa, int codigo_unidade, long codigo_auditoria)
        {
            try
            {
                //Insere Registro no Banco de Dados
                oQualidade.ReabrirAuditoriaQualidade(iCodigoEmpresa: codigo_empresa,
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    lCodigo: codigo_auditoria);

                //Redireciona para Index
                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }

        // POST: /DELETE
        public JsonResult AuditoriaQualidadeExcluir(int codigo_empresa, int codigo_unidade, long codigo_auditoria)
        {
            try
            {
                //Insere Registro no Banco de Dados
                oQualidade.DeleteAuditoriaQualidade(iCodigoEmpresa: codigo_empresa,
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    lCodigo: codigo_auditoria);

                //Redireciona para Index
                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }

        // GET: INDEX
        public ActionResult Auditoria(int status = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool imprimir = false;
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "audit_corporativo",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;

                ViewBag.status = oQualidade.LoadAuditoriaStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()));

                return View(oQualidade.LoadAuditoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                        iStatus: status));
            }
        }

        // GET: /APONTAMENTO
        public ActionResult Apontamento(int codigo_auditoria_interna, int codigo_unidade, long codigo_auditoria, string action_name)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                if(codigo_auditoria == -1)
                {
                    oQualidade.InsertApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoAuditoriaInterna: codigo_auditoria_interna,
                                                    lCodigoAuditoria: ref codigo_auditoria);
                }

                QAApontamento apontamento = null;

                oQualidade.LoadApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: codigo_unidade,
                                            iCodigoAuditoriaInterna: codigo_auditoria_interna,
                                            lCodigoAuditoria: codigo_auditoria,
                                            oQAApontamento: ref apontamento);

                if (apontamento == null)
                {
                    return HttpNotFound();
                }

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;
                bool imprimir = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "qa_auditoria",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);


                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "qa_apontamento",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.administrador = administrador;
                ViewBag.inserir = inserir;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;
                ViewBag.action_name = action_name;

                return View(apontamento);
            }
        }

        // POST: /APONTAMENTO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Apontamento(int codigo_empresa, int codigo_unidade, int codigo_auditoria_interna, long codigo_auditoria, List<QAApontamentoChecklist> checklist, string action_name, bool concluido = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Insere Ordem de Serviço
                oQualidade.UpdateApontamento(iCodigoEmpresa: codigo_empresa,
                                                iCodigoUnidade: codigo_unidade,
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                bConcluido: concluido,
                                                iCodigoAuditoriaQualidade: codigo_auditoria_interna,
                                                lCodigoAuditoria: codigo_auditoria);

                //Insere Checklist
                if (checklist != null)
                {

                    foreach (QAApontamentoChecklist item in checklist)
                    {

                        //Insere Registro no Banco de Dados
                        oQualidade.InsertApontamentoChecklist(iCodigoEmpresa: codigo_empresa,
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                iCodigoUnidade: codigo_unidade,
                                                                lCodigoAuditoria: codigo_auditoria,
                                                                lCodigoChecklist: item.codigo_checklist,
                                                                iCodigoItemChecklist: item.codigo_item_checklist,
                                                                sResultado: item.resultado,
                                                                sPrazo: item.prazo,
                                                                sObservacao: item.observacao);

                    }

                }

                if (concluido)
                {
                    oQualidade.SendEmailPlanoAcao(iCodigoEmpresa: codigo_empresa,
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    lCodigoAuditoria: codigo_auditoria,
                                                    sTipo: "QUALIDADE");                        
                }

                return RedirectToAction(action_name, "Qualidade");
            }
        }

        // POST: /UPLOAD FOTO
        public JsonResult UploadFoto(long codigo_auditoria, int codigo_unidade, int codigo_item_checklist)
        {

            try
            {

                string filename = "";
                string path = Path.Combine("C:\\", "SIM", "PCM", "SITE", "IMAGE", "QUALIDADE", Session["empresa"].ToString(), codigo_unidade.ToString(), codigo_auditoria.ToString());

                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase oHttpPostedFileBase = Request.Files[i];
                    filename = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(oHttpPostedFileBase.FileName);
                    if (System.IO.File.Exists(Path.Combine(path, filename))) { System.IO.File.Delete(Path.Combine(path, filename)); }
                    ResizeAndSaveImage(oHttpPostedFileBase.InputStream, Path.Combine(path, filename));

                    //oHttpPostedFileBase.SaveAs(Path.Combine(sPath, sFileName));

                    oPicture.InsertPicture(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: codigo_unidade,
                                            sTipo: "QUALIDADE",
                                            lCodigo: codigo_auditoria,
                                            iCodigoItemChecklist: codigo_item_checklist,
                                            sImagePath: Path.Combine(path, filename));

                }

                return Json("true");
            }
            catch (Exception ex)
            {
                return Json("false");
            }

        }

        public JsonResult LoadFoto(long codigo_auditoria, int codigo_unidade, int codigo_item_checklist)
        {
            return Json(oPicture.PictureList(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: codigo_unidade,
                                                lCodigo: codigo_auditoria,
                                                sTipo: "QUALIDADE",
                                                iCodigoItemChecklist: codigo_item_checklist));
        }

        // POST: /PRINT
        public ActionResult AuditoriaPrintPDF(int codigo_empresa, int codigo_unidade, int codigo_auditoria)
        {

            ReportDocument oReportDocument = new ReportDocument();
            TableLogOnInfo oTableLogOnInfo = new TableLogOnInfo();
            ConnectionInfo oConnectionInfo = new ConnectionInfo();
            Database oCrDatabase;
            Tables oCrTables;

            oConnectionInfo.ServerName = ConfigurationManager.AppSettings["data_source"].ToString();
            oConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["initial_catalog"].ToString();
            oConnectionInfo.UserID = ConfigurationManager.AppSettings["user_id"].ToString();
            oConnectionInfo.Password = ConfigurationManager.AppSettings["password"].ToString();
            oConnectionInfo.Type = ConnectionInfoType.SQL;
            oConnectionInfo.IntegratedSecurity = false;

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000032.rpt"));

            oCrDatabase = oReportDocument.Database;
            oCrTables = oCrDatabase.Tables;

            foreach (Table crTable in oCrTables)
            {
                oTableLogOnInfo = crTable.LogOnInfo;
                oTableLogOnInfo.ConnectionInfo = oConnectionInfo;
                crTable.ApplyLogOnInfo(oTableLogOnInfo);
            }

            oReportDocument.SetParameterValue("@codigo_unidade", codigo_unidade);
            oReportDocument.SetParameterValue("@codigo_empresa", codigo_empresa);
            oReportDocument.SetParameterValue("@codigo_auditoria", codigo_auditoria);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000032.pdf");
            return File(stream, "application/pdf;");
        }

        public void ResizeAndSaveImage(Stream imageStream, string outputFilePath)
        {

            // Carrega a imagem a partir do Stream
            using (Bitmap originalImage = new Bitmap(imageStream))
            {

                double scaleFactor = (originalImage.Width > originalImage.Height) ? 400.0 / originalImage.Width : 400.0 / originalImage.Height;

                // Calcula a nova largura e altura com base no fator de escala
                int newWidth = (int)(originalImage.Width * scaleFactor);
                int newHeight = (int)(originalImage.Height * scaleFactor);

                // Cria uma nova imagem redimensionada
                using (Bitmap resizedImage = new Bitmap(newWidth, newHeight))
                {
                    // Configura o objeto Graphics para redimensionar a imagem com alta qualidade
                    using (Graphics graphics = Graphics.FromImage(resizedImage))
                    {
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                    }

                    // Salva a imagem redimensionada no caminho de destino
                    resizedImage.Save(outputFilePath, System.Drawing.Imaging.ImageFormat.Png); // ou outro formato conforme necessário
                }
            }
        }

        #endregion

        #region ::: AUDITORIA CRONOGRAMA :::

        // GET: INDEX
        public ActionResult AuditoriaCronograma()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool imprimir = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "qa_auditoria_cronograma",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.data_filtro = String.Concat(System.DateTime.Now.Date.Month.ToString("00"), '/', System.DateTime.Now.Date.Year);
                ViewBag.dia_atual = DateTime.Now.Day;
                ViewBag.maior_dia = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

                return View(oQualidade.AuditoriaCronograma(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                            iMes: DateTime.Now.Month,
                                                            iAno: DateTime.Now.Year));
            }
        }

        // POST: /INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuditoriaCronograma(string data, int unidade = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool imprimir = false;

                int month = Convert.ToInt32(data.Split('/')[0]);
                int year = Convert.ToInt32(data.Split('/')[1]);

                ViewBag.data = data;
                ViewBag.dia_atual = (month == DateTime.Now.Month && year == DateTime.Now.Year) ? DateTime.Now.Day : -1;
                ViewBag.maior_dia = DateTime.DaysInMonth(year, month);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "qa_auditoria_cronograma",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.data_filtro = data;

                return View(oQualidade.AuditoriaCronograma(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            iMes: month,
                                                            iAno: year));
            }
        }

        #endregion

        #region ::: AUDITORIA HISTÓRICO :::

        // GET: INDEX
        public ActionResult AuditoriaHistorico(string data_inicio = "", string data_termino = "", int status = -1, int auditoria_qualidade = -1, int unidade = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool imprimir = false;
                bool inserir = false;
                bool editar = false;
                bool excluir = false;
                bool administrador = false;

                if (data_inicio == "SEM")
                    data_inicio = "";
                else if (data_inicio == "")
                        data_inicio = DateTime.Now.AddDays(-7).ToShortDateString();
                else
                    data_inicio = Convert.ToDateTime(data_inicio).ToShortDateString();

                if (data_termino == "SEM")
                    data_termino = "";
                else if (data_termino == "")
                    data_termino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                else
                    data_termino = Convert.ToDateTime(data_termino).ToShortDateString();

                if (unidade == -1)
                {
                    unidade = Convert.ToInt32(Session["codigo_unidade"].ToString());
                }

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "qa_auditoria",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;
                ViewBag.administrador = administrador;
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.auditoria_qualidade = new SelectList(oCombo.AuditoriaQualidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                        iCodigoModulo: 2,
                                                                                        iCodigoUnidade: unidade), "codigo", "descricao", auditoria_qualidade);
                ViewBag.status = new SelectList(oCombo.StatusAuditoriaQualidade(bHistorico: true), "codigo", "descricao", status);


                return View(oQualidade.AuditoriaHistorico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            sDataInicio: data_inicio,
                                                            sDataTermino: data_termino,
                                                            iCodigoAuditoriaInterna: auditoria_qualidade,
                                                            iStatus: status));
            }
        }

        // POST: /INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuditoriaHistorico(string data, int unidade = -1, string data_inicio = "", string data_termino = "", int auditoria_qualidade = -1, int status = -1)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    //Váriaveis
                    bool imprimir = false;
                    bool inserir = false;
                    bool editar = false;
                    bool excluir = false;
                    bool administrador = false;

                    oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        sFormulario: "qa_auditoria",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador,
                                        bImprimir: ref imprimir);

                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;
                    ViewBag.imprimir = imprimir;
                    ViewBag.administrador = administrador;
                    ViewBag.data_inicio = data_inicio;
                    ViewBag.data_termino = data_termino;
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: false), "codigo", "descricao", unidade);
                    ViewBag.auditoria_qualidade = new SelectList(oCombo.AuditoriaQualidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                 iCodigoModulo: 2,
                                                                 iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                    ViewBag.status = new SelectList(oCombo.StatusAuditoriaQualidade(bHistorico: true), "codigo", "descricao", status);

                    return View(oQualidade.AuditoriaHistorico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                              iCodigoUnidade: unidade,
                                                              sDataInicio: data_inicio,
                                                              sDataTermino: data_termino,
                                                              iCodigoAuditoriaInterna: auditoria_qualidade,
                                                              iStatus: status));
                }
            }

        #endregion

        #region ::: PLANO DE AÇÃO :::

        public ActionResult PlanoAcaoView(int codigo_empresa, int codigo_unidade, long codigo_auditoria, int codigo_departamento)
        {

            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }

            QAApontamento apontamento = null;

            oQualidade.LoadPlanoAcaoView(iCodigoEmpresa: codigo_empresa,
                                            iCodigoUnidade: codigo_unidade,
                                            lCodigoAuditoria: codigo_auditoria,
                                            iCodigoDepartamento: codigo_departamento,
                                            oQAApontamento: ref apontamento);

            if (apontamento == null)
            {
                return HttpNotFound();
            }

            return View(apontamento);
            
        }

        public ActionResult PlanoAcaoPrintPDF(int codigo_empresa, int codigo_unidade, int codigo_auditoria, int codigo_departamento)
        {

            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }

            ReportDocument oReportDocument = new ReportDocument();
            TableLogOnInfo oTableLogOnInfo = new TableLogOnInfo();
            ConnectionInfo oConnectionInfo = new ConnectionInfo();
            Database oCrDatabase;
            Tables oCrTables;

            oConnectionInfo.ServerName = ConfigurationManager.AppSettings["data_source"].ToString();
            oConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["initial_catalog"].ToString();
            oConnectionInfo.UserID = ConfigurationManager.AppSettings["user_id"].ToString();
            oConnectionInfo.Password = ConfigurationManager.AppSettings["password"].ToString();
            oConnectionInfo.Type = ConnectionInfoType.SQL;
            oConnectionInfo.IntegratedSecurity = false;

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000032.rpt"));

            oCrDatabase = oReportDocument.Database;
            oCrTables = oCrDatabase.Tables;

            foreach (Table crTable in oCrTables)
            {
                oTableLogOnInfo = crTable.LogOnInfo;
                oTableLogOnInfo.ConnectionInfo = oConnectionInfo;
                crTable.ApplyLogOnInfo(oTableLogOnInfo);
            }

            oReportDocument.SetParameterValue("@codigo_unidade", codigo_unidade);
            oReportDocument.SetParameterValue("@codigo_empresa", codigo_empresa);
            oReportDocument.SetParameterValue("@codigo_auditoria", codigo_auditoria);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000032.pdf");
            return File(stream, "application/pdf;");
        }

        // GET: INDEX
        public ActionResult OrdemServicoIndex(int status = 1, int auditoria_interna = -1, int unidade = -1, long codigo_auditoria = -1)
            {
                if (Session["empresa"] == null)
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
                    bool administrador_vincular = false;

                    oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        sFormulario: "qa_plano_acao",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        sFormulario: "qa_plano_acao_atribuir",
                                        sDireito: "administrador",
                                        bReturn: ref administrador_vincular);

                    string data_inicio = (status == -1) ? TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString() : "";
                    string data_termino = (status == -1) ? TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString() : "";

                    ViewBag.administrador_vincular = administrador_vincular;
                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;
                    ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                    ViewBag.data_inicio = "";
                    ViewBag.data_termino = "";
                    ViewBag.ordem_servico = "";
                    ViewBag.codigo_unidade = Convert.ToInt32(Session["codigo_unidade"]);
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: false), "codigo", "descricao", unidade);
                    ViewBag.auditoria = new SelectList(oCombo.AuditoriaQualidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                    iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()), 
                                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"])), "codigo", "descricao", auditoria_interna);
                    ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                    ViewBag.prioridade = new SelectList(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"])), "codigo", "descricao", null);
                    ViewBag.solicitante = new SelectList(oCombo.Solicitante(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"])), "codigo", "descricao", null);
                    ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"]),
                                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", null);
                    ViewBag.status = new SelectList(oCombo.StatusManutencao(), "codigo", "descricao", status);
                    ViewBag.justificativa_apontamento = new SelectList(oCombo.JustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                       iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"])), "codigo", "descricao", null);

                    List<MODELS.OrdemServico> ordemServico = new List<MODELS.OrdemServico>();

                    ViewBag.plano_acao_status = oQualidade.LoadPlanoAcaoStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()));
                
                    ordemServico = oQualidade.IndexOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                iCodigoUnidade: (unidade == -1)? Convert.ToInt32(Session["codigo_unidade"].ToString()) : unidade,
                                                                sOrdemServico: "",
                                                                sDataInicio: data_inicio,
                                                                sDataTermino: data_termino,
                                                                iCodigoAuditoria: auditoria_interna,
                                                                iCodigoPrioridade: -1,
                                                                iCodigoDepartamento: -1,
                                                                iCodigoSolicitante: -1,
                                                                sExecutor: "",
                                                                iStatus: status,
                                                                lCodigoAuditoria: codigo_auditoria);

                    return View(ordemServico);
                }
            }

        // POST: INDEX
        [HttpPost]
        public ActionResult OrdemServicoIndex(int unidade = -1, int codigo_unidade = -1, string data_inicio = "", string data_termino = "", string ordem_servico = "", int auditoria_interna = -1, int prioridade = -1, int departamento = -1, int status = -1, string executor = "", int solicitante = -1, int justificativa_apontamento = -1)
        {

            if (Session["empresa"] == null)
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
                bool administrador_vincular = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "qa_plano_acao",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "qa_plano_acao_atribuir",
                                    sDireito: "administrador",
                                    bReturn: ref administrador_vincular);

                ViewBag.administrador_vincular = administrador_vincular;
                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.ordem_servico = ordem_servico;
                ViewBag.codigo_unidade = unidade;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.auditoria = new SelectList(oCombo.AuditoriaQualidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()), 
                                                                                iCodigoUnidade: unidade), "codigo", "descricao", auditoria_interna);
                ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", departamento);
                ViewBag.prioridade = new SelectList(oCombo.Prioridade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"])), "codigo", "descricao", prioridade);
                ViewBag.solicitante = new SelectList(oCombo.Solicitante(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"])), "codigo", "descricao", solicitante);
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"]),
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", null);
                ViewBag.justificativa_apontamento = new SelectList(oCombo.JustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                   iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"])), "codigo", "descricao", justificativa_apontamento);
                ViewBag.status = new SelectList(oCombo.StatusManutencao(), "codigo", "descricao", status);

                List<MODELS.OrdemServico> ordemServico = new List<MODELS.OrdemServico>();

                ViewBag.plano_acao_status = oQualidade.LoadPlanoAcaoStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: (unidade == -1) ? codigo_unidade : unidade);

                ordemServico = oQualidade.IndexOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            iCodigoUnidade: (unidade == -1)? codigo_unidade: unidade,
                                                            sOrdemServico: ordem_servico,
                                                            sDataInicio: data_inicio,
                                                            sDataTermino: data_termino,
                                                            iCodigoAuditoria: auditoria_interna,
                                                            iCodigoPrioridade: prioridade,
                                                            iCodigoDepartamento: departamento,
                                                            iCodigoSolicitante: solicitante,
                                                            iCodigoJustificativaApontamento: justificativa_apontamento,
                                                            sExecutor: executor,
                                                            iStatus: status);

                return View(ordemServico);
            }
        }

        // GET: /VIEW
        public ActionResult OrdemServicoView(long codigo, int unidade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                MODELS.OrdemServico ordem_servico = null;

                oOrdemServico.InfoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                lCodigo: codigo,
                                                iCodigoUnidade: unidade,
                                                sTipo: "OS",
                                                oOrdemServico: ref ordem_servico);

                ViewBag.ordem_servico = ordem_servico;

                return View(oOrdemServico.OrdemServicoApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: unidade,
                                                                    lCodigoOrdemServico: codigo));
            }
        }


        // GET: /APONTAMENTO
        public ActionResult ApontamentoOS(long codigo_pcm_ordem_servico, int codigo_unidade, string page = "AgendaOS", string model = "PCM")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Apontamento apontamento = null;

                oPCM.LoadApontamentoOS(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        iCodigoUnidade: codigo_unidade,
                                        lCodigoPCMOrdemServico: codigo_pcm_ordem_servico,
                                        oApontamento: ref apontamento);

                if (apontamento == null)
                {
                    return HttpNotFound();
                }

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "qa_plano_acao",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.funcionario_vinculado = apontamento.codigo_funcionario;
                ViewBag.page = page;
                ViewBag.model = model;
                ViewBag.inserir = inserir;
                ViewBag.administrador = administrador;
                ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.apontamento = apontamento;
                ViewBag.usuario = new SelectList(oCombo.Usuario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: codigo_unidade), "codigo", "descricao", Convert.ToInt32(User.Identity.GetUserName()));
                ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", apontamento.codigo_departamento);
                ViewBag.tipo_ordem_servico = new SelectList(oCombo.TipoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                    iOrdemServico: 1), "codigo", "descricao", apontamento.codigo_tipo_ordem_servico);
                ViewBag.justificativa_apontamento = new SelectList(oCombo.JustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                    iCodigoUnidade: codigo_unidade), "codigo", "descricao", null);
                ViewBag.produto_combo = new SelectList(oCombo.Produto(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade), "codigo", "descricao", null);
                ViewBag.lote = new SelectList(oCombo.ProdutoLote(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: codigo_unidade,
                                                                    lCodigoProduto: -1), "codigo", "descricao", null);

                List<SaidaEstoque> saidas = new List<SaidaEstoque>();
                SaidaEstoque saida = new SaidaEstoque();
                saida.excluido = 1;
                saidas.Add(saida);

                return View(saidas);

            }

        }

        // POST: /APONTAMENTO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApontamentoOS(string descricao_solucao, string data_inicio, string data_termino, int codigo_unidade, long codigo_pcm_ordem_servico, HttpPostedFileBase arquivo, int usuario, int categoria = -1, string hora_inicio = "00:00", string hora_termino = "00:00", int tipo_servico = -1, int tipo_ordem_servico = -1, bool concluido = false, bool desativar_equipamento = false, int justificativa_apontamento = -1, string observacao = "", string valor = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                long codigo = 0;

            if (usuario != null)
            {
                oPCM.InsertApontamentoOS(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUsuario: usuario,
                                            iCodigoUnidade: codigo_unidade,
                                            lCodigoPCMOrdemServico: codigo_pcm_ordem_servico,
                                            iCodigoTipoServico: tipo_servico,
                                            iCodigoTipoOrdemServico: tipo_ordem_servico,
                                            iCodigoFornecedor: -1,
                                            iCodigoFuncionario: -1,
                                            iCodigoCategoria: categoria,
                                            sDataInicio: data_inicio,
                                            sDataTermino: data_termino,
                                            sHoraInicio: hora_inicio,
                                            sHoraTermino: hora_termino,
                                            dValor: (valor == "") ? 0 : Convert.ToSingle(valor.Replace("R$ ", "").Replace(".", "").Replace(",", ".")),
                                            sDescricaoSolucao: descricao_solucao,
                                            sArquivo: "",
                                            bConcluido: concluido,
                                            iCodigoJustificativaApontamento: (concluido) ? -1 : justificativa_apontamento,
                                            sObservacaoApontamento: (concluido) ? "" : observacao,
                                            bDesativarApontamento: desativar_equipamento,
                                            lCodigo: ref codigo);

                if (arquivo != null)
                {
                    string filename = "";
                    string path = Path.Combine("C:\\", "SIM", "PCM", "SITE", "IMAGE", "QA_OS_APONTAMENTO", Session["empresa"].ToString(), codigo_unidade.ToString(), codigo.ToString());

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filename = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                    if (System.IO.File.Exists(Path.Combine(path, filename))) { System.IO.File.Delete(Path.Combine(path, filename)); }
                    ResizeAndSaveImage(arquivo.InputStream, Path.Combine(path, filename));

                    oPicture.InsertPicture(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: codigo_unidade,
                                            lCodigo: codigo,
                                            sTipo: "ORDEM OS_APONTAMENTO",
                                            iCodigoItemChecklist: -1,
                                            sImagePath: Path.Combine(path, filename));

                }
            }

                return RedirectToAction("OrdemServicoIndex", "Qualidade");
            }
        }

        #endregion

        #region ::: TAREFA :::

        // GET: INDEX
        public ActionResult Tarefa(int status = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool imprimir = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "qa_tarefa",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.status = oQualidade.LoadQualidadeTarefaStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                      iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                      iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()));

                return View(oQualidade.LoadQualidadeTarefa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                           iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                           iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                           iStatus: status));
            }
        }

        // GET: INDEX
        public ActionResult TarefaMes(string data, int unidade = -1, int modulo = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Váriaveis
                bool imprimir = false;

                int month = Convert.ToInt32(data.Split('/')[0]);
                int year = Convert.ToInt32(data.Split('/')[1]);

                ViewBag.data = data;
                ViewBag.dia_atual = (month == DateTime.Now.Month && year == DateTime.Now.Year) ? DateTime.Now.Day : -1;
                ViewBag.maior_dia = DateTime.DaysInMonth(year, month);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "qa_tarefa",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", modulo);

                ViewBag.data_filtro = data;

                return View(oQualidade.QualidadeTarefa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                       iCodigoUnidade: unidade,
                                                       iCodigoModulo: modulo,
                                                       iMes: month,
                                                       iAno: year));
            }
        }

        #endregion

        #region ::: APONTAMENTO - TAREFA :::

        // GET: /APONTAMENTO
        public ActionResult TarefaApontamento(long codigo_qa_tarefa, int codigo_unidade, string formulario, string data = "", long codigo_qa_tarefa_ordem_servico = -1, string actionTarefa = "Tarefa")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                QATarefaDados tarefaDados = null;

                oQualidade.LoadDadosTarefa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                           iCodigoUnidade: codigo_unidade,
                                           lCodigoQATarefa: codigo_qa_tarefa,
                                           lCodigoQATarefaOrdemServico: codigo_qa_tarefa_ordem_servico,
                                           oTarefaDados: ref tarefaDados) ;

                if (tarefaDados == null)
                {
                    return HttpNotFound();
                }

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "qa_tarefa_apontamento",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.administrador = administrador;
                ViewBag.inserir = inserir;
                ViewBag.data = (data == "") ? TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString() : data;
                ViewBag.dados = tarefaDados;
                ViewBag.codigo_qa_tarefa_ordem_servico = codigo_qa_tarefa_ordem_servico;
                ViewBag.action = actionTarefa;

                return View(oQualidade.LoadQualidadeTarefaApontamentoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                               lCodigoQATarefa: codigo_qa_tarefa,
                                                                               lCodigoQATarefaOrdemServico: codigo_qa_tarefa_ordem_servico, 
                                                                               iCodigoUnidade: codigo_unidade));
            }
        }

        // POST: /APONTAMENTO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TarefaApontamento(string descricao_solucao, string data_inicio, string data_termino, int codigo_unidade, long codigo_qa_tarefa, List<QATarefaApontamentoChecklist> checklist, string hora_inicio = "00:00", string hora_termino = "00:00", bool concluido = false, int justificativa_apontamento = -1, long codigo_qa_tarefa_ordem_servico = -1, string actionTarefa = "Tarefa")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                if (codigo_qa_tarefa_ordem_servico < 0)
                {

                    //Insere Ordem de Serviço
                    oQualidade.InsertOrdemServicoTarefa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigoUnidade: codigo_unidade,
                                                        lCodigoQATarefa: codigo_qa_tarefa,
                                                        sSolucao: descricao_solucao,
                                                        bConcluido: concluido,
                                                        sData: data_inicio,
                                                        sDataTermino: data_termino,
                                                        lCodigoQATarefaOrdemServico: ref codigo_qa_tarefa_ordem_servico);

                } else
                {

                    //Atualiza Ordem de Serviço
                    oQualidade.UpdateOrdemServicoTarefa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigoUnidade: codigo_unidade,
                                                        lCodigoQATarefa: codigo_qa_tarefa,
                                                        sDescricaoSolucao: descricao_solucao,
                                                        sData: data_inicio,
                                                        sDataTermino: data_termino,
                                                        bConcluido: concluido,
                                                        lCodigoQATarefaOrdemServico: codigo_qa_tarefa_ordem_servico);

                }

                oQualidade.InsertApontamentoTarefa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    lCodigoQATarefaOrdemServico: codigo_qa_tarefa_ordem_servico,
                                                    iCodigoFuncionario: -1,
                                                    sDescricaoSolucao: descricao_solucao,
                                                    iCodigoJustificativaApontamento: justificativa_apontamento,                                                       
                                                    sDataInicio: data_inicio,
                                                    sDataTermino: data_termino,
                                                    sHoraInicio: hora_inicio,
                                                    sHoraTermino: hora_termino);
                //Insere Checklist
                if (checklist != null)
                {

                    foreach (QATarefaApontamentoChecklist item in checklist)
                    {

                        //Insere Registro no Banco de Dados
                        oQualidade.InsertChecklistTarefa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                         iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                         iCodigoUnidade: codigo_unidade,
                                                         lCodigoQATarefaOrdemServico: codigo_qa_tarefa_ordem_servico,
                                                         iCodigoChecklistItem: item.codigo,
                                                         sResultado: item.resultado,
                                                         sObservacao: item.observacao);

                    }

                }

                return RedirectToAction(actionTarefa, "Qualidade");
            }
        }

        #endregion

        #region ::: TAREFA - ORDEM DE SERVIÇO :::

        // GET
        public ActionResult TarefaOrdemServico(int codigo_unidade, long codigo_qa_tarefa_ordem_servico)
        {
            if (Session["empresa"] == null)
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

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "qa_tarefa_ordem_servico",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.inserir = inserir;
                ViewBag.administrador = administrador;

                QATarefaOrdemServico ordemServico = oQualidade.TarefaOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                  iCodigoUnidade: codigo_unidade,
                                                                                  lCodigoQATarefaOrdemServico: codigo_qa_tarefa_ordem_servico);

                ViewBag.ordem_servico = ordemServico;

                return View(ordemServico.checklist);
            }
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TarefaOrdemServico(string descricao_solucao, string data_inicio, string data_termino, int codigo_unidade, long codigo_qa_tarefa, long codigo_qa_tarefa_ordem_servico, List<QATarefaApontamentoChecklist> checklist, string formulario, string hora_inicio = "00:00", string hora_termino = "00:00", bool concluido = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Insere Ordem de Serviço
                oQualidade.UpdateOrdemServicoTarefa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    lCodigoQATarefa: codigo_qa_tarefa,
                                                    sDescricaoSolucao: descricao_solucao,
                                                    sData: data_inicio,
                                                    sDataTermino: data_termino,
                                                    bConcluido: concluido,
                                                    lCodigoQATarefaOrdemServico: codigo_qa_tarefa_ordem_servico);

                //Insere Checklist
                if (checklist != null)
                {

                    foreach (QATarefaApontamentoChecklist item in checklist)
                    {

                        //Insere Registro no Banco de Dados
                        oQualidade.InsertChecklistTarefa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                         iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                         iCodigoUnidade: codigo_unidade,
                                                         lCodigoQATarefaOrdemServico: codigo_qa_tarefa_ordem_servico,
                                                         iCodigoChecklistItem: item.codigo,
                                                         sResultado: item.resultado,
                                                         sObservacao: item.observacao);

                    }

                }

                return RedirectToAction("TarefaOrdemServico", "Qualidade", new { codigo_unidade = codigo_unidade, codigo_qa_tarefa_ordem_servico = codigo_qa_tarefa_ordem_servico });
            }
        }

        // GET: /PRINT
        public ActionResult TarefaOrdemServicoPrint(long codigo_qa_tarefa_ordem_servico, int unidade)
        {

            ReportDocument oReportDocument = new ReportDocument();
            TableLogOnInfo oTableLogOnInfo = new TableLogOnInfo();
            ConnectionInfo oConnectionInfo = new ConnectionInfo();
            Database oCrDatabase;
            Tables oCrTables;

            oConnectionInfo.ServerName = ConfigurationManager.AppSettings["data_source"].ToString();
            oConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["initial_catalog"].ToString();
            oConnectionInfo.UserID = ConfigurationManager.AppSettings["user_id"].ToString();
            oConnectionInfo.Password = ConfigurationManager.AppSettings["password"].ToString();
            oConnectionInfo.Type = ConnectionInfoType.SQL;
            oConnectionInfo.IntegratedSecurity = false;

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000038.rpt"));

            oCrDatabase = oReportDocument.Database;
            oCrTables = oCrDatabase.Tables;

            foreach (Table crTable in oCrTables)
            {
                oTableLogOnInfo = crTable.LogOnInfo;
                oTableLogOnInfo.ConnectionInfo = oConnectionInfo;
                crTable.ApplyLogOnInfo(oTableLogOnInfo);
            }

            oReportDocument.SetParameterValue("@codigo_unidade", unidade);
            oReportDocument.SetParameterValue("@codigo_empresa", Convert.ToInt32(Session["empresa"].ToString()));
            oReportDocument.SetParameterValue("@codigo_qa_tarefa_ordem_servico", codigo_qa_tarefa_ordem_servico);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000038.pdf");
            return File(stream, "application/pdf;");
        }

        //JSON: /TAREFA/
        public JsonResult DeleteTarefaOrdemServico(int empresa, int usuario, int unidade, long codigo_qa_tarefa_ordem_servico)
        {
            try
            {
                oQualidade.TarefaOrdemServicoDelete(iCodigoEmpresa: empresa,
                                                    iCodigoUsuario: usuario,
                                                    iCodigoUnidade: unidade,
                                                    lCodigoQATarefaOrdemServico: codigo_qa_tarefa_ordem_servico);

                return Json("Tarefa excluida com sucesso!");
            }
            catch(Exception ex)
            {
                return Json(ex.Message.ToString());
            }

        }

        #endregion

        #region ::: TAREFA - HISTÓRICO :::

        // GET: INDEX
        public ActionResult TarefaHistorico()
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
                bool imprimir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "qa_tarefa_historico",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);


                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;

                ViewBag.data_inicio = DateTime.Now.AddMonths(-1).ToShortDateString();
                ViewBag.data_termino = DateTime.Now.ToShortDateString();
                ViewBag.empresa = Session["empresa"].ToString();
                ViewBag.usuario = User.Identity.GetUserName();

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()), bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.tarefa = new SelectList(oCombo.Tarefa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                return View();
            }
        }

        // POST: INDEX
        [HttpPost]
        public ActionResult TarefaHistorico(int empresa, int usuario, int unidade = -1, string data_inicio = "", string data_termino = "", int tarefa = -1)
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
                bool imprimir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "qa_tarefa_historico",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;

                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.empresa = empresa;
                ViewBag.usuario = usuario;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: empresa, iCodigoUsuario: usuario, bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.tarefa = new SelectList(oCombo.Tarefa(iCodigoEmpresa: empresa, iCodigoUnidade:unidade), "codigo", "descricao", tarefa);

                return View();
            }
        }

        //JSON: /TAREFA/
        public JsonResult LoadTarefa(int unidade, string data_inicio, string data_termino, long tarefa)
        {

            return Json(oQualidade.TarefaHistorico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                   iCodigoUnidade: unidade,
                                                   sDataInicio: data_inicio,
                                                   sDataTermino: data_termino,
                                                   lCodigoQATarefa: tarefa));

        }

        #endregion

    }
}