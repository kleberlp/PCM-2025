using PCM.WEB.MODELS;
using System.Configuration;
using System.Web.Http;

namespace PCM.WEB.API.Controllers
{
    public class FirebaseController : ApiController
    {

        private DAL.WebApi oWebApi = new DAL.WebApi(ConfigurationManager.ConnectionStrings["DefaultConnectionAPI"].ConnectionString);

        //POST api/Firebase/insertToken
        [AcceptVerbs("GET", "POST")]
        [HttpPost]
        [Route("api/Firebase/insertToken")]
        public APIFirebaseToken insertToken([FromBody] APIFirebaseToken firebaseToken)
        {
            //Insere Registro - tb_programada_ordem_servico
            oWebApi.InsertToken(iCodigoEmpresa: firebaseToken.codigo_empresa,
                                iCodigoUnidade: firebaseToken.codigo_unidade,
                                sToken: firebaseToken.token);

            return firebaseToken;
        }

        //POST api/Firebase/insertToken
        [AcceptVerbs("GET", "POST")]
        [HttpPost]
        [Route("api/Firebase/insertTokenNew")]
        public APIFirebaseTokenNew insertTokenNew([FromBody] APIFirebaseTokenNew firebaseToken)
        {
            //Insere Registro - tb_programada_ordem_servico
            oWebApi.InsertTokenNew(iCodigoEmpresa: firebaseToken.codigo_empresa,
                                   iCodigoUsuario: firebaseToken.codigo_usuario,
                                   sToken: firebaseToken.token);

            return firebaseToken;
        }
    }
}
