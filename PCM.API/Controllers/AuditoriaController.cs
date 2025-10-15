using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PCM.API.Class;
using PCM.WEB.DAL;
using PCM.WEB.MODELS;
using System;

namespace PCM.API.Controllers
{

    public class AuditoriaController : ControllerBase
    {

        Api oAPI = new Api(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionString").Value);

        Function oFunction = new Function();

        #region ::: AUDITORIA :::

        // GET api/Auditoria/getListAuditoria
        [HttpGet]
        [Route("api/Auditoria/getListAuditoria")]
        public IActionResult getListAuditoria(int codigoEmpresa, int codigoUnidade, int codigoUsuario, int codigoFuncionario, int page = 1)
        {

            pwaAuditoriaList auditoria = new pwaAuditoriaList();

            try
            {

                auditoria = oAPI.getListAuditoriaList(iCodigoEmpresa: codigoEmpresa,
                                                      iCodigoUnidade: codigoUnidade,
                                                      iCodigoFuncionario: codigoFuncionario,
                                                      iPage: page);

                return Ok(auditoria);

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
                                  sResponseBody: oFunction.ConverteObjectParaJSon(auditoria));

            }

        }

        #endregion

    }

}
