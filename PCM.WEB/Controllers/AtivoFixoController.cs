using Microsoft.AspNet.Identity;
using PCM.WEB.DAL;
using PCM.WEB.MODELS;
using PCM.WEB.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static NPOI.HSSF.Util.HSSFColor;

namespace PCM.WEB.Controllers
{
    [Authorize]
    public class AtivoFixoController : BaseController
    {
        private Combo _combo = new Combo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private AtivoFixo _ativoFixo = new AtivoFixo(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account _account = new Account(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: JSON :::

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult LoadListApartamento(int unidade, int setor)
        {
            return Json(_combo.Apartamento(codigoEmpresa, unidade, setor));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult LoadListSetor(int unidade)
        {
            return Json(_combo.Setor(codigoEmpresa, unidade));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult LoadListAsset(int unidade)
        {
            return Json(_combo.LoadCombo(storedProcedure: "sp_select_combo_cadastro_basico_asset", codigoEmpresa: codigoEmpresa, codigoUnidade: unidade));
        }

        [HttpPost]
        public JsonResult hasInventoryOpened(int unidade)
        {
            return Json(_ativoFixo.HasInventoryOpened(codigoEmpresa: codigoEmpresa,
                                                      codigoUnidade: unidade));
        }

        [HttpPost]
        public JsonResult loadInventory(long codigoInventario, int unidade, int setor, int apartamento)
        {
            return Json(_ativoFixo.LoadInventory(codigoInventario: codigoInventario,
                                                 codigoEmpresa: codigoEmpresa,
                                                 codigoUnidade: unidade,
                                                 codigoSetor: setor,
                                                 codigoApartamento: apartamento));
        }

        #endregion

        #region ::: ASSET :::

        public ActionResult AssetIndex()
        {
            if (!IsSessionValid())
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });

            bool editar = false;
            bool inserir = false;
            bool excluir = false;
            bool administrador = false;

            _account.LoadPerfil(iCodigoEmpresa: codigoEmpresa,
                                iCodigoUsuario: codigoUsuario,
                                sFormulario: "cad_asset",
                                bInserir: ref inserir,
                                bEditar: ref editar,
                                bExcluir: ref excluir,
                                bAdministrador: ref administrador);

            ViewBag.inserir = inserir;
            ViewBag.editar = editar;
            ViewBag.excluir = excluir;

            ViewBag.unidade = new SelectList(_combo.Unidade(iCodigoEmpresa: codigoEmpresa, iCodigoUsuario: codigoUsuario, bCadastro: false), "codigo", "descricao", codigoUnidade);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult LoadAsset(int unidade = -1, string codigo = "", string descricao = "", int status = -1, string localizacao = "")
        {
            var data = _ativoFixo.LoadAsset(codigoEmpresa: codigoEmpresa,
                                            codigoUnidade: unidade,
                                            codigo: codigo,
                                            descricao: descricao,
                                            status: status,
                                            localizacao: localizacao);

            return new JsonResult
            {
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue,
                RecursionLimit = 100
            };
        }

        public ActionResult AssetInsert()
        {
            if (!IsSessionValid())
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });

            ViewBag.codigoUnidade = new SelectList(_combo.Unidade(iCodigoEmpresa: codigoEmpresa, iCodigoUsuario: codigoUsuario, bCadastro: false), "codigo", "descricao", codigoUnidade);
            ViewBag.codigoStatus = new SelectList(_combo.LoadCombo(storedProcedure: "sp_select_combo_static_status_asset"), "codigo", "descricao", null);
            ViewBag.codigoSetor = new SelectList(_combo.Setor(codigoEmpresa, codigoUnidade), "codigo", "descricao", null);
            ViewBag.codigoApartamento = new SelectList(_combo.Apartamento(codigoEmpresa, codigoUnidade), "codigo", "descricao", null);
            ViewBag.codigoUsuarioResponsavel = new SelectList(_combo.Usuario(codigoEmpresa, codigoUnidade), "codigo", "descricao", null);
            ViewBag.codigoEmpresa = codigoEmpresa;
            ViewBag.codigoUsuario = codigoUsuario;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssetInsert(int codigoUnidade, string assetCode, string descricao, string numeroSerie, string tag, string contaContabil, string dataCompra, string notaFiscal, HttpPostedFileBase arquivo, int tempoDepreciacaoMes = 0, int codigoStatus = -1, int codigoSetor = -1, int codigoApartamento = -1, int codigoUsuarioResponsavel = -1, float valorCompra = 0)
        {
            if (!IsSessionValid())
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });

            var filePath = "";

            if (arquivo != null)
            {
                string uniqueId = Guid.NewGuid().ToString();
                FileInfo fileInfo = new FileInfo(arquivo.FileName);

                var path = Path.Combine(Directory.GetCurrentDirectory(), "asset", "movement");
                filePath = Path.Combine(path, String.Concat(uniqueId, ".", fileInfo.Extension));

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                arquivo.SaveAs(filePath);
            }

            _ativoFixo.InsertAsset(codigoEmpresa: codigoEmpresa,
                                   codigoUnidade: codigoUnidade,
                                   assetCode: assetCode,
                                   descricao: descricao,
                                   numeroSerie: numeroSerie,
                                   tag: tag,
                                   contaContabil: contaContabil,
                                   dataCompra: dataCompra,
                                   valorCompra: valorCompra,
                                   tempoDepreciacaoMes: tempoDepreciacaoMes,
                                   notaFiscal: notaFiscal,
                                   codigoStatus: codigoStatus,
                                   codigoSetor: codigoSetor,
                                   codigoApartamento: codigoApartamento,
                                   codigoUsuarioResponsavel: codigoUsuarioResponsavel,
                                   arquivo: filePath,
                                   codigoUsuario: codigoUsuario);

            return RedirectToAction("AssetInsert");
        }

