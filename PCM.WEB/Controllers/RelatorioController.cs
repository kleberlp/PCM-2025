using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.AspNet.Identity;
using PCM.WEB.MODELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Mvc;

namespace PCM.WEB.Controllers
{
    public class RelatorioController : Controller
    {
        private DAL.Combo oCombo = new DAL.Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Relatorio oRelatorio = new DAL.Relatorio(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.GreenPlanet oGreenPlanet = new DAL.GreenPlanet(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Account oAccount = new DAL.Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Picture oPicture = new DAL.Picture(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: JSON :::

        //JSON: /MESES
        public JsonResult Meses(string data_inicio, string data_termino)
        {
            return Json(oRelatorio.Meses(sDataInicio: Convert.ToDateTime(data_inicio),
                                            sDataTermino: Convert.ToDateTime(data_termino)));
        }

        //JSON: /FUNCIONÁRIO - CUSTO HORAS TRABALHADAS/
        public JsonResult ChartCustoHorasTrabalhadas(int unidade, string data_inicio, string data_termino, int ativo)
        {
            return Json(oRelatorio.ChartCustoHorasTrabalhadas(iCodigoEmpresa: Convert.ToInt16(Session["empresa"].ToString()),
                                                                iCodigoUnidade: unidade,
                                                                iCodigoModulo: Convert.ToInt16(Session["codigo_modulo"].ToString()),
                                                                sDataInicio: data_inicio,
                                                                sDataTermino: data_termino,
                                                                iAtivo: ativo));
        }

        //JSON: /FUNCIONÁRIO - HORAS TRABALHADAS/
        public JsonResult ChartFuncionarioHorasTrabalhadas(int unidade, string data_inicio, string data_termino, int ativo)
        {
            return Json(oRelatorio.ChartFuncionarioHorasTrabalhadas(iCodigoEmpresa: Convert.ToInt16(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: unidade,
                                                                    iCodigoModulo: Convert.ToInt16(Session["codigo_modulo"].ToString()),
                                                                    sDataInicio: data_inicio,
                                                                    sDataTermino: data_termino,
                                                                    iAtivo: ativo));
        }

        //JSON: /FUNCIONÁRIO - OCIOSIDADE/
        public JsonResult ChartFuncionarioOciosidade(int unidade, string data_inicio, string data_termino, int ativo)
        {
            return Json(oRelatorio.ChartFuncionarioOciosidade(iCodigoEmpresa: Convert.ToInt16(Session["empresa"].ToString()),
                                                                iCodigoUnidade: unidade,
                                                                iCodigoModulo: Convert.ToInt16(Session["codigo_modulo"].ToString()),
                                                                sDataInicio: data_inicio,
                                                                sDataTermino: data_termino,
                                                                iAtivo: ativo));
        }

        //JSON: /GREEN PLANET/
        public JsonResult ChartGreenPlanet(string data_inicio, string data_termino, int forma_calculo, int agrupado_por, int grupo_item_medicao, int item_medicao, int unidade)
        {
            return Json(oRelatorio.ChartGreenPlanet(iCodigoEmpresa: Convert.ToInt16(Session["empresa"].ToString()),
                                                    iCodigoUnidade: unidade,
                                                    sDataInicio: data_inicio,
                                                    sDataTermino: data_termino,
                                                    iAgrupadoPor: agrupado_por,
                                                    iCodigoFormaCalculoGreenPlanet: forma_calculo,
                                                    iCodigoGrupoItemMedicao: grupo_item_medicao,
                                                    iCodigoItemMedicao: item_medicao));
        }

        //JSON: /GREEN PLANET - FATA/
        public JsonResult ChartGreenPlanetData(string data_inicio, string data_termino, int forma_calculo, int agrupado_por, int grupo_item_medicao, int item_medicao, int unidade)
        {
            return Json(oRelatorio.ChartGreenPlanetData(iCodigoEmpresa: Convert.ToInt16(Session["empresa"].ToString()),
                                                        iCodigoUnidade: unidade,
                                                        sDataInicio: data_inicio,
                                                        sDataTermino: data_termino,
                                                        iAgrupadoPor: agrupado_por,
                                                        iCodigoFormaCalculoGreenPlanet: forma_calculo,
                                                        iCodigoGrupoItemMedicao: grupo_item_medicao,
                                                        iCodigoItemMedicao: item_medicao));
        }

        //JSON: /MANUTENÇÃO - ABERTO x CONCLUÍDO/
        public JsonResult ChartManutencaoAbertoConcluido(int unidade, string data_inicio, string data_termino, bool qualidade = false)
        {
            return Json(oRelatorio.ChartManutencaoAbertoConcluido(iCodigoEmpresa: Convert.ToInt16(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: unidade,
                                                                    sDataInicio: data_inicio,
                                                                    sDataTermino: data_termino,
                                                                    bQualidade: qualidade));
        }

        //JSON: /MANUTENÇÃO - CATEGORIA/
        public JsonResult ChartManutencaoCategoria(int unidade, string data_inicio, string data_termino)
        {
            return Json(oRelatorio.ChartManutencaoCategoria(iCodigoEmpresa: Convert.ToInt16(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            sDataInicio: data_inicio,
                                                            sDataTermino: data_termino));
        }

        //JSON: /MANUTENÇÃO - EQUIPAMENTO/
        public JsonResult ChartManutencaoEquipamento(int unidade, string data_inicio, string data_termino)
        {
            return Json(oRelatorio.ChartManutencaoEquipamento(iCodigoEmpresa: Convert.ToInt16(Session["empresa"].ToString()),
                                                                iCodigoUnidade: unidade,
                                                                sDataInicio: data_inicio,
                                                                sDataTermino: data_termino));
        }

        //JSON: /MANUTENÇÃO - EXECUTOR/
        public JsonResult ChartManutencaoExecutor(int unidade, int ano)
        {
            return Json(oRelatorio.ChartManutencaoExecutor(iCodigoEmpresa: Convert.ToInt16(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            iAno: ano));
        }

        //JSON: /MANUTENÇÃO - SETOR/
        public JsonResult ChartManutencaoSetor(int unidade, int ano)
        {
            return Json(oRelatorio.ChartManutencaoSetor(iCodigoEmpresa: Convert.ToInt16(Session["empresa"].ToString()),
                                                        iCodigoUnidade: unidade,
                                                        iAno: ano));
        }

        //JSON: /MANUTENÇÃO - SOLICITANTE/
        public JsonResult ChartManutencaoSolicitante(int unidade, int ano)
        {
            return Json(oRelatorio.ChartManutencaoSolicitante(iCodigoEmpresa: Convert.ToInt16(Session["empresa"].ToString()),
                                                                iCodigoUnidade: unidade,
                                                                iAno: ano));
        }

        //JSON: /MANUTENÇÃO - TEMPO MÉDIO ATENDIMENTO/
        public JsonResult ChartManutencaoTempoMedioAtendimento(int unidade, int ano, bool qualidade = false)
        {
            return Json(oRelatorio.ChartManutencaoTempoMedioAtendimento(iCodigoEmpresa: Convert.ToInt16(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: unidade,
                                                                        iAno: ano,
                                                                        bQualidade: qualidade));
        }

        //JSON: /MANUTENÇÃO - TIPO ORDEM DE SERVIÇO/
        public JsonResult ChartManutencaoTipoOrdemServico(int unidade, int ano)
        {
            return Json(oRelatorio.ChartManutencaoTipoOrdemServico(iCodigoEmpresa: Convert.ToInt16(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: unidade,
                                                                    iAno: ano));
        }

        //JSON: /UNIDADE/
        public JsonResult LoadUnidade()
        {
            return Json(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        bCadastro: false));
        }

        //JSON: /DATA PMOC/
        public JsonResult LoadDataPMOC(int unidade)
        {
            return Json(oCombo.DataPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUnidade: unidade));
        }

        //JSON: /DATA GOVERNANÇA/
        public JsonResult LoadDataGovernanca(int unidade)
        {
            return Json(oCombo.DataGovernanca(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              iCodigoUnidade: unidade));
        }

        //JSON: /Camareira/
        public JsonResult LoadCamareira(int unidade)
        {
            return Json(oCombo.FuncionarioGovernanca(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                     iCodigoUnidade: unidade));
        }

        //JSON: /Grupo Item Medição/
        public JsonResult LoadGrupoItemMedicao(int unidade)
        {
            return Json(oCombo.GrupoItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: unidade));
        }

        //JSON: /Item Medição/
        public JsonResult LoadItemMedicao(int grupo_item_medicao, int unidade)
        {
            return Json(oCombo.ItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: unidade,
                                            iCodigoGrupoItemMedicao: grupo_item_medicao));
        }

        //JSON: /Relatório - Itens Auditáveis/
        public JsonResult LoadRelatorioItensAuditaveis(int unidade)
        {
            return Json(oCombo.RelatorioItensAuditaveis(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: unidade,
                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString())));
        }

        #endregion

        #region ::: FUNCIONÁRIO - HORAS TRABALHADAS :::

        // GET: INDEX
        public ActionResult Atendimento()
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
                                    sFormulario: "rel_funcionario_horas_trabalhadas",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", 1);
                ViewBag.data_inicio = System.TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString();
                ViewBag.data_termino = System.DateTime.Now.Date.ToShortDateString();
                ViewBag.meses = oRelatorio.Meses(sDataInicio: System.DateTime.Now.Date.AddMonths(-1),
                                                    sDataTermino: System.DateTime.Now.Date);
                return View(oRelatorio.FuncionarioHorasTrabalhadas(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                    iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                    sDataInicio: System.DateTime.Now.Date.AddMonths(-1).ToString(),
                                                                    sDataTermino: System.DateTime.Now.Date.ToString(),
                                                                    iAgrupadoPorUnidade: 1,
                                                                    iAtivo: 1));
            }
        }

        // POST: /INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atendimento(string data_inicio, string data_termino, int unidade = -1, int ativo = -1, string nome_fantasia = "")
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
                                    sFormulario: "rel_funcionario_horas_trabalhadas",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", ativo);
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.meses = oRelatorio.Meses(sDataInicio: Convert.ToDateTime(data_inicio),
                                                    sDataTermino: Convert.ToDateTime(data_termino));
                return View(oRelatorio.FuncionarioHorasTrabalhadas(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: unidade,
                                                                    iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                    sDataInicio: data_inicio,
                                                                    sDataTermino: data_termino,
                                                                    iAgrupadoPorUnidade: 1,
                                                                    iAtivo: ativo));
            }
        }

