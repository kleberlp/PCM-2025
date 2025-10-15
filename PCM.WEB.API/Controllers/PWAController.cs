using PCM.WEB.DAL;
using PCM.WEB.MODELS;
using PCM.WEBAPI.Class;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;


namespace PCM.WEB.API.Controllers
{
    public class PWAController : ApiController
    {

        Api oAPI = new Api(ConfigurationManager.ConnectionStrings["DefaultConnectionAPI"].ConnectionString);

        Function oFunction = new Function();

        #region ::: GERAL :::

        // GET api/PWA/getCombo
        [HttpGet]
        [Route("api/PWA/getCombo")]
        public IHttpActionResult getCombo(int codigoEmpresa, int codigoUnidade, string tabela, string codigoAux1 = "", string codigoAux2 = "")
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

        #region ::: LOGIN :::

        // GET api/account/login
        [HttpGet]
        [Route("api/PWA/Login")]
        public IHttpActionResult Login(string email, string senha)
        {

            pwaLogin login = oAPI.Login(sEmail: email,
                                        sSenha: senha);

            return Ok(login);
        }

        // GET api/account/EsqueciSenha
        [HttpGet]
        [Route("api/PWA/EsqueciSenha")]
        public IHttpActionResult EsqueciSenha(string email)
        {

            pwaDefaultResponse response = new pwaDefaultResponse();

            try
            {
                response.success = true;
                response.message = "";
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET api/account/UpdatePassword
        [HttpPut]
        [Route("api/PWA/UpdatePassword")]
        public IHttpActionResult UpdatePassword([FromBody] pwaUpdatePassword updatePassword)
        {

            pwaDefaultResponse response = new pwaDefaultResponse();

            try
            {

                response = oAPI.UpdatePassword(sEmail: updatePassword.email,
                                               sSenha: updatePassword.password);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        #endregion

        #region ::: FIREBASE :::

        // GET api/PWA/insertTokenFirebase
        [HttpPut]
        [Route("api/PWA/insertTokenFirebase")]
        public IHttpActionResult insertTokenFirebase([FromBody] pwaToken token)
        {
            pwaDefaultResponse response = new pwaDefaultResponse();

            try
            {
                response = oAPI.insertFirebaseToken(iCodigoEmpresa: token.codigoEmpresa,
                                                    iCodigoUnidade: token.codigoUnidade,
                                                    iCodigoUsuario: token.codigoUsuario,
                                                    sToken: token.token);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: token.codigoEmpresa,
                                  iCodigoUnidade: token.codigoUnidade,
                                  iCodigoUsuario: token.codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(token),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        #endregion

        #region ::: DASHBOARD :::

        // GET api/PWA/getDashboardOS
        [HttpGet]
        [Route("api/PWA/getDashboardOS")]
        public IHttpActionResult getDashboardOS(int codigoEmpresa, int codigoUnidade, int codigoUsuario)
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

        // GET api/PWA/getForm
        [HttpGet]
        [Route("api/PWA/getForm")]
        public IHttpActionResult getForm(int codigoEmpresa, int codigoUnidade, int codigoUsuario)
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

        #region ::: GREEN PLANET :::

        // GET api/PWA/getGreenPlanet
        [HttpGet]
        [Route("api/PWA/getGreenPlanet")]
        public IHttpActionResult getGreenPlanet(int codigoEmpresa, int codigoUnidade, int codigoUsuario, string data = "")
        {
            pwaGreenPlanetList greenPlanet = new pwaGreenPlanetList();

            try
            {

                greenPlanet = oAPI.getGreenPlanetList(iCodigoEmpresa: codigoEmpresa,
                                                      iCodigoUnidade: codigoUnidade,
                                                      sData: data);

                return Ok(greenPlanet);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                                  iCodigoUnidade: codigoUnidade,
                                  iCodigoUsuario: codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: "",
                                  sResponseBody: oFunction.ConverteObjectParaJSon(greenPlanet));

            }

        }

        // GET api/PWA/insertApontamentoGreenPlanet
        [HttpPut]
        [Route("api/PWA/insertApontamentoGreenPlanet")]
        public IHttpActionResult insertApontamentoGreenPlanet([FromBody] pwaGreenPlanetApontamento greenPlanet)
        {
            pwaDefaultResponse response = new pwaDefaultResponse();

            try
            {
                response = oAPI.insertGreenPlanetApontamento(oGreenPlanet: greenPlanet);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: greenPlanet.codigoEmpresa,
                                  iCodigoUnidade: greenPlanet.codigoUnidade,
                                  iCodigoUsuario: greenPlanet.codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(greenPlanet),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        #endregion

        #region ::: ORDEM DE SERVIÇO :::

        // GET api/PWA/getListOrdemServico
        [HttpGet]
        [Route("api/PWA/getListOrdemServico")]
        public IHttpActionResult getListOrdemServico(int codigoEmpresa, int codigoUnidade, int codigoUsuario, int codigoFuncionario, int page = 1)
        {

            pwaOrdemServicoList ordemServico = oAPI.getListOrdemServicoList(iCodigoEmpresa: codigoEmpresa,
                                                                            iCodigoUnidade: codigoUnidade,
                                                                            iCodigoUsuario: codigoUsuario,
                                                                            iCodigoFuncionario: codigoFuncionario,
                                                                            iPage: page);

            //GRAVA LOG
            oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                              iCodigoUnidade: codigoUnidade,
                              iCodigoUsuario: codigoUsuario,
                              sEndpoint: Request.RequestUri.PathAndQuery,
                              sRequestBody: "",
                              sResponseBody: oFunction.ConverteObjectParaJSon(ordemServico));

            return Ok(ordemServico);
        }

        // GET api/PWA/insertOrdemServico
        [HttpPut]
        [Route("api/PWA/insertOrdemServico")]
        public IHttpActionResult insertOrdemServico([FromBody] pwaOrdemServicoInsert ordemServico)
        {

            pwaOrdemServicoResponse response = oAPI.insertOrdemServico(oOrdemServico: ordemServico);

            //GRAVA LOG
            oAPI.insertLogAPI(iCodigoEmpresa: ordemServico.codigoEmpresa,
                              iCodigoUnidade: ordemServico.codigoUnidade,
                              iCodigoUsuario: ordemServico.codigoUsuario,
                              sEndpoint: Request.RequestUri.PathAndQuery,
                              sRequestBody: oFunction.ConverteObjectParaJSon(ordemServico),
                              sResponseBody: oFunction.ConverteObjectParaJSon(response));

            return Ok(response);
        }

        // GET api/PWA/startOrdemServicoApontamento
        [HttpPut]
        [Route("api/PWA/startOrdemServicoApontamento")]
        public IHttpActionResult startOrdemServicoApontamento([FromBody] pwaOrdemServicoStart ordemServico)
        {

            pwaDefaultResponse response = new pwaDefaultResponse();

            try
            {

                oAPI.ordemServicoStatus(iCodigoEmpresa: ordemServico.codigoEmpresa,
                                        iCodigoUnidade: ordemServico.codigoUnidade,
                                        lCodigoPCMOrdemServico: ordemServico.codigoOrdemServico,
                                        iCodigoUsuario: ordemServico.codigoUsuario,
                                        iStatus: 4);

                response.success = true;
                response.message = "";

            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message.ToString();
            }

            //GRAVA LOG
            oAPI.insertLogAPI(iCodigoEmpresa: ordemServico.codigoEmpresa,
                              iCodigoUnidade: ordemServico.codigoUnidade,
                              iCodigoUsuario: ordemServico.codigoUsuario,
                              sEndpoint: Request.RequestUri.PathAndQuery,
                              sRequestBody: oFunction.ConverteObjectParaJSon(ordemServico),
                              sResponseBody: oFunction.ConverteObjectParaJSon(response));

            return Ok(response);
        }

        // GET api/PWA/insertApontamentoOrdemServico
        [HttpPut]
        [Route("api/PWA/insertApontamentoOrdemServico")]
        public IHttpActionResult insertApontamentoOrdemServico([FromBody] pwaOrdemServicoApontamento ordemServicoApontamento)
        {
            pwaDefaultResponse response = new pwaDefaultResponse();

            try
            {
                response = oAPI.insertOrdemServicoApontamento(oOrdemServicoApontamento: ordemServicoApontamento);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: ordemServicoApontamento.codigoEmpresa,
                                  iCodigoUnidade: ordemServicoApontamento.codigoUnidade,
                                  iCodigoUsuario: ordemServicoApontamento.codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(ordemServicoApontamento),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        // GET api/PWA/getListEstoque
        [HttpGet]
        [Route("api/PWA/getListEstoque")]
        public IHttpActionResult getListEstoque(int codigoEmpresa, int codigoUnidade, int codigoUsuario, int page = 1)
        {

            pwaEstoqueList estoque = oAPI.getListEstoque(codigoEmpresa: codigoEmpresa,
                                                         codigoUnidade: codigoUnidade,
                                                         codigoUsuario: codigoUsuario,
                                                         page: page);

            //GRAVA LOG
            oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                              iCodigoUnidade: codigoUnidade,
                              iCodigoUsuario: codigoUsuario,
                              sEndpoint: Request.RequestUri.PathAndQuery,
                              sRequestBody: "",
                              sResponseBody: oFunction.ConverteObjectParaJSon(estoque));

            return Ok(estoque);
        }

        #endregion

        #region ::: PROGRAMADA :::

        // GET api/PWA/getListManutencaoProgramada
        [HttpGet]
        [Route("api/PWA/getListManutencaoProgramada")]
        public IHttpActionResult getListManutencaoProgramada(int codigoEmpresa, int codigoUnidade, int codigoUsuario, string tipo, int page = 1, bool offline = false)
        {
            pwaManutencaoProgramadaList programada = new pwaManutencaoProgramadaList();

            try
            {

                programada = oAPI.getListManutencaoProgramadaList(iCodigoEmpresa: codigoEmpresa,
                                                                  iCodigoUsuario: codigoUsuario,
                                                                  iCodigoUnidade: codigoUnidade,
                                                                  sTipo: tipo,
                                                                  bOffline: offline,
                                                                  iPage: page);

                return Ok(programada);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                                  iCodigoUnidade: codigoUnidade,
                                  iCodigoUsuario: codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: "",
                                  sResponseBody: oFunction.ConverteObjectParaJSon(programada));

            }

        }

        // GET api/PWA/insertManutencaoProgramada
        [HttpPut]
        [Route("api/PWA/insertApontamentoProgramada")]
        public IHttpActionResult insertApontamentoProgramada([FromBody] pwaManutencaoProgramadaApontamento manutencaoProgramada)
        {
            pwaApontamentoResponse response = new pwaApontamentoResponse();

            try
            {
                response = oAPI.insertManutencaoProgramadaApontamento(oManutencaoProgramada: manutencaoProgramada);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: manutencaoProgramada.codigoEmpresa,
                                  iCodigoUnidade: manutencaoProgramada.codigoUnidade,
                                  iCodigoUsuario: manutencaoProgramada.codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(manutencaoProgramada),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        #endregion

        #region ::: TAREFA :::

        // GET api/PWA/getListTarefa
        [HttpGet]
        [Route("api/PWA/getListTarefa")]
        public IHttpActionResult getListTarefa(int codigoEmpresa, int codigoUnidade, int codigoUsuario, int page = 1, bool offline = false)
        {
            pwaTarefaList tarefa = new pwaTarefaList();

            try
            {

                tarefa = oAPI.getListTarefa(iCodigoEmpresa: codigoEmpresa,
                                            iCodigoUnidade: codigoUnidade,
                                            iPage: page,
                                            bOffline: offline);

                return Ok(tarefa);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                                  iCodigoUnidade: codigoUnidade,
                                  iCodigoUsuario: codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: "",
                                  sResponseBody: oFunction.ConverteObjectParaJSon(tarefa));

            }

        }

        // GET api/PWA/insertApontamentoTarefa
        [HttpPut]
        [Route("api/PWA/insertApontamentoTarefa")]
        public IHttpActionResult insertApontamentoTarefa([FromBody] pwaTarefaApontamento tarefaApontamento)
        {
            pwaApontamentoResponse response = new pwaApontamentoResponse();

            try
            {
                response = oAPI.insertTarefaApontamento(oTarefaApontamento: tarefaApontamento);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: tarefaApontamento.codigoEmpresa,
                                  iCodigoUnidade: tarefaApontamento.codigoUnidade,
                                  iCodigoUsuario: tarefaApontamento.codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(tarefaApontamento),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        #endregion

        #region ::: PMOC :::

        // GET api/PWA/getListPMOC
        [HttpGet]
        [Route("api/PWA/getListPMOC")]
        public IHttpActionResult getListPMOC(int codigoEmpresa, int codigoUnidade, int codigoUsuario, int intervalo = 0, int page = 1, bool offline = false)
        {
            pwaPMOCList pmoc = new pwaPMOCList();

            try
            {

                pmoc = oAPI.getListPMOCList(iCodigoEmpresa: codigoEmpresa,
                                            iCodigoUnidade: codigoUnidade,
                                            iIntervalo: intervalo,
                                            iPage: page,
                                            bOffline: offline);

                return Ok(pmoc);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                                  iCodigoUnidade: codigoUnidade,
                                  iCodigoUsuario: codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: "",
                                  sResponseBody: oFunction.ConverteObjectParaJSon(pmoc));

            }

        }

        // GET api/PWA/insertManutencaoProgramada
        [HttpPut]
        [Route("api/PWA/insertApontamentoPMOC")]
        public IHttpActionResult insertApontamentoPMOC([FromBody] pwaPMOCApontamento pmoc)
        {
            pwaApontamentoResponse response = new pwaApontamentoResponse();

            try
            {
                response = oAPI.insertPMOCApontamento(oPMOCApontamento: pmoc);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: pmoc.codigoEmpresa,
                                  iCodigoUnidade: pmoc.codigoUnidade,
                                  iCodigoUsuario: pmoc.codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(pmoc),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        #endregion

        #region ::: UH :::

        // GET api/PWA/getListUHDia
        [HttpGet]
        [Route("api/PWA/getListUHDia")]
        public IHttpActionResult getListUHDia(int codigoEmpresa, int codigoUnidade, int codigoUsuario, int page = 1, bool offline = false)
        {
            pwaUHDiaList uhDia = new pwaUHDiaList();

            try
            {

                uhDia = oAPI.getListUHDiaList(iCodigoEmpresa: codigoEmpresa,
                                              iCodigoUnidade: codigoUnidade,
                                              iPage: page,
                                              bOffline: offline);

                return Ok(uhDia);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                                  iCodigoUnidade: codigoUnidade,
                                  iCodigoUsuario: codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: "",
                                  sResponseBody: oFunction.ConverteObjectParaJSon(uhDia));

            }

        }

        // GET api/PWA/insertApontamentoUHDia
        [HttpPut]
        [Route("api/PWA/insertApontamentoUHDia")]
        public IHttpActionResult insertApontamentoUHDia([FromBody] pwaUHDiaApontamento uhDia)
        {
            pwaApontamentoResponse response = new pwaApontamentoResponse();

            try
            {
                response = oAPI.insertUHDiaApontamento(oUHDiaApontamento: uhDia);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: uhDia.codigoEmpresa,
                                  iCodigoUnidade: uhDia.codigoUnidade,
                                  iCodigoUsuario: uhDia.codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(uhDia),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        #endregion

        #region ::: GOVERNANÇA :::

        // GET api/PWA/getListGovernanca
        [HttpGet]
        [Route("api/PWA/getListGovernanca")]
        public IHttpActionResult getListGovernanca(int codigoEmpresa, int codigoUnidade, int codigoUsuario, int codigoFuncionario, int page = 1, bool offline = false)
        {
            pwaGovernancaList governanca = new pwaGovernancaList();

            try
            {

                governanca = oAPI.getListGovernancaList(codigoEmpresa: codigoEmpresa,
                                                        codigoUnidade: codigoUnidade,
                                                        codigoFuncionario: codigoFuncionario,
                                                        codigoUsuario: codigoUsuario,
                                                        page: page,
                                                        offline: offline);

                return Ok(governanca);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                                  iCodigoUnidade: codigoUnidade,
                                  iCodigoUsuario: codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: "",
                                  sResponseBody: oFunction.ConverteObjectParaJSon(governanca));

            }

        }

        // GET api/PWA/insertApontamentoUHDia
        [HttpPut]
        [Route("api/PWA/insertApontamentoGovernanca")]
        public async Task<IHttpActionResult> insertApontamentoGovernanca([FromBody] pwaGovernancaApontamento governanca)
        {
            pwaApontamentoResponse response = new pwaApontamentoResponse();

            try
            {
                response = oAPI.insertGovernancaApontamento(oGovernanca: governanca);

                if (governanca.statusGovernanca != "")
                {
                    pwaApontamentoResponse responseStatus = new pwaApontamentoResponse();

                    pwaUHStatusUpdate statusUpdate = new pwaUHStatusUpdate
                    {
                        codigoEmpresa = governanca.codigoEmpresa,
                        codigoUnidade = governanca.codigoUnidade,
                        codigoUsuario = governanca.codigoUsuario,
                        codigoApartamento = governanca.codigoApartamento,
                        status = governanca.statusGovernanca
                    };

                    if (governanca.codigoEmpresa == 1)
                    {
                        // Aguarda a conclusão do método assíncrono
                        responseStatus = await oAPI.updateUHStatusPost(uhStatus: statusUpdate);

                        //if (responseStatus.success)
                        //{
                        //    oAPI.updateUHStatusGovernanca(uhStatus: statusUpdate);
                        //}

                    }
                    else
                    {
                        // Aguarda a conclusão do método assíncrono
                        responseStatus = await oAPI.updateUHStatus(uhStatus: statusUpdate);

                        //if (responseStatus.success)
                        //{
                        //    oAPI.updateUHStatusGovernanca(uhStatus: statusUpdate);
                        //}

                    }

                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: governanca.codigoEmpresa,
                                  iCodigoUnidade: governanca.codigoUnidade,
                                  iCodigoUsuario: governanca.codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(governanca),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        #endregion

        #region ::: GOVERNANÇA - UPDATE STATUS :::


        // GET api/PWA/getListUHDia
        [HttpGet]
        [Route("api/PWA/getListUHStatus")]
        public IHttpActionResult getListUHStatus(int codigoEmpresa, int codigoUnidade, int codigoUsuario, int page = 1)
        {
            pwaUHStatusList uhDia = new pwaUHStatusList();

            try
            {

                uhDia = oAPI.getListUHStatusList(codigoEmpresa: codigoEmpresa,
                                                 codigoUnidade: codigoUnidade,
                                                 page: page);

                return Ok(uhDia);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                                  iCodigoUnidade: codigoUnidade,
                                  iCodigoUsuario: codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: "",
                                  sResponseBody: oFunction.ConverteObjectParaJSon(uhDia));

            }

        }

        // PUT api/PWA/updateUHStatus
        [HttpPut]
        [Route("api/PWA/updateUHStatus")]
        public async Task<IHttpActionResult> updateUHStatus([FromBody] pwaUHStatusUpdate uh)
        {
            pwaApontamentoResponse response = new pwaApontamentoResponse();

            try
            {

                if (uh.codigoEmpresa == 1)
                {
                    // Aguardando a conclusão do método assíncrono
                    response = await oAPI.updateUHStatusPost(uhStatus: uh);
                }
                else
                {
                    // Aguardando a conclusão do método assíncrono
                    response = await oAPI.updateUHStatus(uhStatus: uh);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                // Grava log
                oAPI.insertLogAPI(
                    iCodigoEmpresa: uh.codigoEmpresa,
                    iCodigoUnidade: uh.codigoUnidade,
                    iCodigoUsuario: uh.codigoUsuario,
                    sEndpoint: Request.RequestUri.PathAndQuery,
                    sRequestBody: oFunction.ConverteObjectParaJSon(uh),
                    sResponseBody: oFunction.ConverteObjectParaJSon(response)
                );
            }
        }


        #endregion

        #region ::: DEDETIZAÇÃO :::

        // GET api/PWA/getListDedetizacao
        [HttpGet]
        [Route("api/PWA/getListDedetizacao")]
        public IHttpActionResult getListDedetizacao(int codigoEmpresa, int codigoUnidade, int codigoUsuario, int page = 1)
        {
            pwaDedetizacaoList uhDia = new pwaDedetizacaoList();

            try
            {

                uhDia = oAPI.getListDedetizacaoList(iCodigoEmpresa: codigoEmpresa,
                                                    iCodigoUnidade: codigoUnidade,
                                                    iPage: page);

                return Ok(uhDia);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                                  iCodigoUnidade: codigoUnidade,
                                  iCodigoUsuario: codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: "",
                                  sResponseBody: oFunction.ConverteObjectParaJSon(uhDia));

            }

        }

        // GET api/PWA/insertApontamentoDedetizacao
        [HttpPut]
        [Route("api/PWA/insertApontamentoDedetizacao")]
        public IHttpActionResult insertApontamentoDedetizacao([FromBody] pwaDedetizacaoApontamento uhDia)
        {
            pwaDefaultResponse response = new pwaDefaultResponse();

            try
            {
                response = oAPI.insertDedetizacaoApontamento(oDedetizacaoApontamento: uhDia);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: uhDia.codigoEmpresa,
                                  iCodigoUnidade: uhDia.codigoUnidade,
                                  iCodigoUsuario: uhDia.codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(uhDia),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        #endregion

        #region ::: MAPA MANUTENÇÃO :::

        // GET api/PWA/getListMapaManutencao
        [HttpGet]
        [Route("api/PWA/getListMapaManutencao")]
        public IHttpActionResult getListMapaManutencao(int codigoEmpresa, int codigoUnidade, int codigoUsuario, long codigo = -1, int page = 1)
        {
            pwaMapaManutencaoList uhDia = new pwaMapaManutencaoList();

            try
            {

                uhDia = oAPI.getListMapaManutencaoList(iCodigoEmpresa: codigoEmpresa,
                                                       iCodigoUnidade: codigoUnidade,
                                                       lCodigo: codigo,
                                                       iPage: page);

                return Ok(uhDia);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                                  iCodigoUnidade: codigoUnidade,
                                  iCodigoUsuario: codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: "",
                                  sResponseBody: oFunction.ConverteObjectParaJSon(uhDia));

            }

        }

        // GET api/PWA/getListMapaManutencao
        [HttpGet]
        [Route("api/PWA/getMapaManutencao")]
        public IHttpActionResult getMapaManutencao(int codigoEmpresa, int codigoUnidade, int codigoUsuario, int page = 1)
        {
            pwaMapaList uhDia = new pwaMapaList();

            try
            {

                uhDia = oAPI.getListMapa(iCodigoEmpresa: codigoEmpresa,
                                         iCodigoUnidade: codigoUnidade,
                                         iPage: page);

                return Ok(uhDia);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                                  iCodigoUnidade: codigoUnidade,
                                  iCodigoUsuario: codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: "",
                                  sResponseBody: oFunction.ConverteObjectParaJSon(uhDia));

            }

        }

        // GET api/PWA/insertApontamentoMapaManutencao
        [HttpPut]
        [Route("api/PWA/insertApontamentoMapaManutencao")]
        public IHttpActionResult insertApontamentoMapaManutencao([FromBody] pwaMapaManutencaoApontamento mapaManutencao)
        {
            pwaDefaultResponse response = new pwaDefaultResponse();

            try
            {
                response = oAPI.insertMapaManutencaoApontamento(oMapaManutencaoApontamento: mapaManutencao);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: mapaManutencao.codigoEmpresa,
                                  iCodigoUnidade: mapaManutencao.codigoUnidade,
                                  iCodigoUsuario: mapaManutencao.codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(mapaManutencao),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        #endregion

        #region ::: QUALIDADE - AUDITORIA :::

        // GET api/PWA/getQualidadeAuditoria
        [HttpGet]
        [Route("api/PWA/getQualidadeAuditoria")]
        public IHttpActionResult getQualidadeAuditoria(int codigoEmpresa, int codigoUnidade, int codigoUsuario, int page = 1, bool offline = false)
        {
            pwaQualidadeAuditoriaList auditoria = new pwaQualidadeAuditoriaList();

            try
            {

                auditoria = oAPI.getListQualidadeAuditoria(iCodigoEmpresa: codigoEmpresa,
                                                           iCodigoUnidade: codigoUnidade,
                                                           iCodigoUsuario: codigoUsuario,
                                                           iPage: page,
                                                           bOffline: offline);

                return Ok(auditoria);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                                  iCodigoUnidade: codigoUnidade,
                                  iCodigoUsuario: codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: "",
                                  sResponseBody: oFunction.ConverteObjectParaJSon(auditoria));

            }

        }

        // GET api/PWA/insertApontamentoMapaManutencao
        [HttpPut]
        [Route("api/PWA/insertApontamentoQualidadeAuditoria")]
        public IHttpActionResult insertApontamentoQualidadeAuditoria([FromBody] pwaQualidadeAuditoriaApontamento qualidadeAuditoria)
        {
            pwaApontamentoResponse response = new pwaApontamentoResponse();

            try
            {
                response = oAPI.insertQualidadeAuditoriaApontamento(oQualidadeAuditoriaApontamento: qualidadeAuditoria);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: qualidadeAuditoria.codigoEmpresa,
                                  iCodigoUnidade: qualidadeAuditoria.codigoUnidade,
                                  iCodigoUsuario: qualidadeAuditoria.codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(qualidadeAuditoria),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        #endregion

        #region ::: AUDITORIA - CORPORATIVO :::

        // GET api/PWA/getAuditoriaCorporativo
        [HttpGet]
        [Route("api/PWA/getAuditoriaCorporativo")]
        public IHttpActionResult getAuditoriaCorporativo(int codigoEmpresa, int codigoUnidade, int codigoUsuario, int page = 1, bool offline = false)
        {
            pwaAuditoriaCorporativoList auditoria = new pwaAuditoriaCorporativoList();

            try
            {

                auditoria = oAPI.getListAuditoriaCorporativo(iCodigoEmpresa: codigoEmpresa,
                                                             iCodigoUnidade: codigoUnidade,
                                                             iCodigoUsuario: codigoUsuario,
                                                             iPage: page,
                                                             bOffline: offline);

                return Ok(auditoria);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                                  iCodigoUnidade: codigoUnidade,
                                  iCodigoUsuario: codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: "",
                                  sResponseBody: oFunction.ConverteObjectParaJSon(auditoria));

            }

        }

        // GET api/PWA/insertApontamentoMapaManutencao
        [HttpPut]
        [Route("api/PWA/insertApontamentoAuditoriaCorporativo")]
        public IHttpActionResult insertApontamentoAuditoriaCorporativo([FromBody] pwaAuditoriaCorporativoApontamento auditoriaCorporativo)
        {
            pwaApontamentoResponse response = new pwaApontamentoResponse();

            try
            {
                response = oAPI.insertAuditoriaCorporativoApontamento(oAuditoriaCorporativoApontamento: auditoriaCorporativo);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: auditoriaCorporativo.codigoEmpresa,
                                  iCodigoUnidade: auditoriaCorporativo.codigoUnidade,
                                  iCodigoUsuario: auditoriaCorporativo.codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(auditoriaCorporativo),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        #endregion

        #region ::: CHECKLIST :::

        // GET api/PWA/getChecklist
        [HttpGet]
        [Route("api/PWA/getChecklist")]
        public IHttpActionResult getChecklist(int codigoEmpresa, int codigoUnidade, long codigoChecklist, string tipo, long codigoDocumento, int intervalo = -1)
        {
            pwaChecklist checklist = new pwaChecklist();

            try
            {

                checklist = oAPI.getCheckList(iCodigoEmpresa: codigoEmpresa,
                                              iCodigoUnidade: codigoUnidade,
                                              lCodigoChecklist: codigoChecklist,
                                              sTipo: tipo,
                                              lCodigoDocumento: codigoDocumento,
                                              iIntervalo: intervalo);

                return Ok(checklist);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                                  iCodigoUnidade: codigoUnidade,
                                  iCodigoUsuario: -1,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: "",
                                  sResponseBody: oFunction.ConverteObjectParaJSon(checklist));

            }

        }

        // GET api/PWA/insertManutencaoProgramada
        [HttpPut]
        [Route("api/PWA/insertChecklist")]
        public IHttpActionResult insertChecklist(long codigoDocumento, string tipo, [FromBody] pwaChecklistApontamento checklist)
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

                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: checklist.codigoEmpresa,
                                  iCodigoUnidade: checklist.codigoUnidade,
                                  iCodigoUsuario: checklist.codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(checklist),
                                  sResponseBody: ex.Message);

                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                //oAPI.insertLogAPI(iCodigoEmpresa: checklist.codigoEmpresa,
                //                  iCodigoUnidade: checklist.codigoUnidade,
                //                  iCodigoUsuario: checklist.codigoUsuario,
                //                  sEndpoint: Request.RequestUri.PathAndQuery,
                //                  sRequestBody: oFunction.ConverteObjectParaJSon(checklist),
                //                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        // GET api/PWA/insertManutencaoProgramada
        [HttpPut]
        [Route("api/PWA/insertChecklistArray")]
        public IHttpActionResult insertChecklistArray(long codigoDocumento, int codigoUsuario, string tipo, int intervalo, [FromBody] pwaChecklist checklist)
        {
            pwaDefaultResponse response = new pwaDefaultResponse();
            int empresa = 0;
            int unidade = 0;

            try
            {

                foreach (pwaChecklistGrupo grupo in checklist.grupo)
                {
                    foreach (pwaChecklistSubGrupo subgrupo in grupo.subgrupo)
                    {
                        foreach (pwaChecklistItem checklistSG in subgrupo.checklist)
                        {

                            try
                            {

                                oAPI.insertApontamentoChecklistNew(lCodigoDocumento: codigoDocumento,
                                                                   iCodigoEmpresa: checklist.codigoEmpresa,
                                                                   iCodigoUnidade: checklist.codigoUnidade,
                                                                   iCodigoUsuario: codigoUsuario,
                                                                   lCodigoChecklist: checklist.codigoChecklist,
                                                                   iIntervalo: intervalo,
                                                                   sTipo: tipo,
                                                                   oChecklist: checklistSG);

                                empresa = checklist.codigoEmpresa;
                                unidade = checklist.codigoUnidade;

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }

                    foreach (pwaChecklistItem checklistG in grupo.checklist)
                    {
                        try
                        {

                            oAPI.insertApontamentoChecklistNew(lCodigoDocumento: codigoDocumento,
                                                               iCodigoEmpresa: checklist.codigoEmpresa,
                                                               iCodigoUnidade: checklist.codigoUnidade,
                                                               iCodigoUsuario: codigoUsuario,
                                                               lCodigoChecklist: checklist.codigoChecklist,
                                                               iIntervalo: intervalo,
                                                               sTipo: tipo,
                                                               oChecklist: checklistG,
                                                               bExoval: (grupo.descricao == "00 - ENXOVAL") ? true : false);

                            empresa = checklist.codigoEmpresa;
                            unidade = checklist.codigoUnidade;

                        }
                        catch (Exception ex)
                        {

                        }

                    }
                }

                try
                {
                    oAPI.insertApontamentoPlanoAcao(lCodigoDocumento: codigoDocumento,
                                                    iCodigoEmpresa: empresa,
                                                    iCodigoUnidade: unidade,
                                                    iCodigoUsuario: codigoUsuario,
                                                    sTipo: tipo);

                }
                catch (Exception ex)
                {
                }

                response.success = true;
                response.message = "";

                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: empresa,
                                  iCodigoUnidade: unidade,
                                  iCodigoUsuario: codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(checklist),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));

                return Ok(response);
            }
            catch (Exception ex)
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: empresa,
                                  iCodigoUnidade: unidade,
                                  iCodigoUsuario: codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(checklist),
                                  sResponseBody: ex.Message);

                response.success = false;
                response.message = ex.Message.ToString();

                return Ok(response);
            }
        }

        #endregion

        #region ::: PICTURE :::

        // PUT api/PWA/insertFile
        [HttpPut]
        [Route("api/PWA/insertFile")]
        public IHttpActionResult insertFile([FromBody] pwaArquivoInsert arquivo)
        {
            pwaDefaultResponse response = new pwaDefaultResponse();
            try
            {

                response = oAPI.insertFile(iCodigoEmpresa: arquivo.codigoEmpresa,
                                           iCodigoUnidade: arquivo.codigoUnidade,
                                           iCodigoUsuario: arquivo.codigoUsuario,
                                           lCodigoDocumento: arquivo.codigoDocumento,
                                           sTipo: arquivo.tipo,
                                           oArquivo: arquivo);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: arquivo.codigoEmpresa,
                                  iCodigoUnidade: arquivo.codigoUnidade,
                                  iCodigoUsuario: arquivo.codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(arquivo),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        // PUT api/PWA/insertFileGuid
        [HttpPut]
        [Route("api/PWA/insertFileGuid")]
        public IHttpActionResult insertFileGuid([FromBody] pwaArquivoInsert arquivo)
        {
            pwaDefaultResponse response = new pwaDefaultResponse();
            try
            {

                response = oAPI.insertFileGuid(iCodigoEmpresa: arquivo.codigoEmpresa,
                                               iCodigoUnidade: arquivo.codigoUnidade,
                                               iCodigoUsuario: arquivo.codigoUsuario,
                                               lCodigoDocumento: arquivo.codigoDocumento,
                                               sTipo: arquivo.tipo,
                                               oArquivo: arquivo);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: arquivo.codigoEmpresa,
                                  iCodigoUnidade: arquivo.codigoUnidade,
                                  iCodigoUsuario: arquivo.codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(arquivo),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        public void ResizeAndSaveImage(Stream imageStream, string outputFilePath)
        {

            // Carrega a imagem a partir do Stream
            using (Bitmap originalImage = new Bitmap(imageStream))
            {

                double scaleFactor = (originalImage.Width > originalImage.Height) ? 400.0 / originalImage.Width : 400.0 / originalImage.Height;

                // Calcula a nova largura e altura com base no fator de escala
                int newWidth = (int)(originalImage.Width * scaleFactor);
                int newHeight = (int)(originalImage.Height * scaleFactor);

                // Cria uma nova imagem redimensionada
                using (Bitmap resizedImage = new Bitmap(newWidth, newHeight))
                {
                    // Configura o objeto Graphics para redimensionar a imagem com alta qualidade
                    using (Graphics graphics = Graphics.FromImage(resizedImage))
                    {
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                    }

                    // Salva a imagem redimensionada no caminho de destino
                    resizedImage.Save(outputFilePath, System.Drawing.Imaging.ImageFormat.Png); // ou outro formato conforme necessário
                }
            }
        }

        #endregion

        #region ::: NOTIFICAÇÃO :::

        // GET api/PWA/getNotificacao
        [HttpGet]
        [Route("api/PWA/getNotificacao")]
        public IHttpActionResult getNotificacao(int codigoEmpresa, int codigoUnidade, int codigoUsuario)
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
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                                  iCodigoUnidade: codigoUnidade,
                                  iCodigoUsuario: codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: "",
                                  sResponseBody: oFunction.ConverteObjectParaJSon(notificacao));

            }

        }

        // PUT api/PWA/readNotificacao
        [HttpPut]
        [Route("api/PWA/readNotificacao")]
        public IHttpActionResult readNotificacao([FromBody] pwaReadNotificacao notificacao)
        {
            pwaDefaultResponse response = new pwaDefaultResponse();

            try
            {
                oAPI.readNotificacao(iCodigoEmpresa: notificacao.codigoEmpresa,
                                     iCodigoUnidade: notificacao.codigoUnidade,
                                     lCodigo: notificacao.codigo);

                response.success = true;
                response.message = "";

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: notificacao.codigoEmpresa,
                                  iCodigoUnidade: notificacao.codigoUnidade,
                                  iCodigoUsuario: notificacao.codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(notificacao),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        #endregion

        #region ::: LOG BOOK :::

        // GET api/PWA/getLogBook
        [HttpGet]
        [Route("api/PWA/getLogBook")]
        public IHttpActionResult getLogBook(int codigoEmpresa, int codigoUnidade, int codigoUsuario, string dataInicio, string dataTermino)
        {

            pwaLogBook logBook = new pwaLogBook();

            try
            {
                List<pwaListaLogBook> results = new List<pwaListaLogBook>();

                results = oAPI.getLogBook(iCodigoEmpresa: codigoEmpresa,
                                          iCodigoUnidade: codigoUnidade,
                                          iCodigoUsuario: codigoUsuario,
                                          sDataInicio: dataInicio,
                                          sDataTermino: dataTermino);

                logBook.success = true;
                logBook.message = "";
                logBook.results = results;

                return Ok(logBook);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: codigoEmpresa,
                                  iCodigoUnidade: codigoUnidade,
                                  iCodigoUsuario: codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: "",
                                  sResponseBody: oFunction.ConverteObjectParaJSon(logBook));

            }

        }

        // PUT api/PWA/insertLogBook
        [HttpPut]
        [Route("api/PWA/insertLogBook")]
        public IHttpActionResult insertLogBook([FromBody] pwaInsertLogBook logBook)
        {
            pwaDefaultResponse response = new pwaDefaultResponse();

            try
            {
                oAPI.insertLogBook(iCodigoEmpresa: logBook.codigoEmpresa,
                                   iCodigoUnidade: logBook.codigoUnidade,
                                   iCodigoUsuario: logBook.codigoUsuario,
                                   sDescricao: logBook.descricao);

                response.success = true;
                response.message = "";

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                //GRAVA LOG
                oAPI.insertLogAPI(iCodigoEmpresa: logBook.codigoEmpresa,
                                  iCodigoUnidade: logBook.codigoUnidade,
                                  iCodigoUsuario: logBook.codigoUsuario,
                                  sEndpoint: Request.RequestUri.PathAndQuery,
                                  sRequestBody: oFunction.ConverteObjectParaJSon(logBook),
                                  sResponseBody: oFunction.ConverteObjectParaJSon(response));
            }
        }

        #endregion

    }

}