        public ActionResult AssetEdit(long codigo)
        {
            if (!IsSessionValid())
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });

            var asset = _ativoFixo.LoadAssetInfo(codigoEmpresa: codigoEmpresa,
                                                 codigo: codigo);

            ViewBag.codigoUnidade = new SelectList(_combo.Unidade(iCodigoEmpresa: codigoEmpresa, iCodigoUsuario: codigoUsuario, bCadastro: false), "codigo", "descricao", asset.codigoUnidade);
            ViewBag.codigoStatus = new SelectList(_combo.LoadCombo(storedProcedure: "sp_select_combo_static_status_asset"), "codigo", "descricao", asset.codigoStatus);
            ViewBag.codigoSetor = new SelectList(_combo.Setor(codigoEmpresa, asset.codigoUnidade), "codigo", "descricao", asset.codigoSetor);
            ViewBag.codigoApartamento = new SelectList(_combo.Apartamento(codigoEmpresa, asset.codigoUnidade, (int)asset.codigoSetor), "codigo", "descricao", asset.codigoApartamento);
            ViewBag.codigoUsuarioResponsavel = new SelectList(_combo.Usuario(codigoEmpresa, asset.codigoUnidade), "codigo", "descricao", asset.codigoUsuarioResponsavel);
            ViewBag.codigoUsuario = codigoUsuario;

            if (asset == null)
                return HttpNotFound();

