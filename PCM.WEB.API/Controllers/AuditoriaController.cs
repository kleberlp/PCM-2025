using PCM.WEB.MODELS;
using System;
using System.Configuration;
using System.Web.Http;

namespace PCM.WEB.API.Controllers
{
    public class AuditoriaController : ApiController
    {

        private DAL.WebApi oWebApi = new DAL.WebApi(ConfigurationManager.ConnectionStrings["DefaultConnectionAPI"].ConnectionString);

        // GET api/Auditoria/getAuditoria
        [HttpGet]
        [Route("api/Auditoria/getAuditoria")]
        public APIAuditoria getAuditoria(int codigo_empresa, int codigo_unidade)
        {
            //Ordem de Servico
            return oWebApi.getAuditoria(iCodigoEmpresa: codigo_empresa,
                                        iCodigoUnidade: codigo_unidade);
        }

        // GET api/Auditoria/createAuditoria
        [HttpGet]
        [Route("api/Auditoria/createAuditoria")]
        public APICreateAuditoriaResponse createAuditoria(int codigo_empresa, int codigo_unidade, long codigo_auditoria, String numero_documento, int codigo_usuario)
        {
            //Combo - Checklist
            return oWebApi.createAuditoria(iCodigoEmpresa: codigo_empresa,
                                           iCodigoUnidade: codigo_unidade,
                                           iCodigoUsuario: codigo_usuario,
                                           sNumeroDocumento: numero_documento.ToUpper(),
                                           lCodigoAuditoria: codigo_auditoria);
        }

    }
}
