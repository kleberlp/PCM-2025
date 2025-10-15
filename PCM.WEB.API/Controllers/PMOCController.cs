using PCM.WEB.MODELS;
using System.Configuration;
using System.Web.Http;

namespace PCM.WEB.API.Controllers
{
    public class PMOCController : ApiController
    {

        private DAL.WebApi oWebApi = new DAL.WebApi(ConfigurationManager.ConnectionStrings["DefaultConnectionAPI"].ConnectionString);

        // GET api/PMOC/getPMOC
        [AcceptVerbs("GET", "POST")]
        [HttpGet]
        [Route("api/PMOC/getPMOC")]
        public APIPMOC getPMOC(int codigo_empresa, int codigo_unidade, int intervalo = -1)
        {
            //PMOC
            return oWebApi.getPMOC(iCodigoEmpresa: codigo_empresa,
                                   iCodigoUnidade: codigo_unidade,
                                   iIntervalo: intervalo);
        }

        // GET api/PMOC/getPMOCChecklist
        [AcceptVerbs("GET", "POST")]
        [HttpGet]
        [Route("api/PMOC/getPMOCChecklist")]
        public APIPMOCChecklist getPMOCChecklist(int codigo_empresa, int codigo_unidade, long codigo_equipamento, string data_manutencao)
        {
            //PMOC
            return oWebApi.getPMOCChecklist(iCodigoEmpresa: codigo_empresa,
                                            iCodigoUnidade: codigo_unidade,
                                            lCodigoEquipamento: codigo_equipamento,
                                            sDataManutencao: data_manutencao);
        }

        //POST api/PMOC/Insert
        [AcceptVerbs("GET", "POST")]
        [HttpPost]
        [Route("api/PMOC/insertPMOCApontamento")]
        public ApiPMOCApontamento insertPMOCApontamento([FromBody] ApiPMOCApontamento apontamento)
        {

            //Váriaveis
            long codigo = 0;

            //Insere Registro - tb_pmoc_ordem_servico
            oWebApi.PMOCOrdemServico(iCodigoEmpresa: apontamento.codigo_empresa,
                                     iCodigoUnidade: apontamento.codigo_unidade,
                                     lCodigoEquipamento: apontamento.codigo_equipamento,
                                     sData: apontamento.inicio,
                                     iStatus: 2,
                                     iCodigoUsuario: apontamento.codigo_usuario,
                                     lCodigo: ref codigo);


            //Insere Registro - tb_pmoc_apontamento
            oWebApi.PMOCApontamento(lCodigoPmocOrdemServico: codigo,
                                    iCodigoEmpresa: apontamento.codigo_empresa,
                                    iCodigoUnidade: apontamento.codigo_unidade,
                                    sInicio: apontamento.inicio,
                                    sTermino: apontamento.termino,
                                    iCodigoUsuario: apontamento.codigo_usuario);

            if (apontamento.checklist != null)
            {

                foreach (ApiPMOCApontamentoChecklist checklist in apontamento.checklist)
                {

                    //Insere Registro - tb_pmoc_ordem_servico_checklist
                    oWebApi.PMOCOrdemServicoChecklist(lCodigoPmocOrdemServico: codigo,
                                                      iCodigoEmpresa: apontamento.codigo_empresa,
                                                      iCodigoUnidade: apontamento.codigo_unidade,
                                                      iCodigoTipoArCondicionado: apontamento.codigo_tipo_ar_condicionado,
                                                      iCodigoChecklist: checklist.codigo,
                                                      sResultado: checklist.resultado,
                                                      sObservacao: checklist.observacao);


                    if (checklist.horas != null)
                    {
                        foreach (ApiPMOCApontamentoChecklistHoras horas in checklist.horas)
                        {

                            //Insere Registro - tb_pmoc_ordem_servico_checklist
                            oWebApi.PMOCOrdemServicoChecklistHoras(lCodigoPmocOrdemServico: codigo,
                                                                   iCodigoEmpresa: apontamento.codigo_empresa,
                                                                   iCodigoUnidade: apontamento.codigo_unidade,
                                                                   iCodigoPmocOrdemServicoChecklist: checklist.codigo,
                                                                   sInicio: horas.inicio,
                                                                   sTermino: horas.termino);
                        }
                    }
                }

            }

            return apontamento;
        }
    }
}
