using PCM.WEB.MODELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;

namespace PCM.WEB.API.Controllers
{
    public class ChecklistController : ApiController
    {

        private DAL.WebApi oWebApi = new DAL.WebApi(ConfigurationManager.ConnectionStrings["DefaultConnectionAPI"].ConnectionString);
        private DAL.Qualidade oQualidade = new DAL.Qualidade(ConfigurationManager.ConnectionStrings["DefaultConnectionAPI"].ConnectionString);

        // GET api/Checklist/getGrupoChecklist
        [HttpGet]
        [Route("api/Checklist/getGrupoChecklist")]
        public List<APIGrupoChecklist> getGrupoChecklist(int codigo_empresa, int codigo_unidade, int codigo_checklist)
        {
            //Combo - Grupo Checklist
            return oWebApi.GrupoChecklist(iCodigoEmpresa: codigo_empresa,
                                          iCodigoUnidade: codigo_unidade,
                                          iCodigoChecklist: codigo_checklist);
        }

        // GET api/Checklist/getSubGrupoChecklist
        [HttpGet]
        [Route("api/Checklist/getSubGrupoChecklist")]
        public List<APIGrupoChecklist> getSubGrupoChecklist(int codigo_empresa, int codigo_unidade, int codigo_checklist, string grupo_checklist)
        {
            //Combo - Grupo Checklist
            return oWebApi.GrupoChecklist(iCodigoEmpresa: codigo_empresa,
                                          iCodigoUnidade: codigo_unidade,
                                          iCodigoChecklist: codigo_checklist);
        }

        // GET api/Checklist/getGrupoChecklist
        [HttpGet]
        [Route("api/Checklist/getChecklist")]
        public List<APIChecklist> getChecklist(int codigo_empresa, int codigo_unidade, long codigo_checklist, long codigo, int codigo_usuario, int intervalo = 0)
        {
            //Combo - Checklist
            return oWebApi.Checklist(iCodigoEmpresa: codigo_empresa,
                                     iCodigoUnidade: codigo_unidade,
                                     iCodigoUsuario: codigo_usuario,
                                     lCodigoChecklist: codigo_checklist,
                                     lCodigo: codigo,
                                     iIntervalo: intervalo);
        }

        // POST api/checklist/uploadChecklist
        [HttpPost]
        [Route("api/checklist/uploadChecklist")]
        public APIResponse uploadChecklist([FromBody] APIChecklistInsert checklist)
        {

            APIResponse response = new APIResponse();

            response.message = "";

            API.Class.clsFunction oFunction = new API.Class.clsFunction();

            //GRAVA LOG
            oWebApi.InsertLogAPI(sEndPoint: Request.RequestUri.PathAndQuery,
                                sMethod: Request.Method.ToString(),
                                sRequestBody: oFunction.ConverteObjectParaJSon(checklist),
                                sResponseBody: "",
                                sUsername: checklist.codigo_usuario.ToString());

            long codigo = 0;
            //if (checklist.tipo == "QUALIDADE") { checklist.concluido = true; }
            oWebApi.InsertChecklist(oAPIChecklistInsert: checklist, lCodigo: ref codigo);

            if (checklist.checklistItems != null)
            {

                foreach (APIChecklistItem item in checklist.checklistItems)
                {
                    try
                    {
                        oWebApi.InsertChecklistItem(iCodigoEmpresa: checklist.codigo_empresa,
                                                    iCodigoUnidade: checklist.codigo_unidade,
                                                    lCodigoChecklist: checklist.codigo_checklist,
                                                    sTipo: checklist.tipo,
                                                    lCodigo: codigo,
                                                    iCodigoItemChecklist: item.codigo_checklist_item,
                                                    sResultado: item.resultado,
                                                    sObservacao: item.observacao,
                                                    iPrazo: item.prazo);

                    }
                    catch (Exception ex)
                    {
                        response.message = "INSERT CHECKLIST ITEM: " + ex.Message + " " + item.codigo_checklist_item.ToString();
                        response.codigo = 0;
                    }
                }
            }

            if (checklist.concluido && (checklist.tipo == "QUALIDADE" || checklist.tipo == "AUDITORIA"))
            {
                oQualidade.SendEmailPlanoAcao(iCodigoEmpresa: checklist.codigo_empresa,
                                              iCodigoUsuario: checklist.codigo_usuario,
                                              iCodigoUnidade: checklist.codigo_unidade,
                                              lCodigoAuditoria: codigo,
                                              sTipo: checklist.tipo);
            }

            response.message = (response.message == "") ? "Operação realizada com sucesso!" : response.message;
            response.codigo = codigo;

            return response;
        }

        // POST api/Checklist/uploadChecklistPicture
        [HttpPost]
        [Route("api/Checklist/uploadChecklistPicture")]
        public APIResponse uploadChecklistPicture([FromBody] APIChecklistPicture picture)
        {

            APIResponse response = new APIResponse();

            if (picture != null)
            {
                try
                {
                    oWebApi.InsertChecklistPicture(oAPIChecklistPicture: picture);
                }
                catch
                {
                }
            }

            return response;
        }

        // GET api/Picture/uploadChecklistRefresh
        [HttpGet]
        [Route("api/Picture/uploadChecklistRefresh")]
        public APIPictureReturn uploadChecklistRefresh(String sEndPoint)
        {

            APIPictureReturn oAPIPictureReturn = new APIPictureReturn();

            try
            {

                API.Class.clsFunction oFunction = new API.Class.clsFunction();

                List<string> requestBody = new List<string>();

                requestBody = oWebApi.LoadAPIRefresh(sEndpoint: sEndPoint);

                foreach (string body in requestBody)
                {
                    APIChecklistInsert checklistInsert = oFunction.ConverteJSonParaObject<APIChecklistInsert>(body);

                    uploadChecklist(checklistInsert);

                }

                oAPIPictureReturn.message = "PRONTO";

            }
            catch (Exception ex)
            {
                oAPIPictureReturn.message = ex.Message;
            }

            return oAPIPictureReturn;

        }

    }
}
