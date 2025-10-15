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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace PCM.WEB.Controllers
{
    public class UHController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.UH oUH = new DAL.UH(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Governanca oGovernanca = new DAL.Governanca(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.PMOC oPMOC = new DAL.PMOC(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: JSON :::

        //JSON: /UNIDADE/
        public JsonResult LoadUnidade()
        {
            return Json(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                       iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                       bCadastro: false));
        }

        //JSON: /ATIVIDADE/
        public JsonResult LoadAtividade(int unidade)
        {
            return Json(oCombo.Atividade(Convert.ToInt32(Session["empresa"].ToString()), unidade));
        }

        //JSON: /APARTAMENTO/
        public JsonResult LoadApartamento(int unidade)
        {
            return Json(oCombo.Apartamento(Convert.ToInt32(Session["empresa"].ToString()), unidade));
        }

        #endregion

        #region ::: UH :::

        // GET: CHECKLIST
        public ActionResult ChecklistUH(string status = "", string statusSelected = "")
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
                                    sFormulario: "uh_checklist",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                
                var filterStatus = statusSelected.Split(',').ToList();

                if (filterStatus.Contains(status)) {
                    filterStatus.Remove(status);
                } else if (status != null)
                {
                    filterStatus.Add(status);
                }

                ViewBag.statusSelected = string.Join(",", filterStatus);
                ViewBag.status = oUH.LoadChecklistStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                         iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                         iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()));

                ViewBag.lastUpdate = oGovernanca.LoadLastUploadStatus(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                      codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()));

                return View(oUH.LoadUHChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                sStatus: string.Join(",", filterStatus)));
            }
        }

        // GET: APONTAMENTO CHECKLIST
        public ActionResult ChecklistUHApontamento(int codigo_unidade, int codigo_apartamento, long codigo_vistoria, int status, string color, string page = "ChecklistUH", string model = "UH")
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
                                    sFormulario: "uh_checklist",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                UHApontamento apontamento = null;

                //Carrega Dados - Apontamento
                oUH.LoadDadosApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: codigo_unidade,
                                            iCodigoApartamento: codigo_apartamento,
                                            lCodigo: codigo_vistoria,
                                            oUHApontamento: ref apontamento);

                ViewBag.page = page;
                ViewBag.model = model;
                ViewBag.apontamento = apontamento;
                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;
                ViewBag.color = color;
                ViewBag.status = status;
                ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.administrador_string = (administrador == true) ? "" : "readonly";
                ViewBag.funcionario_vistoria = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                    iCodigoUnidade: codigo_unidade,
                                                                                    iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", apontamento.codigo_funcionario_responsavel_vistoria);
                ViewBag.funcionario_unidade = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                iCodigoUnidade: codigo_unidade,
                                                                                iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", apontamento.codigo_funcionario_responsavel_unidade);

                return View(oUH.LoadUHApontamentoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: codigo_unidade,
                                                            iCodigoApartamento: codigo_apartamento,
                                                            lCodigo: codigo_vistoria));

            }
        }

        // POST: APONTAMENTO CHECKLIST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChecklistUHApontamento(int codigo_unidade, int codigo_apartamento, long codigo_apontamento, int funcionario_vistoria, int funcionario_unidade, string data_inicio, string data_termino, List<UHApontamentoChecklist> checklist, string hora_inicio = "00:00:00", string hora_termino = "00:00:00", string page = "ChecklistUH", string model = "UH")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                long codigo_checklist = 0;

                oUH.InsertUHApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        iCodigoUnidade: codigo_unidade,
                                        iCodigoApartamento: codigo_apartamento,
                                        iCodigoFuncionarioResponsavelVistoria: funcionario_vistoria,
                                        iCodigoFuncionarioResponsavelUnidade: funcionario_unidade,
                                        sDataInicio: data_inicio,
                                        sDataTermino: data_termino,
                                        sHoraInicio: hora_inicio,
                                        sHoraTermino: hora_termino,
                                        lCodigoUHApontamento: ref codigo_apontamento,
                                        lCodigoChecklist: ref codigo_checklist);

                if (checklist != null)
                {

                    foreach (UHApontamentoChecklist item in checklist)
                    {

                        //Insere Registro no Banco de Dados
                        oUH.InsertUHApontamentoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: codigo_unidade,
                                                            lCodigoUHApontamento: codigo_apontamento,
                                                            lCodigoChecklist: codigo_checklist,
                                                            iCodigoChecklistItem: item.codigo,
                                                            sDescricaoChecklist: item.descricao.ToUpper(),
                                                            sOpcao: item.opcao,
                                                            sObservacao: item.observacao,
                                                            bNovaVistoria: item.nova_vistoria);;

                    }

                }

                //Atualiza Status UH em dia
                oUH.UpdateUHStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                   iCodigoUnidade: codigo_unidade,
                                   lCodigoUHApontamento: codigo_apontamento);

            }

            return RedirectToAction(page, model);

        }

        // POST: /PRINT
        public ActionResult PrintChecklist(string unidade, int codigo_unidade, int codigo_apartamento, int codigo)
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
            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000020.rpt"));

            oCrDatabase = oReportDocument.Database;
            oCrTables = oCrDatabase.Tables;

            foreach (Table crTable in oCrTables)
            {
                oTableLogOnInfo = crTable.LogOnInfo;
                oTableLogOnInfo.ConnectionInfo = oConnectionInfo;
                crTable.ApplyLogOnInfo(oTableLogOnInfo);
            }                     

            oReportDocument.SetParameterValue("unidade", unidade);
            oReportDocument.SetParameterValue("usuario", Session["nome"].ToString());
            oReportDocument.SetParameterValue("@codigo_unidade", codigo_unidade);
            oReportDocument.SetParameterValue("@codigo_empresa", Convert.ToInt32(Session["empresa"].ToString()));
            oReportDocument.SetParameterValue("@codigo_apartamento", codigo_apartamento);
            oReportDocument.SetParameterValue("@codigo", codigo);

            oReportDocument.SetDatabaseLogon(ConfigurationManager.AppSettings.GetValues("user_id")[0],
                                                ConfigurationManager.AppSettings.GetValues("password")[0],
                                                ConfigurationManager.AppSettings.GetValues("data_source")[0],
                                                ConfigurationManager.AppSettings.GetValues("initial_catalog")[0]);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000020.pdf");
            return File(stream, "application/pdf;");
        }

        //JSON: /ANDAR/
        public JsonResult DeleteUHApontamento(int unidade, int apartamento, int codigo)
        {
            return Json(oUH.DeleteUHApontamentoChecklist(Convert.ToInt32(Session["empresa"].ToString()), unidade, apartamento, codigo));
        }

        #endregion

        #region ::: CHECKLIST - HISTÓRICO :::

        // GET: CHECKLIST
        public ActionResult ChecklistHistorico()
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
                                    sFormulario: "uh_checklist_historico",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bImprimir: ref imprimir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;
                ViewBag.data_inicio = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString();
                ViewBag.data_termino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()), 
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                return View(oUH.LoadChecklistHistorico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                        sDataInicio: TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString(),
                                                        sDataTermino:  TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString(),
                                                        iCodigoApartamento: -1));

            }
        }

        // GET: CHECKLIST
        public ActionResult ChecklistHistorico2()
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
                                    sFormulario: "uh_checklist_historico",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bImprimir: ref imprimir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;
                ViewBag.data_inicio = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString();
                ViewBag.data_termino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                return View(oUH.LoadChecklistHistorico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                        sDataInicio: TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString(),
                                                        sDataTermino: TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString(),
                                                        iCodigoApartamento: -1));

            }
        }

        // GET: CHECKLIST HISTÓRICO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChecklistHistorico(string data_inicio, string data_termino, int apartamento = -1, int codigo_unidade = -100, int unidade = -100)
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
                                        sFormulario: "uh_checklist_historico",
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
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: true), "codigo", "descricao", (unidade == -100) ? codigo_unidade : unidade);
                    ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: (unidade == -100) ? codigo_unidade : unidade), "codigo", "descricao", apartamento);

                    return View(oUH.LoadChecklistHistorico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                           iCodigoUnidade: (unidade == -100)? codigo_unidade: unidade,
                                                           sDataInicio: data_inicio,
                                                           sDataTermino:  data_termino,
                                                           iCodigoApartamento: apartamento));

                }
            }

        #endregion

        #region ::: UH ATIVIDADE :::

        // GET: UH ATIVIDADE
        public ActionResult UHAtividade(int status = -1, long atividade = -1, int unidade = -1, int status_uh = -1)
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

                if (unidade == -1) { unidade = Convert.ToInt32(Session["codigo_unidade"].ToString()); }

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "uh_atividade",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", unidade);
                ViewBag.atividade = new SelectList(oCombo.Atividade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                    iCodigoUnidade: unidade), "codigo", "descricao", atividade);
                ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: unidade), "codigo", "descricao", null);
                ViewBag.status = new SelectList(oCombo.StatusAtividade(), "codigo", "descricao", status);
                ViewBag.status_uh = new SelectList(oCombo.StatusUHAtividade(), "codigo", "descricao", status_uh);

                List<UHAtividadeBloco> UHAtividadeBloco = new List<UHAtividadeBloco>();

                int iQuantidadeAtrasado = 0;
                int iQuantidadePendente = 0;
                int iQuantidadeRealizado = 0;

                UHAtividadeBloco = oUH.LoadUHBlocoAtividade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            lCodigoAtividade: atividade,
                                                            iCodigoApartamento: -1,
                                                            iStatus: status,
                                                            iStatusUH: status_uh,
                                                            iQuantidadeAtrasado: ref iQuantidadeAtrasado,
                                                            iQuantidadePendente: ref iQuantidadePendente,
                                                            iQuantidadeRealizada: ref iQuantidadeRealizado);
                
                ViewBag.quantidade_atrasado = iQuantidadeAtrasado;
                ViewBag.quantidade_pendente = iQuantidadePendente;
                ViewBag.quantidade_realizada = iQuantidadeRealizado;

                ViewBag.lastUpdate = oGovernanca.LoadLastUploadStatus(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                      codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()));

                return View(UHAtividadeBloco);

            }
        }

        // POST: UH ATIVIDADE
        [HttpPost]
        public ActionResult UHAtividade(int unidade, int atividade, int apartamento = -1, int status = -1, int status_uh = -1)
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
                                    sFormulario: "uh_atividade",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.administrador = administrador;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", unidade);
                ViewBag.atividade = new SelectList(oCombo.Atividade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), iCodigoUnidade: unidade), "codigo", "descricao", atividade);
                ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: unidade), "codigo", "descricao", apartamento);
                ViewBag.status = new SelectList(oCombo.StatusAtividade(), "codigo", "descricao", status);
                ViewBag.status_uh = new SelectList(oCombo.StatusUHAtividade(), "codigo", "descricao", status_uh);
                
                List<UHAtividadeBloco> UHAtividadeBloco = new List<UHAtividadeBloco>();

                int iQuantidadeAtrasado = 0;
                int iQuantidadePendente = 0;
                int iQuantidadeRealizado = 0;

                UHAtividadeBloco = oUH.LoadUHBlocoAtividade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            lCodigoAtividade: atividade,
                                                            iCodigoApartamento: apartamento,
                                                            iStatus: status,
                                                            iStatusUH: status_uh,
                                                            iQuantidadeAtrasado: ref iQuantidadeAtrasado,
                                                            iQuantidadePendente: ref iQuantidadePendente,
                                                            iQuantidadeRealizada: ref iQuantidadeRealizado);
                
                ViewBag.quantidade_atrasado = iQuantidadeAtrasado;
                ViewBag.quantidade_pendente = iQuantidadePendente;
                ViewBag.quantidade_realizada = iQuantidadeRealizado;

                ViewBag.lastUpdate = oGovernanca.LoadLastUploadStatus(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                      codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()));

                return View(UHAtividadeBloco);
            }
        }

        // GET: APONTAMENTO CHECKLIST
        public ActionResult UHAtividadeApontamento(int codigo_apartamento, int codigo_unidade, long codigo_atividade, int status, string color, string page = "UHAtividade", string model = "UH")
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
                                    sFormulario: "uh_atividade",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                UHAtividadeApontamento apontamento = null;

                //Carrega Dados - Apontamento
                apontamento = oUH.LoadUHAtividadeApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: codigo_unidade,
                                                                lCodigoAtividade: codigo_atividade,
                                                                iCodigoApartamento: codigo_apartamento);

                ViewBag.page = page;
                ViewBag.model = model;
                ViewBag.apontamento = apontamento;
                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;
                ViewBag.administrador = administrador;
                ViewBag.color = color;
                ViewBag.status = status;
                ViewBag.status_uh = new SelectList(oCombo.StatusUHAtividade(), "codigo", "descricao", null);
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade,
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", apontamento.codigo_funcionario);
                ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", apontamento.codigo_fornecedor);

                return View();

            }
        }

        // POST: UH ATIVIDADE
        [HttpPost]
        public ActionResult UHAtividadeApontamento(int codigo_apartamento, int codigo_unidade, long codigo_atividade, string observacao, string data_inicio, string data_termino, int status_uh, string[] funcionario, string hora_inicio= "", string hora_termino = "", int fornecedor = -1, string page = "UHAtividade", string model = "UH")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Atualiza Registro - Apontamento
                oUH.UpdateUHAtividadeApartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    lCodigoAtividade: codigo_atividade,
                                                    iCodigoApartamento: codigo_apartamento,
                                                    sObservacao: observacao,
                                                    sDataInicio: data_inicio,
                                                    sHoraInicio: hora_inicio,
                                                    sDataTermino: data_termino,
                                                    sHoraTermino: hora_termino,
                                                    sCodigoFuncionario: string.Join(",", funcionario),
                                                    iCodigoFornecedor: fornecedor,
                                                    iStatusUH: status_uh);
    
                return RedirectToAction(page, model, new {unidade=codigo_unidade, atividade = @codigo_atividade});

            }
        }

        // GET: EXCLUIR APONTAMENTO
        public ActionResult UHAtividadeApontamentoExcluir(int codigo_apartamento, int codigo_unidade, long codigo_atividade, int status, string color, string page = "UHAtividade", string model = "UH")
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
                                    sFormulario: "uh_atividade",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                UHAtividadeApontamento apontamento = null;

                //Carrega Dados - Apontamento
                apontamento = oUH.LoadUHAtividadeApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: codigo_unidade,
                                                                lCodigoAtividade: codigo_atividade,
                                                                iCodigoApartamento: codigo_apartamento);

                ViewBag.page = page;
                ViewBag.model = model;
                ViewBag.apontamento = apontamento;
                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;
                ViewBag.color = color;
                ViewBag.status = status;
                ViewBag.status_uh = new SelectList(oCombo.StatusUHAtividade(), "codigo", "descricao", apontamento.status_uh);
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade,
                                                                        iCodigoModulo: 1), "codigo", "descricao", apontamento.codigo_funcionario);
                ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade), "codigo", "descricao", apontamento.codigo_fornecedor);

                return View();

            }
        }

        // POST: EXCLUIR APONTAMENTO
        [HttpPost]
        public ActionResult UHAtividadeApontamentoExcluir(int codigo_apartamento, int codigo_unidade, long codigo_atividade, string page = "UHAtividade", string model = "UH")
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {

                    //Deleta Dados
                    oUH.DeleteUHAtividadeApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                     iCodigoUnidade: codigo_unidade,
                                                     lCodigoAtividade: codigo_atividade,
                                                     iCodigoApartamento: codigo_apartamento);

                    return RedirectToAction(page, model, new { unidade = codigo_unidade, atividade = @codigo_atividade });

                }
            }

        #endregion

        #region ::: DEDETIZAÇÃO :::

        // GET: DEDETIZAÇÃO
        public ActionResult Dedetizacao(int status = -1)
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
                                    sFormulario: "uh_dedetizacao",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.status = oUH.LoadDedetizacaoStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()));

                ViewBag.lastUpdate = oGovernanca.LoadLastUploadStatus(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                      codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()));

                return View(oUH.LoadDedetizacao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                iCodigoUHAtividade: 1,
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iStatus: status));

            }
        }

        // GET: DEDETIZAÇÃO - APONTAMENTO
        public ActionResult DedetizacaoApontamento(int codigo_unidade, int codigo_apartamento, int codigo_uh_atividade, long codigo, string color)
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
                                    sFormulario: "uh_dedetizacao",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.administrador = administrador;
                ViewBag.data = DateTime.Now.Date.ToShortDateString();
                ViewBag.color = color;

                UHDedetizacaoApontamento apontamento = new UHDedetizacaoApontamento();
                apontamento = oUH.LoadUHDedetizacaoApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: codigo_unidade,
                                                                iCodigoUHAtividade: codigo_uh_atividade,
                                                                iCodigoApartamento: codigo_apartamento,
                                                                lCodigo: codigo);
                ViewBag.apontamento = apontamento;
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: apontamento.codigo_unidade,
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", apontamento.codigo_funcionario);
                ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: apontamento.codigo_unidade), "codigo", "descricao", apontamento.codigo_fornecedor);
                    

                return View();
            }
        }

        // POST: DEDETIZAÇÃO - APONTAMENTO
        [HttpPost]
        public ActionResult DedetizacaoApontamento(int codigo_unidade, int codigo_apartamento, int codigo_uh_atividade, long codigo, string data, string observacao, int funcionario = -1, int fornecedor = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                //Deleta Dados
                oUH.UHDedetizacaoApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: codigo_unidade,
                                                iCodigoUHAtividade: codigo_uh_atividade,
                                                lCodigo: codigo,
                                                iCodigoApartamento: codigo_apartamento,
                                                iCodigoFuncionario: funcionario,
                                                iCodigoFornecedor: fornecedor,
                                                sData: data,
                                                sObservacao: observacao,
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()));

                return RedirectToAction("Dedetizacao", "UH");

            }
        }

        // GET: DEDETIZAÇÃO - APONTAMENTO
        public ActionResult DedetizacaoApontamentoMultiplos(int codigo_unidade, int codigo_uh_atividade)
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
                                    sFormulario: "uh_dedetizacao",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.administrador = administrador;
                ViewBag.data = DateTime.Now.Date.ToShortDateString();

                UHAtividadeInfo atividade = new UHAtividadeInfo();
                atividade = oUH.LoadUHAtividade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: codigo_unidade,
                                                iCodigoUHAtividade: codigo_uh_atividade);
                    
                ViewBag.atividade = atividade;
                ViewBag.uh = new SelectList(oCombo.ApartamentoDedetizacao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: codigo_unidade,
                                                                            iCodigoUHAtividade: codigo_uh_atividade), "codigo", "descricao", null);
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade,
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", null);
                ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade), "codigo", "descricao", null);


                return View();
            }
        }

        // POST: DEDETIZAÇÃO - APONTAMENTO
        [HttpPost]
        public ActionResult DedetizacaoApontamentoMultiplos(int codigo_unidade, int codigo_uh_atividade, string data, string observacao, int[] uh, int funcionario = -1, int fornecedor = -1)
            {
                if (Session["empresa"] == null)
                {
                    return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
                }
                else
                {

                    for(int i = 0; i< uh.Length; i++)
                    {

                        //Deleta Dados
                        oUH.UHDedetizacaoApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                     iCodigoUnidade: codigo_unidade,
                                                     iCodigoUHAtividade: codigo_uh_atividade,
                                                     lCodigo: -1,
                                                     iCodigoApartamento: uh[i],
                                                     iCodigoFuncionario: funcionario,
                                                     iCodigoFornecedor: fornecedor,
                                                     sData: data,
                                                     sObservacao: observacao,
                                                     iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()));
                    }


                    return RedirectToAction("Dedetizacao", "UH");

                }
            }

        #endregion

    }
}