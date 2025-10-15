using PCM.WEB.MODELS;
using System;
using System.Configuration;
using System.Web.Http;

namespace PCM.WEB.API.Controllers
{
    public class GreenPlanetController : ApiController
    {

        private DAL.WebApi oWebApi = new DAL.WebApi(ConfigurationManager.ConnectionStrings["DefaultConnectionAPI"].ConnectionString);

        // GET api/GreenPlanet/getGreenPlanet
        [AcceptVerbs("GET", "POST")]
        [HttpGet]
        [Route("api/GreenPlanet/getGreenPlanet")]
        public APIGreenPlanet getGreenPlanet(int codigo_empresa, int codigo_unidade)
        {
            //GreenPlanet
            return oWebApi.getGreenPlanet(iCodigoEmpresa: codigo_empresa,
                                 iCodigoUnidade: codigo_unidade);
        }

        //POST api/GreenPlanet/insertGreenPlanet
        [AcceptVerbs("GET", "POST")]
        [HttpPost]
        [Route("api/GreenPlanet/insertGreenPlanet")]
        public APIGreenPlanetInputResponse insertGreenPlanet([FromBody] APIGreenPlanetInput green_planet)
        {
            APIGreenPlanetInputResponse aPIGreenPlanetInputResponse = new APIGreenPlanetInputResponse();

            try
            {

                //Insere Registro - tb_programada_ordem_servico
                oWebApi.GreenPlanetInput(iCodigoEmpresa: green_planet.codigo_empresa,
                                         iCodigoUnidade: green_planet.codigo_unidade,
                                         iCodigoItemMedicao: green_planet.codigo_item_medicao,
                                         iCodigoUsuario: green_planet.codigo_usuario,
                                         iQuantidadeHospede: green_planet.quantidade_hospede,
                                         iOcupacaoQuartos: green_planet.ocupacao_quartos,
                                         sValor: green_planet.valor);

                aPIGreenPlanetInputResponse.message = "";

            }
            catch (Exception ex)
            {
                aPIGreenPlanetInputResponse.message = ex.Message;
            }

            return aPIGreenPlanetInputResponse;
        }
    }
}
