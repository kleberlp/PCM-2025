using Microsoft.AspNetCore.Mvc;

namespace PCM.WEB.Controllers
{
    /// <summary>
    /// Base de todos os controllers do PCM migrados para o Core.
    ///
    /// No MVC5 cada controller repetia as mesmas propriedades lendo
    /// Session["empresa"], Session["codigo_unidade"] e o código do usuário do
    /// User.Identity — 32 cópias do mesmo código. Aqui isso vive uma vez só:
    /// ao migrar um controller, troque "Controller" por "BaseController" na
    /// herança e apague as propriedades locais.
    /// </summary>
    public abstract class BaseController : Controller
    {
        /// <summary>Empresa da sessão (0 = sessão expirada/sem login).</summary>
        protected int codigoEmpresa
        {
            get { return HttpContext.Session.GetInt32("empresa") ?? 0; }
        }

        /// <summary>Unidade escolhida no login (0 = TODAS AS UNIDADES).</summary>
        protected int codigoUnidade
        {
            get { return HttpContext.Session.GetInt32("codigo_unidade") ?? 0; }
        }

        /// <summary>
        /// Código do usuário logado. Vem do cookie de autenticação
        /// (User.Identity.Name guarda o código, como no MVC5 com o
        /// FormsAuthentication + GetUserName()).
        /// </summary>
        protected int codigoUsuario
        {
            get
            {
                int codigo;
                return int.TryParse(User.Identity != null ? User.Identity.Name : null, out codigo) ? codigo : 0;
            }
        }

        /// <summary>Nome de exibição do usuário (Session["nome"] do login).</summary>
        protected string nomeUsuario
        {
            get { return HttpContext.Session.GetString("nome") ?? ""; }
        }

        /// <summary>
        /// Sessão viva? Substitui o "if (Session["empresa"] == null)" que abre
        /// toda action do MVC5.
        /// </summary>
        protected bool Logado
        {
            get { return HttpContext.Session.GetInt32("empresa") != null; }
        }

        /// <summary>
        /// Redireciona para o login preservando a URL de retorno — o mesmo
        /// RedirectToAction("Login", "Account", new { returnURL = Request.RawUrl }).
        /// </summary>
        protected IActionResult RedirecionaLogin()
        {
            return RedirectToAction("Login", "Account",
                new { returnURL = Request.Path + Request.QueryString });
        }
    }
}
