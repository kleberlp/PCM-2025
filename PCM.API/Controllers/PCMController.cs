using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PCM.API.Class;
using PCM.WEB.DAL;
using PCM.WEB.MODELS;
using System;
using System.Collections.Generic;

namespace PCM.API.Controllers
{

    public class PCMController : ControllerBase
    {

        Api oAPI = new Api(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionString").Value);

        Function oFunction = new Function();

        #region ::: GERAL :::

        // GET api/PCM/getCombo
        [HttpGet]
        [Route("api/PCM/getCombo")]
        public IActionResult getCombo(int codigoEmpresa, int codigoUnidade, string tabela, string codigoAux1 = "", string codigoAux2 = "")
        {
            try
            {

                List<pwaCombo> combo = oAPI.Combo(iCodigoEmpresa: codigoEmpresa,
                                                  iCodigoUnidade: codigoUnidade,
                                                  sTabela: tabela,
                                                  sCodigoAux1: codigoAux1,
                                                  sCodigoAux2: codigoAux2);

                return Ok(combo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region ::: DASHBOARD :::

        // GET api/PCM/getDashboardOS
        [HttpGet]
        [Route("api/PCM/getDashboardOS")]
        public IActionResult getDashboardOS(int codigoEmpresa, int codigoUnidade, int codigoUsuario)
        {
            try
            {

                pwaDashboardOrdemServico dashboard = oAPI.DashboardOrdemServico(iCodigoEmpresa: codigoEmpresa,
                                                                                iCodigoUnidade: codigoUnidade,
                                                                                iCodigoUsuario: codigoUsuario);

                return Ok(dashboard);
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/PCM/getForm
        [HttpGet]
        [Route("api/PCM/getForm")]
        public IActionResult getForm(int codigoEmpresa, int codigoUnidade, int codigoUsuario)
        {
            try
            {

                List<pwaForm> form = oAPI.Form(iCodigoEmpresa: codigoEmpresa,
                                               iCodigoUnidade: codigoUnidade,
                                               iCodigoUsuario: codigoUsuario);

                return Ok(form);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region ::: ORDEM DE SERVIÇO :::

        // GET api/PCM/getListOrdemServico
        [HttpGet]
        [Route("api/PCM/getListOrdemServico")]
        public IActionResult getListOrdemServico(int codigoEmpresa, int codigoUnidade, int codigoUsuario, int codigoFuncionario, int page = 1)
        {

            pwaOrdemServicoList ordemServico = oAPI.getListOrdemServicoList(iCodigoEmpresa: codigoEmpresa,
                                                                            iCodigoUnidade: codigoUnidade,
                                                                            iCodigoFuncionario: codigoFuncionario,
                                                                            iPage: page);

            //GRAVA LOG
            oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                              iCodigoUnidade: codigoUnidade,
                              iCodigoUsuario: codigoUsuario,
                              sEndpoint: Request.Path,
                              sRequestBody: "",
                              sResponseBody: oFunction.ConverteObjectParaJSon(ordemServico));

            return Ok(ordemServico);
        }

        // GET api/PCM/insertOrdemServico
        [HttpPut]
        [Route("api/PCM/insertOrdemServico")]
        public IActionResult insertOrdemServico([FromBody] pwaOrdemServicoInsert ordemServico)
        {

            pwaOrdemServicoResponse response = oAPI.insertOrdemServico(oOrdemServico: ordemServico);

            //GRAVA LOG
            oAPI.insertLogAPI(iCodigoEmpresa: ordemServico.codigoEmpresa,
                              iCodigoUnidade: ordemServico.codigoUnidade,
                              iCodigoUsuario: ordemServico.codigoUsuario,
                              sEndpoint: Request.Path,
                              sRequestBody: oFunction.ConverteObjectParaJSon(ordemServico),
                              sResponseBody: oFunction.ConverteObjectParaJSon(response));

            return Ok(response);
        }

        // GET api/PCM/insertApontamentoOrdemServico
        [HttpPut]
        [Route("api/PCM/insertApontamentoOrdemServico")]
        public IActionResult insertApontamentoOrdemServico([FromBody] pwaOrdemServicoApontamento ordemServicoApontamento)
        {
            pwaDefaultResponse response = new pwaDefaultResponse();

            try
            {
                response = oAPI.insertOrdemServicoApontamento(oOrdemServicoApontamento: ordemServicoApontamento);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message.ToString();
                return BadRequest(response);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: ordemServicoApontamento.codigoEmpresa,
                                  iCodigoUnidade: ordemServicoApontamento.codigoUnidade,
                                  iCodigoUsuario: ordemServicoApontamento.codigoUsuario,
                                  sEndpoint: Request.Path,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(ordemServicoApontamento),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        #endregion

        #region ::: PROGRAMADA :::

        // GET api/PCM/getListManutencaoProgramada
        [HttpGet]
        [Route("api/PCM/getListManutencaoProgramada")]
        public IActionResult getListManutencaoProgramada(int codigoEmpresa, int codigoUnidade, int codigoUsuario, string tipo, int page = 1)
        {
            pwaManutencaoProgramadaList programada = new pwaManutencaoProgramadaList();

            try
            {

                programada = oAPI.getListManutencaoProgramadaList(iCodigoEmpresa: codigoEmpresa,
                                                                  iCodigoUnidade: codigoUnidade,
                                                                  sTipo: tipo,
                                                                  iPage: page);

                return Ok(programada);

            }
            catch (Exception ex)
            {
                pwaApontamentoResponse response = new pwaApontamentoResponse();
                response.success = false;
                response.message = ex.Message.ToString();
                return BadRequest(response);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                                  iCodigoUnidade: codigoUnidade,
                                  iCodigoUsuario: codigoUsuario,
                                  sEndpoint: Request.Path,
                                  sRequestBody: "",
                                  sResponseBody: oFunction.ConverteObjectParaJSon(programada));

            }

        }

        // GET api/PCM/insertManutencaoProgramada
        [HttpPut]
        [Route("api/PCM/insertApontamentoProgramada")]
        public IActionResult insertApontamentoProgramada([FromBody] pwaManutencaoProgramadaApontamento manutencaoProgramada)
        {
            pwaApontamentoResponse response = new pwaApontamentoResponse();

            try
            {
                response = oAPI.insertManutencaoProgramadaApontamento(oManutencaoProgramada: manutencaoProgramada);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message.ToString();
                return BadRequest(response);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: manutencaoProgramada.codigoEmpresa,
                                  iCodigoUnidade: manutencaoProgramada.codigoUnidade,
                                  iCodigoUsuario: manutencaoProgramada.codigoUsuario,
                                  sEndpoint: Request.Path,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(manutencaoProgramada),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        #endregion

        #region ::: PMOC :::

        // GET api/PCM/getListPMOC
        [HttpGet]
        [Route("api/PCM/getListPMOC")]
        public IActionResult getListPMOC(int codigoEmpresa, int codigoUnidade, int codigoUsuario, int page = 1)
        {
            pwaPMOCList pmoc = new pwaPMOCList();

            try
            {

                pmoc = oAPI.getListPMOCList(iCodigoEmpresa: codigoEmpresa,
                                            iCodigoUnidade: codigoUnidade,
                                            iPage: page);

                return Ok(pmoc);

            }
            catch (Exception ex)
            {
                pwaApontamentoResponse response = new pwaApontamentoResponse();
                response.success = false;
                response.message = ex.Message.ToString();
                return BadRequest(response);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                                  iCodigoUnidade: codigoUnidade,
                                  iCodigoUsuario: codigoUsuario,
                                  sEndpoint: Request.Path,
                                  sRequestBody: "",
                                  sResponseBody: oFunction.ConverteObjectParaJSon(pmoc));

            }

        }

        // GET api/PCM/insertManutencaoProgramada
        [HttpPut]
        [Route("api/PCM/insertApontamentoPMOC")]
        public IActionResult insertApontamentoPMOC([FromBody] pwaPMOCApontamento pmoc)
        {
            pwaApontamentoResponse response = new pwaApontamentoResponse();

            try
            {
                response = oAPI.insertPMOCApontamento(oPMOCApontamento: pmoc);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message.ToString();
                return BadRequest(response);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: pmoc.codigoEmpresa,
                                  iCodigoUnidade: pmoc.codigoUnidade,
                                  iCodigoUsuario: pmoc.codigoUsuario,
                                  sEndpoint: Request.Path,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(pmoc),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        #endregion

        #region ::: CHECKLIST :::

        // GET api/PCM/getChecklist
        [HttpGet]
        [Route("api/PCM/getChecklist")]
        public IActionResult getChecklist(int codigoEmpresa, int codigoUnidade, long codigoChecklist, string tipo, long codigoDocumento)
        {
            List<pwaChecklist> checklist = new List<pwaChecklist>();

            try
            {

                checklist = oAPI.getCheckList(iCodigoEmpresa: codigoEmpresa,
                                              iCodigoUnidade: codigoUnidade,
                                              lCodigoChecklist: codigoChecklist,
                                              sTipo: tipo,
                                              lCodigoDocumento: codigoDocumento);

                return Ok(checklist);

            }
            catch (Exception ex)
            {
                pwaApontamentoResponse response = new pwaApontamentoResponse();
                response.success = false;
                response.message = ex.Message.ToString();
                return BadRequest(response);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                                  iCodigoUnidade: -1,
                                  iCodigoUsuario: -1,
                                  sEndpoint: Request.Path,
                                  sRequestBody: "",
                                  sResponseBody: oFunction.ConverteObjectParaJSon(checklist));

            }

        }

        // GET api/PCM/insertManutencaoProgramada
        [HttpPut]
        [Route("api/PCM/insertChecklist")]
        public IActionResult insertChecklist(long codigoDocumento, string tipo, [FromBody] pwaChecklistApontamento checklist)
        {
            pwaApontamentoResponse response = new pwaApontamentoResponse();

            try
            {
                response = oAPI.insertApontamentoChecklist(lCodigoDocumento: codigoDocumento,
                                                           sTipo: tipo,
                                                           oChecklistApontamento: checklist);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message.ToString();
                return BadRequest(response);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: checklist.codigoEmpresa,
                                  iCodigoUnidade: checklist.codigoUnidade,
                                  iCodigoUsuario: checklist.codigoUsuario,
                                  sEndpoint: Request.Path,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(checklist),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        #endregion

        #region ::: PICTURE :::

        // PUT api/PCM/insertFile
        [HttpPut]
        [Route("api/PCM/insertFile")]
        public IActionResult insertFile(int codigoEmpresa, int codigoUnidade, int codigoUsuario, long codigoDocumento, string tipo, [FromBody] pwaArquivoInsert arquivo)
        {
            pwaDefaultResponse response = new pwaDefaultResponse();

            try
            {
                response = oAPI.insertPicture(iCodigoEmpresa: codigoEmpresa,
                                              iCodigoUnidade: codigoUnidade,
                                              iCodigoUsuario: codigoUsuario,
                                              lCodigoDocumento: codigoDocumento,
                                              sTipo: tipo,
                                              oArquivo: arquivo);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message.ToString();
                return BadRequest(response);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                                  iCodigoUnidade: codigoUnidade,
                                  iCodigoUsuario: codigoUsuario,
                                  sEndpoint: Request.Path,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(arquivo),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        #endregion

        #region ::: NOTIFICAÇÃO :::

        // GET api/PCM/getNotificacao
        [HttpGet]
        [Route("api/PCM/getNotificacao")]
        public IActionResult getNotificacao(int codigoEmpresa, int codigoUnidade, int codigoUsuario)
        {

            pwaNotificacao notificacao = new pwaNotificacao();

            try
            {
                List<pwaListaNotificacao> results = new List<pwaListaNotificacao>();

                results = oAPI.getNotificacao(iCodigoEmpresa: codigoEmpresa,
                                              iCodigoUnidade: codigoUnidade,
                                              iCodigoUsuario: codigoUsuario);

                notificacao.success = true;
                notificacao.message = "";
                notificacao.results = results;

                return Ok(notificacao);

            }
            catch (Exception ex)
            {
                notificacao.success = false;
                notificacao.message = ex.Message.ToString(); 
                return BadRequest(notificacao);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                                  iCodigoUnidade: codigoUnidade,
                                  iCodigoUsuario: codigoUsuario,
                                  sEndpoint: Request.Path,
                                  sRequestBody: "",
                                  sResponseBody: oFunction.ConverteObjectParaJSon(notificacao));

            }

        }

        // PUT api/PCM/insertPicture
        [HttpPut]
        [Route("api/PCM/readNotificacao")]
        public IActionResult readNotificacao(int codigoEmpresa, int codigoUnidade, int codigoUsuario, long codigo)
        {
            pwaDefaultResponse response = new pwaDefaultResponse();

            try
            {
                oAPI.readNotificacao(iCodigoEmpresa: codigoEmpresa,
                                     iCodigoUnidade: codigoUnidade,
                                     lCodigo: codigo);

                response.success = true;
                response.message = "";

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message.ToString();
                return BadRequest(response);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                                  iCodigoUnidade: codigoUnidade,
                                  iCodigoUsuario: codigoUsuario,
                                  sEndpoint: Request.Path,
                                  sRequestBody: "",
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        #endregion

    }

}
