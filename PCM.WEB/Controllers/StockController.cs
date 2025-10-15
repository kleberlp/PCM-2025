using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
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
    public class StockController : Controller
    {
        private CadastroBasico oCadastroBasico = new CadastroBasico(sCon: ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Combo oCombo = new Combo(sCon: ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Stock oStock = new Stock(sCon: ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        private Account oAccount = new Account(sCon: ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        #region ::: JSON :::

        public JsonResult LoadComboProductStock(int unidade)
        {
            return Json(oCombo.LoadCombo("sp_select_combo_register_product_stock", codigoUnidade: unidade));
        }

        public JsonResult LoadComboPurchaseOrder(int unidade)
        {
            return Json(oCombo.LoadCombo("sp_select_combo_stock_purchase_order", codigoUnidade: unidade));
        }

        public JsonResult LoadComboPurchaseOrderSupplier(string branch)
        {
            return Json(oCombo.LoadCombo("sp_select_combo_stock_purchase_order_supplier", sCode: branch));
        }

        public JsonResult LoadComboPurchaseOrderProduct(string branch, string purchase_order)
        {
            return Json(oCombo.LoadCombo("sp_select_combo_stock_purchase_order_product", sCode: branch, sCode2: purchase_order));
        }

        public JsonResult LoadComboProductUomStock(long product)
        {
            return Json(oCombo.LoadCombo("sp_select_combo_register_product_uom_stock", sCode: product.ToString()));
        }

        public JsonResult LoadComboProductBatch(long product)
        {
            return Json(oCombo.LoadCombo("sp_select_combo_register_product_batch_stock", sCode: product.ToString()));
        }

        public JsonResult LoadComboRequester(string branch)
        {
            return Json(oCombo.LoadCombo("sp_select_combo_register_user_branch" sCode: branch));
        }

        public JsonResult LoadComboCostCenter(string branch)
        {
            return Json(oCombo.LoadCombo("sp_select_combo_register_cost_center", sCode: branch));
        }

        public JsonResult LoadComboBinPosition(string branch)
        {
            return Json(oCombo.LoadCombo("sp_select_combo_register_bin_position_stock", sCode: branch, sCode2: "SIM"));
        }

        public JsonResult LoadComboProductBinPosition(string branch, long product)
        {
            return Json(oCombo.LoadCombo("sp_select_combo_register_product_bin_position_stock", sCode: branch, sCode2: product.ToString()));
        }

        public JsonResult LoadComboSupplier(string branch)
        {
            return Json(oCombo.LoadCombo("sp_select_combo_register_supplier_new", sCode: branch));
        }

        public JsonResult LoadProductStockInfo(string branch, string barcode = "", long product = -1, long purchase_order = -1)
        {
            return Json(oInventory.ProductStockInfo(sBranch: branch,
                                                    sBarcode: barcode,
                                                    lProduct: product,
                                                    lPurcharOrderId: purchase_order));
        }

        public JsonResult LoadUOMInfo(int uom)
        {
            return Json(oInventory.UOMInfo(iUOM: uom));
        }

        #endregion

        #region ::: STOCK PICKING :::

        public ActionResult StockPicking()
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
                                    sFormulario: "stock_picking",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.branch = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_branch", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "code", "description", Session["branch"].ToString());
                ViewBag.requester = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_user_branch", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "code", "description", null);
                ViewBag.product = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_product_stock", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "code", "description", null);
                ViewBag.uom = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_product_uom_stock", codigoEmpresa: -1), "code", "description", null);
                ViewBag.batch = new SelectList(oCombo.LoadCombo("sp_select_combo_register_product_batch_stock", codigoEmpresa: -1), "code", "description", null);
                ViewBag.cost_center = new SelectList(oCombo.LoadCombo("sp_select_combo_register_cost_center", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "code", "description", null);
                ViewBag.bin_position = new SelectList(oCombo.LoadCombo("sp_select_combo_register_bin_position_stock", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString()), sCode2:"SIM"), "code", "description", null);

                List<stock_picking> picking = new List<stock_picking>();
                stock_picking info = new stock_picking();
                info.deleted = 1;
                picking.Add(info);

                return View(picking);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StockPicking(string branchSave, string requesterSave, string dateSave, List<stock_picking> picking, int costCenterSave = -1)
        {

            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                string docNumber = oStock.GetDocumentNumberPicking(sBranch: branchSave);

                foreach (stock_picking item in picking)
                {

                    if ((item.product_id != 0) && (item.deleted == 0))
                    {

                        //Insere Registro no Banco de Dados
                        oStock.InsertStockProduct(sBranch: branchSave,
                                                  sDocumentNumber: docNumber,
                                                  sDocumentDate: dateSave,
                                                  sDocumentType: "S",
                                                  sPurchaseOrder: "",
                                                  sPathFile: "",
                                                  sRequester: requesterSave,
                                                  lProductID: item.product_id,
                                                  iUOMID: item.uom_id,
                                                  sBatch: item.batch,
                                                  dQuantity: item.quantity,
                                                  dProductValue: "0",
                                                  sCurrentUser: User.Identity.GetUserName(),
                                                  iCostCenter: costCenterSave);

                    }

                }

                //Redireciona para Index
                return RedirectToAction("StockPicking");
            }
        }

        #endregion

        #region ::: REPLENISHMENT :::

        public ActionResult StockReplenishment()
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
                                    sFormulario: "stock_replenishment",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.branch = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_branch", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "code", "description", Session["branch"].ToString());
                ViewBag.product = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_product_stock", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "code", "description", null);
                ViewBag.uom = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_product_uom_stock", sCode: "-1"), "code", "description", null);
                ViewBag.purchase_order = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_stock_purchase_order_supplier", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "code", "description", null);
                ViewBag.supplier = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_supplier", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "code", "description", null);

                ViewBag.date = DateTime.Now.ToShortDateString();
                List<stock_replenishment> replenishment = new List<stock_replenishment>();
                stock_replenishment info = new stock_replenishment();
                info.deleted = 1;
                replenishment.Add(info);
                return View(replenishment);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StockReplenishment(string branchSave, string documentSave, string purchaseOrderSave, string supplierSave, string dateSave, List<stock_replenishment> replenishment)
        {
            if (Session["language"] == null || User.Identity.GetUserName() == "")
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                string folder = Server.MapPath("~/Content/arq/replenishment");
                string file = "";

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                long id = 0;
                
                //Insere Registro no Banco de Dados
                oStock.InsertReceipt(sBranch: branchSave,
                                     sDocumentNumber: documentSave,
                                     sDocumentDate: dateSave,
                                     sPurchaseOrder: purchaseOrderSave,
                                     iSupplierId: (purchaseOrderSave == "")? Convert.ToInt32(supplierSave) : Convert.ToInt32(purchaseOrderSave.Split(';')[2]),
                                     sCurrentUser: User.Identity.GetUserName(),
                                     lID: ref id);

                foreach (stock_replenishment item in replenishment)
                {

                    if ((item.product_id != 0) && (item.deleted == 0))
                    {

                        if (item.arquivo != null)
                        {
                            file = System.IO.Path.Combine(folder, Guid.NewGuid().ToString() + "." + item.arquivo.Split(';')[0].Split('/')[1]);
                            System.IO.File.WriteAllBytes(file, Convert.FromBase64String(item.arquivo.Split(',')[1]));
                        }

                        //Insere Registro no Banco de Dados
                        oStock.InsertReceiptProduct(sBranch: branchSave,
                                                    lReceiptId: id,
                                                    lPurchaseOrderId: (purchaseOrderSave == "") ? -1 : Convert.ToInt32(purchaseOrderSave.Split(';')[0]),
                                                    lProductID: item.product_id,
                                                    sBatch: item.batch,
                                                    sExpirationDate: item.expiration_date,
                                                    sUOM: item.uom,
                                                    dQuantity: item.quantity,
                                                    dUnitaryValue: item.unitary_value,
                                                    sPathFile: file,
                                                    sCurrentUser: User.Identity.GetUserName());

                    }

                }

                //Redireciona para Index
                return RedirectToAction("StockReplenishment");
            }
        }

        public JsonResult LoadProductInfo(int codigoEmpresa, int codigoUnidade, string product)
        {
            return Json(oStock.ProductInfo(codigoEmpresa: codigoEmpresa,
                                           codigoUnidade: codigoUnidade,
                                           product: product));
        }

        #endregion

        #region ::: REQUEST PO :::

        // GET: INDEX
        public ActionResult RequestPOIndex()
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
                                    sFormulario: "stock_request",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.branch = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_branch", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "code", "description", Session["branch"].ToString());
                ViewBag.supplier = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_supplier", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "code", "description", null);
                ViewBag.supplier_po = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_supplier", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "code", "description", null);
                ViewBag.status = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_static_status_request_po", sCode: Session["language"].ToString()), "code", "description", null);

                ViewBag.start_date = DateTime.Now.AddDays(-7).ToShortDateString();
                ViewBag.end_date = DateTime.Now.ToShortDateString();

                return View();
            }
        }

        //JSON: /LOAD PURCHASE ORDER
        public JsonResult LoadRequestPO(string branch, string request_number, string start_date, string end_date, int status = -1, int supplier = -1)
        {
            return Json(oStock.IndexRequestPO(sBranch: branch,
                                              sRequestNumber: request_number,
                                              iSupplierID: supplier,
                                              sStartDate: start_date,
                                              sEndDate: end_date,
                                              iStatus: status,
                                              sLanguageID: Session["language"].ToString()));

        }

        //JSON: /LOAD PURCHASE ORDER - PRODUCT
        public JsonResult LoadRequestPOProduct(string branch, long request_po_id)
        {
            return Json(oStock.LoadRequestPOProduct(sBranch: branch,
                                                    lRequestPOID: request_po_id));

        }

        //JSON: /LOAD PURCHASE ORDER - PRODUCT
        public JsonResult LoadRequestPOProductSupplier(string branch, int supplier_id)
        {
            return Json(oStock.LoadRequestProduct(sBranch: branch,
                                                  iSupplierID: supplier_id));

        }

        // GET: PurchaseOrderInsert  
        [Authorize]
        public ActionResult RequestPOInsert()
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
                                    sFormulario: "stock_request",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.branch = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_branch", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "code", "description", Session["branch"].ToString());
                ViewBag.product = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_product_stock", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "code", "description", null);
                ViewBag.uom = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_product_uom_stock", sCode: "-1"), "code", "description", null);

                List<stock_request_po_product> products = new List<stock_request_po_product>();
                stock_request_po_product product = new stock_request_po_product();
                product.deleted = 1;
                products.Add(product);

                return View(products);
            }
        }


        // POST: /PurchaseOrderInsert        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestPOInsert(string branch, List<stock_request_po_product> products, int supplier = -1)
        {
            long requestPOID = 0;

            // Serializa a lista de produtos em JSON
            string productsJson = JsonConvert.SerializeObject(products);

            //Chama a função para inserir o pedido e produtos usando JSON
            oStock.InsertRequestPO(sBranch: branch,
                                   iSupplierID: supplier,
                                   sCurrentUser: User.Identity.GetUserName(),
                                   sProductsJson: productsJson,
                                   lRequestPOID: ref requestPOID);

            // Redireciona para Index
            return RedirectToAction("RequestPOIndex");
        }


        // GET: PurchaseOrderInsert 
        [Authorize]
        public ActionResult RequestPOInsertSparePoint()
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
                                    sFormulario: "stock_request",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;

                ViewBag.branch = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_branch", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "code", "description", Session["branch"].ToString());
                ViewBag.supplier = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_supplier", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "code", "description", null);

                List<stock_request_po_product> products = new List<stock_request_po_product>();
                stock_request_po_product product = new stock_request_po_product();
                product.deleted = 1;
                products.Add(product);

                return View(products);
            }
        }
        
        // POST: /PurchaseOrderInsert        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestPOInsertSparePoint(string branch, List<stock_request_po_product> products, int supplier = -1)
        {
            long requestPOID = 0;

            // Serializa a lista de produtos em JSON
            string productsJson = JsonConvert.SerializeObject(products);

            //Chama a função para inserir o pedido e produtos usando JSON
            oStock.InsertRequestPO(sBranch: branch,
                                   iSupplierID: supplier,
                                   sCurrentUser: User.Identity.GetUserName(),
                                   sProductsJson: productsJson,
                                   lRequestPOID: ref requestPOID);

            // Redireciona para Index
            return RedirectToAction("RequestPOIndex");
        }

        //JSON: /Update Status
        public JsonResult UpdateRequestPOStatus(long id, string branch, HttpPostedFileBase arquivo, long supplier = -1, string purchase_order = "", int status = 2)
        {
            try
            {

                string path = "";

                if (arquivo != null)
                {
                    
                    string folder = Server.MapPath("~/Content/arq/Inventory/ApprovalPO");

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

                oStock.UpdateRequestPOStatus(sBranch: branch,
                                             lRequestPOID: id,
                                             lSupplierID: supplier,
                                             sPurchaseOrder: purchase_order.ToUpper(),
                                             iStatus: status,
                                             sCurrentUser: User.Identity.GetUserName(),
                                             sPathFile: path);

                
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            //Redireciona para Index

            if (status == 2)
            {
                return Json(Properties.Resources.purchase_order_created);
            }
            else
            {
                return Json(Properties.Resources.document_cancel);
            }
        }

        #endregion

        #region ::: PURCHASE ORDER :::

        // GET: INDEX
        public ActionResult PurchaseOrderIndex()
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
                                    sFormulario: "stock_purchase_order",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.start_date = DateTime.Now.AddDays(-7).ToShortDateString();
                ViewBag.end_date = DateTime.Now.ToShortDateString();

                ViewBag.branch = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_branch", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "code", "description", Session["branch"].ToString());
                ViewBag.supplier = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_supplier", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "code", "description", null);
                ViewBag.status = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_static_status_purchase_order", sCode: Session["language"].ToString()), "code", "description", null);
                    
                return View();
            }
        }

        //JSON: /LOAD PURCHASE ORDER
        public JsonResult LoadPurchaseOrder(string branch, string purchase_order, string start_date, string end_date, int supplier = -1, int status = -1)
        {
            return Json(oStock.IndexPurchaseOrder(sBranch: branch,
                                                  sPurchaseOrder: purchase_order,
                                                  iSupplierID: supplier,
                                                  iStatus: status,
                                                  sStartDate: start_date,
                                                  sEndDate: end_date,
                                                  sLanguageId: Session["language"].ToString()));

        }

        //JSON: /LOAD PURCHASE ORDER - PRODUCT
        public JsonResult LoadPurchaseOrderProduct(string branch, long purchase_order_id)
        {
            return Json(oStock.LoadPurchaseOrderProduct(sBranch: branch,
                                                        lPurchaseOrderID: purchase_order_id));

        }

        public JsonResult PurchaseOrderDelete(long purchase_order_id, string branch)
        {
            try
            {
                oStock.DeletePurchaseOrder(sBranch: branch,
                                           lPurchaseOrderID: purchase_order_id,
                                           sCurrentUser: User.Identity.GetUserName());

                return Json(Properties.Resources.register_canceled);
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

        #region ::: RECEIPT :::

        // GET: INDEX
        public ActionResult ReceiptIndex()
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
                                    sFormulario: "stock_purchase_order",
                                    bInserir: ref inserir,
                                    bEditar: ref editar,
                                    bExcluir: ref excluir,
                                    bAdministrador: ref administrador);

                ViewBag.inserir = inserir;
                ViewBag.editar = editar;
                ViewBag.excluir = excluir;
                ViewBag.start_date = DateTime.Now.AddDays(-7).ToShortDateString();
                ViewBag.end_date = DateTime.Now.ToShortDateString();

                ViewBag.branch = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_branch", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "code", "description", Session["branch"].ToString());
                ViewBag.supplier = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_supplier_new", codigoEmpresa: Convert.ToInt32(Session["empresa"].ToString())), "code", "description", null);
                ViewBag.status = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_static_status_receipt_product", sCode: Session["language"].ToString()), "code", "description", 1);

                return View();
            }
        }

        public JsonResult LoadReceipt(string branch, string purchase_order, string document_number, string start_date, string end_date, int supplier = -1, int status = -1)
        {
            return Json(oStock.IndexReceiptApproval(sBranch: branch,
                                                    sPurchaseOrder: purchase_order,
                                                    sDocumentNumber: document_number,
                                                    iSupplierID: supplier,
                                                    iStatus: status,
                                                    sStartDate: start_date,
                                                    sEndDate: end_date,
                                                    sLanguageId: Session["language"].ToString()));

        }

        public JsonResult UpdateReceiptStatus(long receipt_id, long product_id, string branch, int status)
        {
            try
            {
                oStock.UpdateReceiptProductStatus(sBranch: branch,
                                                  lReceiptId: receipt_id,
                                                  lProductId: product_id,
                                                  iStatus: status,
                                                  sCurrentUser: User.Identity.GetUserName());

                if (status == 2)
                {
                    return Json(Properties.Resources.document_approval);
                }
                else
                {
                    return Json(Properties.Resources.document_cancel);
                }

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        #endregion

        #region ::: INVENTORY :::

        public ActionResult StockInventory()
        {
            if (Session["empresa"] == null)
            {
                return RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl });
            }
            else
            {

                ViewBag.branch = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_branch", sCode: User.Identity.GetUserName()), "code", "description", Session["branch"].ToString());

                stock_inventory inventory = new stock_inventory();

                inventory.result = new List<stock_inventory_details>();
                inventory.result_error = new List<stock_inventory_details_error>();

                return View(inventory);
            }
        }
        
        [HttpPost]
        public ActionResult StockInventory(string branch, HttpPostedFileBase arquivo, string worksheet, string type)
        {

            stock_inventory inventory = new stock_inventory();

            if (type == "arquivo")
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

                    List<stock_inventory_details> result = new List<stock_inventory_details>();
                    List<stock_inventory_details_error> error = new List<stock_inventory_details_error>();

                    //Upload Arquivo
                    oStock.UploadInventoryExcel(sFile: path,
                                                sWorksheet: worksheet,
                                                sBranch: branch,
                                                sLanguageID: Session["language"].ToString(),
                                                sUsername: User.Identity.GetUserName(),
                                                oDetails: ref result,
                                                oDetailsError: ref error);

                    inventory.result = result;
                    inventory.result_error = error;
                    inventory.success = true;

                    return View(inventory);

                }
                catch (Exception ex)
                {
                    inventory.success = false;
                    inventory.message = ex.Message;
                    inventory.result = new List<stock_inventory_details>();
                    inventory.result_error = new List<stock_inventory_details_error>();

                    return View(inventory);
                }
                finally
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    ViewBag.branch = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_branch", sCode: User.Identity.GetUserName()), "code", "description", branch);
                }

            }
            else if (type == "save")
            {

                oStock.UpdateInventory(sBranch: branch,
                                        sUsername: User.Identity.GetUserName(),
                                        sLanguageID: Session["language"].ToString());

                ViewBag.branch = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_register_branch", sCode: User.Identity.GetUserName()), "code", "description", Session["branch"].ToString());

                inventory.message = Properties.Resources.sheet_upload;

                inventory.result = new List<stock_inventory_details>();
                inventory.result_error = new List<stock_inventory_details_error>();

            }

            return View(inventory);

        }

        // GET: /Download Excel
        [HttpGet]
        public virtual ActionResult StockInventoryDownloadExcel()
        {

            string nome_relatorio = "INV_PRODUCT.xlsx";
            string filename = "INV_PRODUCT.xlsx";
            string path = Server.MapPath("~/Content/Files");
            string arquivo = System.IO.Path.Combine(path, nome_relatorio);

            return File(arquivo, "application/vnd.ms-excel", filename);
        }

        #endregion

    }
}