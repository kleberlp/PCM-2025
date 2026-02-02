using Microsoft.AspNet.Identity;
using NPOI.HSSF.Model;
using PCM.WEB.DAL;
using PCM.WEB.MODELS;
using PCM.WEB.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Google.Apis.Requests.BatchRequest;

namespace PCM.WEB.Controllers
{
    public class GovernancaController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Governanca oGovernanca = new DAL.Governanca(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Relatorio oRelatorio = new DAL.Relatorio(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: JSON :::

        //JSON: /UNIDADE/
        public JsonResult LoadUnidade()
        {
            return Json(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                       iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                       bCadastro: false));
        }

        //JSON: /UNIDADE/
        public JsonResult LoadComboFuncionario(int unidade)
        {
            return Json(oCombo.FuncionarioGovernanca(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                     iCodigoUnidade: unidade));
        }

        //JSON: /UNIDADE/
        public JsonResult LoadComboCamareira(int unidade)
        {
            return Json(oCombo.FuncionarioGovernancaCamareira(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                              iCodigoUnidade: unidade));
        }

        //JSON: /DATA GOVERNANÇA/
        public JsonResult LoadDataGovernanca(int unidade)
        {
            return Json(oCombo.DataGovernanca(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              iCodigoUnidade: unidade));
        }

        //JSON: /UNIDADE/
        public JsonResult LoadComboVistoriador(int unidade)
        {
            return Json(oCombo.FuncionarioGovernancaVistoriador(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: unidade));
        }

        //JSON: /BLOCO/
        public JsonResult LoadComboBloco(int unidade)
        {
            return Json(oCombo.Bloco(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                     iCodigoUnidade: unidade));
        }

        //JSON: /ANDAR/
        public JsonResult LoadComboAndar(int unidade)
        {
            return Json(oCombo.Andar(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                     iCodigoUnidade: unidade));
        }

        //JSON: /ANDAR/
        public JsonResult LoadComboApartamento(int unidade)
        {
            return Json(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                           iCodigoUnidade: unidade));
        }

        //JSON: /FRONT OFFICE/
        public JsonResult LoadComboStatusFrontOffice(int unidade)
        {
            return Json(oCombo.StatusFrontOffice(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                 iCodigoUnidade: unidade));
        }

        //JSON: /ROOM/
        public JsonResult LoadComboStatusRoom(int unidade)
        {
            return Json(oCombo.StatusRoom(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                          iCodigoUnidade: unidade));
        }

        //JSON: /ANDAR/
        public JsonResult LoadComboTipoPerdaEnxoval(int unidade)
        {
            return Json(oCombo.LoadCombo(storedProcedure: "sp_select_combo_cadastro_basico_tipo_perda_enxoval",
                                         codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                         codigoUnidade: unidade));
        }

        #endregion

        #region ::: GOVERNANÇA :::

        // GET: CHECKLIST
        public ActionResult ChecklistGovernanca(int status = -1, int tipoGovernanca = 2, int funcionario = -1, int unidade = -2, string frontOfficeStatus = "", string roomStatus = "")
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
                                    sFormulario: "governanca",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                int codigoUnidade = (unidade == -2) ? Convert.ToInt32(Session["codigo_unidade"].ToString()) : unidade;


                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                              iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                              bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.funcionario = new SelectList(oCombo.FuncionarioGovernanca(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                  iCodigoUnidade: codigoUnidade), "codigo", "descricao", funcionario);
                ViewBag.tipoGovernanca = new SelectList(oCombo.TipoGovernanca(), "codigo", "descricao", tipoGovernanca);
                ViewBag.frontOfficeStatus = new SelectList(oCombo.StatusFrontOffice(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                    iCodigoUnidade: codigoUnidade), "codigo", "descricao", frontOfficeStatus);
                ViewBag.roomStatus = new SelectList(oCombo.StatusRoom(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                      iCodigoUnidade: codigoUnidade), "codigo", "descricao", roomStatus);

                GovernancaStatus statusGovernanca = new GovernancaStatus();
                List<MODELS.Governanca> Governanca = new List<MODELS.Governanca>();

                if (tipoGovernanca > -1)
                {

                    statusGovernanca = oGovernanca.LoadGovernancaStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigoUnidade,
                                                                        iCodigoTipoGovernanca: tipoGovernanca);

