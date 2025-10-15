using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.AspNet.Identity;
using PCM.WEB.DAL;
using PCM.WEB.MODELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace PCM.WEB.Controllers
{
    public class AuditoriaController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Auditoria oAuditoria = new DAL.Auditoria(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Qualidade oQualidade = new DAL.Qualidade(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Picture oPicture = new DAL.Picture(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: JSON :::

            //JSON: /UNIDADE/
            public JsonResult LoadUnidade()
            {
                return Json(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                           iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                           bCadastro: false));
            }

            //JSON: /PROGRAMADA/
            public JsonResult LoadProgramada(int unidade, int tipo_ordem_servico)
            {
                return Json(oCombo.Programada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              iCodigoUnidade: unidade,
                                              iCodigoTipoOrdemServico: tipo_ordem_servico));
            }

            //JSON: /FORNECEDOR/
            public JsonResult LoadFornecedor(int unidade)
            {
                return Json(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              iCodigoUnidade: unidade));
            }

            //JSON: /EQUIPAMENTO/
            public JsonResult LoadEquipamentoLaudo(int unidade, long programada)
            {
                return Json(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                               iCodigoUnidade: unidade,
                                               lCodigoProgramada: programada));
            }

            //JSON: /AUDITORIA QUALIDADE/
            public JsonResult LoadAuditoriaQualidade(int unidade)
            {
                return Json(oCombo.AuditoriaQualidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                      iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                      iCodigoUnidade: unidade));
            }

            //JSON: /AUDITORIA CORPORATIVO/
            public JsonResult LoadAuditoriaCorporativo(int unidade)
            {
                return Json(oCombo.AuditoriaCorporativo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                        iCodigoUnidade: unidade));
            }

            //JSON: /CHECKLIST ITENS AUDITADOS/
            public JsonResult LoadChecklistItemAuditavel(int unidade)
            {
                return Json(oCombo.ChecklistItemAuditavel(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                          iCodigoUnidade: unidade,
                                                          iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString())));
            }

        #endregion

        #region ::: AUDITORIA EXTERNA :::

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
                                        sFormulario: "audit_externa",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    string data_inicio = DateTime.Now.AddYears(-1).ToShortDateString();
                    string data_termino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();

                    ViewBag.data_inicio = data_inicio;
                    ViewBag.data_termino = data_termino;
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                    ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                    return View(oAuditoria.IndexAuditoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"]),
                                                            sDataInicio: data_inicio,
                                                            sDataTermino: data_termino,
                                                            iCodigoFornecedor: -1));
                }
            }

            // POST: INDEX
            [HttpPost]
            public ActionResult AuditoriaIndex(int unidade = -1, int fornecedor = -1, string data_inicio = "", string data_termino = "")
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
                                        sFormulario: "audit_externa",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    ViewBag.data_inicio = data_inicio;
                    ViewBag.data_termino = data_termino;
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: false), "codigo", "descricao", unidade);
                    ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigoUnidade: unidade), "codigo", "descricao", fornecedor);

                    return View(oAuditoria.IndexAuditoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                          iCodigoUnidade: unidade,
                                                          sDataInicio: data_inicio,
                                                          sDataTermino: data_termino,
                                                          iCodigoFornecedor: fornecedor));
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
                    ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigoUnidade: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", null);

                    return View();
                }
            }

            // POST: INSERT
            [HttpPost]
            public ActionResult AuditoriaInsert(int unidade, int fornecedor, string descricao, string data, string data_validade, HttpPostedFileBase arquivo, string valor = "0")
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    string sPath = "";
                    string sFileName = "";

                    if (arquivo != null)
                    {
                        sPath = Server.MapPath(Path.Combine("~/Content/arq/AuditoriaExterna", Session["empresa"].ToString()));
                        sFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                        if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                        if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                        arquivo.SaveAs(Path.Combine(sPath, sFileName));
                    }

                    //Insere Registro no Banco de Dados
                    oAuditoria.InsertAuditoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                               iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                               iCodigoUnidade: unidade,
                                               iCodigoFornecedor: fornecedor,
                                               sDescricao: descricao,
                                               sData: data,
                                               sDataValidade: data_validade,
                                               sValor: valor,
                                               sPathArquivo: Path.Combine("~/Content/arq/AuditoriaExterna", Session["empresa"].ToString(), sFileName),
                                               sArquivo: (arquivo != null) ? arquivo.FileName : "");


                    return RedirectToAction("AuditoriaInsert");
                }
            }

            // GET: /DELETE
            public ActionResult AuditoriaDelete(int unidade, long codigo, string erro = "")
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    AuditoriaExterna Auditoria = null;

                    oAuditoria.InfoAuditoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                             iCodigoUnidade: unidade,
                                             lCodigo: codigo,
                                             oAuditoria: ref Auditoria);

                    if (Auditoria == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.erro = erro;
                    return View(Auditoria);
                }
            }

            // POST: /DELETE
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult AuditoriaDelete([Bind(Include = "codigo, codigo_unidade")] AuditoriaExterna Auditoria)
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
                        oAuditoria.DeleteAuditoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                   iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                   iCodigoUnidade: Auditoria.codigo_unidade,
                                                   lCodigo: Auditoria.codigo);

                        //Redireciona para Index
                        return RedirectToAction("AuditoriaIndex");
                    }
                    catch
                    {
                        return AuditoriaDelete(unidade: Auditoria.codigo_unidade,
                                               codigo: Auditoria.codigo,
                                               erro: PCM.WEB.Properties.Resources.valida_excluir);
                    }
                }
            }

            // POST: /PRINT
            public ActionResult AuditoriaPrint(int codigo_empresa, int codigo_unidade, int codigo_auditoria)
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
                Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000032_RESUMIDO.pdf");
                return File(stream, "application/pdf;");

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

            // POST: /PRINT
            public ActionResult AuditoriaPrintPDFDetalhado(int codigo_empresa, int codigo_unidade, int codigo_auditoria)
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

            // POST: /PRINT
            public ActionResult AuditoriaPrintPDFResumido(int codigo_empresa, int codigo_unidade, int codigo_auditoria)
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

                oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000032_RESUMIDO.rpt"));

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
                Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000032_RESUMIDO.pdf");
                return File(stream, "application/pdf;");
            }

        #endregion

        #region ::: AUDITORIA CORPORATIVA :::

        // GET: INDEX
        public ActionResult AuditoriaCorporativoIndex()
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
                                        sFormulario: "audit_corporativo",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    string data_inicio = DateTime.Now.AddYears(-1).ToShortDateString();
                    string data_termino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();

                    ViewBag.data_inicio = data_inicio;
                    ViewBag.data_termino = data_termino;
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                    ViewBag.auditoria_corporativo = new SelectList(oCombo.AuditoriaCorporativo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                               iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                                               iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                    return View(oAuditoria.IndexAuditoriaCorporativo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                     iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                     sDataInicio: data_inicio,
                                                                     sDataTermino: data_termino,
                                                                     iCodigoAuditoriaInterna: -1,
                                                                     iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString())));
                }
            }

            // POST: INDEX
            [HttpPost]
            public ActionResult AuditoriaCorporativoIndex(int unidade = -1, string data_inicio = "", string data_termino = "", int auditoria_corporativo = -1)
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
                                            sFormulario: "audit_corporativo",
                                            bInserir: ref inserir,
                                            bEditar: ref editar,
                                            bExcluir: ref excluir,
                                            bAdministrador: ref administrador);

                        ViewBag.inserir = inserir;
                        ViewBag.editar = editar;
                        ViewBag.excluir = excluir;

                        ViewBag.data_inicio = data_inicio;
                        ViewBag.data_termino = data_termino;
                        ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                        bCadastro: false), "codigo", "descricao", unidade);
                        ViewBag.auditoria_corporativo = new SelectList(oCombo.AuditoriaCorporativo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                   iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()), 
                                                                                                   iCodigoUnidade: unidade), "codigo", "descricao", auditoria_corporativo);

                        return View(oAuditoria.IndexAuditoriaCorporativo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                         iCodigoUnidade: unidade,
                                                                         sDataInicio: data_inicio,
                                                                         sDataTermino: data_termino,
                                                                         iCodigoAuditoriaInterna: auditoria_corporativo,
                                                                         iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString())));
                    }
                }

            // POST: /DELETE
            public JsonResult AuditoriaCorporativoExcluir(int codigo_empresa, int codigo_unidade, long codigo_auditoria)
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oAuditoria.DeleteAuditoriaCorporativo(iCodigoEmpresa: codigo_empresa,
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

            // GET: /AUDITORIA INSERIR
            public ActionResult AuditoriaCorporativoInsert()
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
                    ViewBag.auditoria_corporativo = new SelectList(oCombo.AuditoriaCorporativo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                               iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                                               iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                
                    return View();
                }
            }

            // GET: /AUDITORIA CORPORATIVO
            [HttpPost]
            public ActionResult AuditoriaCorporativoInsert(int unidade, int auditoria_corporativo, string numero_documento)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {

                    long codigo = 0;

                    oAuditoria.InsertAuditoriaCorporativo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                          iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                          iCodigoUnidade: unidade,
                                                          sNumeroDocumento: numero_documento,
                                                          iCodigoAuditoriaCorporativo: auditoria_corporativo,
                                                          lCodigo: ref codigo);

                    return RedirectToAction("Apontamento", new { codigo_auditoria_interna = auditoria_corporativo, codigo_unidade = unidade, codigo_auditoria = codigo, data = "" });
                }
            }

            // GET: /APONTAMENTO
            public ActionResult Apontamento(int codigo_auditoria_interna, int codigo_unidade, long codigo_auditoria, string data)
                {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {

                    if (codigo_auditoria == -1)
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

                    ViewBag.codigo_usuario = User.Identity.GetUserName();

                    oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        sFormulario: "audit_corporativo",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.administrador = administrador;
                    ViewBag.inserir = inserir;
                    ViewBag.excluir = excluir;
                    ViewBag.data = data;

                    return View(apontamento);
                }
            }

            // POST: /APONTAMENTO
            [HttpPost]
            public ActionResult Apontamento(int codigo_empresa, int codigo_unidade, int codigo_usuario, int codigo_auditoria_interna, long codigo_auditoria, List<QAApontamentoChecklist> checklist, bool concluido = false, int prazo_dias = 0)
            {

                ViewBag.codigo_usuario = User.Identity.GetUserName();

                //Insere Ordem de Serviço
                oQualidade.UpdateApontamento(iCodigoEmpresa: codigo_empresa,
                                             iCodigoUnidade: codigo_unidade,
                                             iCodigoUsuario: codigo_usuario,
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
                                                              iCodigoUsuario: codigo_usuario,
                                                              iCodigoUnidade: codigo_unidade,
                                                              lCodigoAuditoria: codigo_auditoria,
                                                              lCodigoChecklist: item.codigo_checklist,
                                                              iCodigoItemChecklist: item.codigo_item_checklist,
                                                              sResultado: item.resultado,
                                                              sPrazo: (item.prazo.ToString() == "0")? prazo_dias.ToString(): item.prazo,
                                                              sObservacao: item.observacao);

                    }

                }

                if (concluido)
                {
                    oQualidade.SendEmailPlanoAcao(iCodigoEmpresa: codigo_empresa,
                                                  iCodigoUsuario: codigo_usuario,
                                                  iCodigoUnidade: codigo_unidade,
                                                  lCodigoAuditoria: codigo_auditoria,
                                                  sTipo: "AUDITORIA");
                }

                return RedirectToAction("AuditoriaCorporativoIndex", "Auditoria");
                
            }

            // POST: /UPLOAD FOTO
            public JsonResult UploadFoto(long codigo_auditoria, int codigo_unidade, int codigo_item_checklist)
            {

                try
                {

                    string sPath = Path.Combine("C:\\SIM\\PCM\\SITE\\IMAGE\\AUDITORIA", Session["empresa"].ToString(), codigo_unidade.ToString(), codigo_auditoria.ToString());        

                    if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase oHttpPostedFileBase = Request.Files[i];
                        string sFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(oHttpPostedFileBase.FileName);
                        if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));

                        ResizeAndSaveImage(oHttpPostedFileBase.InputStream, Path.Combine(sPath, sFileName));

                        oPicture.InsertPicture(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                               iCodigoUnidade: codigo_unidade,
                                               sTipo: "AUDITORIA",
                                               lCodigo: codigo_auditoria,
                                               iCodigoItemChecklist: codigo_item_checklist,
                                               sImagePath: Path.Combine(sPath, sFileName));

                    }
                           
                    return Json("true");
                } 
                catch(Exception ex)
                {
                    return Json("false");
                }

            }

            // POST: /LOAD FOTO
            public JsonResult LoadFoto(long codigo_auditoria, int codigo_unidade, int codigo_item_checklist)
            {
                
                return Json(oPicture.PictureList(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: codigo_unidade,
                                                lCodigo: codigo_auditoria,
                                                sTipo: "AUDITORIA",
                                                iCodigoItemChecklist: codigo_item_checklist));
            }

            // POST: /DELETE
            public JsonResult AuditoriaCorporativoExcluirFoto(int codigo_empresa, int codigo_unidade, long codigo_auditoria, int codigo_item_checklist, int codigo)
            {
                try
                {
                    //Insere Registro no Banco de Dados
                    oPicture.DeletePicture(iCodigoEmpresa: codigo_empresa,
                                           iCodigoUnidade: codigo_unidade,
                                           sTipo: "AUDITORIA_CORPORATIVO",
                                           lCodigoDocumento: codigo_auditoria,
                                           iCodigoItemChecklist: codigo_item_checklist,
                                           iCodigo: codigo);

                    //Redireciona para Index
                    return Json(true);
                }
                catch
                {
                    return Json(false);
                }
            }

            public void ResizeAndSaveImage(Stream imageStream, string outputFilePath)
            {
            
                // Carrega a imagem a partir do Stream
                using (Bitmap originalImage = new Bitmap(imageStream))
                {

                    double scaleFactor = (originalImage.Width > originalImage.Height)? 400.0 / originalImage.Width : 400.0 / originalImage.Height;

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

        #region ::: LAUDOS :::

            // GET: INDEX
            public ActionResult LaudoIndex()
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
                                        sFormulario: "audit_laudo",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    string data_inicio = DateTime.Now.AddYears(-1).ToShortDateString();
                    string data_termino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();

                    ViewBag.data_inicio = data_inicio;
                    ViewBag.data_termino = data_termino;
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                    ViewBag.programada = new SelectList(oCombo.Programada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                            iCodigoTipoOrdemServico: 6), "codigo", "descricao", null);
                    ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                    ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                            lCodigoProgramada: 0), "codigo", "descricao", null);
                    ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Session["codigo_modulo"].ToString());

                    return View(oAuditoria.IndexLaudo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"]),
                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                        sDataInicio: data_inicio,
                                                        sDataTermino: data_termino,
                                                        lCodigoPCMProgramada: -1,
                                                        iCodigoFornecedor: -1,
                                                        lCodigoEquipamento: -1));
                }
            }
        
            // POST: INDEX
            [HttpPost]
            public ActionResult LaudoIndex(int unidade = -1, int modulo = -1, string data_inicio = "", string data_termino = "", long programada = -1, int fornecedor = -1, long equipamento = -1)
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
                                        sFormulario: "audit_laudo",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    ViewBag.data_inicio = data_inicio;
                    ViewBag.data_termino = data_termino;
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()), 
                                                                    bCadastro: false), "codigo", "descricao", unidade);
                    ViewBag.programada = new SelectList(oCombo.Programada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigoUnidade: unidade,
                                                                          iCodigoTipoOrdemServico: 6), "codigo", "descricao", programada);
                    ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigoUnidade: unidade), "codigo", "descricao", fornecedor);
                    ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: unidade,
                                                                            lCodigoProgramada: programada), "codigo", "descricao", equipamento);
                    ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                  iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", modulo);

                    return View(oAuditoria.IndexLaudo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                      iCodigoUnidade: unidade,
                                                      iCodigoModulo: modulo,
                                                      sDataInicio: data_inicio,
                                                      sDataTermino: data_termino,
                                                      lCodigoPCMProgramada: programada,
                                                      iCodigoFornecedor:fornecedor,
                                                      lCodigoEquipamento: equipamento));
                }
            }

            // GET: INSERT
            public ActionResult LaudoInsert()
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
                    ViewBag.programada = new SelectList(oCombo.Programada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                          iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                          iCodigoTipoOrdemServico: 6), "codigo", "descricao", null);
                    ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                    ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                            lCodigoProgramada: 0), "codigo", "descricao", null);
                    ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                  iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Session["codigo_modulo"].ToString());

                    return View();
                }
            }

            // POST: INSERT
            [HttpPost]
            public ActionResult LaudoInsert(int unidade, int modulo, long programada, int fornecedor, string data, string data_validade, HttpPostedFileBase arquivo, string[] equipamento, string valor = "0")
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    string sPath = "";
                    string sFileName = "";

                    if (arquivo != null)
                    {
                        sPath = Server.MapPath(Path.Combine("~/Content/arq/Laudo", Session["empresa"].ToString()));
                        sFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                        if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                        if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                        arquivo.SaveAs(Path.Combine(sPath, sFileName));
                    }

                    //Insere Registro no Banco de Dados
                    oAuditoria.InsertLaudo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                           iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                           iCodigoUnidade: unidade,
                                           iCodigoModulo: modulo,
                                           lCodigoPCMProgramada: programada,
                                           iCodigoFornecedor: fornecedor,
                                           sData: data,
                                           sDataValidade: data_validade,
                                           sValor: valor,
                                           sEquipamento: (equipamento == null) ? "" : string.Join(",", equipamento),
                                           sPathArquivo: Path.Combine("~/Content/arq/Laudo", Session["empresa"].ToString(), sFileName),
                                           sArquivo: (arquivo != null) ? arquivo.FileName : "");


                    return RedirectToAction("LaudoInsert");
                }
            }

            // GET: /DELETE
            public ActionResult LaudoDelete(int unidade, long codigo, string erro = "")
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    Laudo laudo = null;

                    oAuditoria.InfoLaudo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                         iCodigoUnidade: unidade,
                                         lCodigo: codigo,
                                         oLaudo: ref laudo);

                    if (laudo == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.erro = erro;
                    return View(laudo);
                }
            }

            // POST: /DELETE
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult LaudoDelete([Bind(Include = "codigo, codigo_unidade")] Laudo laudo)
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
                        oAuditoria.DeleteLaudo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                               iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                               iCodigoUnidade: laudo.codigo_unidade,
                                               lCodigo: laudo.codigo);

                        //Redireciona para Index
                        return RedirectToAction("LaudoIndex");
                    }
                    catch
                    {
                        return LaudoDelete(unidade: laudo.codigo_unidade,
                                           codigo: laudo.codigo,
                                           erro: PCM.WEB.Properties.Resources.valida_excluir);
                    }
                }
            }

        #endregion

        #region ::: NORMAS E PROCEDIMENTOS :::

            // GET: INDEX
            public ActionResult NormasProcedimentosIndex()
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
                                        sFormulario: "audit_normas_procedimentos",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    if (editar || excluir)
                        if (Session["unidade"] == "")
                            ViewBag.columnDefs = "columnDefs: [{ orderable: false, targets: [3] }], order: [[0]]";
                        else
                            ViewBag.columnDefs = "columnDefs: [{ orderable: false, targets: [2] }], order: [[0]]";
                    else
                        ViewBag.columnDefs = "order: [[0]]";

                    return View(oAuditoria.IndexNormasProcedimentos(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"])));
                }
            }

            // GET: INSERT
            public ActionResult NormasProcedimentosInsert()
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
                    ViewBag.tipo = new SelectList(oCombo.TipoNormaTecnica(), "codigo", "descricao", null);

                    return View();
                }
            }

            // POST: INSERT
            [HttpPost]
            public ActionResult NormasProcedimentosInsert(string descricao, string comentario, string tipo, HttpPostedFileBase arquivo, bool ativo = false, int unidade = -1)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    string sPath = "";
                    string sFileName = "";

                    if (arquivo != null)
                    {
                        sPath = Server.MapPath(Path.Combine("~/Content/arq/NormasProcedimentos", Session["empresa"].ToString()));
                        sFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                        if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                        if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                        arquivo.SaveAs(Path.Combine(sPath, sFileName));
                    }


                    //Insere Registro no Banco de Dados
                    oAuditoria.InsertNormasProcedimentos(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                         iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                         iCodigoUnidade: unidade,
                                                         sDescricao: descricao,
                                                         sTipo:tipo,
                                                         sComentario: comentario,                                                         
                                                         sPathArquivo: Path.Combine("~/Content/arq/NormasProcedimentos", Session["empresa"].ToString(), sFileName),
                                                         sArquivo: (arquivo != null) ? arquivo.FileName : "",
                                                         bAtivo: ativo);

                    return RedirectToAction("NormasProcedimentosInsert");
                }
            }

            // GET: /EDIT
            public ActionResult NormasProcedimentosEdit(int codigo)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    NormasProcedimentos normas_procedimentos = null;

                    oAuditoria.InfoNormasProcedimentos(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                       iCodigo: codigo,
                                                       oNormasProcedimentos: ref normas_procedimentos);

                    if (normas_procedimentos == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: false), "codigo", "descricao", normas_procedimentos.codigo_unidade);
                    ViewBag.arquivo = normas_procedimentos.arquivo;
                    ViewBag.tipo = new SelectList(oCombo.TipoNormaTecnica(), "codigo", "descricao", normas_procedimentos.tipo);

                    return View(normas_procedimentos);
                }
            }

            // POST: /EDIT
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult NormasProcedimentosEdit(string descricao, string comentario, int codigo, string tipo, HttpPostedFileBase arquivo, string change_arquivo, bool ativo = false, int unidade = -1)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    string sPath = "";
                    string sFileName = "";

                    if (change_arquivo == "change")
                    {
                        if (arquivo != null)
                        {
                            sPath = Server.MapPath(Path.Combine("~/Content/arq/NormasProcedimentos", Session["empresa"].ToString()));
                            sFileName = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + Path.GetExtension(arquivo.FileName);
                            if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                            if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                            arquivo.SaveAs(Path.Combine(sPath, sFileName));
                        }

                    }

                    //Insere Registro no Banco de Dados
                    oAuditoria.UpdateNormasProcedimentos(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                         iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                         iCodigoUnidade: unidade,
                                                         sDescricao: descricao,
                                                         sTipo: tipo,
                                                         sComentario: comentario,
                                                         sArquivo: (arquivo != null) ? arquivo.FileName : "",
                                                         sPathArquivo: Path.Combine("~/Content/arq/NormasProcedimentos", Session["empresa"].ToString(), sFileName),
                                                         bAtivo: ativo,
                                                         iCodigo: codigo);

                    //Redireciona para Index
                    return RedirectToAction("NormasProcedimentosIndex");
                }
            }

            // GET: /DELETE
            public ActionResult NormasProcedimentosDelete(int codigo, string erro = "")
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    NormasProcedimentos normas_procedimentos = null;

                    oAuditoria.InfoNormasProcedimentos(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                       iCodigo: codigo,
                                                       oNormasProcedimentos: ref normas_procedimentos);

                    if (normas_procedimentos == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.erro = erro;
                    return View(normas_procedimentos);
                }
            }

            // POST: /DELETE
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult NormasProcedimentosDelete([Bind(Include = "codigo")] NormasProcedimentos normas_procedimentos)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    try
                    {
                        //Deleta Registro no Banco de Dados
                        oAuditoria.DeleteNormasProcedimentos(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                             iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                             iCodigo: normas_procedimentos.codigo);

                        //Redireciona para Index
                        return RedirectToAction("NormasProcedimentosIndex");
                    }
                    catch
                    {
                        return NormasProcedimentosDelete(codigo: normas_procedimentos.codigo,
                                                         erro: PCM.WEB.Properties.Resources.valida_excluir);
                    }

                }
            }

            //JSON: /VALIDA FUNÇÃO
            public JsonResult ValidaNormasProcedimentos(int unidade, string descricao, int codigo)
            {

                return Json(oAuditoria.ValidaNormasProcedimentos(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                 iCodigoUnidade: unidade,
                                                                 sDescricao: descricao,
                                                                 iCodigo: codigo));

            }

        #endregion

        #region ::: RELATÓRIO :::

            // GET: RELATÓRIO
            public ActionResult Relatorio()
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
                    bool imprimir = false;
                    bool administrador = false;

                    oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        sFormulario: "audit_normas_procedimentos",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador,
                                        bImprimir: ref imprimir);

                    string data_inicio = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString();
                    string data_termino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                    string[] checklist = new string[] { "0|0" };

                    ViewBag.imprimir = imprimir;
                    ViewBag.data_inicio = data_inicio;
                    ViewBag.data_termino = data_termino;
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                    ViewBag.checklist_item_auditavel = new SelectList(oCombo.ChecklistItemAuditavel(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                                    iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString())), "codigo", "descricao", null);
                    ViewBag.informacao = oAuditoria.RelatorioAuditoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                       iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                       sChecklist: checklist);

                    ViewBag.checklist = String.Concat("[]");

                    return View(oAuditoria.RelatorioAuditoriaData(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                  iCodigoUnidade: -1,
                                                                  sChecklist: checklist,
                                                                  sDataInicio: data_inicio,
                                                                  sDataTermino: data_termino));

            }
        }

            // POST: RELATÓRIO
            [HttpPost]
            public ActionResult Relatorio(string data_inicio, string data_termino, string[] checklist_item_auditavel, int unidade = -1)
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
                    bool imprimir = false;

                    oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        sFormulario: "audit_relatorio",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador,
                                        bImprimir: ref imprimir);

                    ViewBag.imprimir = imprimir;                    
                    ViewBag.data_inicio = data_inicio;
                    ViewBag.data_termino = data_termino;
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                    ViewBag.checklist_item_auditavel = new SelectList(oCombo.ChecklistItemAuditavel(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                    iCodigoUnidade: unidade,
                                                                                                    iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString())), "codigo", "descricao", checklist_item_auditavel);
                    ViewBag.informacao = oAuditoria.RelatorioAuditoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                       iCodigoUnidade: unidade,
                                                                       sChecklist: checklist_item_auditavel);

                    string checklist = "";
                    if (checklist_item_auditavel != null)
                    {
                        for (int i = 0; i < checklist_item_auditavel.Length; i++)
                            checklist = String.Concat(checklist, String.Concat((checklist == "") ? "'" : ", '", checklist_item_auditavel[i].ToString(), "'"));
                    }
                    ViewBag.checklist = String.Concat("[", checklist, "]");

                    return View(oAuditoria.RelatorioAuditoriaData(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                  iCodigoUnidade: unidade,
                                                                  sChecklist: checklist_item_auditavel,
                                                                  sDataInicio: data_inicio,
                                                                  sDataTermino: data_termino));
                }
            }

            // GET: RELATÓRIO
            public ActionResult RelatorioMensal()
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {
                    
                    return View();
                }
            }

        #endregion

        #region "::: AUDITORIA HISTÓRICO :::"

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
                                    sFormulario: "audit_corporativo_historico",
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
                ViewBag.auditoria_qualidade = new SelectList(oCombo.AuditoriaCorporativo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                         iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                                         iCodigoUnidade: unidade), "codigo", "descricao", auditoria_qualidade);
                ViewBag.status = new SelectList(oCombo.StatusAuditoriaCorporativo(bHistorico: true), "codigo", "descricao", status);


                return View(oAuditoria.AuditoriaHistorico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
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
                                    sFormulario: "audit_corporativo_historico",
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
                ViewBag.auditoria_qualidade = new SelectList(oCombo.AuditoriaCorporativo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                         iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                                         iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.status = new SelectList(oCombo.StatusAuditoriaCorporativo(bHistorico: true), "codigo", "descricao", status);

                return View(oAuditoria.AuditoriaHistorico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                          iCodigoUnidade: unidade,
                                                          sDataInicio: data_inicio,
                                                          sDataTermino: data_termino,
                                                          iCodigoAuditoriaInterna: auditoria_qualidade,
                                                          iStatus: status));
            }
        }

        #endregion

    }
}