            return View(asset);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssetEdit(int codigoUnidade, string assetCode, string descricao, string numeroSerie, string tag, string contaContabil, string dataCompra, string notaFiscal, long codigo, HttpPostedFileBase arquivo, int tempoDepreciacaoMes = 0, int codigoStatus = -1, int codigoSetor = -1, int codigoApartamento = -1, int codigoUsuarioResponsavel = -1, float valorCompra = 0)
        {
            if (!IsSessionValid())
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });

            var filePath = "";

            if (arquivo != null)
            {
                string uniqueId = Guid.NewGuid().ToString();
                FileInfo fileInfo = new FileInfo(arquivo.FileName);

                var path = Path.Combine(Directory.GetCurrentDirectory(), "asset", "movement");
                filePath = Path.Combine(path, String.Concat(uniqueId, ".", fileInfo.Extension));

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                arquivo.SaveAs(filePath);
            }

            _ativoFixo.UpdateAsset(codigoEmpresa: codigoEmpresa,
                                   codigo: codigo,
                                   codigoUnidade: codigoUnidade,
                                   assetCode: assetCode,
                                   descricao: descricao,
                                   numeroSerie: numeroSerie,
                                   tag: tag,
                                   contaContabil: contaContabil,
                                   dataCompra: dataCompra,
                                   valorCompra: valorCompra,
                                   tempoDepreciacaoMes: tempoDepreciacaoMes,
                                   notaFiscal: notaFiscal,
                                   codigoStatus: codigoStatus,
                                   codigoSetor: codigoSetor,
                                   codigoApartamento: codigoApartamento,
                                   codigoUsuarioResponsavel: codigoUsuarioResponsavel,
                                   arquivo: filePath,
                                   codigoUsuario: codigoUsuario);

            return RedirectToAction("AssetIndex");
        }

        [HttpPost]
        public JsonResult AssetDelete(long codigo)
        {
            defaultResponse _response = new defaultResponse();

            try
            {

                _ativoFixo.DeleteAsset(codigoEmpresa: codigoEmpresa,
                                       codigoUsuario: codigoUsuario,
                                       codigo: codigo);

                _response.success = true;
                _response.message = Properties.Resources.register_deleted;
            }
            catch (Exception ex)
            {
                _response.success = true;
                _response.message = ex.Message;
            }

            return Json(_response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ValidaAsset(string assetCode, long codigo)
        {

            return Json(_ativoFixo.ValidaAsset(codigoEmpresa: codigoEmpresa,
                                               assetCode: assetCode,
                                               codigo: codigo));

        }

        #endregion

        #region ::: ASSET MOVEMENT :::

        public ActionResult assetMovement()
        {
            if (!IsSessionValid())
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });

            bool insert = false;
            bool edit = false;
            bool delete = false;
            bool administrator = false;

            _account.LoadPerfil(iCodigoEmpresa: codigoEmpresa,
                                iCodigoUsuario: codigoUsuario,
                                sFormulario: "assetMovement",
                                bInserir: ref insert,
                                bEditar: ref edit,
                                bExcluir: ref delete,
                                bAdministrador: ref administrator);

            ViewBag.inserir = insert;
            ViewBag.unidade = new SelectList(_combo.Unidade(iCodigoEmpresa: codigoEmpresa, iCodigoUsuario: codigoUsuario, bCadastro: false), "codigo", "descricao", codigoUnidade);
            ViewBag.tipoMovimentacao = new SelectList(_combo.LoadCombo(storedProcedure: "sp_select_combo_stc_tipo_movimentacao_asset"), "codigo", "descricao", null);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult loadAssetMovement(int unidade = -1, int tipoMovimentacao = -1, string assetCode = "", string documento = "", string dataInicio = "", string dataTermino = "", string origem = "", string destino = "")
        {
            var data = _ativoFixo.LoadAssetMovement(codigoEmpresa: codigoEmpresa,
                                                    codigoUnidade: unidade,
                                                    codigoTipoMovimentacao: tipoMovimentacao,
                                                    assetCode: assetCode,
                                                    documento: documento,
                                                    dataInicio: dataInicio,
                                                    dataTermino: dataTermino,
                                                    origem: origem,
                                                    destino: destino);

            return new JsonResult
            {
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue,
                RecursionLimit = 100
            };
        }

        #endregion

        #region ::: ASSET MOVEMENT - INSERT :::

        public ActionResult assetMovementInsert()
        {

            ViewBag.unidade = new SelectList(_combo.Unidade(iCodigoEmpresa: codigoEmpresa, iCodigoUsuario: codigoUsuario, bCadastro: false), "codigo", "descricao", codigoUnidade);
            ViewBag.tipoMovimentacao = new SelectList(_combo.LoadCombo(storedProcedure: "sp_select_combo_stc_tipo_movimentacao_asset"), "codigo", "descricao", null);
            ViewBag.setor = new SelectList(_combo.LoadCombo(storedProcedure: "sp_select_combo_cadastro_basico_setor", codigoEmpresa: codigoEmpresa, codigoUnidade: codigoUnidade), "codigo", "descricao", null);
            ViewBag.apartamento = new SelectList(_combo.Apartamento(iCodigoEmpresa: codigoEmpresa, iCodigoUnidade: codigoUnidade), "codigo", "descricao", null);
            ViewBag.fornecedor = new SelectList(_combo.LoadCombo(storedProcedure: "sp_select_combo_cadastro_basico_fornecedor", codigoEmpresa: codigoEmpresa, codigoUnidade: codigoUnidade), "codigo", "descricao", null);
            ViewBag.asset = new SelectList(_combo.LoadCombo(storedProcedure: "sp_select_combo_cadastro_basico_asset", codigoEmpresa: codigoEmpresa, codigoUnidade: codigoUnidade), "codigo", "descricao", null);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult assetMovementInsert(int unidade, int tipoMovimentacao, string dataMovimentacao, string documento, long asset, HttpPostedFileBase arquivo, int setor = -1, int apartamento = -1, int fornecedor = -1, string valor = "R$ 0,00", string observacao = "")
        {

            var filePath = "";

            if (arquivo != null)
            {
                string uniqueId = Guid.NewGuid().ToString();
                FileInfo fileInfo = new FileInfo(arquivo.FileName);

                var path = Path.Combine(Directory.GetCurrentDirectory(), "asset", "movement");
                filePath = Path.Combine(path, String.Concat(uniqueId, ".", fileInfo.Extension));

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                arquivo.SaveAs(filePath);
            }

            _ativoFixo.InsertAssetMovement(codigoEmpresa: codigoEmpresa,
                                           codigoUnidade: unidade,
                                           codigoTipoMovimentacao: tipoMovimentacao,
                                           dataMovimentacao: dataMovimentacao,
                                           documento: documento,
                                           codigoAsset: asset,
                                           codigoSetorDestino: setor,
                                           codigoApartamentoDestino: apartamento,
                                           codigoFornecedorDestino: fornecedor,
                                           valor: Convert.ToDouble(valor.Replace("R$", "").Replace(".", "").Replace(",", ".")),
                                           observacao: observacao,
                                           arquivo: filePath,
                                           codigoUsuario: codigoUsuario);

            return RedirectToAction("assetMovement");

        }

        [HttpPost]
        public JsonResult LoadConfiguracaoTipoMovimentacao(int tipoMovimentacao)
        {
            AssetTipoMovimentacaoConfig _return = new AssetTipoMovimentacaoConfig();

            try
            {
                _return = _ativoFixo.LoadConfiguracaoTipoMovimentacao(codigoTipoMovimentacao: tipoMovimentacao);
            }
            catch (Exception ex)
            {
                _return.success = false;
                _return.message = ex.Message.ToString();
            }

            return Json(_return);

        }

        #endregion

        #region ::: ASSET INVENTORY MANAGER :::

        public ActionResult assetInventoryMng()
        {
            if (!IsSessionValid())
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });

            bool insert = false;
            bool edit = false;
            bool delete = false;
            bool administrator = false;

            _account.LoadPerfil(iCodigoEmpresa: codigoEmpresa,
                                iCodigoUsuario: codigoUsuario,
                                sFormulario: "assetInventoryMng",
                                bInserir: ref insert,
                                bEditar: ref edit,
                                bExcluir: ref delete,
                                bAdministrador: ref administrator);

            ViewBag.administrator = administrator;
            ViewBag.inventoryOpened = _ativoFixo.HasInventoryOpened(codigoEmpresa: codigoEmpresa,
                                                                    codigoUnidade: codigoUnidade);

            ViewBag.unidade = new SelectList(_combo.Unidade(iCodigoEmpresa: codigoEmpresa, iCodigoUsuario: codigoUsuario, bCadastro: false), "codigo", "descricao", codigoUnidade);
            ViewBag.statusInventario = new SelectList(_combo.LoadCombo("sp_select_combo_stc_status_inventario_asset"), "codigo", "descricao", null);

            return View();
        }

        [HttpPost]
        public JsonResult loadAssetInventoryMng(int unidade, string dataInicio, string dataTermino, string descricao, int statusInventario = -1)
        {
            return Json(_ativoFixo.LoadAssetInventoryMng(codigoEmpresa: codigoEmpresa,
                                                         codigoUnidade: unidade,
                                                         descricao: descricao,
                                                         dataInicio: dataInicio,
                                                         dataTermino: dataTermino,
                                                         statusInventario: statusInventario));
        }

        [HttpPost]
        public JsonResult loadAssetInventoryMngDetails(long codigoInventario)
        {
            return Json(_ativoFixo.LoadAssetInventoryMngDetails(codigoInventario: codigoInventario));
        }

        public ActionResult assetInventoryInsert()
        {
            if (!IsSessionValid())
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });

            ViewBag.unidade = new SelectList(_combo.Unidade(iCodigoEmpresa: codigoEmpresa, iCodigoUsuario: codigoUsuario, bCadastro: false), "codigo", "descricao", codigoUnidade);

            return View();
        }

        [HttpPost]
        public ActionResult assetInventoryInsert(int unidade, string descricao, string contadoresJson)
        {

            defaultResponse _response = new defaultResponse();

            _ativoFixo.InsertInventory(codigoEmpresa: codigoEmpresa,
                                       codigoUnidade: unidade,
                                       descricao: descricao,
                                       codigoUsuario: codigoUsuario,
                                       contadoresJson: contadoresJson);

            _response.success = true;
            _response.message = Resources.inventoryInsertedSuccefully;
            return RedirectToAction("assetInventoryInsert");
        }

        [HttpPost]
        public JsonResult closeInventory(int unidade)
        {
            defaultResponse _response = new defaultResponse();

            try
            {

                _ativoFixo.CloseAssetInventory(codigoEmpresa: codigoEmpresa,
                                               codigoUnidade: unidade,
                                               codigoUsuario: codigoUsuario);

                _response.success = true;
                _response.message = Resources.inventoryClosedSuccefully;
            }
            catch (Exception ex)
            {
                _response.success = true;
                _response.message = ex.Message;
            }

            return Json(_response);
        }

        public ActionResult assetInventoryClose(long codigo)
        {
            if (!IsSessionValid())
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });

            AssetInventoryInfo _info = _ativoFixo.LoadAssetInventoryInfo(codigo: codigo);

            ViewBag.info = _info;
            ViewBag.tipoMovimentacao = new SelectList(_combo.LoadCombo(storedProcedure: "sp_select_combo_stc_tipo_movimentacao_asset"), "codigo", "descricao", null);
            ViewBag.setor = new SelectList(_combo.LoadCombo(storedProcedure: "sp_select_combo_cadastro_basico_setor", codigoEmpresa: codigoEmpresa, codigoUnidade: _info.codigoUnidade), "codigo", "descricao", null);
            ViewBag.apartamento = new SelectList(_combo.Apartamento(iCodigoEmpresa: codigoEmpresa, iCodigoUnidade: _info.codigoUnidade), "codigo", "descricao", null);
            ViewBag.fornecedor = new SelectList(_combo.LoadCombo(storedProcedure: "sp_select_combo_cadastro_basico_fornecedor", codigoEmpresa: codigoEmpresa, codigoUnidade: _info.codigoUnidade), "codigo", "descricao", null);

            return View();
        }

        [HttpPost]
        public JsonResult loadAssetInventoried(long codigoInventario)
        {
            return Json(_ativoFixo.LoadAssetInventoried(codigo: codigoInventario,
                                                        type: 0));
        }

        [HttpPost]
        public JsonResult loadAssetNotFinded(long codigoInventario)
        {
            return Json(_ativoFixo.LoadAssetInventoried(codigo: codigoInventario,
                                                        type: 1));
        }

        [HttpPost]
        public JsonResult loadAssetNotRegistered(long codigoInventario)
        {
            return Json(_ativoFixo.LoadAssetInventoried(codigo: codigoInventario,
                                                        type: 2));
        }

        [HttpPost]
        public JsonResult insertInventoryMovementAsset(long codigo, int tipoMovimentacao, string dataMovimentacao, string documento, string assetCode, HttpPostedFileBase arquivo, int setor = -1, int apartamento = -1, int fornecedor = -1, string valor = "R$ 0,00", string observacao = "")
        {
            defaultResponse _return = new defaultResponse();

            try
            {

                var filePath = "";

                if (arquivo != null)
                {
                    string uniqueId = Guid.NewGuid().ToString();
                    FileInfo fileInfo = new FileInfo(arquivo.FileName);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "asset", "movement");
                    filePath = Path.Combine(path, String.Concat(uniqueId, ".", fileInfo.Extension));

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    arquivo.SaveAs(filePath);
                }

                _ativoFixo.InsertInventoryAssetMovement(codigoInventario: codigo,
                                                        codigoEmpresa: codigoEmpresa,
                                                        codigoTipoMovimentacao: tipoMovimentacao,
                                                        dataMovimentacao: dataMovimentacao,
                                                        documento: documento,
                                                        assetCode: assetCode,
                                                        codigoSetorDestino: setor,
                                                        codigoApartamentoDestino: apartamento,
                                                        codigoFornecedorDestino: fornecedor,
                                                        valor: Convert.ToDouble(valor.Replace("R$", "").Replace(".", "").Replace(",", ".")),
                                                        observacao: observacao,
                                                        arquivo: filePath,
                                                        codigoUsuario: codigoUsuario);

                _return.success = true;
                _return.message = Resources.operacao_realizaca_sucesso;

            }
            catch (Exception ex)
            {
                _return.success = false;
                _return.message = ex.Message.ToString();
            }

            return Json(_return);

        }

        #endregion

        #region ::: ASSET INVENTORY :::

        public ActionResult assetInventory()
        {
            if (!IsSessionValid())
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });

            bool insert = false;
            bool edit = false;
            bool delete = false;
            bool administrator = false;

            _account.LoadPerfil(iCodigoEmpresa: codigoEmpresa,
                                iCodigoUsuario: codigoUsuario,
                                sFormulario: "assetInventory",
                                bInserir: ref insert,
                                bEditar: ref edit,
                                bExcluir: ref delete,
                                bAdministrador: ref administrator);

            ViewBag.administrator = administrator;

            ViewBag.codigoUnidade = codigoUnidade;
            ViewBag.codigoSetor = new SelectList(_combo.Setor(iCodigoEmpresa: codigoEmpresa, iCodigoUnidade: codigoUnidade), "codigo", "descricao", null);
            ViewBag.codigoApartamento = new SelectList(_combo.Apartamento(iCodigoEmpresa: codigoEmpresa, iCodigoUnidade: codigoUnidade), "codigo", "descricao", null);


            AssetInventoryInfo inventoryInfo = _ativoFixo.GetInventarioAtivo(codigoEmpresa: codigoEmpresa,
                                                                             codigoUnidade: codigoUnidade);

            ViewBag.codigoInventario = inventoryInfo.codigoInventario;

            List<AssetInventory> assetInventory = _ativoFixo.LoadInventory(codigoInventario: inventoryInfo.codigoInventario,
                                                                           codigoEmpresa: codigoEmpresa,
                                                                           codigoUnidade: codigoUnidade,
                                                                           codigoSetor: -1,
                                                                           codigoApartamento: -1);

            Session["app_undo_controller"] = "Home";
            Session["app_undo_action"] = "Index";
            Session["app_header_title"] = inventoryInfo.descricao;
            Session["app_header_subtitle"] = "";

            return View(assetInventory);
        }

        [HttpPost]
        public JsonResult insertAssetInventory(long codigoInventario, int unidade, int setor, int apartamento, string assetCode, bool ativoCadastrado, string descricaoInformada)
        {
            defaultResponse response = new defaultResponse();

            try
            {
                _ativoFixo.InsertInventoryAsset(codigoInventario: codigoInventario,
                                                codigoEmpresa: codigoEmpresa,
                                                codigoUnidade: unidade,
                                                codigoSetor: setor,
                                                codigoApartamento: apartamento,
                                                assetCode: assetCode,
                                                codigoUsuario: codigoUsuario,
                                                ativoCadastrado: ativoCadastrado,
                                                descricaoInformada: descricaoInformada);

                response.success = true;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }

            return Json(response);
        }

        [HttpPost]
        public JsonResult loadAssetInventory(long codigoInventario, int unidade, int setor, int apartamento)
        {

            return Json(_ativoFixo.LoadInventory(codigoInventario: codigoInventario,
                                                 codigoEmpresa: codigoEmpresa,
                                                 codigoUnidade: unidade,
                                                 codigoSetor: setor,
                                                 codigoApartamento: apartamento));

        }

        [HttpPost]
        public JsonResult validaAssetInventory(int unidade, string assetCode)
        {
            bool exists = _ativoFixo.ExistsAsset(codigoEmpresa: codigoEmpresa,
                                                 codigoUnidade: unidade,
                                                 assetCode: assetCode);

            return Json(new { success = exists });
        }

        #endregion

        #region ::: ASSET MOVEMENT :::

        //public ActionResult assetMovement()
        //{
        //    if (!IsSessionValid())
        //        return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });

        //    bool insert = false;
        //    bool edit = false;
        //    bool delete = false;
        //    bool administrator = false;

        //    _account.LoadPerfil(iCodigoEmpresa: codigoEmpresa,
        //                        iCodigoUsuario: codigoUsuario,
        //                        sFormulario: "assetMovement",
        //                        bInserir: ref insert,
        //                        bEditar: ref edit,
        //                        bExcluir: ref delete,
        //                        bAdministrador: ref administrator);

        //    ViewBag.administrator = administrator;
        //    ViewBag.unidade = new SelectList(_combo.Unidade(iCodigoEmpresa: codigoEmpresa, iCodigoUsuario: codigoUsuario, bCadastro: false), "codigo", "descricao", codigoUnidade);

        //    return View();
        //}

        //[HttpPost]
        //public JsonResult loadAssetMovement(int unidade, string dataInicio, string dataTermino, string descricao, string assetCode)
        //{
        //    return Json(_ativoFixo.LoadAssetMovement(codigoEmpresa: codigoEmpresa,
        //                                             codigoUnidade: unidade,
        //                                             descricao: descricao,
        //                                             dataInicio: dataInicio,
        //                                             dataTermino: dataTermino,
        //                                             assetCode: assetCode));
        //}

        #endregion

    }
}