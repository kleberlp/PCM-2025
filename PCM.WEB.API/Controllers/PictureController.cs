using PCM.WEB.MODELS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;

namespace PCM.WEB.API.Controllers
{
    public class PictureController : ApiController
    {

        private DAL.WebApi oWebApi = new DAL.WebApi(ConfigurationManager.ConnectionStrings["DefaultConnectionAPI"].ConnectionString);

        // GET api/Picture/PictureInput
        [HttpPost]
        [Route("api/Picture/PictureInput")]
        public APIPictureReturn PictureInput(List<APIPicture> pictures)
        {
            APIPictureReturn oAPIPictureReturn = new APIPictureReturn();

            try
            {

                foreach (APIPicture picture in pictures)
                {
                    oWebApi.InsertPicture(oAPIPicture: picture);
                }

                oAPIPictureReturn.message = "";
            }
            catch (Exception ex)
            {
                oAPIPictureReturn.message = ex.Message;
            }

            return oAPIPictureReturn;

        }

        // GET api/Picture/PictureInputV2
        [HttpPost]
        [Route("api/Picture/PictureInputV2")]
        public APIPictureReturn PictureInputV2(APIPicture picture)
        {
            APIPictureReturn oAPIPictureReturn = new APIPictureReturn();

            try
            {

                API.Class.clsFunction oFunction = new API.Class.clsFunction();

                //GRAVA LOG
                oWebApi.InsertLogAPI(sEndPoint: Request.RequestUri.PathAndQuery,
                                    sMethod: Request.Method.ToString(),
                                    sRequestBody: oFunction.ConverteObjectParaJSon(picture),
                                    sResponseBody: "",
                                    sUsername: "1");

                oWebApi.InsertPicture(oAPIPicture: picture);

                oAPIPictureReturn.message = "";
            }
            catch (Exception ex)
            {
                oAPIPictureReturn.message = ex.Message;
            }

            return oAPIPictureReturn;

        }

        // GET api/Picture/PictureInputList
        [HttpPost]
        [Route("api/Picture/PictureInputList")]
        public APIPictureReturn PictureInputList(List<APIPicture> pictures)
        {
            APIPictureReturn oAPIPictureReturn = new APIPictureReturn();

            try
            {

                foreach (APIPicture picture in pictures)
                {
                    oWebApi.InsertPicture(oAPIPicture: picture);
                }

                oAPIPictureReturn.message = "";
            }
            catch (Exception ex)
            {
                oAPIPictureReturn.message = ex.Message;
            }

            return oAPIPictureReturn;

        }

    }
}
