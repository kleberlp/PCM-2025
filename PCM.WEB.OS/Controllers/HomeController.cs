using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PCM.WEB.OS.DAL;
using PCM.WEB.OS.MODELS;
using System.Configuration;

namespace PCM.WEB.OS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        public OSHospede oOSHospede;
        public Combo oCombo;
        private readonly IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger, IConfiguration config, IWebHostEnvironment env)
        {
            _logger = logger;
            _configuration = config;
            _env = env;
            oOSHospede = new OSHospede(sConnectionString: _configuration.GetConnectionString("DefaultConnection"));
            oCombo = new Combo(sConnectionString: _configuration.GetConnectionString("DefaultConnection"));
        }

        public IActionResult index(string uniqueId = "")
        {
            OSHospedeApartamento apartamento = new OSHospedeApartamento();
            apartamento = oOSHospede.InfoApartamento(uniqueId);

            ViewBag.uniqueId = uniqueId;

            return View(apartamento);
        }

        public IActionResult ordemServico(string uniqueId)
        {
            OSHospedeApartamento apartamento = new OSHospedeApartamento();
            apartamento = oOSHospede.InfoApartamento(uniqueId);


            ViewBag.uniqueId = uniqueId;
            ViewBag.itemOSHospede = new SelectList(oCombo.LoadCombo(storedProcedure: "sp_select_combo_cadastro_basico_item_os_hospede", codigoEmpresa: apartamento.codigoEmpresa, codigoUnidade: apartamento.codigoUnidade), "codigo", "descricao", null);

            return View(apartamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(MultipartBodyLengthLimit = 10_000_000)] // ~10MB
        public async Task<IActionResult> OrdemServico(string uniqueId, long itemOSHospede, string descricao, IFormFile? file, CancellationToken ct)
        {
            string relativePath = string.Empty;
            var fullPath = string.Empty;

            if (file != null && file.Length > 0)
            {
                // Validações
                if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("file", "Apenas imagens são permitidas.");
                }

                const long maxSize = 5 * 1024 * 1024; // 5MB
                if (file.Length > maxSize)
                {
                    ModelState.AddModelError("file", "Imagem muito grande (máx. 5MB).");
                }

                if (ModelState.IsValid)
                {
                    // Pasta de destino: wwwroot/uploads/OS
                    var uploadsRoot = Path.Combine(_env.WebRootPath, "uploads", "OS");
                    Directory.CreateDirectory(uploadsRoot);

                    // Extensão segura/permitida
                    var originalExt = Path.GetExtension(file.FileName);
                    var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                { ".jpg", ".jpeg", ".png", ".webp", ".heic", ".heif" }; 

                    var ext = allowed.Contains(originalExt) ? originalExt : ".jpg";

                    // Nome único
                    var fileName = $"{Guid.NewGuid():N}{ext}";
                    fullPath = Path.Combine(uploadsRoot, fileName);

                    // Grava
                    await using (var stream = System.IO.File.Create(fullPath))
                    {
                        await file.CopyToAsync(stream, ct);
                    }

                    // Caminho relativo para servir na web e salvar no BD
                    relativePath = $"/uploads/OS/{fileName}";
                }
            }

            var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            // Salva no seu repositório
            oOSHospede.InsertOSHospede(
                uniqueId: uniqueId,
                codigoEquipamento: itemOSHospede,
                descricao: descricao,
                filePath: fullPath,  
                ip: remoteIp
            );

            // Se quiser exibir erros de upload após o redirect:
            if (!ModelState.IsValid)
            {
                TempData["UploadErro"] = string.Join("; ",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }

            return RedirectToAction("Index", "Home", new { uniqueId });
        }

        public IActionResult rating(string uniqueId)
        {
            ViewBag.uniqueId = uniqueId;
            ViewBag.questions = oOSHospede.LoadRatingQuestions(uniqueId: uniqueId);

            return View();
        }

        [HttpPost]
        public IActionResult rating([FromForm] RatingForm form, string uniqueId)
        {

            var remoteIp = HttpContext.Connection.RemoteIpAddress;

            return RedirectToAction("ratting", "Home");
        }

    }
}