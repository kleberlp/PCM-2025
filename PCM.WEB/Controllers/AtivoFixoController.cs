using Microsoft.AspNet.Identity;
using PCM.WEB.DAL;
using PCM.WEB.MODELS;
using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;

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

            ViewBag.unidade = new SelectList(_combo.Unidade(iCodigoEmpresa: codigoEmpresa,iCodigoUsuario: codigoUsuario,bCadastro: false), "codigo", "descricao", codigoUnidade);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult LoadAsset(int unidade = -1, string codigo = "", string descricao = "", int status =-1, string localizacao = "")
        {
            return Json(_ativoFixo.LoadAsset(codigoEmpresa: codigoEmpresa,
                                             codigoUnidade: unidade,
                                             codigo: codigo,
                                             descricao: descricao,
                                             status: status,
                                             localizacao: localizacao));
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

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssetInsert(AssetModel model)
        {
            if (!IsSessionValid())
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });

            _ativoFixo.InsertAsset(oModel: model);

            return RedirectToAction("AssetInsert");
        }

        public ActionResult AssetEdit(long codigo)
        {
            if (!IsSessionValid())
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });

            var asset = _ativoFixo.LoadAssetInfo(codigoEmpresa: codigoEmpresa,
                                                 codigo: codigo);

            ViewBag.codigoUnidade = new SelectList(_combo.Unidade(iCodigoEmpresa: codigoEmpresa, iCodigoUsuario: codigoUsuario, bCadastro: false), "codigo", "descricao", asset.codigoUnidade);
            ViewBag.codigoStatus = new SelectList(_combo.LoadCombo(storedProcedure: "sp_select_combo_static_status_asset"), "codigo", "descricao", asset.codigoStatus );
            ViewBag.codigoSetor = new SelectList(_combo.Setor(codigoEmpresa, asset.codigoUnidade), "codigo", "descricao", asset.codigoStatus);
            ViewBag.codigoApartamento = new SelectList(_combo.Apartamento(codigoEmpresa, asset.codigoUnidade, (int)asset.codigoSetor), "codigo", "descricao", asset.codigoUnidade);
            ViewBag.codigoUsuarioResponsavel = new SelectList(_combo.Usuario(codigoEmpresa, asset.codigoUnidade), "codigo", "descricao", asset.codigoUsuarioResponsavel);

            if (asset == null)
                return HttpNotFound();

            return View(asset);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssetEdit(AssetModel model)
        {
            if (!IsSessionValid())
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });

            _ativoFixo.UpdateAsset(oModel: model);

            return RedirectToAction("AssetIndex");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AssetDelete(long codigo)
        {
            try
            {

                _ativoFixo.DeleteAsset(codigoEmpresa: codigoEmpresa,
                                       codigoUsuario: codigoUsuario,
                                       codigo: codigo);

                return Json(Properties.Resources.register_deleted);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
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

    }
}