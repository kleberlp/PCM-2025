using PCM.WEB.MODELS;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;

namespace PCM.WEB.API.Controllers
{
    public class CadastrobasicoController : ApiController
    {

        private DAL.WebApi oWebApi = new DAL.WebApi(ConfigurationManager.ConnectionStrings["DefaultConnectionAPI"].ConnectionString);

        // GET api/CadastroBasico/getUnidade
        [HttpGet]
        [Route("api/CadastroBasico/getUnidade")]
        public List<APIUnidade> getUnidade(int codigo_empresa)
        {
            //Combo - Unidade
            return oWebApi.Unidade(iCodigoEmpresa: codigo_empresa);
        }

        // GET api/CadastroBasico/getUnidade2
        [HttpGet]
        [Route("api/CadastroBasico/getUnidadeUsuario")]
        public List<APIUnidade> getUnidadeUsuario(int codigo_empresa, int codigo_usuario)
        {
            //Combo - Unidade
            return oWebApi.UnidadeUsuario(iCodigoEmpresa: codigo_empresa,
                                          iCodigoUsuario: codigo_usuario);
        }

        // GET api/CadastroBasico/combo
        [HttpGet]
        [Route("api/CadastroBasico/Combo")]
        public List<APIComboBox> Combo(string storage_procedure)
        {
            //Combo - Unidade
            return oWebApi.combo(sStorageProdedure: storage_procedure);
        }

        // GET api/CadastroBasico/combo
        [HttpGet]
        [Route("api/CadastroBasico/Combo")]
        public List<APIComboBox> Combo(int codigo_empresa, int codigo_unidade, string query)
        {
            //Combo - Unidade
            return oWebApi.combo(iCodigoEmpresa: codigo_empresa,
                                 iCodigoUnidade: codigo_unidade,
                                 sQuery: query);
        }

        // GET api/CadastroBasico/combo1
        [HttpGet]
        [Route("api/CadastroBasico/Combo2")]
        public List<APIComboBox> Combo2(int codigo_empresa, int codigo_unidade, int codigo1, string query)
        {
            //Combo - Unidade
            return oWebApi.combo1(iCodigoEmpresa: codigo_empresa,
                                  iCodigoUnidade: codigo_unidade,
                                  iCodigo1: codigo1,
                                  sQuery: query);
        }

        // GET api/CadastroBasico/combo2
        [HttpGet]
        [Route("api/CadastroBasico/Combo3")]
        public List<APIComboBox> Combo3(int codigo_empresa, int codigo_unidade, int codigo1, int codigo2, string query)
        {
            //Combo - Unidade
            return oWebApi.combo2(iCodigoEmpresa: codigo_empresa,
                                  iCodigoUnidade: codigo_unidade,
                                  iCodigo1: codigo1,
                                  iCodigo2: codigo2,
                                  sQuery: query);
        }

        // GET api/CadastroBasico/combo4
        [HttpGet]
        [Route("api/CadastroBasico/Combo4")]
        public List<APIComboBox> Combo4(int codigo_empresa, int codigo_unidade, int codigo1, int codigo2, int codigo3, string query)
        {
            //Combo - Unidade
            return oWebApi.combo3(iCodigoEmpresa: codigo_empresa,
                                  iCodigoUnidade: codigo_unidade,
                                  iCodigo1: codigo1,
                                  iCodigo2: codigo2,
                                  iCodigo3: codigo3,
                                  sQuery: query);
        }

    }
}
