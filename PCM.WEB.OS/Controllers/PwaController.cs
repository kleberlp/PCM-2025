using Microsoft.AspNetCore.Mvc;

namespace PCM.WEB.OS.Controllers
{
    public class PwaController : Controller
    {
        //O app é acessado por link com uniqueId (não há tela de login). Por isso o manifesto
        //é gerado dinamicamente: o start_url leva o uniqueId da tela onde o app foi instalado,
        //senão o atalho da tela inicial abriria uma página sem contexto.
        //O "id" fixo mantém uma única identidade de app, mesmo variando o start_url.
        [HttpGet]
        [Route("manifest.webmanifest")]
        public IActionResult Manifest(string uniqueId = "", string origem = "")
        {
            string startUrl = "/";

            if (!string.IsNullOrWhiteSpace(uniqueId))
            {
                string destino = string.Equals(origem, "inventario", StringComparison.OrdinalIgnoreCase)
                    ? "/AtivoFixo/assetInventory"
                    : "/Home/index";

                startUrl = $"{destino}?uniqueId={Uri.EscapeDataString(uniqueId)}";
            }

            var manifest = new
            {
                id = "/pcm-os",
                name = "PCM by SIM",
                short_name = "PCM",
                description = "Inventário de ativos e ordens de serviço - PCM by SIM",
                lang = "pt-BR",
                dir = "ltr",
                start_url = startUrl,
                scope = "/",
                display = "standalone",
                display_override = new[] { "standalone", "minimal-ui" },
                orientation = "portrait",
                theme_color = "#2B1A6A",
                background_color = "#FFFFFF",
                categories = new[] { "business", "productivity" },
                //new[] (e não object[]): os 4 itens têm a mesma forma, então o tipo
                //anônimo é inferido e o serializador enxerga as propriedades
                icons = new[]
                {
                    new { src = "/images/pwa/icon-192.png", sizes = "192x192", type = "image/png", purpose = "any" },
                    new { src = "/images/pwa/icon-512.png", sizes = "512x512", type = "image/png", purpose = "any" },
                    new { src = "/images/pwa/icon-192-maskable.png", sizes = "192x192", type = "image/png", purpose = "maskable" },
                    new { src = "/images/pwa/icon-512-maskable.png", sizes = "512x512", type = "image/png", purpose = "maskable" }
                }
            };

            //Sem cache: o start_url muda conforme o uniqueId da tela
            Response.Headers["Cache-Control"] = "no-store";

            //Serialização própria: o JSON do MVC aplica camelCase e quebraria as
            //chaves do manifesto (start_url, theme_color, short_name...)
            string json = System.Text.Json.JsonSerializer.Serialize(manifest, new System.Text.Json.JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            return Content(json, "application/manifest+json");
        }
    }
}
