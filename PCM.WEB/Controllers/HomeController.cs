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

namespace PCM.WEB.Controllers
{

    public class HomeController : Controller
    {
        private Home oHome = new Home(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Relatorio oRelatorio = new DAL.Relatorio(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Qualidade oQualidade = new DAL.Qualidade(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Dashboard oDashboard = new DAL.Dashboard(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.Governanca oGovernanca = new DAL.Governanca(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.LogBook oLogBook = new DAL.LogBook(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: JSON :::

        //JSON: /CONFIRM LOGBOOK/
        public void ConfirmLogBook()
        {
            oLogBook.UpdateLogBook(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()));
        }

        public JsonResult DadosOrdemServico(int unidade)
        {
            return Json(oHome.DadosOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
        }

        public JsonResult PrincipaisOcorrencias(int unidade, int quantidade, bool qualidade = false)
        {
            return Json(oHome.PrincipaisOcorrencias(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                    iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                    iQuantidade: quantidade,
                                                    bQualidade: qualidade));
        }

        public JsonResult PlanoAcaoRecorrencia(int unidade, int filtro)
        {
            return Json(oDashboard.QualidadePlanoAcaoRecorrenciaUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: unidade,
                                                                        iFiltro: filtro));
        }

        /// <summary>
        /// Retorna as listas de unidade e módulo para o select do header global.
        /// Usado por todas as páginas — não depende de ViewBag.
        /// </summary>
        public JsonResult HeaderCombos()
        {
            int empresa = Convert.ToInt32(Session["empresa"].ToString());
            int usuario = Convert.ToInt32(User.Identity.GetUserName());
            int unidadeAtual = Convert.ToInt32(Session["codigo_unidade"].ToString());
            int moduloAtual = Convert.ToInt32(Session["codigo_modulo"].ToString());

            var unidades = oCombo.Unidade(iCodigoEmpresa: empresa,
                                           iCodigoUsuario: usuario,
                                           bCadastro: false)
                                  .Select(u => new { codigo = u.codigo, descricao = u.descricao, selecionado = (u.codigo == unidadeAtual) });

            var modulos = oCombo.Modulo(iCodigoEmpresa: empresa,
                                          iCodigoUsuario: usuario)
                                  .Select(m => new { codigo = m.codigo, descricao = m.descricao, selecionado = (m.codigo == moduloAtual) });

            return Json(new { unidades, modulos }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna todos os dados do dashboard em um único request AJAX,
        /// usado pelo auto-refresh sem reload de página.
        /// </summary>
        public JsonResult DashboardSnapshot()
        {
            int empresa = Convert.ToInt32(Session["empresa"].ToString());
            int unidade = Convert.ToInt32(Session["codigo_unidade"].ToString());
            int modulo = Convert.ToInt32(Session["codigo_modulo"].ToString());

            InfoOrdemServicoNew os = oHome.InfoOrdemServico(iCodigoEmpresa: empresa,
                                                              iCodigoUnidade: unidade,
                                                              iCodigoModulo: modulo,
                                                              bQualidade: false);

            var gauge = oHome.ChartGauge(iCodigoEmpresa: empresa,
                                         iCodigoUnidade: unidade,
                                         iCodigoModulo: modulo);

            var ocorrencias = oHome.PrincipaisOcorrencias(iCodigoEmpresa: empresa,
                                                          iCodigoUnidade: unidade,
                                                          iCodigoModulo: modulo,
                                                          bQualidade: false,
                                                          iQuantidade: 30);

            return Json(new { os, gauge, ocorrencias }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ::: INDEX :::

        public ActionResult Index()
        {
            if (@Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                int iCodigoEmpresaPMOC = Convert.ToInt32(Session["empresa"].ToString());
                int iCodigoUnidadePMOC = Convert.ToInt32(Session["codigo_unidade"].ToString());
                int iCodigoTipoUnidade = 0;
                string sHotelOpera = "";

                oHome.DadosUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                   iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                   iCodigoEmpresaPMOC: ref iCodigoEmpresaPMOC,
                                   iCodigoUnidadePMOC: ref iCodigoUnidadePMOC,
                                   iCodigoTipoUnidade: ref iCodigoTipoUnidade,
                                   sHotelOpera: ref sHotelOpera);

                Session["codigo_empresa_pmoc"] = iCodigoEmpresaPMOC;
                Session["codigo_unidade_pmoc"] = iCodigoUnidadePMOC;
                Session["hotel_opera"] = sHotelOpera;

                bool bDashboardPreventiva = false;
                bool bDashboardLaudo = false;
                bool bDashboardRotina = false;
                bool bDashboardPMOC = false;
                bool bDashboardUH = false;
                bool bDashboardGreenPlanet = false;


                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_preventiva",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardPreventiva);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_laudo",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardLaudo);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_rotina",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardRotina);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_pmoc",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardPMOC);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_uh",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardUH);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_green_planet",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardGreenPlanet);

                ViewBag.dashboard_preventiva = bDashboardPreventiva;
                ViewBag.dashboard_laudo = bDashboardLaudo;
                ViewBag.dashboard_rotina = bDashboardRotina;
                ViewBag.dashboard_pmoc = bDashboardPMOC;
                ViewBag.dashboard_uh = bDashboardUH;
                ViewBag.dashboard_green_planet = bDashboardGreenPlanet;

                ViewBag.InfoOrdemServico = oHome.InfoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                  iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                  iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                  bQualidade: false);
                ViewBag.data_inicio = System.DateTime.Now.Date.AddMonths(-1).ToShortDateString();
                ViewBag.data_termino = System.DateTime.Now.Date.ToShortDateString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Session["codigo_modulo"].ToString());
                ViewBag.agrupado_por = new SelectList(oCombo.AgrupadoPorData(), "codigo", "descricao", null);
                ViewBag.grupo_item_medicao = new SelectList(oCombo.GrupoItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao");
                ViewBag.item_medicao = new SelectList(oCombo.ItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                            iCodigoGrupoItemMedicao: -1), "codigo", "descricao");
                ViewBag.forma_calculo = new SelectList(oCombo.FormaCalculoGreenPlanet(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao");
                ViewBag.ChartGauge = oHome.ChartGauge(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                      iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                      iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()));
                ViewBag.PrincipaisOcorrencias = oHome.PrincipaisOcorrencias(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                            bQualidade: false,
                                                                            iQuantidade: 30);

                if (Convert.ToInt32(Session["codigo_unidade"].ToString()) == -1)
                {
                    ViewBag.dashboardInfo = oHome.DashboardInfo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()));
                }

                ViewBag.codigo_tipo_unidade = iCodigoTipoUnidade;

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int unidade, int modulo)
        {
            if (@Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Session["codigo_unidade"] = unidade;
                Session["codigo_modulo"] = modulo;

                if (modulo == 2)
                {
                    Session["dashboard"] = "IndexQA";
                    Session["modulo"] = "QUALIDADE";
                    Session["modulo_css"] = "text-danger";
                    return RedirectToAction("IndexQA", new { unidade = unidade, modulo = modulo });
                }

                if (modulo == 3)
                {
                    Session["dashboard"] = "IndexGovernanca";
                    Session["modulo"] = "GOVERNANÇA";
                    Session["modulo_css"] = "text-success";
                    return RedirectToAction("IndexGovernanca", new { unidade = unidade, modulo = modulo });
                }

                if (modulo == 4)
                {
                    Session["dashboard"] = "IndexAEB";
                    Session["modulo"] = "AEB";
                    Session["modulo_css"] = "text-earth";
                    return RedirectToAction("IndexAEB", new { unidade = unidade, modulo = modulo });
                }

                int iCodigoEmpresaPMOC = 0;
                int iCodigoUnidadePMOC = 0;
                int iCodigoTipoUnidade = 0;
                string sHotelOpera = "";

                oHome.DadosUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                   iCodigoUnidade: unidade,
                                   iCodigoEmpresaPMOC: ref iCodigoEmpresaPMOC,
                                   iCodigoUnidadePMOC: ref iCodigoUnidadePMOC,
                                   iCodigoTipoUnidade: ref iCodigoTipoUnidade,
                                   sHotelOpera: ref sHotelOpera);

                Session["codigo_empresa_pmoc"] = iCodigoEmpresaPMOC;
                Session["codigo_unidade_pmoc"] = iCodigoUnidadePMOC;
                Session["hotel_opera"] = sHotelOpera;

                bool bDashboardPreventiva = false;
                bool bDashboardLaudo = false;
                bool bDashboardRotina = false;
                bool bDashboardPMOC = false;
                bool bDashboardUH = false;
                bool bDashboardGreenPlanet = false;


                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_preventiva",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardPreventiva);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_laudo",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardLaudo);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_rotina",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardRotina);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_pmoc",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardPMOC);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_uh",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardUH);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_green_planet",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardGreenPlanet);

                ViewBag.dashboard_preventiva = bDashboardPreventiva;
                ViewBag.dashboard_laudo = bDashboardLaudo;
                ViewBag.dashboard_rotina = bDashboardRotina;
                ViewBag.dashboard_pmoc = bDashboardPMOC;
                ViewBag.dashboard_uh = bDashboardUH;
                ViewBag.dashboard_green_planet = bDashboardGreenPlanet;

                FormularioVisualizar formulario_visualizar = null;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    oFormularioVisualizar: ref formulario_visualizar);

