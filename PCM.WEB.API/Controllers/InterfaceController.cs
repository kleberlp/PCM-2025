using PCM.WEB.DAL;
using System.Configuration;
using System.Web.Http;

namespace PCM.WEB.API.Controllers
{
    public class InterfaceController : ApiController
    {

        private InterfaceApiOracle interfaceApi = new InterfaceApiOracle(sCon: ConfigurationManager.ConnectionStrings["DefaultConnectionIntercity"].ConnectionString);

        [HttpGet]
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("StatusUH")]
        public object StatusUH(string hotelId)
        {

            return Ok(interfaceApi.StatusUH(sHotelId: hotelId));

        }

    }
}
