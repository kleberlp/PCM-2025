using PCM.WEB.DAL;
using PCM.WEB.MODELS;
using PCM.WEB.MODELS;
using System;
using System.Configuration;
using System.Web.Http;

namespace PCM.WEB.API.Controllers
{
    [RoutePrefix("api/Interface")]
    public class InterfaceController : ApiController
    {
        private InterfaceApiOracle interfaceApi = new InterfaceApiOracle(sCon: ConfigurationManager.ConnectionStrings["DefaultConnectionIntercity"].ConnectionString);
       
        [HttpGet]
        [Route("StatusUH")]
        public object StatusUH(string hotelId)
        {

            try
            {
                return Ok(interfaceApi.StatusUH(sHotelId: hotelId));

            } catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }

        }

        [HttpGet]
        [Route("ReservaUH")]
        public object ReservaUH(string hotelId)
        {

            try
            {
                return Ok(interfaceApi.ReservaUH(sHotelId: hotelId));

            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }

        }

        [HttpGet]
        [Route("ListStatusUH")]
        public object ListStatusUH()
        {

            try
            {
                return Ok(interfaceApi.ListStatusUH());

            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }

        }

        [HttpPost]
        [Route("UpdateStatusUH")]
        public object UpdateStatusUH([FromBody] StatusUHInterface status)
        {

            try
            {
                interfaceApi.UpdateStatusUH(hotelId: status.hotelId,
                                            uh: status.uh,
                                            status: status.status);
                return Ok();

            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }

        }

    }
}
