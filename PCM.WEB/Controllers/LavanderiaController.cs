using Microsoft.AspNet.Identity;
using NPOI.SS.Formula.Functions;
using PCM.WEB.DAL;
using PCM.WEB.MODELS;
using PCM.WEB.Properties;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace PCM.WEB.Controllers
{
    public class LavanderiaController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Lavanderia oLavanderia = new Lavanderia(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Excel oExcel = new Excel(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: JSON :::

        public JsonResult LoadComboEquipamento(int unidade)
        {
            return Json(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                           iCodigoUnidade: unidade));
        }

        public JsonResult LoadComboCliente(int unidade)
        {
            return Json(oCombo.Cliente(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                       codigoUnidade: unidade));
        }

        public JsonResult LoadComboFuncionario(int unidade)
        {
            return Json(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                           iCodigoUnidade: unidade,
                                           iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString())));
        }

        public JsonResult LoadComboEnxoval(int unidade, int cliente = -1)
        {
            return Json(oCombo.Enxoval(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                       codigoUnidade: unidade,
                                       codigoCliente: cliente));
        }

        public JsonResult LoadComboMes(int unidade)
        {
            return Json(oCombo.MesLavanderia(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                             codigoUnidade: unidade));
        }
        
        #endregion

        #region ::: CICLO LAVAGEM :::

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
                                    sFormulario: "lavanderia_ciclo",
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
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.cliente = new SelectList(oCombo.Cliente(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);

                return View();
            }
        }

        [HttpPost]
        public ActionResult Apontamento(int empresa, int unidade, int cliente, string data, string equipamentoJson, string enxovalJson, string funcionarioJson, string peso = "0", string pesoRelave = "0")
        {
            oLavanderia.Apontamento(codigoEmpresa: empresa,
                                    codigoUnidade: unidade,
                                    codigoCliente: cliente,
                                    data: data,
                                    peso: peso,
                                    pesoRelave:pesoRelave,
                                    equipamento:equipamentoJson,
                                    enxoval: enxovalJson,
                                    funcionario: funcionarioJson,
                                    codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()));
            
            return RedirectToAction("Apontamento");
        }

        [HttpPost]
        public JsonResult LoadLavanderiaEquipamento(int codigoEmpresa, int codigoUnidade)
        {

            return Json(oLavanderia.LoadEquipamento(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    codigoUnidade: codigoUnidade));

        }

        [HttpPost]
        public JsonResult LoadLavanderiaEnxoval(int codigoEmpresa, int codigoUnidade, int codigoCliente)
        {

            return Json(oLavanderia.LoadEnxoval(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                codigoUnidade: codigoUnidade,
                                                codigoCliente: codigoCliente));

        }

        [HttpPost]
        public JsonResult LoadLavanderiaFuncionario(int codigoEmpresa, int codigoUnidade)
        {

            return Json(oLavanderia.LoadFuncionario(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    codigoUnidade: codigoUnidade));

        }

        #endregion

        #region ::: HISTÓRICO :::

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
                                    sFormulario: "lav_historico",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bImprimir: ref imprimir,
                                    bAdministrador: ref administrador);

                ViewBag.empresa = Session["empresa"].ToString();
                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.imprimir = imprimir;
                ViewBag.dataInicio = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).AddMonths(-1).ToShortDateString();
                ViewBag.dataTermino = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                               iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                               bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.cliente = new SelectList(oCombo.Cliente(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                 ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString())), "codigo", "descricao", null);


                return View();
            }
        }

        public JsonResult LoadApontamentoHistorico(int unidade, string dataInicio, string dataTermino, int cliente = -1, long equipamento = -1, int funcionario = -1)
        {

            return Json(oLavanderia.Historico(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                              codigoUnidade: unidade,
                                              codigoCliente: cliente,
                                              codigoEquipamento: equipamento,
                                              dataInicio: dataInicio,
                                              dataTermino: dataTermino,
                                              codigoFuncionario: funcionario));

        }

        public JsonResult LoadLavanderiaHistoricoDetalhe(int empresa, int unidade, long codigo)
        {

            return Json(oLavanderia.HistoricoDetalhe(codigoEmpresa: empresa,
                                                     codigoUnidade: unidade,
                                                     codigo: codigo));

        }

        public JsonResult DeleteApontamento(int empresa, int unidade, long codigo)
        {

            try
            {
                oLavanderia.DeleteApontamento(codigoEmpresa: empresa,
                                              codigoUnidade: unidade,
                                              codigo: codigo);

                return Json(true);

            }
            catch
            {
                return Json(false);
            }

        }

        #endregion

        #region ::: REPORT :::

        #region ::: CONTROLE DE LAVAGEM :::

        public ActionResult RelatorioControleGeral()
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
                                    sFormulario: "rel_controle_lavagem",
                                    sDireito: "imprimir",
                                    bReturn: ref imprimir);

                ViewBag.imprimir = imprimir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                              iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                              bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.cliente = new SelectList(oCombo.Cliente(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.equipamento = new SelectList(oCombo.Equipamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                       iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.mes = new SelectList(oCombo.MesLavanderia(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                  codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.funcionario = new SelectList(oCombo.Funcionario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString()),
                                                                        iCodigoModulo: Convert.ToInt32(Session["codigo_modulo"].ToString())), "codigo", "descricao", null);
                ViewBag.agrupadoPor = new SelectList(oCombo.LoadComboString("sp_select_combo_static_agrupamento_relatorico_controle_lavagem"), "codigo", "descricao", null);

                return View();
            }
        }

        [HttpPost]
        public JsonResult LoadControleLavagem(int unidade, int cliente, long equipamento, int funcionario, string mes, string agrupadoPor)
        {
            return Json(oLavanderia.RelatorioControleLavagem(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                             codigoUnidade: unidade,
                                                             codigoCliente: cliente,
                                                             codigoEquipamento: equipamento,
                                                             codigoFuncionario: funcionario,
                                                             data: mes,
                                                             agrupadoPor: agrupadoPor));
        }

        #endregion

        #endregion

    }
}