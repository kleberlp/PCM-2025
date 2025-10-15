using PCM.WEB.MODELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace PCM.WEB.API.Controllers
{
    public class OrdemServicoController : ApiController
    {

        private DAL.WebApi oWebApi = new DAL.WebApi(ConfigurationManager.ConnectionStrings["DefaultConnectionAPI"].ConnectionString);

        public void SendMessage(String ordem_servico, String descricao, string to)
        {

            List<string> deviceTokens = to.Split(new char[] { (char)13 }).ToList();
            deviceTokens.Remove("");

            if (deviceTokens.Count > 0)
            {

                String timeStamp = DateTime.Now.ToFileTime().ToString();

                var data = new
                {
                    registration_ids = deviceTokens.ToArray(),
                    priority = "high",
                    content_available = true,
                    data = new
                    {
                        message = descricao.ToUpper(),
                        title = "Nº OS: " + ordem_servico,
                        is_background = true,
                        timestamp = timeStamp
                    }
                };

                SendNotification(data);
            }

        }

        public void SendNotification(object data)
        {
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(data);
            Byte[] byteArray = Encoding.UTF8.GetBytes(json);

            SendNotification(byteArray);
        }

        public void SendNotification(Byte[] byteArray)
        {
            try
            {
                string server_api_key = "AAAAhu64c6k:APA91bG9TSbnmR9nKnlR5qcrl6J5QcML-gVnur-D2YYvdua5WRxqwLtOVS6CmNJN0qQaLPGjqmePxj7Azg7e5AYqvqMmjM-Fa3mZseqFXoVCB1MzA3t9mqJs9psxZ0JBJ8OUbW2yNnpg";
                string sender_id = "579530683305";

                WebRequest webRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                webRequest.Method = "post";
                webRequest.ContentType = "application/json";
                webRequest.Headers.Add($"Authorization: key={server_api_key}");
                webRequest.Headers.Add($"Sender: id={sender_id}");

                webRequest.ContentLength = byteArray.Length;
                Stream dataStream = webRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse webResponse = webRequest.GetResponse();
                dataStream = webResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(dataStream);

                string sResponseFromServer = streamReader.ReadToEnd();

                streamReader.Close();
                dataStream.Close();
                webResponse.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // GET api/OrdemServico/getOrdemServico
        [HttpGet]
        [Route("api/OrdemServico/getOrdemServico")]
        public APIOrdemServico getOrdemServico(int codigo_empresa, int codigo_unidade, int codigo_funcionario, int codigo_usuario = -1)
        {
            //Ordem de Servico
            return oWebApi.getOrdemServico(iCodigoEmpresa: codigo_empresa,
                                           iCodigoUnidade: codigo_unidade,
                                           iCodigoFuncionario: codigo_funcionario,
                                           iCodigoUsuario: codigo_usuario);
        }

        [HttpPost]
        [Route("api/OrdemServico/OrdemServicoInput")]
        //POST api/OrdemServico/OrdemServicoInput
        public ApiOrdemServicoInputResponse OrdemServicoInput([FromBody] ApiOrdemServicoInput ordem_servico)
        {
            ApiOrdemServicoInputResponse apiOrdemServicoInputResponse = new ApiOrdemServicoInputResponse();

            //Vincular Ordem de Serviço
            oWebApi.OrdemServicoInput(oApiOrdemServicoInput: ref ordem_servico,
                                      oApiOrdemServicoInputResponse: ref apiOrdemServicoInputResponse);

            return apiOrdemServicoInputResponse;
        }

        [HttpPost]
        [Route("api/OrdemServico/OrdemServicoApontamento")]
        //POST api/OrdemServico/OrdemServicoApontamento
        public ApiOrdemServicoApontamentoResponse OrdemServicoApontamento([FromBody] ApiOrdemServicoApontamento apontamento)
        {

            ApiOrdemServicoApontamentoResponse response = new ApiOrdemServicoApontamentoResponse();

            API.Class.clsFunction oFunction = new API.Class.clsFunction();

            //GRAVA LOG
            oWebApi.InsertLogAPI(sEndPoint: Request.RequestUri.PathAndQuery,
                                sMethod: Request.Method.ToString(),
                                sRequestBody: oFunction.ConverteObjectParaJSon(apontamento),
                                sResponseBody: "",
                                sUsername: apontamento.codigo_usuario.ToString());

            try
            {
                long codigo = 0;

                //Vincular Ordem de Serviço
                oWebApi.InsertApontamentoOS(iCodigoEmpresa: apontamento.codigo_empresa,
                                            iCodigoUsuario: apontamento.codigo_usuario,
                                            lCodigoPCMOrdemServico: apontamento.codigo_ordem_servico,
                                            iCodigoUnidade: apontamento.codigo_unidade,
                                            iCodigoCategoria: apontamento.codigo_categoria,
                                            sDataInicio: apontamento.data_inicio,
                                            sDataTermino: apontamento.data_termino,
                                            sDescricaoSolucao: apontamento.solucao,
                                            sImagem: apontamento.imagem,
                                            bConcluido: (apontamento.concluido == 1) ? true : false,
                                            iCodigoJustificativaApontamento: apontamento.codigo_justificativa_apontamento,
                                            lCodigoEquipamento: apontamento.codigo_equipamento,
                                            lCodigo: ref codigo);

                response.codigo = codigo;
                response.message = "";

            }
            catch (Exception ex)
            {
                response.codigo = 0;
                response.message = ex.Message;
            }

            return response;
        }

        [HttpPost]
        [Route("api/OrdemServico/RequisicaoInput")]
        //POST api/OrdemServico/RequisicaoInput
        public ApiRequisicaoInputResponse RequisicaoInput([FromBody] ApiRequisicaoInput requisicao)
        {

            ApiRequisicaoInputResponse apiRequisicaoInputResponse = new ApiRequisicaoInputResponse();

            //Vincular Ordem de Serviço
            oWebApi.RequisicaoInput(oApiRequisicaoInput: ref requisicao,
                                    oApiRequisicaoInputResponse: ref apiRequisicaoInputResponse);

            return apiRequisicaoInputResponse;
        }

        // GET api/OrdemServico/InfoEquipamento
        [HttpGet]
        [Route("api/OrdemServico/InfoEquipamento")]
        public ApiInfoEquipamentoResponse InfoEquipamento(int codigo_empresa, int codigo_unidade, long codigo_equipamento)
        {
            //Ordem de Servico
            return oWebApi.getEquipamentoInfo(iCodigoEmpresa: codigo_empresa,
                                              iCodigoUnidade: codigo_unidade,
                                              lCodigoEquipamento: codigo_equipamento);
        }

        // GET api/OrdemServico/InfoEquipamento
        [HttpPost]
        [Route("api/OrdemServico/statusOrdemServico")]
        public ApiStatusOrdemServico statusOrdemServico([FromBody] ApiStatusOrdemServico statusOrdemServico)
        {
            //Ordem de Servico
            oWebApi.OrdemServicoStatus(iCodigoEmpresa: statusOrdemServico.codigo_empresa,
                                       iCodigoUnidade: statusOrdemServico.codigo_unidade,
                                       lCodigoPCMOrdemServico: statusOrdemServico.codigo_ordem_servico,
                                       iCodigoUsuario: statusOrdemServico.codigo_usuario,
                                       iStatus: statusOrdemServico.status);

            return statusOrdemServico;
        }

    }
}
