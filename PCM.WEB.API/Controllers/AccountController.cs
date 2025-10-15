using PCM.WEB.MODELS;
using System.Configuration;
using System.Web.Http;

namespace PCM.WEB.API.Controllers
{
    public class AccountController : ApiController
    {

        private DAL.WebApi oWebApi = new DAL.WebApi(ConfigurationManager.ConnectionStrings["DefaultConnectionAPI"].ConnectionString);

        // GET api/account/login
        [HttpGet]
        [Route("api/Account/Login")]
        public ApiLogin Login(string email, string senha)
        {
            //Login
            return oWebApi.Login(sEmail: email,
                                 sSenha: senha);
        }

    }
}
