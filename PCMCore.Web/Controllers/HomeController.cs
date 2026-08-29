using Microsoft.AspNetCore.Mvc;

namespace PCM.WEB.Controllers
{
    /// <summary>
    /// Placeholder enquanto os módulos migram: prova que o pipeline (rota,
    /// views, layout, session) está de pé e serve de exemplo do padrão — herda
    /// de BaseController, como todo controller migrado deve herdar.
    /// </summary>
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
