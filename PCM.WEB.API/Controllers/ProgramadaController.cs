using PCM.WEB.MODELS;
using System;
using System.Configuration;
using System.Web.Http;

namespace PCM.WEB.API.Controllers
{
    public class ProgramadaController : ApiController
    {

        private DAL.WebApi oWebApi = new DAL.WebApi(ConfigurationManager.ConnectionStrings["DefaultConnectionAPI"].ConnectionString);

        // GET api/Programada/getProgramada
        [AcceptVerbs("GET", "POST")]
        [HttpGet]
        [Route("api/Programada/getProgramada")]
        public APIProgramada getProgramada(int codigo_empresa, int codigo_unidade, string tipo, int codigo_usuario = -1)
        {
            //Programada
            return oWebApi.getProgramada(iCodigoEmpresa: codigo_empresa,
                                         iCodigoUnidade: codigo_unidade,
                                         iCodigoUsuario: codigo_usuario,
                                         sTipo: tipo);
        }

        // GET api/Programada/getProgramadaChecklist
        [AcceptVerbs("GET", "POST")]
        [HttpGet]
        [Route("api/Programada/getProgramadaChecklist")]
        public APIProgramadaChecklist getProgramadaChecklist(int codigo_empresa, int codigo_unidade, long codigo_pcm_programada, string tipo)
        {
            //Programada
            return oWebApi.getProgramadaChecklist(iCodigoEmpresa: codigo_empresa,
                                                  iCodigoUnidade: codigo_unidade,
                                                  lCodigoPCMProgramada: codigo_pcm_programada,
                                                  sTipo: tipo);
        }

        //POST api/Programada/Insert
        [AcceptVerbs("GET", "POST")]
        [HttpPost]
        [Route("api/Programada/insertProgramadaApontamento")]
        public ApiProgramadaApontamento insertProgramadaApontamento([FromBody] ApiProgramadaApontamento apontamento)
        {

            //Váriaveis
            long codigo = 0;

            //Insere Registro - tb_programada_ordem_servico
            oWebApi.ProgramadaOrdemServico(iCodigoEmpresa: apontamento.codigo_empresa,
                                           iCodigoUnidade: apontamento.codigo_unidade,
                                           lCodigoPCMProgramada: apontamento.codigo_pcm_programada,
                                           sData: apontamento.inicio,
                                           bConcluido: (apontamento.concluido == 1) ? true : false,
                                           sSolucao: apontamento.solucao,
                                           dValor: Convert.ToDouble((apontamento.valor == "") ? "0" : apontamento.valor),
                                           iQuantidadeEquipamento: Convert.ToInt32((apontamento.quantidade_equipamento == "") ? "0" : apontamento.quantidade_equipamento),
                                           iStatus: 2,
                                           iCodigoUsuario: apontamento.codigo_usuario,
                                           lCodigo: ref codigo);


            //Insere Registro - tb_programada_apontamento
            oWebApi.ProgramadaApontamento(lCodigoProgramadaOrdemServico: codigo,
                                          iCodigoEmpresa: apontamento.codigo_empresa,
                                          iCodigoUnidade: apontamento.codigo_unidade,
                                          sInicio: apontamento.inicio,
                                          sTermino: apontamento.termino,
                                          iCodigoUsuario: apontamento.codigo_usuario);

            if (apontamento.checklist != null)
            {

                foreach (ApiProgramadaApontamentoChecklist checklist in apontamento.checklist)
                {

                    //Insere Registro - tb_programada_ordem_servico_checklist
                    oWebApi.ProgramadaOrdemServicoChecklist(lCodigoProgramadaOrdemServico: codigo,
                                                            lCodigoPCMProgramada: apontamento.codigo_pcm_programada,
                                                            iCodigoEmpresa: apontamento.codigo_empresa,
                                                            iCodigoUnidade: apontamento.codigo_unidade,
                                                            iCodigoChecklist: checklist.codigo,
                                                            sResultado: checklist.resultado,
                                                            sObservacao: checklist.observacao,
                                                            iCodigoUsuario: apontamento.codigo_usuario);


                    if (checklist.horas != null)
                    {
                        foreach (ApiProgramadaApontamentoChecklistHoras horas in checklist.horas)
                        {

                            //Insere Registro - tb_programada_ordem_servico_checklist
                            oWebApi.ProgramadaOrdemServicoChecklistHoras(lCodigoProgramadaOrdemServico: codigo,
                                                                         iCodigoEmpresa: apontamento.codigo_empresa,
                                                                         iCodigoUnidade: apontamento.codigo_unidade,
                                                                         iCodigoProgramadaOrdemServicoChecklist: checklist.codigo,
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
