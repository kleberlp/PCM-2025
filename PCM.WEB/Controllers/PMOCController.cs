using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.AspNet.Identity;
using PCM.WEB.DAL;
using PCM.WEB.MODELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace PCM.WEB.Controllers
{
    public class PMOCController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private PMOC oPMOC = new PMOC(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private CadastroBasico oCadastroBasico = new CadastroBasico(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: JSON :::

        //JSON: /APARTAMENTO/
        public JsonResult LoadApartamento(int unidade, int setor = -1)
        {
            return Json(oCombo.Apartamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                            iCodigoUnidade: unidade, 
                                            iCodigoSetor: setor));
        }

        //JSON: /SETORAud
        public JsonResult LoadSetor(int unidade)
        {
            return Json(oCombo.Setor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUnidade: unidade));
        }

        //JSON: /AR CONDICIONADO/
        public JsonResult LoadArCondicionado(int unidade)
        {
            return Json(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: unidade));
        }

        //JSON: /AR CONDICIONADO/
        public JsonResult LoadArCondicionadoPMOC(int unidade)
        {
            return Json(oCombo.ArCondicionadoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                  iCodigoUnidade: unidade));
        }

        //JSON: /AR CONDICIONADO - SETOR/
        public JsonResult LoadArCondicionadoSetor(int unidade, int setor)
        {
            return Json(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: unidade,
                                            iCodigoSetor: setor));
        }

        //JSON: /FUNCIONÁRIO/
        public JsonResult LoadFuncionario(int unidade)
        {
            return Json(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: unidade,
                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])));
        }

        //JSON: /FUNCIONÁRIO/
        public JsonResult LoadFuncionarioPMOC(int empresa, int unidade)
        {
            return Json(oCombo.Funcionario(iCodigoEmpresa: empresa,
                                            iCodigoUnidade: unidade,
                                            iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])));
        }

        //JSON: /UNIDADE/
        public JsonResult LoadUnidade()
        {
            return Json(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        bCadastro: false));
        }

        //JSON: /UNIDADE/
        public JsonResult LoadEmpresaPMOC(int empresa, int unidade)
        {
            return Json(oCombo.EmpresaPMOC(iCodigoEmpresa: empresa,
                                            iCodigoUnidade: unidade));
        }

        //JSON: /UNIDADE/
        public JsonResult LoadUnidadePMOC(int empresa, int unidade)
            {
                    return Json(oCombo.UnidadePMOC(iCodigoEmpresa: empresa,
                                                iCodigoUnidade: unidade));
            }

        #endregion

        #region ::: CEP :::

        //JSON: /CEP/
        public JsonResult CEPInfo(string cep)
            {
                return Json(oCadastroBasico.CEPInternet(cep));
            }

        #endregion

        #region ::: PMOC :::

        // GET: INDEX
        public ActionResult PMOC(int status = -1)
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
                                    sFormulario: "pmoc_apontamento",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.administrador = administrador;
                ViewBag.status = oPMOC.LoadPMOCStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()));
                    
                return View(oPMOC.LoadPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                            iStatus: status));
            }
        }

        #endregion

        #region ::: PMOC2 :::

        // GET: INDEX
        public ActionResult PMOC2(int status = -1, string type = "grid")
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
                                    sFormulario: "pmoc_apontamento",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.administrador = administrador;
                ViewBag.status = oPMOC.LoadPMOCStatus(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()));
                ViewBag.type = type;
                ViewBag.status_filtro = status;

                return View(oPMOC.LoadPMOC2(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                            iStatus: status));
            }
        }

        #endregion

        #region ::: PMOC - BUp :::

        // GET: INDEX
        public ActionResult PMOCBup()
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
                int status = -1;
                string data = "";
                string type = "grid";

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pmoc_apontamento",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                data = data == "" ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd") : data;

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.administrador = administrador;
                ViewBag.statusId = status;
                ViewBag.data = new SelectList(oCombo.MesAno(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", data);
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.type = type;
                ViewBag.status_filtro = status;

                return View(oPMOC.LoadPMOCMesFechado(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                     iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                     iStatus: status,
                                                     sData: data));
            }
        }

        // GET: INDEX
        [HttpPost]
        public ActionResult PMOCBup(int status = -1, string type = "grid", string data = "")
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
                                    sFormulario: "pmoc_apontamento",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                data = data == "" ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd") : data;

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.administrador = administrador;
                ViewBag.statusId = status;
                ViewBag.data = new SelectList(oCombo.MesAno(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", data);
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.type = type;
                ViewBag.status_filtro = status;

                return View(oPMOC.LoadPMOCMesFechado(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                     iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                     iStatus: status,
                                                     sData: data));
            }
        }

        #endregion

        #region ::: ORDEM DE SERVIÇO :::

        // GET: ORDEM DE SERVIÇO - PMOC
        public ActionResult OrdemServicoPMOC(int codigo_unidade, long codigo_pmoc_ordem_servico, string view)
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
                                        sFormulario: "pmoc_apontamento",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;
                    ViewBag.inserir = inserir;
                    ViewBag.administrador = administrador;
                    
                    PMOCOrdemServico ordem_servico;

                    ordem_servico = oPMOC.OrdemServicoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                           iCodigoUnidade: codigo_unidade,
                                                           lCodigoPMOCOrdemServico: codigo_pmoc_ordem_servico);

                    ViewBag.ordem_servico = ordem_servico;

                    ViewBag.view = view;

                    return View(ordem_servico.checklist);
                }
            }

        // POST: ORDEM DE SERVIÇO - PMOC
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrdemServicoPMOC(int codigo_unidade, long codigo_equipamento, long codigo_pmoc_ordem_servico, List<PMOCOrdemServicoChecklist> checklist, HttpPostedFileBase arquivo, string view, bool concluido = false)
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
                    sPath = Server.MapPath(Path.Combine("~/Content/img/Cliente/PMOC", Session["empresa"].ToString()));
                    sFileName = DateTime.Now.ToString("yyMMdd_HHmmss_") + arquivo.FileName.Replace("+", "").Replace("-", "").Replace(" ", "");
                    if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                    if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                    arquivo.SaveAs(Path.Combine(sPath, sFileName));
                }

                //Insere Ordem de Serviço
                oPMOC.UpdateOrdemServicoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigoUnidade: codigo_unidade,
                                                lCodigoPMOCOrdemServico: codigo_pmoc_ordem_servico,
                                                sArquivo: (arquivo != null) ? arquivo.FileName : "",
                                                sPathArquivo: Path.Combine("~/Content/img/Cliente/PMOC", Session["empresa"].ToString(), sFileName),
                                                bConcluido: concluido);

                //Insere Checklist
                if (checklist != null)
                {

                    foreach (PMOCOrdemServicoChecklist item in checklist)
                    {

                        //Insere Registro no Banco de Dados
                        oPMOC.InsertChecklistPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    lCodigoPMOCOrdemServico: codigo_pmoc_ordem_servico,
                                                    iCodigoChecklistItem: item.codigo,
                                                    sResultado: item.resultado,
                                                    sObservacao: item.observacao);

                    }

                }

                return RedirectToAction("OrdemServicoPMOC", "PMOC", new { codigo_unidade = codigo_unidade, codigo_pmoc_ordem_servico = codigo_pmoc_ordem_servico, view = view });
            }
        }

        // GET: ORDEM DE SERVIÇO - PMOC / VIEW
        public ActionResult OrdemServicoPMOCView(int codigo_empresa, int codigo_unidade, long codigo_pmoc_ordem_servico, string view)
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

                oAccount.LoadPerfil(iCodigoEmpresa: codigo_empresa,
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pmoc_apontamento",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.inserir = inserir;
                ViewBag.administrador = administrador;

                PMOCOrdemServico ordem_servico;

                ordem_servico = oPMOC.OrdemServicoPMOC(iCodigoEmpresa: codigo_empresa,
                                                        iCodigoUnidade: codigo_unidade,
                                                        lCodigoPMOCOrdemServico: codigo_pmoc_ordem_servico);

                ViewBag.ordem_servico = ordem_servico;

                ViewBag.view = view;

                return View(ordem_servico.checklist);
            }
        }

        // GET: /APONTAMENTO
        public ActionResult ApontamentoPMOC(long codigo_equipamento, int codigo_unidade, string data_manutencao, string view)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                PMOCApontamento apontamento = null;

                oPMOC.LoadApontamentoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            lCodigoEquipamento: codigo_equipamento,
                                            iCodigoUnidade: codigo_unidade,
                                            oPMOCApontamento: ref apontamento);

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
                                    sFormulario: "pmoc_apontamento",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.aponta_horas = true;
                ViewBag.administrador = administrador;
                ViewBag.administrador_string = (administrador == true)? "": "readonly";
                ViewBag.inserir = inserir;
                ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.pmoc = apontamento;
                ViewBag.view = view;
                ViewBag.justificativa_apontamento = new SelectList(oCombo.JustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                    iCodigoUnidade: codigo_unidade), "codigo", "descricao", null);
                ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), 
                                                                        iCodigoUnidade: codigo_unidade), "codigo", "descricao", apontamento.codigo_fornecedor);
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade,
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", apontamento.codigo_funcionario);
                    
                return View(oPMOC.LoadApontamentoCheckListPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                lCodigoEquipamento: codigo_equipamento,
                                                                sDataManutencao: data_manutencao,
                                                                iCodigoUnidade: codigo_unidade));
            }
        }

        // POST: /APONTAMENTO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApontamentoPMOC(string data_inicio, string data_termino, int codigo_unidade, long codigo_equipamento, List<PMOCOrdemServicoChecklist> checklist, HttpPostedFileBase arquivo, string view, int[] funcionario, int fornecedor = -1, string hora_inicio = "00:00", string hora_termino = "00:00", bool concluido = false)
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
                    sPath = Server.MapPath(Path.Combine("~/Content/img/Cliente/PMOC", Session["empresa"].ToString()));
                    sFileName = DateTime.Now.ToString("yyMMdd_HHmmss_") + arquivo.FileName.Replace("+", "").Replace("-", "").Replace(" ", "");
                    if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                    if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                    arquivo.SaveAs(Path.Combine(sPath, sFileName));
                }

                long codigo = 0;

                //Insere Ordem de Serviço
                oPMOC.InsertOrdemServicoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                             iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                             iCodigoUnidade: codigo_unidade,
                                             lCodigoEquipamento: codigo_equipamento,
                                             sArquivo: (arquivo != null) ? arquivo.FileName : "",
                                             sPathArquivo: Path.Combine("~/Content/img/Cliente/PMOC", Session["empresa"].ToString(), sFileName),
                                             bConcluido: concluido,
                                             lCodigoPMOCOrdemServico: ref codigo);

                //Insere Apontamento
                if (funcionario == null)
                {

                    oPMOC.InsertApontamentoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigoUnidade: codigo_unidade,
                                                lCodigoPMOCOrdemServico: codigo,
                                                iCodigoFornecedor: fornecedor,
                                                iCodigoFuncionario: -1,
                                                sDataInicio: data_inicio,
                                                sDataTermino: data_termino,
                                                sHoraInicio: hora_inicio,
                                                sHoraTermino: hora_termino);
                }
                else
                {

                    for (int i = 0; i < funcionario.Length; i++)
                    {

                        oPMOC.InsertApontamentoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    lCodigoPMOCOrdemServico: codigo,
                                                    iCodigoFornecedor: fornecedor,
                                                    iCodigoFuncionario: funcionario[i],
                                                    sDataInicio: data_inicio,
                                                    sDataTermino: data_termino,
                                                    sHoraInicio: hora_inicio,
                                                    sHoraTermino: hora_termino);
                    }
                }

                //Insere Checklist
                if (checklist != null)
                {

                    foreach (PMOCOrdemServicoChecklist item in checklist)
                    {

                        //Insere Registro no Banco de Dados
                        oPMOC.InsertChecklistPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                  iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                  iCodigoUnidade: codigo_unidade,
                                                  lCodigoPMOCOrdemServico: codigo,
                                                  iCodigoChecklistItem: item.codigo,
                                                  sResultado: item.resultado,
                                                  sObservacao: item.observacao);

                    }

                }

                return RedirectToAction("PMOC2", "PMOC");
            }
        }

        // GET: /APONTAMENTO
        public ActionResult ApontamentoPMOCBUp(long codigo_equipamento, int codigo_unidade, int intervalo)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                PMOCApontamento apontamento = null;

                oPMOC.LoadApontamentoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            lCodigoEquipamento: codigo_equipamento,
                                            iCodigoUnidade: codigo_unidade,
                                            oPMOCApontamento: ref apontamento);

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pmoc_apontamento",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.aponta_horas = true;
                ViewBag.administrador = administrador;
                ViewBag.administrador_string = (administrador == true) ? "" : "readonly";
                ViewBag.inserir = inserir;
                ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.pmoc = apontamento;
                ViewBag.intervalo = intervalo;
                ViewBag.justificativa_apontamento = new SelectList(oCombo.JustificativaApontamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                                    iCodigoUnidade: codigo_unidade), "codigo", "descricao", null);
                ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade), "codigo", "descricao", apontamento.codigo_fornecedor);
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade,
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", apontamento.codigo_funcionario);

                return View(oPMOC.LoadApontamentoCheckListPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               lCodigoEquipamento: codigo_equipamento,
                                                               iIntervalo: intervalo,
                                                               iCodigoUnidade: codigo_unidade));
            }
        }

        // POST: /APONTAMENTO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApontamentoPMOCBUp(string data_inicio, string data_termino, int codigo_unidade, long codigo_equipamento, List<PMOCOrdemServicoChecklist> checklist, HttpPostedFileBase arquivo, string view, int[] funcionario, int fornecedor = -1, string hora_inicio = "00:00", string hora_termino = "00:00", bool concluido = false, int intervalo = 2)
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
                    sPath = Server.MapPath(Path.Combine("~/Content/img/Cliente/PMOC", Session["empresa"].ToString()));
                    sFileName = DateTime.Now.ToString("yyMMdd_HHmmss_") + arquivo.FileName.Replace("+", "").Replace("-", "").Replace(" ", "");
                    if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
                    if (System.IO.File.Exists(Path.Combine(sPath, sFileName))) System.IO.File.Delete(Path.Combine(sPath, sFileName));
                    arquivo.SaveAs(Path.Combine(sPath, sFileName));
                }

                long codigo = 0;

                //Insere Ordem de Serviço
                oPMOC.InsertOrdemServicoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                             iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                             iCodigoUnidade: codigo_unidade,
                                             lCodigoEquipamento: codigo_equipamento,
                                             iIntervalo: intervalo,
                                             sArquivo: (arquivo != null) ? arquivo.FileName : "",
                                             sPathArquivo: Path.Combine("~/Content/img/Cliente/PMOC", Session["empresa"].ToString(), sFileName),
                                             bConcluido: concluido,
                                             lCodigoPMOCOrdemServico: ref codigo);

                //Insere Apontamento
                if (funcionario == null)
                {

                    oPMOC.InsertApontamentoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                iCodigoUnidade: codigo_unidade,
                                                lCodigoPMOCOrdemServico: codigo,
                                                iCodigoFornecedor: fornecedor,
                                                iCodigoFuncionario: -1,
                                                sDataInicio: data_inicio,
                                                sDataTermino: data_termino,
                                                sHoraInicio: hora_inicio,
                                                sHoraTermino: hora_termino);
                }
                else
                {

                    for (int i = 0; i < funcionario.Length; i++)
                    {

                        oPMOC.InsertApontamentoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: codigo_unidade,
                                                    lCodigoPMOCOrdemServico: codigo,
                                                    iCodigoFornecedor: fornecedor,
                                                    iCodigoFuncionario: funcionario[i],
                                                    sDataInicio: data_inicio,
                                                    sDataTermino: data_termino,
                                                    sHoraInicio: hora_inicio,
                                                    sHoraTermino: hora_termino);
                    }
                }

                //Insere Checklist
                if (checklist != null)
                {

                    foreach (PMOCOrdemServicoChecklist item in checklist)
                    {

                        //Insere Registro no Banco de Dados
                        oPMOC.InsertChecklistPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                  iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                  iCodigoUnidade: codigo_unidade,
                                                  lCodigoPMOCOrdemServico: codigo,
                                                  iCodigoChecklistItem: item.codigo,
                                                  sResultado: item.resultado,
                                                  sObservacao: item.observacao);

                    }

                }

                return RedirectToAction("PMOCBup", "PMOC");
            }
        }

        //JSON: /UNIDADE/
        public JsonResult ApontamentoPMOCJustificativa(int unidade, long equipamento, int justificativa_apontamento)
        {
            oPMOC.InsertApontamentoPMOCJustificativa(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                     iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                     iCodigoUnidade: unidade,
                                                     lCodigoEquipamento: equipamento,
                                                     iCodigoJustificativaApontamento: justificativa_apontamento);

            return Json("OK");
        }

        // GET: /APONTAMENTO EDITAR
        public ActionResult ApontamentoPMOCEdit(long codigo_pmoc_ordem_servico, long codigo_apontamento, int codigo_unidade, string view)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                PMOCApontamento apontamento = null;

                oPMOC.LoadApontamentoPMOCInfo(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                lCodigoPMOCOrdemServico: codigo_pmoc_ordem_servico,
                                                iCodigoUnidade: codigo_unidade,
                                                lCodigoPMOCApontamento: codigo_apontamento,
                                                oPMOCApontamento: ref apontamento);

                if (apontamento == null)
                {
                    return HttpNotFound();
                }

                ViewBag.view = view;
                ViewBag.apontamento = apontamento;
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade,
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", apontamento.codigo_funcionario);
                ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade), "codigo", "descricao", apontamento.codigo_fornecedor);

                return View();
            }
        }

        // POST: /APONTAMENTO EDITAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApontamentoPMOCEdit(string data_inicio, string data_termino, int codigo_unidade, long codigo_pmoc_ordem_servico, long codigo_apontamento, string view, int funcionario = -1, int fornecedor = -1, string hora_inicio = "00:00", string hora_termino = "00:00")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                oPMOC.UpdateApontamentoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                            iCodigoUnidade: codigo_unidade,
                                            lCodigoApontamento: codigo_apontamento,
                                            lCodigoPMOCOrdemServico: codigo_pmoc_ordem_servico,
                                            iCodigoFornecedor: fornecedor,
                                            iCodigoFuncionario: funcionario,
                                            sDataInicio: data_inicio,
                                            sDataTermino: data_termino,
                                            sHoraInicio: hora_inicio,
                                            sHoraTermino: hora_termino);

                return RedirectToAction("OrdemServicoPMOC", "PMOC", new { codigo_unidade = codigo_unidade, codigo_pmoc_ordem_servico = codigo_pmoc_ordem_servico, view = view });
            }
        }

        // GET: /APONTAMENTO INSERT
        public ActionResult ApontamentoPMOCInsert(long codigo_pmoc_ordem_servico, int codigo_unidade, string view)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade,
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"])), "codigo", "descricao", null);
                ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: codigo_unidade), "codigo", "descricao", null);
                ViewBag.ordem_servico = oPMOC.OrdemServicoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: codigo_unidade,
                                                                lCodigoPMOCOrdemServico: codigo_pmoc_ordem_servico);
                ViewBag.view = view;
                    
                return View();
            }
        }

        // POST: /APONTAMENTO INSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApontamentoPMOCInsert(string data_inicio, string data_termino, int codigo_unidade, long codigo_pmoc_ordem_servico, string view, int funcionario = -1, int fornecedor = -1, string hora_inicio = "00:00", string hora_termino = "00:00")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                oPMOC.InsertApontamentoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                            iCodigoUnidade: codigo_unidade,
                                            lCodigoPMOCOrdemServico: codigo_pmoc_ordem_servico,
                                            iCodigoFornecedor: fornecedor,
                                            iCodigoFuncionario: funcionario,
                                            sDataInicio: data_inicio,
                                            sDataTermino: data_termino,
                                            sHoraInicio: hora_inicio,
                                            sHoraTermino: hora_termino);

                return RedirectToAction("OrdemServicoPMOC", "PMOC", new { codigo_unidade = codigo_unidade, codigo_pmoc_ordem_servico = codigo_pmoc_ordem_servico, view = view });
            }
        }

        // GET: /APONTAMENTO EXCLUIR
        public bool ApontamentoPMOCExcluir(long codigo_apontamento, int codigo_unidade, long codigo_pmoc_ordem_servico)
        {

            try
            {
                oPMOC.DeleteApontamentoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                            iCodigoUnidade: codigo_unidade,
                                            lCodigoPMOCOrdemServico: codigo_pmoc_ordem_servico,
                                            lCodigo: codigo_apontamento);

                return true;
            }
            catch
            {
                return false;
            }

        }

        // GET: INDEX
        public ActionResult DeleteOrdemServicoPMOC(int codigo_unidade, long codigo_pmoc_ordem_servico)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                oPMOC.DeleteOrdemServicoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: codigo_unidade,
                                                lCodigoPMOCOrdemServico: codigo_pmoc_ordem_servico,
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()));

                return RedirectToAction("PMOC2", "PMOC");
            }
        }
        
        // GET: Concluir 
        public ActionResult ConcluirPMOC(long codigo_pmoc_ordem_servico, int codigo_unidade, string view)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                oPMOC.UpdateStatus(lCodigoPMOCOrdemServico: codigo_pmoc_ordem_servico,
                                   iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                   iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                   iCodigoUnidade: codigo_unidade,
                                   iStatus: 2);

                return RedirectToAction("OrdemServicoPMOC", "PMOC", new { codigo_unidade = codigo_unidade, codigo_pmoc_ordem_servico = codigo_pmoc_ordem_servico, view = view });
            }
        }

        // GET: Reabrir 
        public ActionResult ReabrirPMOC(long codigo_pmoc_ordem_servico, int codigo_unidade, string view)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                oPMOC.UpdateStatus(lCodigoPMOCOrdemServico: codigo_pmoc_ordem_servico,
                                    iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    iCodigoUnidade: codigo_unidade,
                                    iStatus: 4);

                return RedirectToAction("OrdemServicoPMOC", "PMOC", new { codigo_unidade = codigo_unidade, codigo_pmoc_ordem_servico = codigo_pmoc_ordem_servico, view = view });
            }
        }

        #endregion

        #region ::: CRONOGRAMA :::

        // GET: PMOC - Cronograma
        public ActionResult CronogramaPMOC()
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
                                sFormulario: "pmoc_cronograma",
                                bInserir: ref inserir,
                                bEditar: ref editar,
                                bExcluir: ref excluir,
                                bImprimir: ref imprimir,
                                bAdministrador: ref administrador);

            ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
            ViewBag.mes_1 = DateTime.Now.ToString("MM/yy");
            ViewBag.mes_2 = DateTime.Now.AddMonths(1).ToString("MM/yy");
            ViewBag.mes_3 = DateTime.Now.AddMonths(2).ToString("MM/yy");
            ViewBag.mes_4 = DateTime.Now.AddMonths(3).ToString("MM/yy");
            ViewBag.mes_5 = DateTime.Now.AddMonths(4).ToString("MM/yy");
            ViewBag.mes_6 = DateTime.Now.AddMonths(5).ToString("MM/yy");
            ViewBag.mes_7 = DateTime.Now.AddMonths(6).ToString("MM/yy");
            ViewBag.mes_8 = DateTime.Now.AddMonths(7).ToString("MM/yy");
            ViewBag.mes_9 = DateTime.Now.AddMonths(8).ToString("MM/yy");
            ViewBag.mes_10 = DateTime.Now.AddMonths(9).ToString("MM/yy");
            ViewBag.mes_11 = DateTime.Now.AddMonths(10).ToString("MM/yy");
            ViewBag.mes_12 = DateTime.Now.AddMonths(11).ToString("MM/yy");
                
            ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                            iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                            bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());

            return View(oPMOC.CronogramaPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                sData: TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString()));

        }
    }

        // GET: PMOC - HISTÓRICO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CronogramaPMOC(string data, int unidade = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                unidade = (unidade == -1) ? Convert.ToInt32(Session["codigo_unidade"].ToString()) : unidade;

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool imprimir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pmoc_historico",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                DateTime data2 = Convert.ToDateTime(data);
                ViewBag.data = data2.ToShortDateString();
                ViewBag.mes_1 = data2.ToString("MM/yy");
                ViewBag.mes_2 = data2.AddMonths(1).ToString("MM/yy");
                ViewBag.mes_3 = data2.AddMonths(2).ToString("MM/yy");
                ViewBag.mes_4 = data2.AddMonths(3).ToString("MM/yy");
                ViewBag.mes_5 = data2.AddMonths(4).ToString("MM/yy");
                ViewBag.mes_6 = data2.AddMonths(5).ToString("MM/yy");
                ViewBag.mes_7 = data2.AddMonths(6).ToString("MM/yy");
                ViewBag.mes_8 = data2.AddMonths(7).ToString("MM/yy");
                ViewBag.mes_9 = data2.AddMonths(8).ToString("MM/yy");
                ViewBag.mes_10 = data2.AddMonths(9).ToString("MM/yy");
                ViewBag.mes_11 = data2.AddMonths(10).ToString("MM/yy");
                ViewBag.mes_12 = data2.AddMonths(11).ToString("MM/yy");
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", unidade);

                return View(oPMOC.CronogramaPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: unidade,
                                                    sData: data));

            }
        }

        #endregion

        #region ::: PMOC HISTÓRICO :::

        // GET: PMOC - Cronograma
        public ActionResult PMOCHistorico()
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
                                    sFormulario: "pmoc_cronograma",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bImprimir: ref imprimir,
                                    bAdministrador: ref administrador);

                ViewBag.ano = new SelectList(oCombo.Ano(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", DateTime.Now.Year.ToString());

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());

                return View(oPMOC.PMOCHistorico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                               iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                               iAno: DateTime.Now.Year));

            }
        }

        // GET: PMOC - HISTÓRICO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PMOCHistorico(int ano, int unidade = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                unidade = (unidade == -1) ? Convert.ToInt32(Session["codigo_unidade"].ToString()) : unidade;

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool imprimir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pmoc_historico",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.ano = new SelectList(oCombo.Ano(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", ano);

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", unidade);

                return View(oPMOC.PMOCHistorico(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: unidade,
                                                iAno: ano));

            }
        }

        #endregion

        #region ::: PMOC ANDAR :::

        // GET: PMOC - ANDAR
        public ActionResult PMOCAndar()
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
                DateTime currentDate = DateTime.Today;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pmoc_andar",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bImprimir: ref imprimir,
                bAdministrador: ref administrador);
                ViewBag.data = new SelectList(oCombo.MesAno(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", new DateTime(currentDate.Year, currentDate.Month, 1).ToString("yyyy-MM-dd"));
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());

                return View(oPMOC.PMOCAndar(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                            sData: new DateTime(currentDate.Year, currentDate.Month, 1).ToString()));

            }
        }

        // GET: PMOC - ANDAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PMOCAndar(int unidade = -1, string data = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                unidade = (unidade == -1) ? Convert.ToInt32(Session["codigo_unidade"].ToString()) : unidade;

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool imprimir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pmoc_andar",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.data = new SelectList(oCombo.MesAno(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", data);
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", unidade);

                return View(oPMOC.PMOCAndar(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: unidade,
                                            sData: data));


            }
        }

        #endregion

        #region ::: HISTÓRICO :::

        // GET: PMOC - HISTÓRICO
        public ActionResult HistoricoPMOC()
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
                                        sFormulario: "pmoc_historico",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bImprimir: ref imprimir,
                                        bAdministrador: ref administrador);

                    ViewBag.editar = editar;
                    ViewBag.data_inicio = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString();
                    ViewBag.data_termino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());

                    ViewBag.equipamento = new SelectList(oCombo.ArCondicionadoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                   iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                    return View(oPMOC.HistoricoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                    lCodigoEquipamento: -1,
                                                    sDataInicio: TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString(),
                                                    sDataTermino: TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString()));

                }
            }

        // GET: PMOC - HISTÓRICO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HistoricoPMOC(string data_inicio, string data_termino, int unidade = -1, long equipamento = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                unidade = (unidade == -1)? Convert.ToInt32(Session["codigo_unidade"].ToString()): unidade;

                //Váriaveis
                bool editar = false;
                bool inserir = false;
                bool excluir = false;
                bool imprimir = false;
                bool administrador = false;

                oAccount.LoadPerfil(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    sFormulario: "pmoc_historico",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador,
                                    bImprimir: ref imprimir);

                ViewBag.editar = editar;
                ViewBag.data_inicio = data_inicio;
                ViewBag.data_termino = data_termino;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", unidade);

                ViewBag.equipamento = new SelectList(oCombo.ArCondicionadoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                               iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", equipamento);

                return View(oPMOC.HistoricoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUnidade: unidade,
                                                lCodigoEquipamento: equipamento,
                                                sDataInicio: data_inicio,
                                                sDataTermino: data_termino));

            }
        }

        // GET: /PRINT
        public ActionResult PrintReportPMOC(long codigo_pmoc_ordem_servico, int unidade)
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000034.rpt"));

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
            oReportDocument.SetParameterValue("@codigo_pmoc_ordem_servico", codigo_pmoc_ordem_servico);

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000034.pdf");
            return File(stream, "application/pdf;");
        }

        #endregion

        #region ::: PMOC :::

        // GET: INDEX
        public ActionResult PMOCIndex()
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
                                        sFormulario: "pmoc",
                                        bInserir: ref inserir,
                                        bEditar: ref editar,
                                        bExcluir: ref excluir,
                                        bAdministrador: ref administrador);

                    ViewBag.inserir = inserir;
                    ViewBag.editar = editar;
                    ViewBag.excluir = excluir;

                    return View(oPMOC.IndexPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()), 
                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())));
                }
            }

        // GET: INSERT
        public ActionResult PMOCInsert()
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
                ViewBag.empresa_pmoc = new SelectList(oCombo.EmpresaPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.unidade_pmoc = new SelectList(oCombo.UnidadePMOC(iCodigoEmpresa: -1,
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                
                return View();
            }
        }

        // POST: INSERT
        [HttpPost]
        public ActionResult PMOCInsert(int unidade, string responsavel_tecnico, string numero_art, string data_inicio_vigencia_art, string data_termino_vigencia_art, int empresa_pmoc = -1, int unidade_pmoc = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Insere Registro no Banco de Dados
                oPMOC.InsertPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    iCodigoUnidade: unidade,
                                    iCodigoResponsavelLegal: -1,
                                    iCodigoTipoServico: 3,
                                    iCodigoEmpresaPMOC: empresa_pmoc,
                                    iCodigoUnidadePMOC: unidade_pmoc,
                                    sResponsavelTecnicoPMOC: responsavel_tecnico,
                                    sNumeroARTPMOC: numero_art,
                                    sDataInicioVigenciaARTPMOC: data_inicio_vigencia_art,
                                    sDataTerminoVigenciaPMOC: data_termino_vigencia_art);

                return RedirectToAction("PMOCInsert");
            }
        }

        // GET: /EDIT
        public ActionResult PMOCEdit(int codigo, int unidade)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                PMOCCadastro pmoc = null;

                oPMOC.InfoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                iCodigo: codigo,
                                iCodigoUnidade: unidade,
                                oPMOC: ref pmoc);


                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", pmoc.codigo_unidade);
                ViewBag.empresa_pmoc = new SelectList(oCombo.EmpresaPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", pmoc.codigo_empresa_pmoc);
                ViewBag.unidade_pmoc = new SelectList(oCombo.UnidadePMOC(iCodigoEmpresa: pmoc.codigo_empresa_pmoc,
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", pmoc.codigo_unidade_pmoc);

                ViewBag.pmoc = pmoc;

                return View(pmoc);
            }
        }

        // POST: /EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PMOCEdit(int codigo, int unidade, string responsavel_tecnico, string numero_art, string data_inicio_vigencia_art, string data_termino_vigencia_art, int empresa_pmoc = -1, int unidade_pmoc = -1)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                //Atualiza Registro no Banco de Dados
                oPMOC.UpdatePMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                    iCodigoUnidade: unidade,
                                    iCodigoResponsavelLegal: -1,
                                    iCodigoTipoServico: 3,
                                    iCodigoEmpresaPMOC: empresa_pmoc,
                                    iCodigoUnidadePMOC: unidade_pmoc,
                                    sResponsavelTecnicoPMOC: responsavel_tecnico,
                                    sNumeroARTPMOC: numero_art,
                                    sDataInicioVigenciaARTPMOC: data_inicio_vigencia_art,
                                    sDataTerminoVigenciaPMOC: data_termino_vigencia_art,
                                    iCodigo: codigo);

                //Redireciona para Index
                return RedirectToAction("PMOCIndex");
                    
            }
        }

        // GET: /DELETE
        public ActionResult PMOCDelete(int codigo, int unidade, string erro = "")
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {
                PMOCCadastro pmoc = null;

                oPMOC.InfoPMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                iCodigo: codigo,
                                iCodigoUnidade: unidade,
                                oPMOC: ref pmoc);

                ViewBag.erro = erro;

                if (pmoc == null)
                {
                    return HttpNotFound();
                }

                return View(pmoc);
            }
        }

        // POST: /DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PMOCDelete([Bind(Include = "codigo, codigo_unidade")] PMOCCadastro pmoc)
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
                    oPMOC.DeletePMOC(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        iCodigo: pmoc.codigo,
                                        iCodigoUnidade: pmoc.codigo_unidade);

                    //Redireciona para Index
                    return RedirectToAction("PMOCIndex");
                }
                catch
                {
                    return PMOCDelete(codigo: pmoc.codigo,
                                        unidade: pmoc.codigo_unidade,
                                        erro: PCM.WEB.Properties.Resources.valida_excluir);
                }
            }
        }

        // POST: /PRINT
        public ActionResult PrintPMOCImplantacao(int unidade)
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

            oReportDocument.Load(Path.Combine(Server.MapPath("~/Reports"), "RPT000000031.rpt"));

            oCrDatabase = oReportDocument.Database;
            oCrTables = oCrDatabase.Tables;

            foreach (Table crTable in oCrTables)
            {
                oTableLogOnInfo = crTable.LogOnInfo;
                oTableLogOnInfo.ConnectionInfo = oConnectionInfo;
                crTable.ApplyLogOnInfo(oTableLogOnInfo);
            }

            oReportDocument.SetParameterValue("usuario", Session["nome"].ToString());
            oReportDocument.SetParameterValue("@codigo_unidade", unidade);
            oReportDocument.SetParameterValue("@codigo_empresa", Convert.ToInt32(Session["empresa"].ToString()));

            Stream stream = oReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            oReportDocument.Close(); oReportDocument.Dispose(); oReportDocument = null; GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
            stream.Seek(0, SeekOrigin.Begin);
            Response.AppendHeader("Content-Length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=RPT000000031.pdf");
            return File(stream, "application/pdf;");
        }

        #endregion

    }
}