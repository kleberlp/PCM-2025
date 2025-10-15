using PCM.WEB.MODELS;
using System.Configuration;
using System.Web.Http;

namespace PCM.WEB.API.Controllers
{
    public class QualidadeController : ApiController
    {

        private DAL.WebApi oWebApi = new DAL.WebApi(ConfigurationManager.ConnectionStrings["DefaultConnectionAPI"].ConnectionString);

        // GET api/Qualidade/getAuditoria
        [AcceptVerbs("GET", "POST")]
        [HttpGet]
        [Route("api/Qualidade/getAuditoria")]
        public APIAuditoriaQualidade getAuditoria(int codigo_empresa, int codigo_unidade, int codigo_usuario = -1)
        {
            //Programada
            return oWebApi.getAuditoriaQualidade(iCodigoEmpresa: codigo_empresa,
                                                 iCodigoUnidade: codigo_unidade,
                                                 iCodigoUsuario: codigo_usuario);
        }

        // GET api/Qualidade/getTarefa
        [AcceptVerbs("GET", "POST")]
        [HttpGet]
        [Route("api/Qualidade/getTarefa")]
        public APITarefa getTarefa(int codigo_empresa, int codigo_unidade)
        {
            //Programada
            return oWebApi.getTarefa(iCodigoEmpresa: codigo_empresa,
                                     iCodigoUnidade: codigo_unidade);
        }

    }
}