                    Governanca = oGovernanca.LoadGovernanca(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                           iCodigoUnidade: codigoUnidade,
                                                           iCodigoTipoGovernanca: tipoGovernanca,
                                                           iCodigoFuncionario: funcionario,
                                                           iStatus: status,
                                                           sStatusFrontOffice: frontOfficeStatus,
                                                           sStatusRoom: roomStatus);

                }

                ViewBag.statusFilter = status;
                ViewBag.status = statusGovernanca;

                ViewBag.lastUpdate = oGovernanca.LoadLastUploadStatus(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                      codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()));

                return View(Governanca);

            }
        }

        // GET: APONTAMENTO CHECKLIST
        public ActionResult ChecklistGovernancaApontamento(int codigo_unidade, int codigo_tipo_governanca, int codigo_apartamento, int status, long codigo_vistoria, long codigo_checklist)
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
                                    sFormulario: "governanca",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                //Carrega Dados Apontamento
                GovernancaDados governanca = new GovernancaDados();

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.codigo_unidade = codigo_unidade;
                ViewBag.codigo_apartamento = codigo_apartamento;
                ViewBag.administrador_string = (administrador == true) ? "" : "readonly";
                ViewBag.opcao = new SelectList(oCombo.Opcoes(), "codigo", "descricao", null);
                ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.status = status;
                ViewBag.codigo_tipo_governanca = codigo_tipo_governanca;
                ViewBag.codigo_checklist = codigo_checklist;
                ViewBag.governanca = oGovernanca.LoadGovernancaDados(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                     iCodigoUnidade: codigo_unidade,
                                                                     lCodigoVistoria: codigo_vistoria);
                ViewBag.funcionario = new SelectList(oCombo.FuncionarioGovernancaCamareira(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                           iCodigoUnidade: codigo_unidade), "codigo", "descricao", governanca.codigo_funcionario);
                ViewBag.vistoriador = new SelectList(oCombo.FuncionarioGovernancaVistoriador(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                             iCodigoUnidade: codigo_unidade), "codigo", "descricao", governanca.codigo_funcionario_responsavel_vistoria);
                return View(oGovernanca.LoadGovernancaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                  iCodigoUnidade: codigo_unidade,
                                                                  iCodigoApartamento: codigo_apartamento,
                                                                  iCodigoTipoGovernanca: codigo_tipo_governanca,
                                                                  lCodigoGovernancaApontamento: codigo_vistoria));

            }
        }

        // POST: APONTAMENTO CHECKLIST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChecklistGovernancaApontamento(string data, int codigo_unidade, int codigo_apartamento, int codigo_tipo_governanca, long codigo_checklist, int funcionario, int vistoriador, GovernancaApto apontamento, string hora_inicio = "", string hora_termino = "", int status = -1, bool nao_perturbe = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                long codigo = 0;

                oGovernanca.InsertGovernancaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                        iCodigoUnidade: codigo_unidade,
                                                        iCodigoApartamento: codigo_apartamento,
                                                        iCodigoTipoGovernanca: codigo_tipo_governanca,
                                                        lCodigoChecklist: codigo_checklist,
                                                        iCodigoFuncionario: funcionario,
                                                        iCodigoVistoriador: vistoriador,
                                                        sData: data,
                                                        sHoraInicio: hora_inicio,
                                                        sHoraTermino: hora_termino,
                                                        bNaoPerturbe: nao_perturbe,
                                                        lCodigoGovernancaApontamento: ref codigo);

                if (apontamento != null && !nao_perturbe)
                {
                    if (apontamento.checklist != null)
                    {

                        foreach (GovernancaApontamentoChecklist checklist in apontamento.checklist)
                        {

                            //Insere Registro no Banco de Dados
                            oGovernanca.InsertGovernancaApontamentoChecklist(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                             iCodigoUnidade: codigo_unidade,
                                                                             lCodigoGovernancaApontamento: codigo,
                                                                             lCodigoChecklist: codigo_checklist,
                                                                             iCodigoChecklistItem: checklist.codigo,
                                                                             sResultado: checklist.resultado,
                                                                             sObservacao: checklist.observacao,
                                                                             bNovaVistoria: checklist.nova_vistoria);

                        }
                    }

                    if (apontamento.enxoval != null)
                    {

                        foreach (GovernancaApontamentoEnxoval enxoval in apontamento.enxoval)
                        {

                            //Insere Registro no Banco de Dados
                            oGovernanca.InsertGovernancaApontamentoEnxoval(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                           iCodigoUnidade: codigo_unidade,
                                                                           lCodigoGovernancaApontamento: codigo,
                                                                           iCodigoEnxoval: enxoval.codigo_enxoval,
                                                                           iQuantidade: enxoval.quantidade);
                        }
                    }

                }

            }

            return RedirectToAction("ChecklistGovernanca", new { unidade = codigo_unidade, tipoGovernanca = codigo_tipo_governanca });

        }

        // GET: APONTAMENTO CHECKLIST
        public ActionResult ChecklistGovernancaVistoria(int codigo_unidade, int codigo_tipo_governanca, int codigo_apartamento, int status, long codigo_vistoria, long codigo_checklist)
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
                                    sFormulario: "governanca",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                //Carrega Dados Apontamento
                GovernancaDados governanca = new GovernancaDados();

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.codigo_unidade = codigo_unidade;
                ViewBag.codigo_apartamento = codigo_apartamento;
                ViewBag.codigo_tipo_governanca = codigo_tipo_governanca;
                ViewBag.administrador_string = (administrador == true) ? "" : "readonly";
                ViewBag.opcao = new SelectList(oCombo.Opcoes(), "codigo", "descricao", null);
                ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.codigo = codigo_vistoria;
                ViewBag.funcionario = new SelectList(oCombo.FuncionarioGovernancaVistoriador(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                             iCodigoUnidade: codigo_unidade), "codigo", "descricao", Session["codigo_funcionario"]);
                governanca = oGovernanca.LoadGovernancaDados(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                             iCodigoUnidade: codigo_unidade,
                                                             lCodigoVistoria: codigo_vistoria);
                ViewBag.governanca = governanca;
                ViewBag.camareira = new SelectList(oCombo.FuncionarioGovernancaCamareira(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                         iCodigoUnidade: codigo_unidade), "codigo", "descricao", governanca.codigo_funcionario);
                return View(oGovernanca.LoadGovernancaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                  iCodigoUnidade: codigo_unidade,
                                                                  iCodigoApartamento: codigo_apartamento,
                                                                  iCodigoTipoGovernanca: codigo_tipo_governanca,
                                                                  lCodigoGovernancaApontamento: codigo_vistoria));
            }
        }

        // POST: APONTAMENTO CHECKLIST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChecklistGovernancaVistoria(int codigo_unidade, int funcionario, long codigo, string data, int codigo_apartamento, bool nao_perturbe, int codigo_tipo_governanca, GovernancaApto apontamento, string hora_inicio = "", string hora_termino = "", int camareira = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                oGovernanca.UpdateGovernancaApontamentoVistoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: codigo_unidade,
                                                                iCodigoCamareira: camareira,
                                                                iCodigoVistoriador: funcionario,
                                                                lCodigoApartamento: codigo_apartamento,
                                                                iCodigoTipoGovernanca: codigo_tipo_governanca,
                                                                bNaoPerturbe: nao_perturbe,
                                                                lCodigo: ref codigo,
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.Name.ToString()),
                                                                sData: data,
                                                                sHoraInicio: hora_inicio,
                                                                sHoraTermino: hora_termino);

                if (apontamento.checklist != null)
                {
                    foreach (GovernancaApontamentoChecklist checklist in apontamento.checklist)
                    {

                        //Insere Registro no Banco de Dados
                        oGovernanca.InsertGovernancaApontamentoChecklistVistoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                 iCodigoUnidade: codigo_unidade,
                                                                                 lCodigoGovernancaApontamento: codigo,
                                                                                 iCodigoChecklistItem: checklist.codigo,
                                                                                 sResultado: checklist.resultado,
                                                                                 sObservacao: checklist.observacao);

                    }

                }

                return RedirectToAction("ChecklistGovernanca", new { unidade = codigo_unidade, tipoGovernanca = codigo_tipo_governanca });
            }

        }

        [HttpPost]
        public JsonResult NaoPerturbe(int codigoUnidade, long codigoApartamento, int codigoTipoGovernanca)
        {
            GovernancaApontamentoResponse response = new GovernancaApontamentoResponse();

            try
            {
                // Gerando as datas entre o intervalo
                oGovernanca.NaoPerturbe(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.Name.ToString()),
                                        iCodigoUnidade: codigoUnidade,
                                        lCodigoApartamento: codigoApartamento,
                                        iCodigoTipoGovernanca: codigoTipoGovernanca);

                response.success = true;
                response.message = "";

            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region ::: DASHBOARD :::

        public ActionResult Dashboard(string dataInicio = "", string dataTermino = "")
        {

            dataInicio = (dataInicio == "") ? DateTime.Now.AddDays(-7).ToShortDateString() : dataInicio;
            dataTermino = (dataTermino == "") ? DateTime.Now.ToShortDateString() : dataTermino;

            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.nome_fantasia = Session["unidade"].ToString();

                ViewBag.dashboard_info = oGovernanca.DashboardInfo(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                   codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                   dataInicio: dataInicio,
                                                                   dataTermino: dataTermino);

                ViewBag.ProdutividadeCamareira = oGovernanca.LoadChartProdutividadeCamareira(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                             codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                             dataInicio: dataInicio,
                                                                                             dataTermino: dataTermino);

                ViewBag.ProdutividadeVistoriador = oGovernanca.LoadChartProdutividadeVistoriador(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                 codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                                 dataInicio: dataInicio,
                                                                                                 dataTermino: dataTermino);

                ViewBag.AtendimentoOS = oGovernanca.LoadAtendimentoOrdemServico(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                dataInicio: dataInicio,
                                                                                dataTermino: dataTermino);
                ViewBag.dataInicio = dataInicio;
                ViewBag.dataTermino = dataTermino;

                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dashboard(int unidade, string dataInicio = "", string dataTermino = "")
        {
            if (@Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);

                ViewBag.dashboard_info = oGovernanca.DashboardInfo(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                   codigoUnidade: unidade,
                                                                   dataInicio: dataInicio,
                                                                   dataTermino: dataTermino);

                ViewBag.ProdutividadeCamareira = oGovernanca.LoadChartProdutividadeCamareira(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                             codigoUnidade: unidade,
                                                                                             dataInicio: dataInicio,
                                                                                             dataTermino: dataTermino);

                ViewBag.ProdutividadeVistoriador = oGovernanca.LoadChartProdutividadeVistoriador(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                 codigoUnidade: unidade,
                                                                                                 dataInicio: dataInicio,
                                                                                                 dataTermino: dataTermino);

                ViewBag.AtendimentoOS = oGovernanca.LoadAtendimentoOrdemServico(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                codigoUnidade: unidade,
                                                                                dataInicio: dataInicio,
                                                                                dataTermino: dataTermino);
                ViewBag.nome_fantasia = Session["unidade"].ToString();

                ViewBag.dataInicio = dataInicio;
                ViewBag.dataTermino = dataTermino;

                return View();
            }

        }

        #region ::: CHART :::

        [HttpPost]
        public JsonResult ChartArrumadoxVistoriado(int unidade, string dataInicio, string dataTermino)
        {
            try
            {
                // Convertendo as strings para DateTime
                DateTime startDate = DateTime.ParseExact(dataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(dataTermino, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                // Gerando as datas entre o intervalo
                List<dashboardGovernancaArrumadoxVistoriado> result = oGovernanca.LoadChartArrumadoxVistoriado(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                               codigoUnidade: unidade,
                                                                                                               dataInicio: dataInicio,
                                                                                                               dataTermino: dataTermino);
                var chartData = new
                {
                    labels = result.Select(d => d.unidade).ToArray(),
                    datasets = new[]
                    {
                        new { label = "UH's Pendentes", data = result.Select(d => d.quantidadeUHs).ToArray(), backgroundColor = "red" },
                        new { label = "UH's Vistoriados", data = result.Select(d => d.quantidadeVistoriados).ToArray(), backgroundColor = "green" },
                        new { label = "UH's Arrumados", data = result.Select(d => d.quantidadeArrumados).ToArray(), backgroundColor = "blue" }
                    }
                };

                return Json(chartData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Tratamento de erro para lidar com formatos inválidos ou exceções
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ChartArrumacaoDia(int unidade, string dataInicio, string dataTermino)
        {
            try
            {
                // Convertendo as strings para DateTime
                DateTime startDate = DateTime.ParseExact(dataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(dataTermino, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                // Gerando as datas entre o intervalo
                List<dashboardGovernancaChartArrumacaoDia> result = oGovernanca.LoadChartArrumacaoDia(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                      codigoUnidade: unidade,
                                                                                                      dataInicio: dataInicio,
                                                                                                      dataTermino: dataTermino);
                var uniqueDates = result
                   .Select(d => d.data)
                   .Distinct()
                   .OrderBy(d => DateTime.ParseExact(d, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                   .ToList();

                var uniqueCamareiras = result
                    .Select(d => d.camareira)
                    .Distinct()
                    .ToList();

                var colors = new List<string>
                {
                    "rgba(255, 99, 132, 0.7)",
                    "rgba(54, 162, 235, 0.7)",
                    "rgba(255, 206, 86, 0.7)",
                    "rgba(75, 192, 192, 0.7)",
                    "rgba(153, 102, 255, 0.7)",
                    "rgba(255, 159, 64, 0.7)"
                };

                int colorIndex = 0;
                var datasets = uniqueCamareiras.Select(camareira => new
                {
                    label = camareira,
                    data = uniqueDates.Select(date =>
                        result.FirstOrDefault(r => r.data == date && r.camareira == camareira)?.quantidade
                    ).ToArray(),
                    fill = false,
                    backgroundColor = colors[colorIndex % colors.Count],
                    borderColor = colors[colorIndex++ % colors.Count],
                    tension = 0.1
                }).ToList();

                return Json(new { labels = uniqueDates, datasets = datasets }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                // Tratamento de erro para lidar com formatos inválidos ou exceções
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ChartVistoriaDia(int unidade, string dataInicio, string dataTermino)
        {
            try
            {
                // Convertendo as strings para DateTime
                DateTime startDate = DateTime.ParseExact(dataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(dataTermino, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                // Gerando as datas entre o intervalo
                List<dashboardGovernancaChartVistoriaDia> result = oGovernanca.LoadChartVistoriaDia(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                    codigoUnidade: unidade,
                                                                                                    dataInicio: dataInicio,
                                                                                                    dataTermino: dataTermino);
                var uniqueDates = result
                   .Select(d => d.data)
                   .Distinct()
                   .OrderBy(d => DateTime.ParseExact(d, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                   .ToList();

                var uniqueVistoriador = result
                    .Select(d => d.vistoriador)
                    .Distinct()
                    .ToList();

                var colors = new List<string>
                {
                    "rgba(255, 99, 132, 0.7)",
                    "rgba(54, 162, 235, 0.7)",
                    "rgba(255, 206, 86, 0.7)",
                    "rgba(75, 192, 192, 0.7)",
                    "rgba(153, 102, 255, 0.7)",
                    "rgba(255, 159, 64, 0.7)"
                };

                int colorIndex = 0;
                var datasets = uniqueVistoriador.Select(vistoriador => new
                {
                    label = vistoriador,
                    data = uniqueDates.Select(date =>
                        result.FirstOrDefault(r => r.data == date && r.vistoriador == vistoriador)?.quantidade
                    ).ToArray(),
                    fill = false,
                    backgroundColor = colors[colorIndex % colors.Count],
                    borderColor = colors[colorIndex++ % colors.Count],
                    tension = 0.1
                }).ToList();

                return Json(new { labels = uniqueDates, datasets = datasets }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                // Tratamento de erro para lidar com formatos inválidos ou exceções
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ChartProdutividadeCamareira(int unidade, string dataInicio, string dataTermino)
        {
            try
            {
                // Convertendo as strings para DateTime
                DateTime startDate = DateTime.ParseExact(dataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(dataTermino, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                // Gerando as datas entre o intervalo
                List<dashboardGovernancaChartProdutividade> result = oGovernanca.LoadChartProdutividadeCamareira(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                                 codigoUnidade: unidade,
                                                                                                                 dataInicio: dataInicio,
                                                                                                                 dataTermino: dataTermino);
                var chartData = new
                {
                    labels = result.Select(r => r.unidade).ToArray(), // Eixo X: Unidade
                    datasets = new[]
                    {
                        new {
                            label = "Percentual de Produtividade (%)",
                            data = result.Select(r => decimal.TryParse(r.percentual, out var val) ? val : 0).ToArray(),
                            backgroundColor = "rgba(54, 162, 235, 0.5)",
                            borderColor = "rgba(54, 162, 235, 1)",
                            borderWidth = 1
                        }
                    }
                };
                
                return Json(chartData, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                // Tratamento de erro para lidar com formatos inválidos ou exceções
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ChartProdutividadeVistoriador(int unidade, string dataInicio, string dataTermino)
        {
            try
            {
                // Convertendo as strings para DateTime
                DateTime startDate = DateTime.ParseExact(dataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(dataTermino, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                // Gerando as datas entre o intervalo
                List<dashboardGovernancaChartProdutividade> result = oGovernanca.LoadChartProdutividadeVistoriador(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                                   codigoUnidade: unidade,
                                                                                                                   dataInicio: dataInicio,
                                                                                                                   dataTermino: dataTermino);
                var chartData = new
                {
                    labels = result.Select(r => r.unidade).ToArray(),
                    datasets = new[]
                    {
                        new {
                            label = "Percentual de Produtividade (%)",
                            data = result.Select(r => decimal.TryParse(r.percentual, out var val) ? val : 0).ToArray(),
                            backgroundColor = "rgba(54, 162, 235, 0.5)",
                            borderColor = "rgba(54, 162, 235, 1)",
                            borderWidth = 1
                        }
                    }
                };

                return Json(chartData, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                // Tratamento de erro para lidar com formatos inválidos ou exceções
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ChartNaoConformidadeDia(int unidade, string dataInicio, string dataTermino)
        {
            try
            {
                // Convertendo as strings para DateTime
                DateTime startDate = DateTime.ParseExact(dataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(dataTermino, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                // Gerando as datas entre o intervalo
                List<dashboardGovernancaNCDia> result = oGovernanca.LoadChartNaoConformidadeDia(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                codigoUnidade: unidade,
                                                                                                dataInicio: dataInicio,
                                                                                                dataTermino: dataTermino);
                
                if (unidade == -1)
                {
                    var chartData = new
                    {
                        labels = result.Select(d => d.unidade).ToArray(),
                        datasets = new[]
                        {
                            new { label = "NC's", data = result.Select(d => d.quantidadeNC).ToArray(), backgroundColor = "red" }
                        }
                    };

                    return Json(chartData, JsonRequestBehavior.AllowGet);

                } else
                {
                    var chartData = new
                    {
                        labels = result.Select(d => d.data).ToArray(),
                        datasets = new[]
                        {
                            new { label = "NC's", data = result.Select(d => d.quantidadeNC).ToArray(), backgroundColor = "red" }
                        }
                    };

                    return Json(chartData, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                // Tratamento de erro para lidar com formatos inválidos ou exceções
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        //public JsonResult ChartNaoConformidadeTipo(int unidade, string dataInicio, string dataTermino)
        //{
        //    try
        //    {
        //        // Convertendo as strings para DateTime
        //        DateTime startDate = DateTime.ParseExact(dataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //        DateTime endDate = DateTime.ParseExact(dataTermino, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        //        // Gerando as datas entre o intervalo
        //        //List<dashboardGovernancaChartNaoConformidadeTipo> naoConformidadeTipo = oGovernanca.LoadChartNaoConformidadeTipo(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
        //        //                                                                                                                 codigoUnidade: unidade,
        //        //                                                                                                                 dataInicio: dataInicio,
        //        //                                                                                                                 dataTermino: dataTermino);

        //        //return Json(naoConformidadeTipo, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Tratamento de erro para lidar com formatos inválidos ou exceções
        //        return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        #endregion

        #region

        //[HttpPost]
        //public JsonResult LoadGovernancaHistorico(int unidade, string data, string tipoGovernanca = "", string camareira = "", string naoConformidade = "")
        //{
        //    var result = oGovernanca.LoadDashboardApontamentos(
        //        iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
        //        iCodigoUnidade: unidade,
        //        sData: data,
        //        sTipoGovernanca: tipoGovernanca,
        //        sCamareira: camareira,
        //        sNaoConformidade: naoConformidade
        //    );

        //    return new JsonResult
        //    {
        //        Data = result,
        //        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
        //        MaxJsonLength = int.MaxValue // Aumenta o tamanho máximo permitido
        //    };
        //}


        //[HttpPost]
        //public JsonResult LoadGovernancaNaoConformidade(int unidade, string dataInicio, string dataTermino)
        //{

        //    return Json(oGovernanca.LoadDashboardNaoConformidade(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
        //                                                         codigoUnidade: unidade,
        //                                                         dataInicio: dataInicio,
        //                                                         dataTermino: dataTermino));

        //}

        #endregion

        #endregion

        #region ::: FUNCIONÁRIO :::

        // GET: INDEX
        public ActionResult FuncionarioIndex()
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
                                    sFormulario: "gov_funcionario",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                               bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.funcao = new SelectList(oCombo.Funcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", null);
                ViewBag.tipo_funcionario = new SelectList(oCombo.TipoFuncionario(), "codigo", "descricao", null);
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", 1);

                return View();

            }
        }

        // GET: INDEX
        [HttpPost]
        public ActionResult FuncionarioIndex(string nome, int funcao = -1, int tipo_funcionario = -1, int ativo = -1, int unidade = -1)
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
                                    sFormulario: "gov_funcionario",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                               bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.funcao = new SelectList(oCombo.Funcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: unidade), "codigo", "descricao", funcao);
                ViewBag.tipo_funcionario = new SelectList(oCombo.TipoFuncionario(), "codigo", "descricao", tipo_funcionario);
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", ativo);

                return View();

            }
        }

        // GET: INDEX
        [HttpPost]
        public JsonResult LoadFuncionario(string nome, int funcao = -1, int tipo_funcionario = -1, int ativo = -1, int unidade = -1)
        {
            return Json(oGovernanca.IndexFuncionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                     iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                     iCodigoUnidade: unidade,
                                                     sNome: nome,
                                                     iCodigoFuncao: funcao,
                                                     iCodigoTipoFuncionario: tipo_funcionario,
                                                     iAtivo: ativo));

        }

        // GET: /INSERT
        public ActionResult FuncionarioInsert()
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
                ViewBag.funcao = new SelectList(oCombo.Funcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.usuario = new SelectList(oCombo.Usuario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.tipo_funcionario = new SelectList(oCombo.TipoFuncionario(), "codigo", "descricao", null);

                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult FuncionarioInsert(int unidade, string nome, string cpf, string telefone, int tipo_funcionario, string valor_hora = "0", bool ativo = false, int usuario = -1, int funcao = -1, bool contabiliza_hora = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                float valorHora = 0;
                float.TryParse(valor_hora, out valorHora);

                //Insere Registro no Banco de Dados
                oGovernanca.InsertFuncionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                              iCodigoUnidade: unidade,
                                              sNome: nome,
                                              sCPF: cpf,
                                              iCodigoFuncao: funcao,
                                              sTelefone: telefone,
                                              iCodigoUsuarioVinculado: usuario,
                                              iCodigoTipoFuncionario: tipo_funcionario,
                                              dValorHora: valorHora,
                                              bAtivo: ativo,
                                              bContabilizaHora: contabiliza_hora);

                return RedirectToAction("FuncionarioInsert");
            }
        }

        // GET: /EDIT
        public ActionResult FuncionarioEdit(int codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                GovFuncionario funcionario = null;

                oGovernanca.InfoFuncionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oFuncionario: ref funcionario);

                if (funcionario == null)
                {
                    return HttpNotFound();
                }

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", funcionario.codigo_unidade);
                ViewBag.funcao = new SelectList(oCombo.Funcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: funcionario.codigo_unidade), "codigo", "descricao", funcionario.codigo_funcao);
                ViewBag.usuario = new SelectList(oCombo.Usuario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: funcionario.codigo_unidade), "codigo", "descricao", funcionario.codigo_usuario);
                ViewBag.tipo_funcionario = new SelectList(oCombo.TipoFuncionario(), "codigo", "descricao", funcionario.codigo_tipo_funcionario);

                return View(funcionario);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FuncionarioEdit(int unidade, string nome, string cpf, string telefone, int tipo_funcionario, int codigo, string valor_hora = "0", bool ativo = false, int usuario = -1, int funcao = -1, bool contabiliza_hora = false)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                float valorHora = 0;
                float.TryParse(valor_hora, out valorHora);

                //Insere Registro no Banco de Dados
                oGovernanca.UpdateFuncionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                              iCodigoUnidade: unidade,
                                              sNome: nome,
                                              sCPF: cpf,
                                              iCodigoFuncao: funcao,
                                              sTelefone: telefone,
                                              iCodigoUsuarioVinculado: usuario,
                                              iCodigoTipoFuncionario: tipo_funcionario,
                                              bAtivo: ativo,
                                              dValorHora: valorHora,
                                              bContabilizaHora: contabiliza_hora,
                                              iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("FuncionarioIndex");
            }
        }

        // GET: /DELETE
        public ActionResult FuncionarioDelete(int codigo, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                GovFuncionario funcionario = null;

                oGovernanca.InfoFuncionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigo: codigo,
                                            oFuncionario: ref funcionario);

                if (funcionario == null)
                {
                    return HttpNotFound();
                }

                ViewBag.erro = erro;
                return View(funcionario);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FuncionarioDelete([Bind(Include = "codigo")] GovFuncionario funcionario)
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
                    oGovernanca.DeleteFuncionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                  iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                  iCodigo: funcionario.codigo);

                    //Redireciona para Index
                    return RedirectToAction("FuncionarioIndex");
                }
                catch
                {
                    return FuncionarioDelete(codigo: funcionario.codigo,
                                             erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        //JSON: /VALIDA
        public JsonResult ValidaFuncionario(string cpf, int codigo, int unidade)
        {

            return Json(oGovernanca.ValidaFuncionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                      iCodigoUnidade: unidade,
                                                      iCodigo: codigo,
                                                      sCPF: cpf));

        }

        #endregion

        #region ::: PLANEJAMENTO :::

        // GET: INDEX
        public ActionResult Planejamento()
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
                                    sFormulario: "gov_funcionario",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.usuario = User.Identity.GetUserName();
                ViewBag.data = DateTime.Now.ToShortDateString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                               bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.camareira = new SelectList(oCombo.FuncionarioGovernancaCamareira(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                         iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.funcionario = new SelectList(oCombo.FuncionarioGovernanca(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                  iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.uhAssociada = new SelectList(oCombo.SimNao(), "codigo", "descricao", 0);
                ViewBag.bloco = new SelectList(oCombo.Bloco(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.andar = new SelectList(oCombo.Andar(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.frontOfficeStatus = new SelectList(oCombo.StatusFrontOffice(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.roomStatus = new SelectList(oCombo.StatusRoom(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                      iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.tipoGovernanca = new SelectList(oCombo.TipoGovernanca(), "codigo", "descricao", null);
                ViewBag.tipoGovernancaAssociar = new SelectList(oCombo.TipoGovernanca(), "codigo", "descricao", null);

                ViewBag.lastUpdate = oGovernanca.LoadLastUploadStatus(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                      codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()));

                return View();

            }
        }

        //JSON: /CARREGA PLANEJAMENTO
        public JsonResult LoadPlanejamento(int unidade, int tipoGovernanca, string data, string bloco, string andar, string frontOfficeStatus, string roomStatus)
        {

            return Json(oGovernanca.LoadPlanejamento(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                     codigoUnidade: unidade,
                                                     codigoTipoGovernanca: tipoGovernanca,
                                                     data: data,
                                                     bloco: bloco,
                                                     andar: andar,
                                                     statusFrontOffice: frontOfficeStatus,
                                                     statusRoom: roomStatus));

        }

        //JSON: /LIMPA PLANEJAMENTO
        public JsonResult ClearPlanejamento(int unidade, int usuario, string data, string json)
        {

            try
            {


                oGovernanca.ClearPlanejamento(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              codigoUnidade: unidade,
                                              data: data,
                                              json: json);

                return Json(Properties.Resources.operacao_realizaca_sucesso);

            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }

        }

        //JSON: /UPDATE TIPO GOVERNANÇA
        public JsonResult UpdateTipoGovernanca(int unidade, string data, int tipoGovernanca, string json, int usuario)
        {

            try
            {

                oGovernanca.UpdateTipoGovernanca(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                 codigoUnidade: unidade,
                                                 data: data,
                                                 codigoTipoGovernanca: tipoGovernanca,
                                                 json: json);

                return Json(Properties.Resources.operacao_realizaca_sucesso);

            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }

        }

        //JSON: /UPDATE CAMAREIRA
        public JsonResult UpdateCamareira(int unidade, string data, int camareira, string json, int usuario)
        {

            try
            {

                oGovernanca.UpdateCamareira(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            codigoUnidade: unidade,
                                            data: data,
                                            codigoCamareira: camareira,
                                            json: json);

                return Json(Properties.Resources.operacao_realizaca_sucesso);

            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }

        }

        #endregion

        #region ::: PLANEJAMENTO HISTÓRICO :::

        // GET: INDEX
        public ActionResult PlanejamentoHistorico()
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
                                    sFormulario: "gov_planejamento_historico",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.usuario = User.Identity.GetUserName();
                ViewBag.dataInicio = DateTime.Now.ToShortDateString();
                ViewBag.dataTermino = DateTime.Now.ToShortDateString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                               bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.camareira = new SelectList(oCombo.FuncionarioGovernancaCamareira(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                         iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.tipoGovernanca = new SelectList(oCombo.TipoGovernanca(), "codigo", "descricao", null);
                ViewBag.bloco = new SelectList(oCombo.Bloco(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.andar = new SelectList(oCombo.Andar(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                return View();

            }
        }

        //JSON: /CARREGA PLANEJAMENTO
        public JsonResult LoadPlanejamentoHistorico(int unidade, int tipoGovernanca, string dataInicio, string dataTermino, int camareira, string bloco, string andar, string apartamento)
        {

            return Json(oGovernanca.LoadPlanejamentoHistorico(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                              codigoUnidade: unidade,
                                                              dataInicio: dataInicio,
                                                              dataTermino: dataTermino,
                                                              camareira: camareira,
                                                              codigoTipoGovernanca: tipoGovernanca,
                                                              bloco: bloco,
                                                              andar: andar,
                                                              apartamento: apartamento));

        }

        #endregion

        #region ::: APONTAMENTO :::

        // GET: INDEX
        public ActionResult Apontamento()
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
                                    sFormulario: "gov_apontamento",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.usuario = User.Identity.GetUserName();
                ViewBag.data = DateTime.Now.ToShortDateString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                               bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.camareira = new SelectList(oCombo.FuncionarioGovernancaCamareira(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                         iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.uhAssociada = new SelectList(oCombo.SimNao(), "codigo", "descricao", 0);
                ViewBag.bloco = new SelectList(oCombo.Bloco(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.andar = new SelectList(oCombo.Andar(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.frontOfficeStatus = new SelectList(oCombo.StatusFrontOffice(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.roomStatus = new SelectList(oCombo.StatusRoom(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                      iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.lastUpdate = oGovernanca.LoadLastUploadStatus(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                      codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()));
                ViewBag.tipoGovernanca = new SelectList(oCombo.TipoGovernanca(), "codigo", "descricao", null);
                ViewBag.tipoGovernancaAssociar = new SelectList(oCombo.TipoGovernanca(), "codigo", "descricao", null);

                return View();

            }
        }

        //JSON: /CARREGA PLANEJAMENTO
        public JsonResult LoadApontamento(int unidade, int tipoGovernanca, string data, string bloco, string andar, string frontOfficeStatus, string roomStatus, int uhInicio = -1, int uhTermino = -1)
        {

            return Json(oGovernanca.LoadApontamento(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    codigoUnidade: unidade,
                                                    codigoTipoGovernanca: tipoGovernanca,
                                                    data: data,
                                                    bloco: bloco,
                                                    andar: andar,
                                                    statusFrontOffice: frontOfficeStatus,
                                                    statusRoom: roomStatus,
                                                    uhInicio: uhInicio,
                                                    uhTermino: uhTermino));

        }

        //JSON: /LIMPA PLANEJAMENTO
        public JsonResult ClearApontamento(int unidade, int usuario, string data, string json)
        {

            try
            {


                oGovernanca.ClearApontamento(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                             codigoUnidade: unidade,
                                             data: data,
                                             json: json);

                return Json(Properties.Resources.operacao_realizaca_sucesso);

            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }

        }

        //JSON: /UPDATE CAMAREIRA
        public JsonResult ApontamentoCamareira(int unidade, string data, int camareira, string json, int usuario)
        {

            try
            {

                oGovernanca.insertGovernancaApontamentoCamareira(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                 codigoUnidade: unidade,
                                                                 data: data,
                                                                 codigoCamareira: camareira,
                                                                 codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                 json: json);

                return Json(Properties.Resources.operacao_realizaca_sucesso);

            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }

        }

        #endregion

        #region::: GOVERNANÇA - HISTÓRICO :::

        // GET: CHECKLIST
        public ActionResult Historico()
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
                                    sFormulario: "gov_historico",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bImprimir: ref imprimir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;
                ViewBag.dataInicio = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddDays(-1).ToShortDateString();
                ViewBag.dataTermino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.tipoGovernanca = new SelectList(oCombo.TipoGovernanca(), "codigo", "descricao", null);
                ViewBag.camareira = new SelectList(oCombo.FuncionarioGovernanca(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                return View();
            }
        }

        // GET: CHECKLIST HISTÓRICO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Historico(string dataInicio, string dataTermino, int apartamento = -1, int unidade = -1, int tipoGovernanca = -1, int camareira = -1)
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
                                    sFormulario: "gov_historico",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;
                ViewBag.dataInicio = dataInicio;
                ViewBag.dataTermino = dataTermino;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", unidade);
                ViewBag.tipoGovernanca = new SelectList(oCombo.TipoGovernanca(), "codigo", "descricao", tipoGovernanca);
                ViewBag.camareira = new SelectList(oCombo.FuncionarioGovernanca(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                iCodigoUnidade: unidade), "codigo", "descricao", camareira);
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", unidade);
                ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: unidade), "codigo", "descricao", apartamento);

                return View();
            }
        }

        public JsonResult LoadApontamentoHistorico(int empresa, string dataInicio, string dataTermino, int apartamento = -1, int unidade = -1, int tipoGovernanca = -1, int camareira = -1)
        {

            return Json(oGovernanca.LoadGovernancaHistorico(iCodigoEmpresa: empresa,
                                                            iCodigoUnidade: unidade,
                                                            sDataInicio: dataInicio,
                                                            sDataTermino: dataTermino,
                                                            iCodigoTipoGovernanca: tipoGovernanca,
                                                            iCodigoCamareira: camareira,
                                                            iCodigoApartamento: apartamento));

        }

        public JsonResult LoadGovernancaHistoricoDetails(int empresa, int unidade, long codigo)
        {

            return Json(oGovernanca.LoadGovernancaHistoricoDetails(iCodigoEmpresa: empresa,
                                                                   iCodigoUnidade: unidade,
                                                                   lCodigo: codigo));

        }

        public JsonResult DeleteGovernanca(int empresa, int unidade, long codigo)
        {

            try
            {
                oGovernanca.DeleteGovernanca(iCodigoEmpresa: empresa,
                                             iCodigoUnidade: unidade,
                                             lCodigo: codigo);

                return Json(true);

            }
            catch
            {
                return Json(false);
            }

        }

        // GET: APONTAMENTO CHECKLIST
        public ActionResult GovernancaView(int empresa, int unidade, long codigo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                ViewBag.governanca = oGovernanca.LoadGovernancaDados(iCodigoEmpresa: empresa,
                                                                     iCodigoUnidade: unidade,
                                                                     lCodigoVistoria: codigo);

                return View(oGovernanca.LoadGovernancaApontamento(iCodigoEmpresa: empresa,
                                                                  iCodigoUnidade: unidade,
                                                                  iCodigoApartamento: -1,
                                                                  iCodigoTipoGovernanca: -1,
                                                                  lCodigoGovernancaApontamento: codigo));
            }
        }

        #endregion

        #region::: GOVERNANÇA - INVENTÁRIO ENXOVAL :::

        // GET: CHECKLIST
        public ActionResult InventarioEnxoval()
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
                                    sFormulario: "govInventarioEnxoval",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bImprimir: ref imprimir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;
                ViewBag.dataInicio = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddDays(-1).ToShortDateString();
                ViewBag.dataTermino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.status = new SelectList(oCombo.LoadCombo("sp_select_combo_static_status_inventario_governanca"), "codigo", "descricao", null);

                return View();
            }
        }

        public JsonResult LoadInventarioEnxoval(int empresa, string dataInicio, string dataTermino, int unidade = -1, int status = -1)
        {

            return Json(oGovernanca.LoadGovernancaInventarioEnxoval(codigoEmpresa: empresa,
                                                                    codigoUnidade: unidade,
                                                                    dataInicio: dataInicio,
                                                                    dataTermino: dataTermino,
                                                                    status: status));

        }

        public JsonResult LoadGovernancaInventarioEnxovalDetalhe(long codigoInventarioGovernanca)
        {

            return Json(oGovernanca.LoadGovernancaInventarioEnxovalDetalhe(codigoInventarioGovernanca: codigoInventarioGovernanca));

        }

        public JsonResult ChangeStatusInventarioEnxoval(long codigoInventarioGovernanca, int status)
        {            
            return Json(oGovernanca.ChangeStatusInventarioEnxoval(codigoInventarioGovernanca: codigoInventarioGovernanca, 
                                                                  status: status,
                                                                  codigoUsuario: Convert.ToInt32(User.Identity.GetUserName())));
        }

        public ActionResult InventarioEnxovalNovo(int unidade = -1)
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
                                    sFormulario: "govInventarioEnxoval",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.data = DateTime.Now.ToString("dd/MM/yyyy");
                ViewBag.codigoEmpresa = Session["empresa"].ToString();

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", (unidade == -1) ? Session["codigo_unidade"].ToString() : unidade.ToString());

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InventarioEnxovalNovo(int unidade, string data, string jsonEnxoval)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {                
                
                oGovernanca.InsertInventarioEnxoval(codigoEmpresa: Convert.ToInt32(Session["empresa"]),
                                                    codigoUnidade: unidade,
                                                    codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    data: data,
                                                    jsonEnxoval: jsonEnxoval);

                return RedirectToAction("InventarioEnxoval", "Governanca");
            }
        }

        public JsonResult LoadEnxoval(int empresa, int unidade, string data)
        {

            return Json(oGovernanca.LoadEnxoval(codigoEmpresa: empresa,
                                                codigoUnidade: unidade,
                                                data: data));

        }

        #endregion

        #region::: GOVERNANÇA - MOVIMENTAÇÃO ENXOVAL :::

        // GET: CHECKLIST
        public ActionResult MovimentacaoEnxoval()
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
                                    sFormulario: "govMovimentacaoEnxoval",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bImprimir: ref imprimir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;
                ViewBag.dataInicio = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddDays(-1).ToShortDateString();
                ViewBag.dataTermino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.enxoval = new SelectList(oCombo.LoadCombo("sp_select_combo_cadastro_basico_enxoval",
                                                                  codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                  codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                return View();
            }
        }

        public JsonResult LoadMovimentacaoEnxoval(int empresa, string dataInicio, string dataTermino, int unidade = -1, int enxoval = -1)
        {

            return Json(oGovernanca.LoadMovimentacaoEnxoval(codigoEmpresa: empresa,
                                                            codigoUnidade: unidade,
                                                            dataInicio: dataInicio,
                                                            dataTermino: dataTermino,
                                                            enxoval: enxoval));

        }

        public JsonResult LoadMovimentacaoEnxovalDetalhe(int empresa, string dataInicio, string dataTermino, int unidade, int enxoval)
        {

            return Json(oGovernanca.LoadMovimentacaoEnxovalDetalhe(codigoEmpresa: empresa,
                                                                   codigoUnidade: unidade,
                                                                   dataInicio: dataInicio,
                                                                   dataTermino: dataTermino,
                                                                   enxoval: enxoval));

        }

        #endregion

        #region ::: GOVERNANÇA - APONTAMENTO :::

        // GET: INDEX
        public ActionResult GovernancaApontamento(int codigoUnidade, string data, int codigoFuncionario = -1)
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
                                    sFormulario: "governanca_apontamento",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.administrador = administrador;
                ViewBag.codigoEmpresa = Convert.ToInt32(Session["empresa"].ToString());
                ViewBag.codigoUnidade = codigoUnidade;
                ViewBag.data = data;
                ViewBag.codigoFuncionario = codigoFuncionario;

                return View();
            }
        }

        #endregion

        #region::: CADASTRO BÁSICO - TIPO DE PERDA ENXOVAL :::

        public ActionResult TipoPerda()
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
                                    sFormulario: "govCadTipoPerda",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bImprimir: ref imprimir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;
                ViewBag.usuario = User.Identity.GetUserName();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());

                return View();
            }
        }

        [HttpPost]
        public JsonResult LoadTipoPerda(int empresa, int unidade, string descricao)
        {

            return Json(oGovernanca.LoadTipoPerdaEnxoval(codigoEmpresa: empresa,
                                                         codigoUnidade: unidade,
                                                         descricao: descricao));

        }

        public ActionResult TipoPerdaInsert()
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
                                    sFormulario: "govCadTipoPerda",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bImprimir: ref imprimir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;
                ViewBag.usuario = User.Identity.GetUserName();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString()); ;
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", 1);

                return View();
            }
        }

        [HttpPost]
        public ActionResult TipoPerdaInsert(int unidade, string descricao, int ativo, int usuario)
        {
            oGovernanca.InsertTipoPerdaEnxoval(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                               codigoUnidade: unidade,
                                               descricao: descricao,
                                               ativo: ativo,
                                               codigoUsuario: usuario);

            return RedirectToAction("TipoPerda");
        }

        public ActionResult TipoPerdaEdit(int codigo)
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
                                    sFormulario: "govCadTipoPerda",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bImprimir: ref imprimir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;
                ViewBag.usuario = User.Identity.GetUserName();

                TipoPerdaEnxoval tipoPerdaEnxoval = oGovernanca.LoadTipoPerdaEnxoval(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                     codigo: codigo);
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", tipoPerdaEnxoval.codigoUnidade); ;
                ViewBag.ativo = new SelectList(oCombo.SimNao(), "codigo", "descricao", tipoPerdaEnxoval.ativoValue);

                return View(tipoPerdaEnxoval);
            }
        }

        [HttpPost]
        public ActionResult TipoPerdaEdit(string descricao, int ativo, int usuario, int codigo)
        {
            oGovernanca.UpdateTipoPerdaEnxoval(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                               descricao: descricao,
                                               ativo: ativo,
                                               codigoUsuario: usuario,
                                               codigo: codigo);

            return RedirectToAction("TipoPerda");
        }

        [HttpPost]
        public JsonResult TipoPerdaDelete(int usuario, int codigo)
        {
            defaultResponse response = new defaultResponse();

            try
            {                
                oGovernanca.DeleteTipoPerdaEnxoval(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                   codigoUsuario: usuario,
                                                   codigo: codigo);

                response.success = true;
                response.message = Resources.register_deleted;

            } 
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }

            return Json(response);

        }

        [HttpPost]
        public JsonResult ValidaTipoPerda(int unidade, string descricao, int codigo)
        {
            return Json(oGovernanca.ValidaTipoPerdaEnxoval(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                           codigoUnidade: unidade,
                                                           descricao: descricao,
                                                           codigo: codigo));
        }

        #endregion

        #region ::: REPORT :::

        #region ::: FUNCIONÁRIO - HORAS TRABALHADAS GOVERNANÇA :::

        public ActionResult FuncionarioHorasTrabalhadasGovernanca()
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
                ViewBag.horasFaltas = new SelectList(oCombo.HorasFaltas(), "codigo", "descricao", 1);
                ViewBag.ano = new SelectList(oCombo.Ano(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", System.DateTime.Now.Year);

                return View();
            }
        }

        [HttpPost]
        public JsonResult LoadFuncionarioHorasTrabalhadasGovernanca(int ano, int unidade, int ativo, int funcionario)
        {
            return Json(oRelatorio.FuncionarioHorasTrabalhadasGovernanca(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                         iCodigoUnidade: unidade,
                                                                         iCodigoFuncionario: funcionario,
                                                                         iAtivo: ativo,
                                                                         iAno: ano));
        }

        #endregion

        #region ::: RELATÓRIO ENXOVAL - DIA :::

        public ActionResult RelatorioConsumoEnxovalDia()
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
                                    sFormulario: "rel_consumo_enxoval",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);

                ViewBag.imprimir = imprimir;
                ViewBag.data = new SelectList(oCombo.DataGovernanca(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.formaAnalise = new SelectList(oCombo.FormaAnaliseConsumoEnxoval(), "codigo", "descricao", date.ToString("dd/MM/yyyy"));

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RelatorioConsumoEnxovalDia(string data, int formaAnalise, int unidade = -1)
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
                                    sFormulario: "rel_consumo_enxoval",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.data = new SelectList(oCombo.DataGovernanca(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: unidade), "codigo", "descricao", data);
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.formaAnalise = new SelectList(oCombo.FormaAnaliseConsumoEnxoval(), "codigo", "descricao", formaAnalise);

                return View();
            }
        }

        public JsonResult LoadRelatorioConsumoEnxovalDia(string data, int formaAnalise, int unidade = -1)
        {

            return Json(oRelatorio.RelatorioConsumoEnxovalDia(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                              iCodigoUnidade: unidade,
                                                              sData: data,
                                                              iCodigoFormaAnalise: formaAnalise));

        }

        #endregion

        #region ::: RELATÓRIO CAMAREIRA - US :::

        public ActionResult RelatorioCamareiraUH()
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
                                    sFormulario: "rel_consumo_enxoval",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);

                ViewBag.imprimir = imprimir;
                ViewBag.data = new SelectList(oCombo.DataGovernanca(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.camareira = new SelectList(oCombo.FuncionarioGovernancaCamareira(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                         iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RelatorioCamareiraUH(string data, int camareira = -1, int unidade = -1)
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
                                    sFormulario: "rel_consumo_enxoval",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.data = new SelectList(oCombo.DataGovernanca(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: unidade), "codigo", "descricao", data);
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.camareira = new SelectList(oCombo.FuncionarioGovernancaCamareira(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                         iCodigoUnidade: unidade), "codigo", "descricao", camareira);

                return View();
            }
        }

        public JsonResult LoadRelatorioCamareiraUH(string data, int camareira = -1, int unidade = -1)
        {

            return Json(oRelatorio.RelatorioCamareiraUH(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: unidade,
                                                        sData: data,
                                                        iCodigoCamareira: camareira));

        }

        #endregion

        #region ::: RELATÓRIO RESPONSÁVEL VISTORIA - US :::

        // GET: INDEX
        public ActionResult RelatorioVistoriadorUH()
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
                                    sFormulario: "rel_consumo_enxoval",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);

                ViewBag.imprimir = imprimir;
                ViewBag.data = new SelectList(oCombo.DataGovernanca(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.responsavelVistoria = new SelectList(oCombo.FuncionarioGovernancaVistoriador(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                     iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                return View();
            }
        }

        public JsonResult LoadRelatorioResponsavelVistoriaUH(string data, int responsavelVistoria = -1, int unidade = -1)
        {

            return Json(oRelatorio.RelatorioResponsavelVistoria(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: unidade,
                                                                sData: data,
                                                                iCodigoFuncionarioGovernanca: responsavelVistoria));

        }

        #endregion

        #region ::: RELATÓRIO CAMAREIRA - NC :::

        // GET: INDEX
        public ActionResult RelatorioCamareiraNC()
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
                                    sFormulario: "rel_camareira_nc",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);

                ViewBag.imprimir = imprimir;
                ViewBag.data = new SelectList(oCombo.DataGovernanca(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.camareira = new SelectList(oCombo.FuncionarioGovernancaCamareira(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                         iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.tipoNC = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_static_governanca_tipo_nc"), "codigo", "descricao", 3);
                ViewBag.viewReportNC = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_static_governanca_forma_visualizar_report_nc"), "codigo", "descricao", 2);

                return View();
            }
        }

        public JsonResult LoadRelatorioCamareiraNC(string data, int camareira = -1, int unidade = -1, int tipoNC = -1, int viewReportNC = -1)
        {

            return Json(oRelatorio.RelatorioCamareiraNC(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        codigoUnidade: unidade,
                                                        data: data,
                                                        codigoCamareira: camareira,
                                                        tipoNC: tipoNC,
                                                        viewReportNC: viewReportNC));

        }

        #endregion

        #region ::: RELATÓRIO UH - NC :::

        // GET: INDEX
        public ActionResult RelatorioUHNC()
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
                                    sFormulario: "rel_uh_nc",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);

                ViewBag.imprimir = imprimir;
                ViewBag.data = new SelectList(oCombo.DataGovernanca(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.apartamento = new SelectList(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.tipoNC = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_static_governanca_tipo_nc"), "codigo", "descricao", 3);
                ViewBag.viewReportNC = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_static_governanca_forma_visualizar_report_nc"), "codigo", "descricao", 2);

                return View();
            }
        }

        public JsonResult LoadRelatorioUHNC(string data, int apartamento = -1, int unidade = -1, int tipoNC = -1, int viewReportNC = -1)
        {

            return Json(oRelatorio.RelatorioUHNC(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                 codigoUnidade: unidade,
                                                 data: data,
                                                 codigoApartamento: apartamento,
                                                 tipoNC: tipoNC,
                                                 viewReportNC: viewReportNC));

        }

        #endregion

        #region ::: STATUS UH :::

        public ActionResult StatusUH()
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
                ViewBag.codigoEmpresa = Session["empresa"].ToString();

                return View();
            }
        }

        [HttpPost]
        public ActionResult StatusUH(int empresa, int unidade, int usuario)
        {
            oGovernanca.UpdateStatusUH(codigoEmpresa: empresa,
                                        codigoUnidade: unidade,
                                        codigoUsuario: usuario);

            return RedirectToAction("StatusUH");

        }

        [HttpPost]
        public JsonResult UploadStatusUH(int codigoEmpresa, int codigoUnidade, HttpPostedFileBase arquivo, string planilha)
        {

            GovernancaStatusUH statusUH = new GovernancaStatusUH();

            string folder = Server.MapPath("~/Content/arq/excel");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string path = Server.MapPath("~/Content/arq/excel/inventory_" + DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("yyyyMMddhhmmss") + "_" + arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            arquivo.SaveAs(path);

            try
            {

                List<GovernancaStatusUHDetalhe> result = new List<GovernancaStatusUHDetalhe>();
                List<GovernancaStatusUHDetalheError> error = new List<GovernancaStatusUHDetalheError>();

                //Upload Arquivo
                oGovernanca.UploadStatuUHExcel(codigoEmpresa: codigoEmpresa,
                                               codigoUnidade: codigoUnidade,
                                               codigoUsuario: Convert.ToInt32(User.Identity.Name),
                                               file: path,
                                               worksheet: planilha,
                                               oDetails: ref result,
                                               oDetailsError: ref error);

                statusUH.result = result;
                statusUH.resultError = error;
                statusUH.success = true;


            }
            catch (Exception ex)
            {
                statusUH.success = false;
                statusUH.message = ex.Message;
                statusUH.result = new List<GovernancaStatusUHDetalhe>();
                statusUH.resultError = new List<GovernancaStatusUHDetalheError>();
            }
            finally
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            return Json(statusUH);
        }

        // GET: /Download Excel
        [HttpGet]
        public virtual ActionResult StatusUHDownloadExcel()
        {

            string nome_relatorio = "STATUS_UH.xlsx";
            string filename = "STATUS_UH.xlsx";
            string path = Server.MapPath("~/Content/Files");
            string arquivo = System.IO.Path.Combine(path, nome_relatorio);

            return File(arquivo, "application/vnd.ms-excel", filename);
        }

        #endregion

        #region ::: CICLO LAVAGEM :::

        // GET: INDEX
        public ActionResult ApontamentoLavanderia(int unidade = -1)
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
                                    sFormulario: "gov_lavanderia",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.data = DateTime.Now.ToString("dd/MM/yyyy");
                ViewBag.codigoEmpresa = Session["empresa"].ToString();

                ViewBag.tipo = new SelectList(oCombo.LoadCombo("sp_select_combo_static_tipo_movimentacao"), "codigo", "descricao", (unidade == -1) ? Session["codigo_unidade"].ToString() : unidade.ToString());
                ViewBag.tipoPerdaEnxoval = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_cadastro_basico_tipo_perda_enxoval",
                                                                           codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                           codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", (unidade == -1) ? Session["codigo_unidade"].ToString(): unidade.ToString());

                return View();
            }
        }

        [HttpPost]
        public ActionResult ApontamentoLavanderia(int empresa, int unidade, int tipo, string data, string enxovalJson, string peso = "0", string quantidadeHospede = "0", string ocupacaoQuartos = "0", int tipoPerdaEnxoval = -1)
        {
            oGovernanca.ApontamentoLavanderia(codigoEmpresa: empresa,
                                              codigoUnidade: unidade,
                                              tipo: tipo,
                                              data: data,
                                              quantidadeHospede: quantidadeHospede,
                                              ocupacaoQuartos: ocupacaoQuartos,
                                              peso: peso,
                                              enxoval: enxovalJson,
                                              tipoPerdaEnxoval: tipoPerdaEnxoval,
                                              codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()));

            return RedirectToAction("ApontamentoLavanderia", "Governanca", new { unidade = unidade});
        }

        [HttpPost]
        public JsonResult ApontamentoLavanderiaInfo(int empresa, int unidade, int tipo, string data)
        {
            return Json(oGovernanca.LoadLavanderiaInfo(codigoEmpresa: empresa,
                                                       codigoUnidade: unidade,
                                                       tipo: tipo,
                                                       data: data));
        }

        [HttpPost]
        public JsonResult LoadLavanderiaEnxoval(int codigoEmpresa, int codigoUnidade, int tipo, string data, string dataInicio, string dataTermino)
        {

            return Json(oGovernanca.LoadEnxoval(codigoEmpresa: codigoEmpresa,
                                                codigoUnidade: codigoUnidade,
                                                tipo: tipo,
                                                data: data,
                                                dataInicio: dataInicio,
                                                dataTermino: dataTermino));

        }

        public ActionResult RolLavanderia(int empresa, int unidade, int tipo, string data)
        {

            ViewBag.data = data;

            return View(oGovernanca.LoadEnxoval(codigoEmpresa: empresa,
                                                codigoUnidade: unidade,
                                                tipo: tipo,
                                                data: data,
                                                dataInicio:"",
                                                dataTermino: ""));
        }

        #endregion

        #region ::: RELATÓRIO CAMAREIRA - NC :::

        // GET: INDEX
        public ActionResult LogAlteracaoStatusGov()
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
                                    sFormulario: "gov_log_alteracao_status_gov",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);

                ViewBag.imprimir = imprimir;
                ViewBag.dataInicio = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");
                ViewBag.dataTermino = DateTime.Now.ToString("dd/MM/yyyy");
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.statusPMS = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_stc_status_governanca_pms", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", null);
                
                return View();
            }
        }

        [HttpPost]
        public JsonResult LoadLogAlteracaoStatusGov(int codigoUnidade, string dataInicio, string dataTermino, string apartamento, string status)
        {

            return Json(oGovernanca.LoadLogAlteracaoStatusGov(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                              codigoUnidade: codigoUnidade,
                                                              dataInicio: dataInicio,
                                                              dataTermino: dataTermino,
                                                              status: status,
                                                              apartamento: apartamento));

        }

        #endregion

        #endregion

    }
}