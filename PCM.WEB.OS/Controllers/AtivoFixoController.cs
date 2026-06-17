using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PCM.WEB.OS.DAL;
using PCM.WEB.OS.MODELS;
using Microsoft.AspNetCore.Http;

namespace PCM.WEB.OS.Controllers
{
    public class AtivoFixoController : Controller
    {
        private readonly ILogger<AtivoFixoController> _logger;
        private readonly IConfiguration _configuration;
        private readonly AtivoFixo _ativoFixo;
        private readonly Combo _combo;

        public AtivoFixoController(ILogger<AtivoFixoController> logger, IConfiguration config)
        {
            _logger = logger;
            _configuration = config;
            _ativoFixo = new AtivoFixo(sConnectionString: config.GetConnectionString("DefaultConnection"));
            _combo = new Combo(sConnectionString: config.GetConnectionString("DefaultConnection"));
        }

        public IActionResult assetInventory(string uniqueId = "")
        {
            var inventory = _ativoFixo.InfoInventario(uniqueId);

            // Persiste empresa e unidade na Session para as chamadas AJAX subsequentes
            HttpContext.Session.SetInt32("inv_codigoEmpresa", inventory.codigoEmpresa);
            HttpContext.Session.SetInt32("inv_codigoUnidade", inventory.codigoUnidade);

            var codigoInventario = _ativoFixo.GetInventarioAtivo(codigoEmpresa: inventory.codigoEmpresa, codigoUnidade: inventory.codigoUnidade);

            ViewBag.uniqueId = uniqueId;
            ViewBag.codigoUnidade = inventory.codigoUnidade;
            ViewBag.codigoInventario = codigoInventario;

            ViewBag.codigoSetor = new SelectList(_combo.LoadCombo(storedProcedure: "sp_select_combo_cadastro_basico_setor", codigoEmpresa: inventory.codigoEmpresa, codigoUnidade: inventory.codigoUnidade), "codigo", "descricao", null);

            ViewBag.codigoApartamento = new SelectList(_combo.LoadCombo(storedProcedure: "sp_select_combo_cadastro_basico_apartamento", codigoEmpresa: inventory.codigoEmpresa, codigoUnidade: inventory.codigoUnidade), "codigo", "descricao", null);

            var items = _ativoFixo.LoadAssetInventory(codigoInventario: codigoInventario,
                                                      codigoEmpresa: inventory.codigoEmpresa,
                                                      codigoUnidade: inventory.codigoUnidade,
                                                      codigoSetor: -1,
                                                      codigoApartamento: -1);

            var viewModel = new AssetInventoryViewModel
            {
                inventory = inventory,
                items = items
            };

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult loadListApartamento(int unidade, int setor)
        {
            int empresa = HttpContext.Session.GetInt32("inv_codigoEmpresa") ?? -1;

            var list = _combo.LoadCombo(storedProcedure: "sp_select_combo_asset_cadastro_basico_apartamento", codigoEmpresa: empresa, codigoUnidade: unidade, codigo:setor);

            return Json(list);
        }

        [HttpPost]
        public JsonResult validaAssetInventory(int unidade, string assetCode)
        {
            int empresa = HttpContext.Session.GetInt32("inv_codigoEmpresa") ?? -1;

            bool exists = _ativoFixo.ExistsAsset(codigoEmpresa: empresa,
                                                 codigoUnidade: unidade,
                                                 assetCode: assetCode);

            return Json(new { success = exists });
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 10_000_000)]
        public async Task<JsonResult> insertAssetInventory(long codigoInventario,
                                                           int unidade,
                                                           int setor,
                                                           int apartamento,
                                                           string assetCode,
                                                           bool ativoCadastrado,
                                                           string descricaoInformada,
                                                           bool statusOk = true,
                                                           string? observacao = null,
                                                           IFormFile? foto = null)
        {
            int empresa = HttpContext.Session.GetInt32("inv_codigoEmpresa") ?? -1;
            string codigoUsuario = HttpContext.Session.GetString("inv_codigoUsuario") ?? "";

            try
            {
                string fotoPath = string.Empty;

                if (foto != null && foto.Length > 0)
                {
                    var uploadDir = Path.Combine("C:\\SIM\\PCM\\SITE\\IMAGE\\OS", "INVENTARIO");
                    Directory.CreateDirectory(uploadDir);

                    var ext = Path.GetExtension(foto.FileName);
                    var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp", ".heic", ".heif" };
                    ext = allowed.Contains(ext) ? ext : ".jpg";

                    var fileName = $"{Guid.NewGuid():N}{ext}";
                    var fullPath = Path.Combine(uploadDir, fileName);

                    var imageService = new ImageService();
                    using var stream = foto.OpenReadStream();
                    await imageService.ResizeAndSaveAsync(stream, fullPath);

                    fotoPath = fullPath;
                }

                _ativoFixo.InsertInventoryAsset(codigoInventario: codigoInventario,
                                                codigoEmpresa: empresa,
                                                codigoUnidade: unidade,
                                                codigoSetor: setor,
                                                codigoApartamento: apartamento,
                                                assetCode: assetCode,
                                                codigoUsuario: codigoUsuario,
                                                ativoCadastrado: ativoCadastrado,
                                                descricaoInformada: descricaoInformada,
                                                statusOk: statusOk,
                                                observacao: observacao ?? string.Empty,
                                                fotoPath: fotoPath);

                return Json(new { success = true, message = "" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult loadAssetInventory(long codigoInventario,
                                             int unidade,
                                             int setor,
                                             int apartamento)
        {

            int empresa = HttpContext.Session.GetInt32("inv_codigoEmpresa") ?? -1;

            var items = _ativoFixo.LoadAssetInventory(codigoInventario: codigoInventario,
                                                      codigoEmpresa: empresa,
                                                      codigoUnidade: unidade,
                                                      codigoSetor: setor,
                                                      codigoApartamento: apartamento);

            return Json(items);
        }

    }

}