        #endregion

        #region ::: FUNCIONÁRIO - HORAS TRABALHADAS :::

        // GET: INDEX
        public ActionResult CustoHorasTrabalhadas()
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
                                    sFormulario: "rel_funcionario_horas_trabalhadas",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", 1);
                ViewBag.data_inicio = System.TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString();
                ViewBag.data_termino = System.DateTime.Now.Date.ToShortDateString();
                ViewBag.meses = oRelatorio.Meses(sDataInicio: System.DateTime.Now.Date.AddMonths(-1),
                                                    sDataTermino: System.DateTime.Now.Date);
                return View(oRelatorio.CustoHorasTrabalhadas(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                iCodigoModulo: Convert.ToInt16(Session["codigo_modulo"].ToString()),
                                                                sDataInicio: System.DateTime.Now.Date.AddMonths(-1).ToString(),
                                                                sDataTermino: System.DateTime.Now.Date.ToString(),
                                                                iAtivo: 1));
            }
        }

        // POST: /INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustoHorasTrabalhadas(string data_inicio, string data_termino, int unidade = -1, int ativo = -1, string nome_fantasia = "")
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
                                    sFormulario: "rel_funcionario_horas_trabalhadas",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", ativo);
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.meses = oRelatorio.Meses(sDataInicio: Convert.ToDateTime(data_inicio),
                                                    sDataTermino: Convert.ToDateTime(data_termino));
                return View(oRelatorio.CustoHorasTrabalhadas(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: unidade,
                                                                iCodigoModulo: Convert.ToInt16(Session["codigo_modulo"].ToString()),
                                                                sDataInicio: data_inicio,
                                                                sDataTermino: data_termino,
                                                                iAtivo: ativo));
            }
        }

        // POST: /PRINT
        public ActionResult PrintCustoHorasTrabalhadas(string data_inicio, string data_termino, int unidade = -1, string nome_fantasia = "")
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000013.rpt"));

            oCrDatabase = oReportDocument.Database;
            oCrTables = oCrDatabase.Tables;

            foreach (Table crTable in oCrTables)
            {
                oTableLogOnInfo = crTable.LogOnInfo;
                oTableLogOnInfo.ConnectionInfo = oConnectionInfo;
                crTable.ApplyLogOnInfo(oTableLogOnInfo);
            }

            oReportDocument.SetParameterValue("unidade", nome_fantasia);
            oReportDocument.SetParameterValue("usuario", Session["nome"].ToString());
            oReportDocument.SetParameterValue("@codigo_unidade", unidade);
            oReportDocument.SetParameterValue("@codigo_empresa", Convert.ToInt32(Session["empresa"].ToString()));
            oReportDocument.SetParameterValue("@data_inicio", data_inicio);
            oReportDocument.SetParameterValue("@data_termino", data_termino);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000013.pdf");
            return File(stream, "application/pdf;");
        }

        #endregion

        #region ::: FUNCIONÁRIO - HORAS TRABALHADAS :::

        // GET: INDEX
        public ActionResult FuncionarioHorasTrabalhadas()
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
                                    sFormulario: "rel_funcionario_horas_trabalhadas",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", 1);
                ViewBag.agrupado_por_unidade = new SelectList(oCombo.SimNao(), "codigo", "descricao", 1);
                ViewBag.data_inicio = System.DateTime.Now.AddDays(-7).ToShortDateString();
                ViewBag.data_termino = System.DateTime.Now.Date.ToShortDateString();
                ViewBag.meses = oRelatorio.Meses(sDataInicio: System.DateTime.Now.AddDays(-7),
                                                 sDataTermino: System.DateTime.Now.Date);

                return View(oRelatorio.FuncionarioHorasTrabalhadas(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                    iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                    sDataInicio: System.DateTime.Now.AddDays(-7).ToShortDateString(),
                                                                    sDataTermino: System.DateTime.Now.Date.ToShortDateString(),
                                                                    iAgrupadoPorUnidade: 1,
                                                                    iAtivo: 1));
            }
        }

        // POST: /INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FuncionarioHorasTrabalhadas(string data_inicio, string data_termino, int unidade = -1, int ativo = -1, int agrupado_por_unidade = -1, string nome_fantasia = "")
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
                                    sFormulario: "rel_funcionario_horas_trabalhadas",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", ativo);
                ViewBag.agrupado_por_unidade = new SelectList(oCombo.SimNao(), "codigo", "descricao", agrupado_por_unidade);
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.meses = oRelatorio.Meses(sDataInicio: Convert.ToDateTime(data_inicio),
                                                 sDataTermino: Convert.ToDateTime(data_termino));

                return View(oRelatorio.FuncionarioHorasTrabalhadas(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                   iCodigoUnidade: unidade,
                                                                   iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                   sDataInicio: data_inicio,
                                                                   sDataTermino: data_termino,
                                                                   iAgrupadoPorUnidade: agrupado_por_unidade,
                                                                   iAtivo: ativo));
            }
        }

        // POST: /PRINT
        public ActionResult PrintHorasTrabalhadas(string data_inicio, string data_termino, int unidade = -1, string nome_fantasia = "")
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000006.rpt"));

            oCrDatabase = oReportDocument.Database;
            oCrTables = oCrDatabase.Tables;

            foreach (Table crTable in oCrTables)
            {
                oTableLogOnInfo = crTable.LogOnInfo;
                oTableLogOnInfo.ConnectionInfo = oConnectionInfo;
                crTable.ApplyLogOnInfo(oTableLogOnInfo);
            }

            oReportDocument.SetParameterValue("unidade", nome_fantasia);
            oReportDocument.SetParameterValue("usuario", Session["nome"].ToString());
            oReportDocument.SetParameterValue("@codigo_unidade", unidade);
            oReportDocument.SetParameterValue("@codigo_empresa", Convert.ToInt32(Session["empresa"].ToString()));
            oReportDocument.SetParameterValue("@data_inicio", data_inicio);
            oReportDocument.SetParameterValue("@data_termino", data_termino);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000006.pdf");
            return File(stream, "application/pdf;");
        }

        #endregion

        #region ::: FUNCIONÁRIO - HORAS TRABALHADAS NEW :::

        public ActionResult FuncionarioHorasTrabalhadasNew()
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
                                    sFormulario: "rel_manutencao_solicitante",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                        iCodigoModulo: 1), "codigo", "descricao", null);
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", true);
                ViewBag.ano = new SelectList(oCombo.Ano(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", System.DateTime.Now.Year);

                return View();
            }
        }

        [HttpPost]
        public JsonResult LoadFuncionarioHorasTrabalhadas(int ano, int unidade, int ativo, int funcionario)
        {
            return Json(oRelatorio.FuncionarioHorasTrabalhadas(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               iCodigoUnidade: unidade,
                                                               iCodigoFuncionario: funcionario,
                                                               iAtivo: ativo,
                                                               iAno: ano));
        }

        #endregion

        #region ::: FUNCIONÁRIO - OCIOSIDADE :::

        // GET: INDEX
        public ActionResult FuncionarioOciosidade()
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
                                    sFormulario: "rel_funcionario_ociosidade",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", 1);
                ViewBag.data_inicio = System.DateTime.Now.AddDays(-7).ToShortDateString();
                ViewBag.data_termino = System.DateTime.Now.Date.ToShortDateString();
                ViewBag.meses = oRelatorio.Meses(sDataInicio: System.DateTime.Now.AddDays(-7),
                                                    sDataTermino: System.DateTime.Now.Date);
                return View(oRelatorio.FuncionarioOciosidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                iCodigoModulo: Convert.ToInt16(Session["codigo_modulo"].ToString()),
                                                                sDataInicio: System.DateTime.Now.AddDays(-7).ToShortDateString(),
                                                                sDataTermino: System.DateTime.Now.Date.ToString(),
                                                                iAtivo: 1));
            }
        }

        // POST: /INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FuncionarioOciosidade(string data_inicio, string data_termino, int unidade = -1, int ativo = -1, string nome_fantasia = "")
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
                                    sFormulario: "rel_funcionario_ociosidade",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", ativo);
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.meses = oRelatorio.Meses(sDataInicio: Convert.ToDateTime(data_inicio),
                                                    sDataTermino: Convert.ToDateTime(data_termino));
                return View(oRelatorio.FuncionarioOciosidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: unidade,
                                                                iCodigoModulo: Convert.ToInt16(Session["codigo_modulo"].ToString()),
                                                                sDataInicio: data_inicio,
                                                                sDataTermino: data_termino,
                                                                iAtivo: ativo));
            }
        }

        // POST: /PRINT
        public ActionResult PrintOciosidade(string data_inicio, string data_termino, int unidade = -1, string nome_fantasia = "")
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000007.rpt"));

            oCrDatabase = oReportDocument.Database;
            oCrTables = oCrDatabase.Tables;

            foreach (Table crTable in oCrTables)
            {
                oTableLogOnInfo = crTable.LogOnInfo;
                oTableLogOnInfo.ConnectionInfo = oConnectionInfo;
                crTable.ApplyLogOnInfo(oTableLogOnInfo);
            }

            oReportDocument.SetParameterValue("unidade", nome_fantasia);
            oReportDocument.SetParameterValue("usuario", Session["nome"].ToString());
            oReportDocument.SetParameterValue("@codigo_unidade", unidade);
            oReportDocument.SetParameterValue("@codigo_empresa", Convert.ToInt32(Session["empresa"].ToString()));
            oReportDocument.SetParameterValue("@data_inicio", data_inicio);
            oReportDocument.SetParameterValue("@data_termino", data_termino);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000007.pdf");
            return File(stream, "application/pdf;");
        }

        #endregion

        #region ::: GREEN PLANET :::

        // GET: INDEX
        public ActionResult GreenPlanet()
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
                                    sFormulario: "rel_green_planet",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                MODELS.GreenPlanet relatorio = null;
                List<MODELS.GreenPlanet> list_relatorio = new List<MODELS.GreenPlanet>();

                ViewBag.imprimir = imprimir;
                ViewBag.relatorio = relatorio;
                ViewBag.data_inicio = System.DateTime.Now.Date.AddMonths(-1).ToShortDateString();
                ViewBag.data_termino = System.DateTime.Now.Date.ToShortDateString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.agrupado_por = new SelectList(oCombo.AgrupadoPorData(), "codigo", "descricao", null);
                ViewBag.grupo_item_medicao = new SelectList(oCombo.GrupoItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.item_medicao = new SelectList(oCombo.ItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                            iCodigoGrupoItemMedicao: -1), "codigo", "descricao", null);
                ViewBag.forma_calculo = new SelectList(oCombo.FormaCalculoGreenPlanet(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", null);

                return View(list_relatorio);
            }
        }

        // POST: /INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GreenPlanet(string data_inicio, string data_termino, int forma_calculo, int agrupado_por, int grupo_item_medicao, int item_medicao = -1, int unidade = -1, int codigo_unidade = -1)
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
                                    sFormulario: "rel_green_planet",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", (unidade == -1) ? codigo_unidade : unidade);
                ViewBag.agrupado_por = new SelectList(oCombo.AgrupadoPorData(), "codigo", "descricao", agrupado_por);
                ViewBag.grupo_item_medicao = new SelectList(oCombo.GrupoItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                    iCodigoUnidade: (unidade == -1) ? codigo_unidade : unidade), "codigo", "descricao", grupo_item_medicao);
                ViewBag.item_medicao = new SelectList(oCombo.ItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: (unidade == -1) ? codigo_unidade : unidade,
                                                                            iCodigoGrupoItemMedicao: grupo_item_medicao), "codigo", "descricao", item_medicao);
                ViewBag.forma_calculo = new SelectList(oCombo.FormaCalculoGreenPlanet(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", forma_calculo);
                ViewBag.unidade_medida = oRelatorio.ChartGreenPlanetUnidadeMedida(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                    iCodigoUnidade: unidade,
                                                                                    iCodigoGrupoItemMedicao: grupo_item_medicao);

                return View();
            }
        }

        public JsonResult LoadGreenPlanet(string dataInicio, string dataTermino, int formaCalculo, int agrupadoPor, int grupoItemMedicao, int itemMedicao = -1, int unidade = -1, int codigo_unidade = -1)
        {
            return Json(oRelatorio.GreenPlanet(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                               iCodigoUnidade: unidade,
                                               sDataInicio: dataInicio,
                                               sDataTermino: dataTermino,
                                               iAgrupadoPor: agrupadoPor,
                                               iCodigoFormaCalculoGreenPlanet: formaCalculo,
                                               iCodigoGrupoItemMedicao: grupoItemMedicao,
                                               iCodigoItemMedicao: itemMedicao));
        }

        // GET: INDEX
        public ActionResult GreenPlanetLancamento(int codigo_unidade, int codigo_item_medicao, string data_inicio, string data_termino, int codigo_formula_calculo, string erro)
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
                                    sFormulario: "rel_green_planet",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;

                return View(oRelatorio.GreenPlanetLancamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: codigo_unidade,
                                                                iCodigoItemMedicao: codigo_item_medicao,
                                                                sDataInicio: data_inicio,
                                                                sDataTermino: data_termino,
                                                                iCodigoFormaCalculoGreenPlanet: codigo_formula_calculo,
                                                                sErro: erro));
            }
        }

        // POST: /PRINT
        public ActionResult PrintGreenPlanet(string dataInicio, string dataTermino, int formaCalculo, int agrupadoPor, int grupoItemMedicao, int itemMedicao = -1, int unidade = -1, string nomeFantasia = "")
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000040.rpt"));

            oCrDatabase = oReportDocument.Database;
            oCrTables = oCrDatabase.Tables;

            foreach (Table crTable in oCrTables)
            {
                oTableLogOnInfo = crTable.LogOnInfo;
                oTableLogOnInfo.ConnectionInfo = oConnectionInfo;
                crTable.ApplyLogOnInfo(oTableLogOnInfo);
            }

            oReportDocument.SetParameterValue("unidade", nomeFantasia);
            oReportDocument.SetParameterValue("usuario", Session["nome"].ToString());
            oReportDocument.SetParameterValue("@codigo_unidade", unidade);
            oReportDocument.SetParameterValue("@codigo_empresa", Convert.ToInt32(Session["empresa"].ToString()));
            oReportDocument.SetParameterValue("@agrupado_por", agrupadoPor);
            oReportDocument.SetParameterValue("@codigo_forma_calculo_green_planet", formaCalculo);
            oReportDocument.SetParameterValue("@codigo_grupo_item_medicao", grupoItemMedicao);
            oReportDocument.SetParameterValue("@codigo_item_medicao", itemMedicao);
            oReportDocument.SetParameterValue("@data_inicio", dataInicio);
            oReportDocument.SetParameterValue("@data_termino", dataTermino);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000011.pdf");
            return File(stream, "application/pdf;");
        }

        // POST: INDEX
        public string GreenPlanetUpdate(int unidade, string data, int codigoItemMedicao, int ocupacaoUH, int quantidadeHospede, string valor)
        {

            //Insere Registro no Banco de Dados
            oGreenPlanet.InsertMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        iCodigoUnidade: unidade,
                                        iCodigoItemMedicao: codigoItemMedicao,
                                        sData: data,
                                        dValor: Convert.ToDouble(valor),
                                        iQuantidadeHospede: quantidadeHospede,
                                        iOcupacaoQuartos: ocupacaoUH);

            return "Registro atualizado com sucesso";
        }

        // POST: /LOAD FOTO
        public JsonResult LoadFotoGreenPlanet(int codigo_unidade, string data)
        {

            return Json(oPicture.PictureList(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: codigo_unidade,
                                            sTipo: "GREENPLANET",
                                            sData: data));
        }

        #endregion

        #region ::: MANUTENÇÃO ABERTO x CONCLUÍDO :::

        // GET: INDEX
        public ActionResult ManutencaoAbertoConcluido()
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
                                    sFormulario: "rel_manutencao_aberto_concluido",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ManutencaoAbertoConcluido relatorio = null;
                List<ManutencaoAbertoConcluido> list_relatorio = null;

                list_relatorio = oRelatorio.ManutencaoAbertoConcluido(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                        sDataInicio: System.DateTime.Now.Date.AddMonths(-1).ToShortDateString(),
                                                                        sDataTermino: System.DateTime.Now.Date.ToShortDateString(),
                                                                        oRelatorioTotal: ref relatorio);


                ViewBag.imprimir = imprimir;
                ViewBag.relatorio = relatorio;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.data_inicio = System.DateTime.Now.Date.AddMonths(-1).ToShortDateString();
                ViewBag.data_termino = System.DateTime.Now.Date.ToShortDateString();

                return View(list_relatorio);
            }
        }

        // POST: /INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManutencaoAbertoConcluido(string data_inicio, string data_termino, int unidade = -1)
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
                                    sFormulario: "rel_manutencao_aberto_concluido",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ManutencaoAbertoConcluido relatorio = null;
                List<ManutencaoAbertoConcluido> list_relatorio = null;

                list_relatorio = oRelatorio.ManutencaoAbertoConcluido(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: unidade,
                                                                        sDataInicio: data_inicio,
                                                                        sDataTermino: data_termino,
                                                                        oRelatorioTotal: ref relatorio);


                ViewBag.imprimir = imprimir;
                ViewBag.relatorio = relatorio;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;

                return View(list_relatorio);
            }
        }

        // POST: /PRINT
        public ActionResult PrintManutencaoAbertoConcluido(string data_inicio, string data_termino, int unidade = -1, string nome_fantasia = "")
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000010.rpt"));

            oCrDatabase = oReportDocument.Database;
            oCrTables = oCrDatabase.Tables;

            foreach (Table crTable in oCrTables)
            {
                oTableLogOnInfo = crTable.LogOnInfo;
                oTableLogOnInfo.ConnectionInfo = oConnectionInfo;
                crTable.ApplyLogOnInfo(oTableLogOnInfo);
            }

            oReportDocument.SetParameterValue("unidade", nome_fantasia);
            oReportDocument.SetParameterValue("usuario", Session["nome"].ToString());
            oReportDocument.SetParameterValue("@codigo_unidade", unidade);
            oReportDocument.SetParameterValue("@codigo_empresa", Convert.ToInt32(Session["empresa"].ToString()));
            oReportDocument.SetParameterValue("@data_inicio", data_inicio);
            oReportDocument.SetParameterValue("@data_termino", data_termino);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000010.pdf");
            return File(stream, "application/pdf;");
        }

        #endregion

        #region ::: MANUTENÇÃO CATEGORIA :::

        // GET: INDEX
        public ActionResult ManutencaoCategoria()
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
                                    sFormulario: "rel_manutencao_categoria",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.data_inicio = System.TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString();
                ViewBag.data_termino = System.DateTime.Now.Date.ToShortDateString();
                ViewBag.meses = oRelatorio.Meses(sDataInicio: System.DateTime.Now.Date.AddMonths(-1),
                                                    sDataTermino: System.DateTime.Now.Date);
                return View(oRelatorio.ManutencaoCategoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                            sDataInicio: System.DateTime.Now.Date.AddMonths(-1).ToString(),
                                                            sDataTermino: System.DateTime.Now.Date.ToString()));
            }
        }

        // POST: /INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManutencaoCategoria(string data_inicio, string data_termino, int unidade = -1, string chart = "")
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
                                    sFormulario: "rel_manutencao_categoria",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.meses = oRelatorio.Meses(sDataInicio: Convert.ToDateTime(data_inicio),
                                                    sDataTermino: Convert.ToDateTime(data_termino));
                return View(oRelatorio.ManutencaoCategoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            sDataInicio: data_inicio,
                                                            sDataTermino: data_termino));
            }
        }

        // POST: /PRINT
        public ActionResult PrintManutencaoCategoria(string data_inicio, string data_termino, int unidade = -1, string nome_fantasia = "", string chart = "")
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000001.rpt"));

            oCrDatabase = oReportDocument.Database;
            oCrTables = oCrDatabase.Tables;

            foreach (Table crTable in oCrTables)
            {
                oTableLogOnInfo = crTable.LogOnInfo;
                oTableLogOnInfo.ConnectionInfo = oConnectionInfo;
                crTable.ApplyLogOnInfo(oTableLogOnInfo);
            }

            oReportDocument.SetParameterValue("unidade", nome_fantasia);
            oReportDocument.SetParameterValue("usuario", Session["nome"].ToString());
            oReportDocument.SetParameterValue("@chart", chart);
            oReportDocument.SetParameterValue("@codigo_unidade", unidade);
            oReportDocument.SetParameterValue("@codigo_empresa", Convert.ToInt32(Session["empresa"].ToString()));
            oReportDocument.SetParameterValue("@data_inicio", data_inicio);
            oReportDocument.SetParameterValue("@data_termino", data_termino);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000001.pdf");
            return File(stream, "application/pdf;");
        }

        #endregion

        #region ::: MANUTENÇÃO EQUIPAMENTO :::

        // GET: INDEX
        public ActionResult ManutencaoEquipamento()
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
                                    sFormulario: "rel_manutencao_equipamento",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.data_inicio = System.TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString();
                ViewBag.data_termino = System.DateTime.Now.Date.ToShortDateString();
                ViewBag.meses = oRelatorio.Meses(sDataInicio: System.DateTime.Now.Date.AddMonths(-1),
                                                    sDataTermino: System.DateTime.Now.Date);
                return View(oRelatorio.ManutencaoEquipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                sDataInicio: System.DateTime.Now.Date.AddMonths(-1).ToString(),
                                                                sDataTermino: System.DateTime.Now.Date.ToString()));
            }
        }

        // POST: /INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManutencaoEquipamento(string data_inicio, string data_termino, int unidade = -1, string chart = "")
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
                                    sFormulario: "rel_manutencao_equipamento",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.meses = oRelatorio.Meses(sDataInicio: Convert.ToDateTime(data_inicio),
                                                    sDataTermino: Convert.ToDateTime(data_termino));
                return View(oRelatorio.ManutencaoEquipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: unidade,
                                                                sDataInicio: data_inicio,
                                                                sDataTermino: data_termino));
            }
        }

        // POST: /PRINT
        public ActionResult PrintManutencaoEquipamento(int ano, int unidade = -1, string nome_fantasia = "")
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000002.rpt"));

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
            oReportDocument.SetParameterValue("@ano", ano);
            oReportDocument.SetParameterValue("unidade", nome_fantasia);
            oReportDocument.SetParameterValue("usuario", Session["nome"].ToString());

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000002.pdf");
            return File(stream, "application/pdf;");
        }

        #endregion

        #region ::: MANUTENÇÃO EXECUTOR :::

        // GET: INDEX
        public ActionResult ManutencaoExecutor()
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
                                    sFormulario: "rel_manutencao_executor",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ManutencaoExecutor relatorio = null;
                List<ManutencaoExecutor> list_relatorio = null;

                list_relatorio = oRelatorio.ManutencaoExecutor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                iAno: System.DateTime.Now.Year,
                                                                oRelatorioTotal: ref relatorio);


                ViewBag.imprimir = imprimir;
                ViewBag.relatorio = relatorio;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.ano = new SelectList(oCombo.Ano(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", System.DateTime.Now.Year);

                return View(list_relatorio);
            }
        }

        // POST: /INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManutencaoExecutor(int ano, int unidade = -1)
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
                                    sFormulario: "rel_manutencao_executor",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ManutencaoExecutor relatorio = null;
                List<ManutencaoExecutor> list_relatorio = null;

                list_relatorio = oRelatorio.ManutencaoExecutor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: unidade,
                                                                iAno: ano,
                                                                oRelatorioTotal: ref relatorio);


                ViewBag.imprimir = imprimir;
                ViewBag.relatorio = relatorio;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.ano = new SelectList(oCombo.Ano(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", ano);

                return View(list_relatorio);
            }
        }

        // POST: /PRINT
        public ActionResult PrintManutencaoExecutor(int ano, int unidade = -1, string nome_fantasia = "")
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000003.rpt"));

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
            oReportDocument.SetParameterValue("@ano", ano);
            oReportDocument.SetParameterValue("unidade", nome_fantasia);
            oReportDocument.SetParameterValue("usuario", Session["nome"].ToString());

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000003.pdf");
            return File(stream, "application/pdf;");
        }

        #endregion

        #region ::: MANUTENÇÃO SETOR :::

        // GET: INDEX
        public ActionResult ManutencaoSetor()
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
                                    sFormulario: "rel_manutencao_setor",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ManutencaoSetor relatorio = null;
                List<ManutencaoSetor> list_relatorio = null;

                list_relatorio = oRelatorio.ManutencaoSetor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                            iAno: System.DateTime.Now.Year,
                                                            oRelatorioTotal: ref relatorio);


                ViewBag.imprimir = imprimir;
                ViewBag.relatorio = relatorio;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.ano = new SelectList(oCombo.Ano(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", System.DateTime.Now.Year);

                return View(list_relatorio);
            }
        }

        // POST: /INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManutencaoSetor(int ano, int unidade = -1)
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
                                    sFormulario: "rel_manutencao_setor",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ManutencaoSetor relatorio = null;
                List<ManutencaoSetor> list_relatorio = null;

                list_relatorio = oRelatorio.ManutencaoSetor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade,
                                                            iAno: ano,
                                                            oRelatorioTotal: ref relatorio);


                ViewBag.imprimir = imprimir;
                ViewBag.relatorio = relatorio;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.ano = new SelectList(oCombo.Ano(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", ano);

                return View(list_relatorio);
            }
        }

        // POST: /PRINT
        public ActionResult PrintManutencaoSetor(int ano, int unidade = -1, string nome_fantasia = "")
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000004.rpt"));

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
            oReportDocument.SetParameterValue("@ano", ano);
            oReportDocument.SetParameterValue("unidade", nome_fantasia);
            oReportDocument.SetParameterValue("usuario", Session["nome"].ToString());

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000004.pdf");
            return File(stream, "application/pdf;");
        }

        #endregion

        #region ::: MANUTENÇÃO SOLICITANTE :::

        public ActionResult ManutencaoSolicitante()
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
                                    sFormulario: "rel_manutencao_solicitante",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.departamento = new SelectList(oCombo.Departamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                ViewBag.ano = new SelectList(oCombo.Ano(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", System.DateTime.Now.Year);

                return View();
            }
        }

        [HttpPost]
        public JsonResult LoadManutencaoSolicitante(int ano, int unidade, int departamento)
        {
            return Json(oRelatorio.ManutencaoSolicitante(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                         iCodigoUnidade: unidade,
                                                         iCodigoDepartamento: departamento,
                                                         iAno: ano));
        }

        public ActionResult PrintManutencaoSolicitante(int ano, int unidade = -1, string nome_fantasia = "", int departamento = -1)
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000005.rpt"));

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
            oReportDocument.SetParameterValue("@ano", ano);
            oReportDocument.SetParameterValue("unidade", nome_fantasia);
            oReportDocument.SetParameterValue("usuario", Session["nome"].ToString());

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000005.pdf");
            return File(stream, "application/pdf;");
        }

        #endregion

        #region ::: MANUTENÇÃO :::

        public ActionResult Manutencao()
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
                                    sFormulario: "rel_manutencao",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.tipo = new SelectList(oCombo.TipoFiltroManutencao(), "codigo", "descricao", null);
                ViewBag.ano = new SelectList(oCombo.Ano(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", System.DateTime.Now.Year);

                return View();
            }
        }

        [HttpPost]
        public JsonResult LoadManutencao(int ano, int unidade, string tipo)
        {
            return Json(oRelatorio.LoadManutencao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                  iCodigoUnidade: unidade,
                                                  sTipo: tipo,
                                                  iAno: ano));
        }

        #endregion

        #region ::: MANUTENÇÃO TIPO ORDEM SERVIÇO :::

        // GET: INDEX
        public ActionResult ManutencaoTipoOrdemServico()
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
                                    sFormulario: "rel_manutencao_tipo_ordem_servico",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ManutencaoTipoOrdemServico relatorio = null;
                List<ManutencaoTipoOrdemServico> list_relatorio = null;

                list_relatorio = oRelatorio.ManutencaoTipoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                        iAno: System.DateTime.Now.Year,
                                                                        oRelatorioTotal: ref relatorio);


                ViewBag.imprimir = imprimir;
                ViewBag.relatorio = relatorio;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.ano = new SelectList(oCombo.Ano(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", System.DateTime.Now.Year);

                return View(list_relatorio);
            }
        }

        // POST: /INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManutencaoTipoOrdemServico(int ano, int unidade = -1)
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
                                    sFormulario: "rel_manutencao_tipo_ordem_servico",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ManutencaoTipoOrdemServico relatorio = null;
                List<ManutencaoTipoOrdemServico> list_relatorio = null;

                list_relatorio = oRelatorio.ManutencaoTipoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: unidade,
                                                                        iAno: ano,
                                                                        oRelatorioTotal: ref relatorio);


                ViewBag.imprimir = imprimir;
                ViewBag.relatorio = relatorio;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.ano = new SelectList(oCombo.Ano(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", ano);

                return View(list_relatorio);
            }
        }

        // POST: /PRINT
        public ActionResult PrintManutencaoTipoOrdemServico(int ano, int unidade = -1, string nome_fantasia = "")
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000008.rpt"));

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
            oReportDocument.SetParameterValue("@ano", ano);
            oReportDocument.SetParameterValue("unidade", nome_fantasia);
            oReportDocument.SetParameterValue("usuario", Session["nome"].ToString());

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000008.pdf");
            return File(stream, "application/pdf;");
        }

        #endregion

        #region ::: MANUTENÇÃO TEMPO MÉDIO ATENDIMENTO :::

        // GET: INDEX
        public ActionResult ManutencaoTempoMedioAtendimento()
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
                                    sFormulario: "rel_manutencao_tempo_medio_atendimento",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                List<ManutencaoTempoMedioAtendimento> list_relatorio = null;

                list_relatorio = oRelatorio.ManutencaoTempoMedioAtendimento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                            iAno: System.DateTime.Now.Year);


                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.ano = new SelectList(oCombo.Ano(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", System.DateTime.Now.Year);

                return View(list_relatorio);
            }
        }

        // POST: /INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManutencaoTempoMedioAtendimento(int ano, int unidade = -1)
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
                                    sFormulario: "rel_manutencao_tempo_medio_atendimento",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                List<ManutencaoTempoMedioAtendimento> list_relatorio = null;

                list_relatorio = oRelatorio.ManutencaoTempoMedioAtendimento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: unidade,
                                                                            iAno: ano);


                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.ano = new SelectList(oCombo.Ano(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", ano);

                return View(list_relatorio);
            }
        }

        // POST: /PRINT
        public ActionResult PrintManutencaoTempoMedioAtendimento(int ano, int unidade = -1, string nome_fantasia = "")
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000009.rpt"));

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
            oReportDocument.SetParameterValue("@ano", ano);
            oReportDocument.SetParameterValue("unidade", nome_fantasia);
            oReportDocument.SetParameterValue("usuario", Session["nome"].ToString());

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000009.pdf");
            return File(stream, "application/pdf;");
        }

        #endregion

        #region ::: PMOC - MÊS :::

        // GET: INDEX
        public ActionResult PMOCMes()
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
                                    sFormulario: "rel_pmoc_mes",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.unidade_pmoc = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                     iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                     bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());

                ViewBag.data_pmoc = new SelectList(oCombo.DataPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                   iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                List<PMOCAno> list_relatorio = null;

                list_relatorio = oRelatorio.PMOCAno(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()));


                ViewBag.imprimir = imprimir;

                return View(list_relatorio);
            }
        }

        // POST: /PRINT
        public ActionResult PrintPMOCMes(int unidade, string mes, string ano, string report)
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), String.Concat(report, ".rpt")));

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
            oReportDocument.SetParameterValue("@mes", mes);
            oReportDocument.SetParameterValue("@ano", ano);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=" + report + ".pdf");
            return File(stream, "application/pdf;");
        }

        // POST: /PRINT
        [HttpPost]
        public string RefreshReport(int unidade, string data)
        {
            return oRelatorio.PMOCRefresh(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                          iCodigoUnidade: unidade,
                                          sData: data);
        }

        #endregion

        #region ::: PMOC - AGRUPADO :::

        // GET: INDEX
        public ActionResult PMOCAgrupado()
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
                                    sFormulario: "rel_pmoc_bimestre",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.ano = new SelectList(oCombo.AnoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", DateTime.Now.Year);
                ViewBag.tipo = new SelectList(oCombo.TipoPMOC(), "codigo", "descricao", "BIMESTRAL");



                List<PMOCAno> list_relatorio = null;

                list_relatorio = oRelatorio.LoadPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                     iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                     iAno: DateTime.Now.Year,
                                                     sTipo: "BIMESTRAL");


                ViewBag.imprimir = imprimir;

                return View(list_relatorio);
            }
        }

        // GET: INDEX
        [HttpPost]
        public ActionResult PMOCAgrupado(string tipo, int unidade = -1, int ano = -1)
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
                                    sFormulario: "rel_pmoc_bimestre",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", unidade);
                ViewBag.ano = new SelectList(oCombo.AnoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: unidade), "codigo", "descricao", ano);
                ViewBag.tipo = new SelectList(oCombo.TipoPMOC(), "codigo", "descricao", tipo);



                List<PMOCAno> list_relatorio = null;

                list_relatorio = oRelatorio.LoadPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                     iCodigoUnidade: unidade,
                                                     iAno: ano,
                                                     sTipo: tipo);


                return View(list_relatorio);
            }
        }

        #endregion

        #region ::: PMOC - BIMESTRAL :::

        // GET: INDEX
        public ActionResult PMOCBimestral()
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
                                    sFormulario: "rel_pmoc_bimestre",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.ano = new SelectList(oCombo.AnoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.tipo = new SelectList(oCombo.TipoPMOC(), "codigo", "descricao", null);



                List<PMOCAno> list_relatorio = null;

                list_relatorio = oRelatorio.PMOCAno(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()));


                ViewBag.imprimir = imprimir;

                return View(list_relatorio);
            }
        }

        // POST: /PRINT
        public ActionResult PrintPMOCBimestre(int unidade, string startDate, string endDate, string report)
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), String.Concat(report, ".rpt")));

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
            oReportDocument.SetParameterValue("@start_date", startDate);
            oReportDocument.SetParameterValue("@end_date", endDate);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=" + report + ".pdf");
            return File(stream, "application/pdf;");
        }

        // POST: /PRINT
        public ActionResult PrintPMOCAno(int unidade, int ano, string report)
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), String.Concat(report, ".rpt")));

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
            oReportDocument.SetParameterValue("@ano", ano);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=" + report + ".pdf");
            return File(stream, "application/pdf;");
        }

        #endregion

        #region ::: Preventiva - MÊS :::

        // GET: INDEX
        public ActionResult PreventivaMes()
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
                                    sFormulario: "rel_preventiva_mes",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                List<PreventivaMes> list_relatorio = oRelatorio.PreventivaMes(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                              iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()));


                ViewBag.imprimir = imprimir;

                return View(list_relatorio);
            }
        }

        // POST: /PRINT
        public ActionResult PrintPreventivaMes(int unidade, int ano, int mes, string report)
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), String.Concat(report, ".rpt")));

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
            oReportDocument.SetParameterValue("@ano", ano);
            oReportDocument.SetParameterValue("@mes", mes);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=" + report + ".pdf");
            return File(stream, "application/pdf;");
        }

        #endregion

        #region ::: LOG BOOK :::

        // GET: INDEX
        public ActionResult LogBookReport()
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
                                    sFormulario: "rel_log_book",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                List<RelatorioLogBook> relatorio = null;

                string data_inicio = DateTime.Now.AddDays(-1).ToShortDateString();
                string data_termino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();

                relatorio = oRelatorio.LogBook(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                               iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                               sDataInicio: data_inicio,
                                               sDataTermino: data_termino);


                ViewBag.imprimir = imprimir;
                ViewBag.relatorio = relatorio;
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.data_inicio = System.DateTime.Now.Date.AddMonths(-1).ToShortDateString();
                ViewBag.data_termino = System.DateTime.Now.Date.ToShortDateString();

                return View(relatorio);
            }
        }

        // POST: /INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogBookReport(string data_inicio, string data_termino, int unidade = -1)
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
                                    sFormulario: "rel_log_book",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                List<RelatorioLogBook> relatorio = null;

                relatorio = oRelatorio.LogBook(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                               iCodigoUnidade: unidade,
                                               sDataInicio: data_inicio,
                                               sDataTermino: data_termino);


                ViewBag.imprimir = imprimir;
                ViewBag.relatorio = relatorio;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;

                return View(relatorio);
            }
        }

        // GET: /PRINT
        public ActionResult PrintLogBook(string data_inicio, string data_termino, int unidade = -1, string nome_fantasia = "")
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000011.rpt"));

            oCrDatabase = oReportDocument.Database;
            oCrTables = oCrDatabase.Tables;

            foreach (Table crTable in oCrTables)
            {
                oTableLogOnInfo = crTable.LogOnInfo;
                oTableLogOnInfo.ConnectionInfo = oConnectionInfo;
                crTable.ApplyLogOnInfo(oTableLogOnInfo);
            }

            oReportDocument.SetParameterValue("unidade", nome_fantasia);
            oReportDocument.SetParameterValue("usuario", Session["nome"].ToString());
            oReportDocument.SetParameterValue("@codigo_unidade", unidade);
            oReportDocument.SetParameterValue("@codigo_empresa", Convert.ToInt32(Session["empresa"].ToString()));
            oReportDocument.SetParameterValue("@data_inicio", data_inicio);
            oReportDocument.SetParameterValue("@data_termino", data_termino);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000011.pdf");
            return File(stream, "application/pdf;");
        }

        #endregion

        #region ::: NÃO CONFORMIDADE :::

        // GET: INDEX
        public ActionResult NaoConformidade()
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
                                    sFormulario: "rel_nao_conformidade",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.data = System.DateTime.Now.Date.ToShortDateString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.tipo = new SelectList(oCombo.TipoProgramada(), "codigo", "descricao", null);
                ViewBag.manutencao_programada = new SelectList(oCombo.Programada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                 iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                 iCodigoTipoOrdemServico: -1), "codigo", "descricao", null);
                ViewBag.meses = oRelatorio.Meses(sDataInicio: System.TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-11),
                                                 sDataTermino: System.DateTime.Now.Date);
                return View(oRelatorio.NaoConformidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                       iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                       iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                       iCodigoTipo: -1,
                                                       lCodigoManutencaoProgramada: -1,
                                                       sData: System.DateTime.Now.Date.ToShortDateString()));

            }
        }

        // POST: /INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NaoConformidade(string data, int unidade = -1, int tipo = -1, long manutencao_programada = -1)
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
                                    sFormulario: "rel_nao_conformidade",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.data = data;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", unidade);
                ViewBag.tipo = new SelectList(oCombo.TipoProgramada(), "codigo", "descricao", tipo);
                ViewBag.manutencao_programada = new SelectList(oCombo.Programada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                 iCodigoUnidade: unidade,
                                                                                 iCodigoTipoOrdemServico: tipo), "codigo", "descricao", manutencao_programada);
                ViewBag.meses = oRelatorio.Meses(sDataInicio: Convert.ToDateTime(data).AddMonths(-11),
                                                 sDataTermino: Convert.ToDateTime(data));
                return View(oRelatorio.NaoConformidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                       iCodigoUnidade: unidade,
                                                       iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                       iCodigoTipo: tipo,
                                                       lCodigoManutencaoProgramada: manutencao_programada,
                                                       sData: data));

            }
        }

        #endregion


        #region ::: RELATÓRIO DINÂMICO :::

        // GET: RELATÓRIO
        public ActionResult RelatorioDinamico()
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
                                    sFormulario: "rel_dinamico",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                string data_inicio = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString();
                string data_termino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                string[] checklist = new string[] { "0|0" };

                ViewBag.imprimir = imprimir;
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.relatorio_itens_auditaveis = new SelectList(oCombo.RelatorioItensAuditaveis(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                                    iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString())), "codigo", "descricao", null);
                ViewBag.informacao = oRelatorio.RelatorioDinamico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                  iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                  iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                  lCodigoRelatorioItensAuditavies: -1);

                return View(oRelatorio.RelatorioDinamicoData(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                             iCodigoUnidade: -1,
                                                             iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                             lCodigoRelatorioItensAuditaveis: -1,
                                                             sDataInicio: data_inicio,
                                                             sDataTermino: data_termino));

            }
        }

        // POST: RELATÓRIO
        [HttpPost]
        public ActionResult RelatorioDinamico(string data_inicio, string data_termino, long relatorio_itens_auditaveis, int unidade = -1)
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
                                    sFormulario: "rel_dinamico",
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
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.relatorio_itens_auditaveis = new SelectList(oCombo.RelatorioItensAuditaveis(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                    iCodigoUnidade: unidade,
                                                                                                    iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString())), "codigo", "descricao", relatorio_itens_auditaveis);

                ViewBag.informacao = oRelatorio.RelatorioDinamico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                  iCodigoUnidade: unidade,
                                                                  iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                  lCodigoRelatorioItensAuditavies: relatorio_itens_auditaveis);

                return View(oRelatorio.RelatorioDinamicoData(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                             iCodigoUnidade: unidade,
                                                             iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                             lCodigoRelatorioItensAuditaveis: relatorio_itens_auditaveis,
                                                             sDataInicio: data_inicio,
                                                             sDataTermino: data_termino));
            }
        }

        #endregion

        #region ::: RELATÓRIO DINÂMICO DIA :::

        // GET: INDEX
        public ActionResult RelatorioDinamicoDia()
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
                                    sFormulario: "rel_dinamico",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.data = new SelectList(oCombo.DataProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.relatorio_itens_auditaveis = new SelectList(oCombo.RelatorioItensAuditaveis(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                                    iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString())), "codigo", "descricao", null);

                return View();
            }
        }

        // POST: /INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RelatorioDinamicoDia(string data, long relatorio_itens_auditaveis, int unidade = -1)
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
                                    sFormulario: "rel_dinamico",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.data = new SelectList(oCombo.DataProgramada(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: unidade), "codigo", "descricao", data);
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.relatorio_itens_auditaveis = new SelectList(oCombo.RelatorioItensAuditaveis(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                    iCodigoUnidade: unidade,
                                                                                                    iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString())), "codigo", "descricao", relatorio_itens_auditaveis);

                return View();
            }
        }

        public JsonResult LoadRelatorioDinamicoDia(string data, long relatorio_itens_auditaveis, int unidade = -1)
        {
            return Json(oRelatorio.RelatorioDinamicoDia(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: unidade,
                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                        sData: data,
                                                        lCodigoRelatorioItensAuditavies: relatorio_itens_auditaveis));

        }

        // POST: /PRINT
        public ActionResult PrintRelatorioDinamicoDia(string data, long relatorio_itens_auditaveis, int unidade = -1)
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000039.rpt"));

            oCrDatabase = oReportDocument.Database;
            oCrTables = oCrDatabase.Tables;

            foreach (Table crTable in oCrTables)
            {
                oTableLogOnInfo = crTable.LogOnInfo;
                oTableLogOnInfo.ConnectionInfo = oConnectionInfo;
                crTable.ApplyLogOnInfo(oTableLogOnInfo);
            }

            oReportDocument.SetParameterValue("@codigo_empresa", Convert.ToInt32(Session["empresa"].ToString()));
            oReportDocument.SetParameterValue("@codigo_unidade", unidade);
            oReportDocument.SetParameterValue("@codigo_modulo", Convert.ToInt32(Session["codigo_modulo"].ToString()));
            oReportDocument.SetParameterValue("@codigo_relatorio_itens_auditaveis", relatorio_itens_auditaveis);
            oReportDocument.SetParameterValue("@ano", Convert.ToDateTime(data).Year);
            oReportDocument.SetParameterValue("@ano", Convert.ToDateTime(data).Year);
            oReportDocument.SetParameterValue("@mes", Convert.ToDateTime(data).Month);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000039.pdf");
            return File(stream, "application/pdf;");
        }

        #endregion

    }
}