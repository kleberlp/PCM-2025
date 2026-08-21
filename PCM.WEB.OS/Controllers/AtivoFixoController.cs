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

            // Sem uniqueId (atalho do PWA) ou link inválido/expirado: a tela abre
            // pedindo o e-mail do contador para localizar o inventário em aberto
            if (string.IsNullOrWhiteSpace(uniqueId) || inventory.codigoEmpresa <= 0)
            {
                ViewBag.solicitarEmail = true;
                ViewBag.uniqueId = "";

                return View(new AssetInventoryViewModel { inventory = inventory });
            }

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

        // Localiza o inventário em aberto pelo e-mail do contador (app aberto sem uniqueId)
        [HttpPost]
        public JsonResult identificarContador(string email)
        {
            try
            {
                email = (email ?? "").Trim();

                if (string.IsNullOrWhiteSpace(email))
                {
                    return Json(new { success = false, message = "Informe o e-mail cadastrado." });
                }

                string uniqueId = _ativoFixo.GetUniqueIdByEmail(email);

                //O acesso não é liberado direto na tela: o link vai para o e-mail
                //cadastrado, que é quem de fato comprova a identidade do contador
                if (!string.IsNullOrWhiteSpace(uniqueId))
                {
                    string link = $"{Request.Scheme}://{Request.Host}{Url.Action("assetInventory", "AtivoFixo", new { uniqueId = uniqueId })}";

                    _ativoFixo.EnviarAcessoContador(email: email, link: link);
                }

                //Resposta igual achando ou não: não revela quais e-mails estão cadastrados
                return Json(new
                {
                    success = true,
                    message = "Se este e-mail estiver vinculado a um inventário em aberto, você receberá o link de acesso em instantes. Verifique também a caixa de spam."
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
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

            //Ponto 4: ativo encontrado já vem pré-classificado com a última avaliação
            var avaliacao = new AssetLastEvaluation();

            if (exists)
            {
                try
                {
                    avaliacao = _ativoFixo.GetAssetLastEvaluation(codigoEmpresa: empresa,
                                                                  codigoUnidade: unidade,
                                                                  assetCode: assetCode);
                }
                catch (Exception)
                {
                    //Sem a SP de avaliação, segue sem pré-classificação
                }
            }

            return Json(new
            {
                success = exists,
                possuiAvaliacao = avaliacao.possuiAvaliacao,
                statusOk = avaliacao.statusOk,
                observacao = avaliacao.observacao,
                possuiFoto = avaliacao.possuiFoto
            });
        }

        //Nomes do cadastro pré-definido, para o fluxo de cadastro de novo ativo
        [HttpPost]
        public JsonResult buscarDescricaoAtivo(string termo)
        {
            int empresa = HttpContext.Session.GetInt32("inv_codigoEmpresa") ?? -1;
            int unidade = HttpContext.Session.GetInt32("inv_codigoUnidade") ?? -1;

            termo = (termo ?? "").Trim();

            //A busca só começa com 3 caracteres
            if (termo.Length < 3)
            {
                return Json(new List<string>());
            }

            try
            {
                return Json(_ativoFixo.BuscarDescricaoAtivo(codigoEmpresa: empresa,
                                                            codigoUnidade: unidade,
                                                            termo: termo));
            }
            catch (Exception)
            {
                return Json(new List<string>());
            }
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 10_000_000)]
        public async Task<JsonResult> insertAssetInventory(string uniqueId,
                                                           long codigoInventario,
                                                           int unidade,
                                                           int setor,
                                                           int apartamento,
                                                           string assetCode,
                                                           bool ativoCadastrado,
                                                           string descricaoInformada,
                                                           bool statusOk = true,
                                                           string? observacao = null,
                                                           IFormFile? foto = null,
                                                           bool movimentar = false)
        {
            int empresa = HttpContext.Session.GetInt32("inv_codigoEmpresa") ?? -1;
            string codigoUsuario = HttpContext.Session.GetString("inv_codigoUsuario") ?? "";

            try
            {
                bool possuiFoto = foto != null && foto.Length > 0;

                //Observação obrigatória quando o item é apontado como Não OK
                if (!statusOk && string.IsNullOrWhiteSpace(observacao))
                {
                    return Json(new { success = false, message = "Descreva o problema encontrado para registrar o item como Não OK." });
                }

                //Foto é permitida/exigida em 3 casos: avaliação Não OK, novo cadastro e
                //ativo que ainda não possui foto
                if (!possuiFoto)
                {
                    if (!statusOk)
                    {
                        return Json(new { success = false, message = "É necessário tirar uma foto para registrar o item como Não OK." });
                    }

                    if (!ativoCadastrado)
                    {
                        return Json(new { success = false, message = "É necessário tirar uma foto para concluir o cadastro do novo ativo." });
                    }

                    //Ativo já cadastrado e avaliado como OK: só exige foto se ele ainda não tiver uma
                    try
                    {
                        var avaliacao = _ativoFixo.GetAssetLastEvaluation(codigoEmpresa: empresa,
                                                                          codigoUnidade: unidade,
                                                                          assetCode: assetCode);

                        if (!avaliacao.possuiFoto)
                        {
                            return Json(new { success = false, message = "Este ativo ainda não possui foto. Tire uma foto para concluir." });
                        }
                    }
                    catch (Exception)
                    {
                        //Sem a SP de avaliação, não bloqueia a contagem
                    }
                }

                string fotoPath = string.Empty;

                if (possuiFoto)
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

                _ativoFixo.InsertInventoryAsset(uniqueId: uniqueId,
                                                codigoInventario: codigoInventario,
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
                                                fotoPath: fotoPath,
                                                movimentar: movimentar);

                return Json(new { success = true, message = "" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        //Verifica se o ativo já foi contado neste inventário e onde (ponto 5 do teste)
        [HttpPost]
        public JsonResult checkAssetInventoried(long codigoInventario, string assetCode, int setor, int apartamento = -1)
        {
            try
            {
                return Json(_ativoFixo.CheckInventoryAssetLocation(codigoInventario: codigoInventario,
                                                                   assetCode: assetCode,
                                                                   codigoSetor: setor,
                                                                   codigoApartamento: apartamento));
            }
            catch (Exception)
            {
                //Em caso de falha na verificação, segue o fluxo normal de contagem
                return Json(new AssetInventoryCheck());
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