using System;
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
    public class GreenPlanetController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private DAL.GreenPlanet oGreenPlanet = new DAL.GreenPlanet(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: JSON :::

        //JSON: /UNIDADE/
        public JsonResult LoadUnidade()
        {
            return Json(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                        iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                        bCadastro: false));
        }

        #endregion

        #region ::: LANÇAMENTO :::

        // GET: INDEX
        public ActionResult LancamentoIndex(int unidade = -1, string data = null, string a = "")
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
                                    sFormulario: "green_lancamento",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                if (editar && excluir)
                    ViewBag.columnDefs = "columnDefs: [{ orderable: false, targets: [0, 1] }], order: [[2]]";
                else if (editar || excluir)
                    ViewBag.columnDefs = "columnDefs: [{ orderable: false, targets: [0] }], order: [[1]]";
                else
                    ViewBag.columnDefs = "order: [[0]]";

                LancamentoMedicao lancamento = new LancamentoMedicao();

                oGreenPlanet.LoadLancamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: Convert.ToInt32((unidade == -1) ? Session["codigo_unidade"].ToString() : unidade.ToString()),
                                            sData: (data == null) ? System.DateTime.Today.ToString("dd/MM/yyyy") : data,
                                            iCodigoItemMedicao: -1,
                                            oLancamento: ref lancamento);

                ViewBag.ocupacao_quartos = lancamento.ocupacao_quartos;
                ViewBag.quantidade_hospede = lancamento.quantidade_hospede;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", (unidade == -1) ? Session["codigo_unidade"].ToString() : unidade.ToString());
                ViewBag.data = (data==null) ? System.DateTime.Today.ToString("dd/MM/yyyy"): data;

                return View(oGreenPlanet.LoadLancamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), iCodigoUnidade: Convert.ToInt32((unidade == -1)? Session["codigo_unidade"].ToString(): unidade.ToString()), sData: ViewBag.data, iCodigoItemMedicao: -1));
            }
        }

        // POST: INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LancamentoIndex(int unidade = -1, string data = null)
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
                                    sFormulario: "green_lancamento",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                if (editar && excluir)
                    ViewBag.columnDefs = "columnDefs: [{ orderable: false, targets: [0, 1] }], order: [[2]]";
                else if (editar || excluir)
                    ViewBag.columnDefs = "columnDefs: [{ orderable: false, targets: [0] }], order: [[1]]";
                else
                    ViewBag.columnDefs = "order: [[0]]";

                LancamentoMedicao lancamento = new LancamentoMedicao();

                oGreenPlanet.LoadLancamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: unidade,
                                            sData: (data == null) ? System.DateTime.Today.ToString("dd/MM/yyyy") : Convert.ToDateTime(data).ToString("dd/MM/yyyy"),
                                            iCodigoItemMedicao: -1,
                                            oLancamento: ref lancamento);

                ViewBag.ocupacao_quartos = lancamento.ocupacao_quartos;
                ViewBag.quantidade_hospede = lancamento.quantidade_hospede;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", unidade);
                ViewBag.data = data;

                return View(oGreenPlanet.LoadLancamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), iCodigoUnidade: unidade, sData: data, iCodigoItemMedicao: -1));
            }
        }

        // POST: INDEX
        public ActionResult LancamentoIndex2(int unidade, string data, int item_medicao)
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
                                    sFormulario: "green_lancamento",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                if (editar && excluir)
                    ViewBag.columnDefs = "columnDefs: [{ orderable: false, targets: [0, 1] }], order: [[2]]";
                else if (editar || excluir)
                    ViewBag.columnDefs = "columnDefs: [{ orderable: false, targets: [0] }], order: [[1]]";
                else
                    ViewBag.columnDefs = "order: [[0]]";

                LancamentoMedicao lancamento = new LancamentoMedicao();

                oGreenPlanet.LoadLancamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                            iCodigoUnidade: unidade,
                                            sData: (data == null) ? System.DateTime.Today.ToString("dd/MM/yyyy") : data,
                                            iCodigoItemMedicao: item_medicao,
                                            oLancamento: ref lancamento);

                ViewBag.ocupacao_quartos = lancamento.ocupacao_quartos;
                ViewBag.quantidade_hospede = lancamento.quantidade_hospede;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", unidade);
                ViewBag.data = data;

                return View(oGreenPlanet.LoadLancamento(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), iCodigoUnidade: unidade, sData: data, iCodigoItemMedicao: item_medicao));
            }
        }

        // POST: INDEX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LancamentoInsert(List<LancamentoMedicao> medicao, int unidade = -1, string data = null, int quantidade_hospede = 0, int ocupacao_quartos = 0)
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                    foreach (LancamentoMedicao item in medicao)
                    {

                        //Insere Registro no Banco de Dados
                        oGreenPlanet.InsertMedicao(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                    iCodigoUnidade: unidade,
                                                    iCodigoItemMedicao: item.codigo_item_medicao,
                                                    sData: data,
                                                    dValor: item.valor,
                                                    iQuantidadeHospede: quantidade_hospede,
                                                    iOcupacaoQuartos: ocupacao_quartos);

                    }

                return RedirectToAction("LancamentoIndex", "GreenPlanet", new { unidade = unidade, data = data });

            }
        }

        #endregion

    }
}
