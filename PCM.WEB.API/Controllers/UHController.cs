using PCM.WEB.MODELS;
using System.Configuration;
using System.Web.Http;

namespace PCM.WEB.API.Controllers
{
    public class UHController : ApiController
    {

        private DAL.WebApi oWebApi = new DAL.WebApi(ConfigurationManager.ConnectionStrings["DefaultConnectionAPI"].ConnectionString);

        // GET api/UH/getUH
        [AcceptVerbs("GET", "POST")]
        [HttpGet]
        [Route("api/UH/getUH")]
        public APIUH getUH(int codigo_empresa, int codigo_unidade)
        {
            //UH
            return oWebApi.getUH(iCodigoEmpresa: codigo_empresa,
                                 iCodigoUnidade: codigo_unidade);
        }

        // GET api/UH/getUHChecklist
        [AcceptVerbs("GET", "POST")]
        [HttpGet]
        [Route("api/UH/getUHChecklist")]
        public APIUHChecklist getUHChecklist(int codigo_empresa, int codigo_unidade, long codigo_apartamento, long codigo_apontamento_origem)
        {
            //UH
            return oWebApi.getUHChecklist(iCodigoEmpresa: codigo_empresa,
                                          iCodigoUnidade: codigo_unidade,
                                          lCodigoApartamento: codigo_apartamento,
                                          lCodigoApontamentoOrigem: codigo_apontamento_origem);
        }

        //POST api/UH/Insert
        [AcceptVerbs("GET", "POST")]
        [HttpPost]
        [Route("api/UH/insertUHApontamento")]
        public ApiUHApontamento insertUHApontamento([FromBody] ApiUHApontamento apontamento)
        {

            //Váriaveis
            long codigo = 0;

            //Insere Registro - Apontamento
            oWebApi.InsertUHApontamento(iCodigoEmpresa: apontamento.codigo_empresa,
                                        iCodigoUsuario: apontamento.codigo_usuario,
                                        iCodigoUnidade: apontamento.codigo_unidade,
                                        lCodigoApartamento: apontamento.codigo_apartamento,
                                        iCodigoFuncionarioResponsavelUnidade: apontamento.codigo_responsavel_unidade,
                                        sDataInicio: apontamento.inicio,
                                        sDataTermino: apontamento.termino,
                                        lCodigoUHApontamento: ref codigo);


            //Insere Registro - Checklist
            if (apontamento.checklist != null)
            {

                foreach (ApiUHApontamentoChecklist checklist in apontamento.checklist)
                {
                    //Insere Registro - Checklist            
                    oWebApi.InsertUHApontamentoChecklist(iCodigoEmpresa: apontamento.codigo_empresa,
                                                         iCodigoUnidade: apontamento.codigo_unidade,
                                                         lCodigoUHApontamento: codigo,
                                                         iCodigoChecklist: checklist.codigo,
                                                         iOpcao: checklist.opcao,
                                                         sObservacao: checklist.observacao);
                }
            }

            return apontamento;
        }
    }
}
