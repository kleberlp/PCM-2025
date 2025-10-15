using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PCM.API.Class;
using PCM.WEB.DAL;
using PCM.WEB.MODELS;
using System;

namespace PCM.API.Controllers
{

    public class AccountController : ControllerBase
    {

        Api oAPI = new Api(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionString").Value);

        Function oFunction = new Function();

        // GET api/account/login
        [HttpGet]
        [Route("api/Account/Login")]
        public IActionResult Login(string email, string senha)
        {


            pwaLogin login = oAPI.Login(sEmail: email,
                                        sSenha: senha);

            ////GRAVA LOG
            //oAPI.InsertLogAPI(sBranch: new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Branch").Value,
            //                  sEndPoint: Request.Path,
            //                  sMethod: Request.Method.ToString(),
            //                  sRequestBody: "",
            //                  sResponseBody: oFunction.ConverteObjectParaJSon(login),
            //                  sUsername: username);

            return Ok(login);
        }

        // GET api/account/EsqueciSenha
        [HttpGet]
        [Route("api/Account/EsqueciSenha")]
        public IActionResult EsqueciSenha(string email)
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
                response.success = false;
                response.message = ex.Message;
                return BadRequest(response);
            } 

        }

    }

}
