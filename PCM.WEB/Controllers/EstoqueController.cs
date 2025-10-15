using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Web;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PCM.WEB.MODELS;
using PCM.WEB.DAL;
using System.Net.Mail;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Collections;
using Newtonsoft.Json;
using OfficeOpenXml.ConditionalFormatting;
using SYSPACK.WEB.MODELS;

namespace PCM.WEB.Controllers
{
    public class EstoqueController : Controller
    {
        private Combo oCombo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Estoque oEstoque = new Estoque(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Stock oStock = new Stock(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);


        #region ::: JSON :::

        public JsonResult LoadComboFornecedor(int empresa, int unidade)
        {
            return Json(oCombo.Fornecedor(iCodigoEmpresa: empresa,
                                          iCodigoUnidade: unidade));
        }

        public JsonResult LoadComboOrdemCompra(int empresa, int unidade)
        {
            return Json(oCombo.OrdemCompra(codigoEmpresa: empresa,
                                           codigoUnidade: unidade));
        }

        public JsonResult LoadComboOrdemCompraProduto(long codigoOrdemCompra)
        {
            return Json(oCombo.OrdemCompraProduto(codigoOrdemCompra: codigoOrdemCompra));
        }

        public JsonResult LoadComboProduto(int empresa, int unidade)
        {
            return Json(oCombo.Produto(iCodigoEmpresa: empresa,
                                       iCodigoUnidade: unidade));
        }

        public JsonResult LoadComboProdutoLoteSaida(int empresa, long produto)
        {
            return Json(oCombo.ProdutoLoteSaida(codigoEmpresa: empresa,
                                                codigoProduto: produto));
        }

        public JsonResult LoadComboUOM(int empresa)
        {
            return Json(oCombo.UOM(codigoEmpresa: empresa));
        }

        public JsonResult LoadProdutoInfo(int empresa, long produto, long codigoOrdemCompra)
        {
            return Json(oStock.LoadProdutoInfo(codigoEmpresa: empresa,
                                               codigoOrdemCompra: codigoOrdemCompra,
                                               codigoProduto: produto));
        }

        public JsonResult LoadProdutoInfoSaida(int empresa, long produto, string lote)
        {
            return Json(oStock.LoadProdutoInfoSaida(codigoEmpresa: empresa,
                                                    codigoProduto: produto,
                                                    lote: lote));
        }

        public JsonResult LoadOrdemCompraInfo(long codigoOrdemCompra)
        {
            return Json(oStock.LoadOrdemCompraInfo(codigoOrdemCompra: codigoOrdemCompra));
        }

        public JsonResult LoadComboUsuario(int empresa, int unidade)
        {
            return Json(oCombo.Usuario(iCodigoEmpresa: empresa,
                                       iCodigoUnidade: unidade));
        }

        public JsonResult LoadComboOrdemServico(int empresa, int unidade)
        {
            return Json(oCombo.OrdemServico(codigoEmpresa: empresa,
                                            codigoUnidade: unidade));
        }

        #endregion

        #region ::: ENTRADA :::

        public ActionResult Entrada()
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
                                    sFormulario: "est_entrada",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                      iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.ordemCompra = new SelectList(oCombo.OrdemCompra(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                        codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.produto = new SelectList(oCombo.Produto(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.uom = new SelectList(oCombo.UOM(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", null);
                ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.empresa = Session["empresa"].ToString();
                ViewBag.codigoUsuario = User.Identity.GetUserName();
                ViewBag.codigoEmpresa = Session["empresa"].ToString();

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Entrada(int codigoEmpresa, int codigoUsuario, int unidade, string numeroDocumento, string data, string produtosJSON, long ordemCompra = -1, int codigoFornecedor = -1)
        {

            oStock.InsertEntrada(codigoEmpresa: codigoEmpresa,
                                 codigoUnidade: unidade,
                                 numeroDocumento: numeroDocumento,
                                 codigoOrdemCompra: ordemCompra,
                                 codigoFornecedor: codigoFornecedor,
                                 data: data,
                                 codigoUsuario: codigoUsuario,
                                 pathFile: "",
                                 produtosJson: produtosJSON);

            return RedirectToAction("Entrada");
        }

        #endregion

        #region ::: LISTAGEM :::

        public ActionResult Listagem()
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
                                    sFormulario: "est_listagem",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.produto = new SelectList(oCombo.Produto(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.status = new SelectList(oCombo.StatusEstoque(), "codigo", "descricao", null);
                ViewBag.codigoEmpresa = Session["empresa"].ToString();
                ViewBag.codigoUsuario = User.Identity.GetUserName();

                return View();
            }
        }

        public JsonResult LoadListagem(int empresa, int usuario, int unidade, long produto = -1, int status = -1)
        {
            return Json(oStock.EstoqueListagem(codigoEmpresa: empresa,
                                               codigoUnidade: unidade,
                                               codigoUsuario: usuario,
                                               codigoProduto: produto,
                                               status: status));
        }

        #endregion

        #region ::: ORDEM COMPRA :::

        public ActionResult OrdemCompra()
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
                                    sFormulario: "estoque_ordem_compra",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.administrador = administrador;
                ViewBag.dataInicio = DateTime.Now.AddDays(-7).ToShortDateString();
                ViewBag.dataTermino = DateTime.Now.ToShortDateString();

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                false), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                      iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.status = new SelectList(oCombo.StatusOrdemCompra(), "codigo", "descricao", null);
                ViewBag.codigoEmpresa = Session["empresa"].ToString();

                return View();
            }
        }

        public JsonResult LoadOrdemCompra(int codigoUnidade, string ordemCompra, string dataInicio, string dataTermino, int fornecedor = -1, int status = -1)
        {
            return Json(oStock.LoadOrdemCompra(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                               codigoUnidade: codigoUnidade,
                                               ordemCompra: ordemCompra,
                                               codigoFornecedor: fornecedor,
                                               status: status,
                                               dataInicio: dataInicio,
                                               dataTermino: dataTermino));

        }

        public JsonResult LoadOrdemCompraProduto(int codigoEmpresa, int codigoUnidade, long codigoOrdemCompra)
        {
            return Json(oStock.LoadOrdemCompraProduto(codigoEmpresa: codigoEmpresa,
                                                      codigoUnidade: codigoUnidade,
                                                      codigoOrdemCompra: codigoOrdemCompra));

        }

        public JsonResult DeleteOrdemCompra(int unidade, long codigoOrdemCompra)
        {
            try
            {
                oStock.DeleteOrdemCompra(codigoOrdemCompra: codigoOrdemCompra,
                                         codigoUsuario: User.Identity.GetUserName());

                return Json(Properties.Resources.registroCancelado);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [AllowAnonymous]
        public FileResult DownloadFile(string path)
        {

            string type = path.Split('.')[path.Split('.').Length - 1];
            string contentType = "";

            switch (type.ToLower())
            {
                case "pdf":
                    contentType = "application/pdf";
                    break;
                case "xlsx":
                    contentType = "application/xlsx";
                    break;
                case "docx":
                    contentType = "application/docx";
                    break;
                case "png":
                    contentType = "image/png";
                    break;
                case "jpg":
                    contentType = "image/jpg";
                    break;
                case "jpeg":
                    contentType = "image/jpeg";
                    break;
                case "bmp":
                    contentType = "image/bmp";
                    break;
            }

            return File(path, contentType, Guid.NewGuid().ToString() + "." + path.Split('.')[path.Split('.').Length - 1]);
        }

        #endregion

        #region ::: SAÍDA :::

        public ActionResult Saida()
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
                                    sFormulario: "est_saida",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.usuarioRequisitante = new SelectList(oCombo.Usuario(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                            iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.ordemServico = new SelectList(oCombo.OrdemServico(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                          codigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.produto = new SelectList(oCombo.Produto(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.lote = new SelectList(new List<ListComboString>(), "codigo", "descricao", null);
                ViewBag.data = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).ToShortDateString();
                ViewBag.empresa = Session["empresa"].ToString();
                ViewBag.codigoUsuario = User.Identity.GetUserName();
                ViewBag.codigoEmpresa = Session["empresa"].ToString();

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Saida(int codigoEmpresa, int codigoUsuario, int unidade, int usuarioRequisitante, string data, string produtosJSON, int ordemServico = -1)
        {

            oStock.InsertSaida(codigoEmpresa: codigoEmpresa,
                               codigoUnidade: unidade,
                               codigoUsuarioRequisitante: usuarioRequisitante,
                               codigoOrdemServico: ordemServico,
                               data: data,
                               codigoUsuario: codigoUsuario,
                               pathFile: "",
                               produtosJson: produtosJSON);

            return RedirectToAction("Saida");
        }

        #endregion

        #region ::: REQUISIÇÃO COMPRA :::

        public ActionResult RequisicaoCompra()
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
                                    sFormulario: "est_requisicao_compra",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                      iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.fornecedorOrdemCompra = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                                 iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.status = new SelectList(oCombo.StatusRequisicaoCompra(), "codigo", "descricao", null);
                ViewBag.dataInicio = DateTime.Now.AddDays(-7).ToShortDateString();
                ViewBag.dataTermino = DateTime.Now.ToShortDateString();
                ViewBag.codigoEmpresa = Session["empresa"].ToString();

                return View();
            }
        }

        public JsonResult LoadRequisicaoCompra(int codigoEmpresa, int codigoUnidade, string requisicaoCompra, int codigoFornecedor, string dataInicio, string dataTermino, int status)
        {
            return Json(oStock.LoadRequisicaoCompra(codigoEmpresa: codigoEmpresa,
                                                    codigoUnidade: codigoUnidade,
                                                    requisicaoCompra: requisicaoCompra,
                                                    codigoFornecedor: codigoFornecedor,
                                                    dataInicio: dataInicio,
                                                    dataTermino: dataTermino,
                                                    status: status));

        }

        public JsonResult LoadRequisicaoCompraProduto(long codigoRequisicaoCompra)
        {
            return Json(oStock.LoadRequisicaoCompraProduto(codigoRequisicaoCompra: codigoRequisicaoCompra));

        }

        public JsonResult LoadRequisicaoCompraProdutoFornecedor(int codigoEmpresa, int codigoUnidade)
        {
            return Json(oStock.LoadRequisicaoCompraSparePart(codigoEmpresa: codigoEmpresa,
                                                             codigoUnidade: codigoUnidade));

        }

        [Authorize]
        public ActionResult RequisicaoCompraInsert()
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
                                    sFormulario: "est_requisicao_compra",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.produto = new SelectList(oCombo.Produto(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.uom = new SelectList(oCombo.UOM(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "codigo", "descricao", null);
                ViewBag.codigoEmpresa = Session["empresa"].ToString();

                return View();
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequisicaoCompraInsert(int unidade, string produtosJson, int fornecedor = -1)
        {
            //Chama a função para inserir o pedido e produtos usando JSON
            oStock.InsertRequisicaoOrdemCompra(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                               codigoUnidade: unidade,
                                               codigoFornecedor: fornecedor,
                                               produtosJson: produtosJson,
                                               codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()));

            // Redireciona para Index
            return RedirectToAction("RequisicaoCompra");
        }

        public JsonResult UpdateRequisicaoCompraStatus(long codigoRequisicaoCompra, int unidade, HttpPostedFileBase arquivo, int fornecedor = -1, string ordemCompra = "", int status = 2)
        {
            try
            {

                string path = "";

                if (arquivo != null)
                {

                    string folder = Server.MapPath("~/Content/arq/Inventario/AprovacaoRequisicaoCompra");

                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    path = System.IO.Path.Combine(folder, Guid.NewGuid().ToString() + "." + arquivo.FileName.Split('.')[arquivo.FileName.Split('.').Length - 1]);

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    arquivo.SaveAs(path);

                }

                oStock.UpdateRequisicaoCompraStatus(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                    codigoUnidade: unidade,
                                                    codigoRequisicaoCompra: codigoRequisicaoCompra,
                                                    codigoFornecedor: fornecedor,
                                                    status: status,
                                                    ordemCompra: ordemCompra,
                                                    pathFile: path,
                                                    codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()));


            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            //Redireciona para Index

            if (status == 2)
            {
                return Json(Properties.Resources.ordemCompraGerada);
            }
            else
            {
                return Json(Properties.Resources.requisicaoCompraReprovada);
            }
        }

        #endregion

        #region ::: REQUISIÇÂO COMPRA - SPARE PART :::

        [Authorize]
        public ActionResult RequisicaoCompraInsertSparePoint()
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
                                    sFormulario: "est_requisicao_compra",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", Session["codigo_unidade"].ToString());
                ViewBag.fornecedor = new SelectList(oCombo.Fornecedor(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                      iCodigoUnidade: Convert.ToInt32(Session["codigo_unidade"].ToString())), "codigo", "descricao", null);
                ViewBag.codigoEmpresa = Session["empresa"].ToString();

                return View();
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequisicaoCompraInsertSparePoint(int unidade, string produtosJson, int fornecedor = -1)
        {
            //Chama a função para inserir o pedido e produtos usando JSON
            oStock.InsertRequisicaoOrdemCompra(codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                               codigoUnidade: unidade,
                                               codigoFornecedor: fornecedor,
                                               produtosJson: produtosJson,
                                               codigoUsuario: Convert.ToInt32(User.Identity.GetUserName()));

            // Redireciona para Index
            return RedirectToAction("RequisicaoCompra");
        }

        public JsonResult LoadRequisicaoCompraSparePoint(int codigoEmpresa, int codigoUnidade)
        {
            return Json(oStock.LoadRequisicaoCompraSparePart(codigoEmpresa: codigoEmpresa,
                                                             codigoUnidade: codigoUnidade));

        }

        #endregion

        #region ::: INVENTARIO :::

        public ActionResult Inventario()
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
        public ActionResult Inventario(int empresa, int unidade, int usuario, HttpPostedFileBase arquivo, string worksheet, string tipo)
        {

            EstoqueInventario inventario = new EstoqueInventario();

            if (tipo == "arquivo")
            {

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

                    List<EstoqueInventarioDetalhe> result = new List<EstoqueInventarioDetalhe>();
                    List<EstoqueInventarioDetalheError> error = new List<EstoqueInventarioDetalheError>();

                    //Upload Arquivo
                    oStock.UploadInventarioExcel(codigoEmpresa: empresa,
                                                 codigoUnidade: unidade,
                                                 codigoUsuario: usuario,
                                                 file: path,
                                                 worksheet: worksheet,
                                                 oDetails: ref result,
                                                 oDetailsError: ref error);

                    inventario.result = result;
                    inventario.resultError = error;
                    inventario.success = true;

                    return View(inventario);

                }
                catch (Exception ex)
                {
                    inventario.success = false;
                    inventario.message = ex.Message;
                    inventario.result = new List<EstoqueInventarioDetalhe>();
                    inventario.resultError = new List<EstoqueInventarioDetalheError>();

                    return View(inventario);
                }
                finally
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    
                    ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                    iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                    bCadastro: true), "codigo", "descricao", unidade);

                }

            }
            else if (tipo == "save")
            {

                oStock.UpdateInventario(codigoEmpresa: empresa,
                                        codigoUnidade: unidade,
                                        codigoUsuario: usuario);

                ViewBag.unidade = new SelectList(oCombo.Unidade(iCodigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()),
                                                                iCodigoUsuario: Convert.ToInt32(User.Identity.GetUserName()),
                                                                bCadastro: true), "codigo", "descricao", unidade);

                inventario.message = Properties.Resources.inventarioAtualizadoSucesso;

                inventario.result = new List<EstoqueInventarioDetalhe>();
                inventario.resultError = new List<EstoqueInventarioDetalheError>();

            }

            ViewBag.codigoEmpresa = Session["empresa"].ToString();
            return View(inventario);

        }

        [HttpPost]
        public JsonResult UploadInventario(int codigoEmpresa, int codigoUnidade, HttpPostedFileBase arquivo, string planilha)
        {

            EstoqueInventario inventario = new EstoqueInventario();

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

                List<EstoqueInventarioDetalhe> result = new List<EstoqueInventarioDetalhe>();
                List<EstoqueInventarioDetalheError> error = new List<EstoqueInventarioDetalheError>();

                //Upload Arquivo
                oStock.UploadInventarioExcel(codigoEmpresa: codigoEmpresa,
                                                codigoUnidade: codigoUnidade,
                                                codigoUsuario: Convert.ToInt32(User.Identity.Name),
                                                file: path,
                                                worksheet: planilha,
                                                oDetails: ref result,
                                                oDetailsError: ref error);

                inventario.result = result;
                inventario.resultError = error;
                inventario.success = true;


            }
                catch (Exception ex)
                {
                    inventario.success = false;
                    inventario.message = ex.Message;
                    inventario.result = new List<EstoqueInventarioDetalhe>();
                    inventario.resultError = new List<EstoqueInventarioDetalheError>();
                }
                finally
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

            return Json(inventario);
        }
        
        // GET: /Download Excel
        [HttpGet]
        public virtual ActionResult StockInventoryDownloadExcel()
        {

            string nome_relatorio = "INV_PRODUTO.xlsx";
            string filename = "INV_PRODUTO.xlsx";
            string path = Server.MapPath("~/Content/Files");
            string arquivo = System.IO.Path.Combine(path, nome_relatorio);

            return File(arquivo, "application/vnd.ms-excel", filename);
        }

        #endregion

    }
}