                InfoOrdemServicoNew info_ordem_servico = oHome.InfoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                iCodigoUnidade: unidade,
                                                                                iCodigoModulo: modulo,
                                                                                bQualidade: false);

                Session["unidade"] = info_ordem_servico.unidade;
                Session["unidade_descricao"] = (info_ordem_servico.unidade == "") ? "TODAS AS UNIDADES" : info_ordem_servico.unidade;

                ViewBag.InfoOrdemServico = info_ordem_servico;
                ViewBag.data_inicio = System.DateTime.Now.Date.AddMonths(-1).ToShortDateString();
                ViewBag.data_termino = System.DateTime.Now.Date.ToShortDateString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.agrupado_por = new SelectList(oCombo.AgrupadoPorData(), "codigo", "descricao", null);
                ViewBag.grupo_item_medicao = new SelectList(oCombo.GrupoItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                    iCodigoUnidade: unidade), "codigo", "descricao");
                ViewBag.item_medicao = new SelectList(oCombo.ItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: unidade,
                                                                            iCodigoGrupoItemMedicao: -1), "codigo", "descricao");
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Session["codigo_modulo"].ToString());
                ViewBag.forma_calculo = new SelectList(oCombo.FormaCalculoGreenPlanet(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao");
                ViewBag.ChartGauge = oHome.ChartGauge(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: unidade,
                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()));
                ViewBag.PrincipaisOcorrencias = oHome.PrincipaisOcorrencias(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: unidade,
                                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                            bQualidade: false,
                                                                            iQuantidade: 30);
                ViewBag.formulario_visualizar = formulario_visualizar;
                ViewBag.codigo_tipo_unidade = iCodigoTipoUnidade;

                if (Convert.ToInt32(Session["codigo_unidade"].ToString()) == -1)
                {
                    ViewBag.dashboardInfo = oHome.DashboardInfo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()));
                }

                return View();
            }
        }

        #endregion

        #region ::: INDEX - QA :::

        public ActionResult IndexQA()
        {
            if (@Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                int iCodigoEmpresaPMOC = 0;
                int iCodigoUnidadePMOC = 0;
                int iCodigoTipoUnidade = 0;
                string sHotelOpera = "";

                oHome.DadosUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                    iCodigoEmpresaPMOC: ref iCodigoEmpresaPMOC,
                                    iCodigoUnidadePMOC: ref iCodigoUnidadePMOC,
                                    iCodigoTipoUnidade: ref iCodigoTipoUnidade,
                                    sHotelOpera: ref sHotelOpera);

                Session["codigo_empresa_pmoc"] = iCodigoEmpresaPMOC;
                Session["codigo_unidade_pmoc"] = iCodigoUnidadePMOC;
                Session["hotel_opera"] = sHotelOpera;

                FormularioVisualizar formulario_visualizar = null;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    oFormularioVisualizar: ref formulario_visualizar);

                InfoOrdemServico info_ordem_servico = null;

                oHome.DadosOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                        bQualidade: true,
                                        oInfoOrdemServico: ref info_ordem_servico);
                ViewBag.data_inicio = System.DateTime.Now.Date.AddMonths(-1).ToShortDateString();
                ViewBag.data_termino = System.DateTime.Now.Date.ToShortDateString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Session["codigo_modulo"].ToString());
                ViewBag.InfoOrdemServico = info_ordem_servico;
                ViewBag.PrincipaisOcorrencias = oHome.PrincipaisOcorrencias(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                            bQualidade: true,
                                                                            iQuantidade: 30);
                //ViewBag.formulario_visualizar = formulario_visualizar;
                ViewBag.codigo_tipo_unidade = iCodigoTipoUnidade;
                ViewBag.plano_acao_status = oQualidade.LoadPlanoAcaoStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()));
                ViewBag.status = oQualidade.LoadAuditoriaStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()));

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IndexQA(int unidade, int modulo)
        {
            if (@Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Session["codigo_unidade"] = unidade;
                Session["codigo_modulo"] = modulo;

                if (modulo == 1)
                {
                    Session["dashboard"] = "Index";
                    Session["modulo"] = "MANUTENÇÃO";
                    Session["modulo_css"] = "text-primary";
                    return RedirectToAction("Index", new { unidade = unidade, modulo = modulo });
                }

                if (modulo == 3)
                {
                    Session["dashboard"] = "IndexGovernanca";
                    Session["modulo"] = "GOVERNANÇA";
                    Session["modulo_css"] = "text-success";
                    return RedirectToAction("IndexGovernanca", new { unidade = unidade, modulo = modulo });
                }

                if (modulo == 4)
                {
                    Session["dashboard"] = "IndexAEB";
                    Session["modulo"] = "AEB";
                    Session["modulo_css"] = "text-earth";
                    return RedirectToAction("IndexAEB", new { unidade = unidade, modulo = modulo });
                }

                int iCodigoEmpresaPMOC = 0;
                int iCodigoUnidadePMOC = 0;
                int iCodigoTipoUnidade = 0;
                string sHotelOpera = "";

                oHome.DadosUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                   iCodigoUnidade: unidade,
                                   iCodigoEmpresaPMOC: ref iCodigoEmpresaPMOC,
                                   iCodigoUnidadePMOC: ref iCodigoUnidadePMOC,
                                   iCodigoTipoUnidade: ref iCodigoTipoUnidade,
                                   sHotelOpera: ref sHotelOpera);

                Session["codigo_empresa_pmoc"] = iCodigoEmpresaPMOC;
                Session["codigo_unidade_pmoc"] = iCodigoUnidadePMOC;
                Session["hotel_opera"] = sHotelOpera;

                InfoOrdemServico info_ordem_servico = null;

                oHome.DadosOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUnidade: unidade,
                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                        bQualidade: true,
                                        oInfoOrdemServico: ref info_ordem_servico);

                Session["unidade"] = info_ordem_servico.unidade;
                Session["unidade_descricao"] = (info_ordem_servico.unidade == "") ? "TODAS AS UNIDADES" : info_ordem_servico.unidade;

                ViewBag.data_inicio = System.DateTime.Now.Date.AddMonths(-1).ToShortDateString();
                ViewBag.data_termino = System.DateTime.Now.Date.ToShortDateString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Session["codigo_modulo"].ToString());
                ViewBag.InfoOrdemServico = info_ordem_servico;
                ViewBag.PrincipaisOcorrencias = oHome.PrincipaisOcorrencias(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: unidade,
                                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                            bQualidade: true,
                                                                            iQuantidade: 30);
                ViewBag.plano_acao_status = oQualidade.LoadPlanoAcaoStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()));

                //ViewBag.formulario_visualizar = formulario_visualizar;
                ViewBag.codigo_tipo_unidade = iCodigoTipoUnidade;
                ViewBag.status = oQualidade.LoadAuditoriaStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: unidade);

                return View();
            }
        }

        #endregion

        #region ::: INDEX - AEB :::

        public ActionResult IndexAEB()
        {
            if (@Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                int iCodigoEmpresaPMOC = Convert.ToInt32(Session["empresa"].ToString());
                int iCodigoUnidadePMOC = Convert.ToInt32(Session["codigo_unidade"].ToString());
                int iCodigoTipoUnidade = 0;
                string sHotelOpera = "";

                oHome.DadosUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                   iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                   iCodigoEmpresaPMOC: ref iCodigoEmpresaPMOC,
                                   iCodigoUnidadePMOC: ref iCodigoUnidadePMOC,
                                   iCodigoTipoUnidade: ref iCodigoTipoUnidade,
                                   sHotelOpera: ref sHotelOpera);

                Session["codigo_empresa_pmoc"] = iCodigoEmpresaPMOC;
                Session["codigo_unidade_pmoc"] = iCodigoUnidadePMOC;
                Session["hotel_opera"] = sHotelOpera;

                bool bDashboardPreventiva = false;
                bool bDashboardLaudo = false;
                bool bDashboardRotina = false;


                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_preventiva",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardPreventiva);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_laudo",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardLaudo);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_rotina",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardRotina);

                ViewBag.dashboard_preventiva = bDashboardPreventiva;
                ViewBag.dashboard_laudo = bDashboardLaudo;
                ViewBag.dashboard_rotina = bDashboardRotina;

                ViewBag.InfoOrdemServico = oHome.InfoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                  iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                  iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                  bQualidade: false);
                ViewBag.data_inicio = System.DateTime.Now.Date.AddMonths(-1).ToShortDateString();
                ViewBag.data_termino = System.DateTime.Now.Date.ToShortDateString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.agrupado_por = new SelectList(oCombo.AgrupadoPorData(), "codigo", "descricao", null);
                ViewBag.grupo_item_medicao = new SelectList(oCombo.GrupoItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao");
                ViewBag.item_medicao = new SelectList(oCombo.ItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                            iCodigoGrupoItemMedicao: -1), "codigo", "descricao");
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Session["codigo_modulo"].ToString());
                ViewBag.forma_calculo = new SelectList(oCombo.FormaCalculoGreenPlanet(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao");
                ViewBag.ChartGauge = oHome.ChartGauge(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                      iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                      iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()));
                ViewBag.PrincipaisOcorrencias = oHome.PrincipaisOcorrencias(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                            bQualidade: false,
                                                                            iQuantidade: 30);

                ViewBag.codigo_tipo_unidade = iCodigoTipoUnidade;

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IndexAEB(int unidade, int modulo)
        {
            if (@Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Session["codigo_unidade"] = unidade;
                Session["codigo_modulo"] = modulo;

                if (modulo == 1)
                {
                    Session["dashboard"] = "Index";
                    Session["modulo"] = "MANUTENÇÃO";
                    Session["modulo_css"] = "text-primary";
                    return RedirectToAction("Index", new { unidade = unidade, modulo = modulo });
                }

                if (modulo == 2)
                {
                    Session["dashboard"] = "IndexQA";
                    Session["modulo"] = "QUALIDADE";
                    Session["modulo_css"] = "text-danger";
                    return RedirectToAction("IndexQA", new { unidade = unidade, modulo = modulo });
                }

                if (modulo == 3)
                {
                    Session["dashboard"] = "IndexGovernanca";
                    Session["modulo"] = "GOVERNANÇA";
                    Session["modulo_css"] = "text-success";
                    return RedirectToAction("IndexGovernanca", new { unidade = unidade, modulo = modulo });
                }

                int iCodigoEmpresaPMOC = 0;
                int iCodigoUnidadePMOC = 0;
                int iCodigoTipoUnidade = 0;
                string sHotelOpera = "";

                oHome.DadosUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                   iCodigoUnidade: unidade,
                                   iCodigoEmpresaPMOC: ref iCodigoEmpresaPMOC,
                                   iCodigoUnidadePMOC: ref iCodigoUnidadePMOC,
                                   iCodigoTipoUnidade: ref iCodigoTipoUnidade,
                                   sHotelOpera: ref sHotelOpera);

                Session["codigo_empresa_pmoc"] = iCodigoEmpresaPMOC;
                Session["codigo_unidade_pmoc"] = iCodigoUnidadePMOC;
                Session["hotel_opera"] = sHotelOpera;

                bool bDashboardPreventiva = false;
                bool bDashboardLaudo = false;
                bool bDashboardRotina = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_preventiva",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardPreventiva);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_laudo",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardLaudo);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_rotina",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardRotina);

                ViewBag.dashboard_preventiva = bDashboardPreventiva;
                ViewBag.dashboard_laudo = bDashboardLaudo;
                ViewBag.dashboard_rotina = bDashboardRotina;

                FormularioVisualizar formulario_visualizar = null;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    oFormularioVisualizar: ref formulario_visualizar);

                InfoOrdemServicoNew info_ordem_servico = oHome.InfoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                iCodigoUnidade: unidade,
                                                                                iCodigoModulo: modulo,
                                                                                bQualidade: false);

                Session["unidade"] = info_ordem_servico.unidade;
                Session["unidade_descricao"] = (info_ordem_servico.unidade == "") ? "TODAS AS UNIDADES" : info_ordem_servico.unidade;

                ViewBag.InfoOrdemServico = info_ordem_servico;
                ViewBag.data_inicio = System.DateTime.Now.Date.AddMonths(-1).ToShortDateString();
                ViewBag.data_termino = System.DateTime.Now.Date.ToShortDateString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.agrupado_por = new SelectList(oCombo.AgrupadoPorData(), "codigo", "descricao", null);
                ViewBag.grupo_item_medicao = new SelectList(oCombo.GrupoItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                    iCodigoUnidade: unidade), "codigo", "descricao");
                ViewBag.item_medicao = new SelectList(oCombo.ItemMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                         iCodigoUnidade: unidade,
                                                                         iCodigoGrupoItemMedicao: -1), "codigo", "descricao");
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                              iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Session["codigo_modulo"].ToString());
                ViewBag.forma_calculo = new SelectList(oCombo.FormaCalculoGreenPlanet(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao");
                ViewBag.ChartGauge = oHome.ChartGauge(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                      iCodigoUnidade: unidade,
                                                      iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()));
                ViewBag.PrincipaisOcorrencias = oHome.PrincipaisOcorrencias(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: unidade,
                                                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                            bQualidade: false,
                                                                            iQuantidade: 30);
                ViewBag.formulario_visualizar = formulario_visualizar;
                ViewBag.codigo_tipo_unidade = iCodigoTipoUnidade;

                return View();
            }
        }

        #endregion

        #region ::: INDEX - GOVERNANÇA :::

        public ActionResult IndexGovernanca()
        {

            if (@Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                int iCodigoEmpresaPMOC = Convert.ToInt32(Session["empresa"].ToString());
                int iCodigoUnidadePMOC = Convert.ToInt32(Session["codigo_unidade"].ToString());
                int iCodigoTipoUnidade = 0;
                string sHotelOpera = "";

                oHome.DadosUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                   iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                   iCodigoEmpresaPMOC: ref iCodigoEmpresaPMOC,
                                   iCodigoUnidadePMOC: ref iCodigoUnidadePMOC,
                                   iCodigoTipoUnidade: ref iCodigoTipoUnidade,
                                   sHotelOpera: ref sHotelOpera);

                Session["codigo_empresa_pmoc"] = iCodigoEmpresaPMOC;
                Session["codigo_unidade_pmoc"] = iCodigoUnidadePMOC;
                Session["hotel_opera"] = sHotelOpera;

                bool bDashboardPreventiva = false;
                bool bDashboardLaudo = false;
                bool bDashboardRotina = false;
                bool bDashboardPMOC = false;
                bool bDashboardUH = false;
                bool bDashboardGreenPlanet = false;


                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_preventiva",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardPreventiva);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_laudo",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardLaudo);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_rotina",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardRotina);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_pmoc",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardPMOC);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_uh",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardUH);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_green_planet",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardGreenPlanet);

                ViewBag.codigo_tipo_unidade = iCodigoTipoUnidade;
                ViewBag.dashboard_preventiva = bDashboardPreventiva;
                ViewBag.dashboard_laudo = bDashboardLaudo;
                ViewBag.dashboard_rotina = bDashboardRotina;
                ViewBag.dashboard_pmoc = bDashboardPMOC;
                ViewBag.dashboard_uh = bDashboardUH;
                ViewBag.dashboard_green_planet = bDashboardGreenPlanet;


                DateTime currentDate = DateTime.Now;
                DateTime primeiroDia = new DateTime(currentDate.Year, currentDate.Month, 1);
                DateTime ultimoDia = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));

                string dataInicio = primeiroDia.ToShortDateString();
                string dataTermino = ultimoDia.ToShortDateString();

                ViewBag.data = new SelectList(oCombo.DataGovernanca(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", ultimoDia.ToString("dd/MM/yyyy"));
                ViewBag.InfoOrdemServico = oHome.InfoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                  iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                  iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                  bQualidade: false);
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", Session["codigo_modulo"].ToString());

                ViewBag.nome_fantasia = Session["unidade"].ToString();

                ViewBag.dashboard_info = oGovernanca.DashboardInfo(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                   codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                   dataInicio: dataInicio,
                                                                   dataTermino: dataTermino);

                ViewBag.ChartGauge = oHome.ChartGauge(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                      iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                      iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()));

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
        public ActionResult IndexGovernanca(int unidade, int modulo, string data = "")
        {
            if (@Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                Session["codigo_unidade"] = unidade;
                Session["codigo_modulo"] = modulo;

                if (modulo == 1)
                {
                    Session["dashboard"] = "Index";
                    Session["modulo"] = "MANUTENÇÃO";
                    Session["modulo_css"] = "text-primary";
                    return RedirectToAction("Index", new { unidade = unidade, modulo = modulo });
                }

                if (modulo == 2)
                {
                    Session["dashboard"] = "IndexQA";
                    Session["modulo"] = "QUALIDADE";
                    Session["modulo_css"] = "text-danger";
                    return RedirectToAction("IndexQA", new { unidade = unidade, modulo = modulo });
                }

                if (modulo == 4)
                {
                    Session["dashboard"] = "IndexAEB";
                    Session["modulo"] = "AEB";
                    Session["modulo_css"] = "text-earth";
                    return RedirectToAction("IndexAEB", new { unidade = unidade, modulo = modulo });
                }

                int iCodigoEmpresaPMOC = 0;
                int iCodigoUnidadePMOC = 0;
                int iCodigoTipoUnidade = 0;
                string sHotelOpera = "";

                oHome.DadosUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                   iCodigoUnidade: unidade,
                                   iCodigoEmpresaPMOC: ref iCodigoEmpresaPMOC,
                                   iCodigoUnidadePMOC: ref iCodigoUnidadePMOC,
                                   iCodigoTipoUnidade: ref iCodigoTipoUnidade,
                                   sHotelOpera: ref sHotelOpera);

                Session["codigo_empresa_pmoc"] = iCodigoEmpresaPMOC;
                Session["codigo_unidade_pmoc"] = iCodigoUnidadePMOC;
                Session["hotel_opera"] = sHotelOpera;

                bool bDashboardPreventiva = false;
                bool bDashboardLaudo = false;
                bool bDashboardRotina = false;
                bool bDashboardPMOC = false;
                bool bDashboardUH = false;
                bool bDashboardGreenPlanet = false;


                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_preventiva",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardPreventiva);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_laudo",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardLaudo);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_rotina",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardRotina);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_pmoc",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardPMOC);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_uh",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardUH);

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "dashboard_green_planet",
                                    sDireito: "visualizar",
                                    bReturn: ref bDashboardGreenPlanet);

                ViewBag.codigo_tipo_unidade = iCodigoTipoUnidade;
                ViewBag.dashboard_preventiva = bDashboardPreventiva;
                ViewBag.dashboard_laudo = bDashboardLaudo;
                ViewBag.dashboard_rotina = bDashboardRotina;
                ViewBag.dashboard_pmoc = bDashboardPMOC;
                ViewBag.dashboard_uh = bDashboardUH;
                ViewBag.dashboard_green_planet = bDashboardGreenPlanet;

                FormularioVisualizar formulario_visualizar = null;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    oFormularioVisualizar: ref formulario_visualizar);

                InfoOrdemServicoNew info_ordem_servico = oHome.InfoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                iCodigoUnidade: unidade,
                                                                                iCodigoModulo: modulo,
                                                                                bQualidade: false);

                ViewBag.InfoOrdemServico = info_ordem_servico;
                Session["unidade"] = info_ordem_servico.unidade;
                Session["unidade_descricao"] = (info_ordem_servico.unidade == "") ? "TODAS AS UNIDADES" : info_ordem_servico.unidade;

                DateTime primeiroDia;
                DateTime ultimoDia;

                if (data == "")
                {
                    DateTime currentDate = DateTime.Now;

                    primeiroDia = new DateTime(currentDate.Year, currentDate.Month, 1);
                    ultimoDia = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                }
                else
                {
                    DateTime currentDate = DateTime.ParseExact(data, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    primeiroDia = new DateTime(currentDate.Year, currentDate.Month, 1);
                    ultimoDia = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));

                }

                string dataInicio = primeiroDia.ToShortDateString();
                string dataTermino = ultimoDia.ToShortDateString();

                ViewBag.data = new SelectList(oCombo.DataGovernanca(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", data);
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);

                ViewBag.modulo = new SelectList(oCombo.Modulo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())), "codigo", "descricao", modulo);
                ViewBag.dashboard_info = oGovernanca.DashboardInfo(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                   codigoUnidade: unidade,
                                                                   dataInicio: dataInicio,
                                                                   dataTermino: dataTermino);

                ViewBag.ChartGauge = oHome.ChartGauge(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                      iCodigoUnidade: unidade,
                                                      iCodigoModulo: modulo);

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

        [HttpPost]
        public JsonResult LoadProdutividadeCamareira(int empresa, int unidade, string data)
        {
            try
            {

                DateTime primeiroDia;
                DateTime ultimoDia;

                if (data == "")
                {
                    DateTime currentDate = DateTime.Now;
                    primeiroDia = new DateTime(currentDate.Year, currentDate.Month, 1);
                    ultimoDia = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                }
                else
                {
                    DateTime currentDate = DateTime.ParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    primeiroDia = new DateTime(currentDate.Year, currentDate.Month, 1);
                    ultimoDia = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                }

                return Json(oGovernanca.LoadChartProdutividadeCamareira(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        codigoUnidade: unidade,
                                                                        dataInicio: primeiroDia,
                                                                        dataTermino: ultimoDia));
            }
            catch (Exception ex)
            {
                // Tratamento de erro para lidar com formatos inválidos ou exceções
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult LoadProdutividadeVistoriador(int empresa, int unidade, string data)
        {
            try
            {

                DateTime primeiroDia;
                DateTime ultimoDia;

                if (data == "")
                {
                    DateTime currentDate = DateTime.Now;
                    primeiroDia = new DateTime(currentDate.Year, currentDate.Month, 1);
                    ultimoDia = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                }
                else
                {
                    DateTime currentDate = DateTime.ParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    primeiroDia = new DateTime(currentDate.Year, currentDate.Month, 1);
                    ultimoDia = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                }

                return Json(oGovernanca.LoadChartProdutividadeVistoriador(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          codigoUnidade: unidade,
                                                                          dataInicio: primeiroDia,
                                                                          dataTermino: ultimoDia));
            }
            catch (Exception ex)
            {
                // Tratamento de erro para lidar com formatos inválidos ou exceções
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult LoadNCCamareira(int empresa, int unidade, string data)
        {
            try
            {

                DateTime primeiroDia;
                DateTime ultimoDia;

                if (data == "")
                {
                    DateTime currentDate = DateTime.Now;
                    primeiroDia = new DateTime(currentDate.Year, currentDate.Month, 1);
                    ultimoDia = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                }
                else
                {
                    DateTime currentDate = DateTime.ParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    primeiroDia = new DateTime(currentDate.Year, currentDate.Month, 1);
                    ultimoDia = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                }

                return Json(oGovernanca.LoadNCCamareira(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        codigoUnidade: unidade,
                                                        dataInicio: primeiroDia,
                                                        dataTermino: ultimoDia));
            }
            catch (Exception ex)
            {
                // Tratamento de erro para lidar com formatos inválidos ou exceções
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult LoadNCDetalhado(int empresa, int unidade, string data)
        {
            try
            {

                DateTime primeiroDia;
                DateTime ultimoDia;

                if (data == "")
                {
                    DateTime currentDate = DateTime.Now;
                    primeiroDia = new DateTime(currentDate.Year, currentDate.Month, 1);
                    ultimoDia = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                }
                else
                {
                    DateTime currentDate = DateTime.ParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    primeiroDia = new DateTime(currentDate.Year, currentDate.Month, 1);
                    ultimoDia = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                }

                return Json(oGovernanca.LoadNCDetalhado(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        codigoUnidade: unidade,
                                                        dataInicio: primeiroDia,
                                                        dataTermino: ultimoDia));
            }
            catch (Exception ex)
            {
                // Tratamento de erro para lidar com formatos inválidos ou exceções
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult LoadRankingCamareira(int empresa, int unidade, string data)
        {
            try
            {

                DateTime primeiroDia;
                DateTime ultimoDia;

                if (data == "")
                {
                    DateTime currentDate = DateTime.Now;
                    primeiroDia = new DateTime(currentDate.Year, currentDate.Month, 1);
                    ultimoDia = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                }
                else
                {
                    DateTime currentDate = DateTime.ParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    primeiroDia = new DateTime(currentDate.Year, currentDate.Month, 1);
                    ultimoDia = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                }

                return Json(oGovernanca.RankingCamareira(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                         codigoUnidade: unidade,
                                                         dataInicio: primeiroDia,
                                                         dataTermino: ultimoDia));
            }
            catch (Exception ex)
            {
                // Tratamento de erro para lidar com formatos inválidos ou exceções
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
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
                                                                                                               dataInicio: startDate,
                                                                                                               dataTermino: endDate);

                return Json(result, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public JsonResult ChartProdutividadeCamareira(int unidade, string dataInicio, string dataTermino)
        //{
        //    try
        //    {
        //        // Convertendo as strings para DateTime
        //        DateTime startDate = DateTime.ParseExact(dataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //        DateTime endDate = DateTime.ParseExact(dataTermino, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        //        // Gerando as datas entre o intervalo
        //        List<dashboardGovernancaChartProdutividade> result = oGovernanca.LoadChartProdutividadeCamareira(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
        //                                                                                                         codigoUnidade: unidade,
        //                                                                                                         dataInicio: dataInicio,
        //                                                                                                         dataTermino: dataTermino);
        //        var chartData = new
        //        {
        //            labels = result.Select(r => r.unidade).ToArray(), // Eixo X: Unidade
        //            datasets = new[]
        //            {
        //                new {
        //                    label = "Percentual de Produtividade (%)",
        //                    data = result.Select(r => decimal.TryParse(r.percentual, out var val) ? val : 0).ToArray(),
        //                    backgroundColor = "rgba(54, 162, 235, 0.5)",
        //                    borderColor = "rgba(54, 162, 235, 1)",
        //                    borderWidth = 1
        //                }
        //            }
        //        };

        //        return Json(chartData, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception ex)
        //    {
        //        // Tratamento de erro para lidar com formatos inválidos ou exceções
        //        return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //[HttpPost]
        //public JsonResult ChartProdutividadeVistoriador(int unidade, string dataInicio, string dataTermino)
        //{
        //    try
        //    {
        //        // Convertendo as strings para DateTime
        //        DateTime startDate = DateTime.ParseExact(dataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //        DateTime endDate = DateTime.ParseExact(dataTermino, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        //        // Gerando as datas entre o intervalo
        //        List<dashboardGovernancaChartProdutividade> result = oGovernanca.LoadChartProdutividadeVistoriador(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
        //                                                                                                           codigoUnidade: unidade,
        //                                                                                                           dataInicio: dataInicio,
        //                                                                                                           dataTermino: dataTermino);
        //        var chartData = new
        //        {
        //            labels = result.Select(r => r.unidade).ToArray(),
        //            datasets = new[]
        //            {
        //                new {
        //                    label = "Percentual de Produtividade (%)",
        //                    data = result.Select(r => decimal.TryParse(r.percentual, out var val) ? val : 0).ToArray(),
        //                    backgroundColor = "rgba(54, 162, 235, 0.5)",
        //                    borderColor = "rgba(54, 162, 235, 1)",
        //                    borderWidth = 1
        //                }
        //            }
        //        };

        //        return Json(chartData, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception ex)
        //    {
        //        // Tratamento de erro para lidar com formatos inválidos ou exceções
        //        return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        #endregion

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

        #region ::: DASHBOARD ATUAL :::

        public ActionResult DashboardAtual(string data = "")
        {

            data = (data == "") ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToShortDateString() : data;

            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());

                InfoOrdemServicoNew info_ordem_servico = oHome.InfoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                                bQualidade: false);

                ViewBag.nome_fantasia = info_ordem_servico.unidade;

                ViewBag.data = new SelectList(oCombo.DataDashboard(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", data);

                ViewBag.dashboard_info = oDashboard.DashboardInfo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                    sData: data);

                ViewBag.manutencao_laudo = oDashboard.ManutencaoLaudo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                        sData: data,
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                        iCodigoTipoOrdemServico: 6,
                                                                        bRotina: false);


                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DashboardAtual(int unidade, string data = "")
        {
            if (@Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());

                InfoOrdemServicoNew info_ordem_servico = oHome.InfoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                iCodigoUnidade: unidade,
                                                                                iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                                bQualidade: false);

                ViewBag.nome_fantasia = info_ordem_servico.unidade;

                ViewBag.data = new SelectList(oCombo.DataDashboard(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", data);

                ViewBag.dashboard_info = oDashboard.DashboardInfo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                    sData: data);


                ViewBag.manutencao_laudo = oDashboard.ManutencaoLaudo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: unidade,
                                                                        sData: data,
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                        iCodigoTipoOrdemServico: 6,
                                                                        bRotina: false);

                return View();
            }

        }

        #endregion

        #region "::: DESEMPENHO :::"

        public ActionResult Desempenho(int unidade = -1, string data = "")
        {

            data = (data == "") ? new DateTime(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.DaysInMonth(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month)).ToShortDateString() : data;

            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                Session["codigo_unidade"] = unidade;

                if (unidade == -1)
                {
                    return RedirectToAction("DesempenhoUnidades", new { data = data });
                }
                else
                {
                    return RedirectToAction("DashboardAtual", new { data = data });
                }

            }
        }

        public ActionResult DesempenhoUnidades(string data = "")
        {

            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                data = (data == "") ? new DateTime(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.DaysInMonth(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month)).ToShortDateString() : data;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());

                ViewBag.data = new SelectList(oCombo.DataDashboard(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", data);

                ViewBag.dashboard_info = oDashboard.DashboardInfo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                    sData: data);

                ViewBag.ranking_unidades = oDashboard.RankingUnidades(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        sData: data);

                ViewBag.notas_unidades = oDashboard.NotasUnidades(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    sData: data);

                ViewBag.percentual_nota = oDashboard.Percentual(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()));

                ViewBag.pmoc = oDashboard.MetricaUnidades(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            sData: data,
                                                            sField: "pmoc");

                ViewBag.laudo = oDashboard.MetricaUnidades(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            sData: data,
                                                            sField: "laudo");

                ViewBag.rotina = oDashboard.MetricaUnidades(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            sData: data,
                                                            sField: "rotina");

                ViewBag.preventiva = oDashboard.MetricaUnidades(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                sData: data,
                                                                sField: "preventiva");

                ViewBag.uh_dia = oDashboard.MetricaUnidades(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            sData: data,
                                                            sField: "uh_dia");

                ViewBag.atendimento_os = oDashboard.AtendimentoOrdemServico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            sData: data);

                ViewBag.apontamento_horas = oDashboard.ApontamentoHoras(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        sData: data);

                ViewBag.manutencao_laudo = oDashboard.ManutencaoLaudo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                        sData: data,
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString()),
                                                                        iCodigoTipoOrdemServico: 6,
                                                                        bRotina: false);

                return View();
            }

        }

        #endregion

        #region "::: QUALIDADE :::"

        public ActionResult QualidadeDesempenho(int unidade = -1, string data = "")
        {

            data = (data == "") ? new DateTime(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.DaysInMonth(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month)).ToShortDateString() : data;

            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                Session["codigo_unidade"] = unidade;

                if (unidade == -1)
                {
                    return RedirectToAction("QualidadeDesempenhoTodasUnidades", new { data = data });
                }
                else
                {
                    return RedirectToAction("QualidadeDesempenhoUnidade", new { unidade = unidade, data = data });
                }

            }
        }

        public ActionResult QualidadeDesempenhoUnidade(string data = "")
        {

            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                data = (data == "") ? new DateTime(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.DaysInMonth(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month)).ToShortDateString() : data;

                DateTime oDate = Convert.ToDateTime(data);

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.data = new SelectList(oCombo.DataDashboard(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", data);
                ViewBag.mes = Convert.ToDateTime(data).ToString("MMM/yyyy");
                ViewBag.inicio_mes = new DateTime(oDate.Year, oDate.Month, 1).ToShortDateString();
                ViewBag.termino_mes = new DateTime(oDate.Year, oDate.Month, DateTime.DaysInMonth(oDate.Year, oDate.Month)).ToShortDateString();
                ViewBag.nota = oDashboard.QualidadeNotasUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                sData: data);
                ViewBag.plano_acao = oDashboard.QualidadePlanoAcaoUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                            sData: data);
                ViewBag.plano_acao_justificativa = oDashboard.QualidadePlanoAcaoJustificativaUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                                        sData: data);
                ViewBag.plano_acao_recorrencia = oDashboard.QualidadePlanoAcaoRecorrenciaUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                                                    iFiltro: 1);

                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QualidadeDesempenhoUnidade(int unidade, string data)
        {

            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                data = (data == "") ? new DateTime(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.DaysInMonth(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month)).ToShortDateString() : data;

                DateTime oDate = Convert.ToDateTime(data);

                Session["codigo_unidade"] = unidade;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.data = new SelectList(oCombo.DataDashboard(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: unidade), "codigo", "descricao", data);
                ViewBag.inicio_mes = new DateTime(oDate.Year, oDate.Month, 1).ToShortDateString();
                ViewBag.termino_mes = new DateTime(oDate.Year, oDate.Month, DateTime.DaysInMonth(oDate.Year, oDate.Month)).ToShortDateString();
                ViewBag.mes = Convert.ToDateTime(data).ToString("MMM/yyyy");
                ViewBag.nota = oDashboard.QualidadeNotasUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: unidade,
                                                                sData: data);
                ViewBag.plano_acao = oDashboard.QualidadePlanoAcaoUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: unidade,
                                                                            sData: data);
                ViewBag.plano_acao_justificativa = oDashboard.QualidadePlanoAcaoJustificativaUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                        iCodigoUnidade: unidade,
                                                                                                        sData: data);
                ViewBag.plano_acao_recorrencia = oDashboard.QualidadePlanoAcaoRecorrenciaUnidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                    iCodigoUnidade: unidade,
                                                                                                    iFiltro: 1);

                return View();
            }

        }

        public ActionResult QualidadeDesempenhoTodasUnidades(string data = "")
        {

            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                data = (data == "") ? new DateTime(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.DaysInMonth(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month)).ToShortDateString() : data;

                DateTime oDate = Convert.ToDateTime(data);

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.data = new SelectList(oCombo.DataDashboard(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", data);
                ViewBag.mes = Convert.ToDateTime(data).ToString("MMM/yyyy");
                ViewBag.inicio_mes = new DateTime(oDate.Year, oDate.Month, 1).ToShortDateString();
                ViewBag.termino_mes = new DateTime(oDate.Year, oDate.Month, DateTime.DaysInMonth(oDate.Year, oDate.Month)).ToShortDateString();
                ViewBag.plano_acao = oDashboard.QualidadePlanoAcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    sData: data);
                ViewBag.nota = oDashboard.QualidadeNotas(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            sData: data);

                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QualidadeDesempenhoTodasUnidades(int unidade, string data = "")
        {
            if (@Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                data = (data == "") ? new DateTime(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.DaysInMonth(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month)).ToShortDateString() : data;

                DateTime oDate = Convert.ToDateTime(data);

                Session["codigo_unidade"] = unidade;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", unidade);
                ViewBag.data = new SelectList(oCombo.DataDashboard(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: unidade), "codigo", "descricao", data);
                ViewBag.inicio_mes = new DateTime(oDate.Year, oDate.Month, 1).ToShortDateString();
                ViewBag.termino_mes = new DateTime(oDate.Year, oDate.Month, DateTime.DaysInMonth(oDate.Year, oDate.Month)).ToShortDateString();
                ViewBag.mes = Convert.ToDateTime(data).ToString("MMM/yyyy");
                ViewBag.plano_acao = oDashboard.QualidadePlanoAcao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            sData: data);
                ViewBag.nota = oDashboard.QualidadeNotas(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            sData: data);

                return View();
            }

        }

        #endregion

        #region "::: DESEMPENHO :::"

        public ActionResult DesempenhoGovernanca()
        {


            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                string data = new DateTime(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.DaysInMonth(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month)).ToShortDateString();

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: false), "codigo", "descricao", Convert.ToInt32(Session["codigo_unidade"].ToString()));

                ViewBag.data = new SelectList(oCombo.DataDashboard(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", data);

                return View();
            }

        }

        [HttpPost]
        public JsonResult LoadDesempenhoGovernanca(int unidade = -1, string data = "")
        {
            return Json(oDashboard.DashboardGovernancaInfo(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                  codigoUnidade: unidade,
                                                                  data: data));

        }

        #endregion

        #region "::: NOTIFICAÇÃO :::"

        public JsonResult Notificacao()
        {
            if (User.Identity.GetUserName() != null && Session["empresa"] != null)
            {
                return Json(oHome.LoadNotificacao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName())));
            }
            else
            {
                return Json("");
            }
        }

        public JsonResult NotificacaoOSHospede()
        {
            if (User.Identity.GetUserName() != null && Session["empresa"] != null && Session["codigo_unidade"] != null && Session["codigo_unidade"].ToString() != "-1")
            {
                return Json(oHome.LoadNotificacaoOSHospede(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                           codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                           codigoUsuario: Convert.ToInt32(User.Identity.GetUserName())));
            }
            else
            {
                return Json("");
            }
        }

        public void NotificacaoVista(int codigo_empresa, long codigo)
        {
            oHome.SetNotificacao(iCodigoEmpresa: codigo_empresa,
                                    lCodigo: codigo);
        }

        #endregion

        #region "::: IOs :::"

        public ActionResult LoadIOsLink()
        {
            return Redirect(oHome.LoadLinkIOS());
        }

        #endregion

    }
}