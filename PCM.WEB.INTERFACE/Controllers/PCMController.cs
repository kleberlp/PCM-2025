using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PCM.WEB.DAL;
using PCM.WEB.MODELS;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PCM.WEB.INTERFACE.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PCMController : ControllerBase
    {   
        private IConfiguration _config;
        public PCMController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] UserCredentials user)
        {

            InterfaceApi interfaceApi = new InterfaceApi(sCon: _config["ConnectionString"].ToString());

            InterfaceUserInfo info = interfaceApi.Authentication(sUsername: user.Username,
                                                                 sPassword: user.Password);

            if (info.success)
            {

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>()
                {
                    new Claim("username", user.Username),
                    new Claim("name", info.name),
                    new Claim("codigoEmpresa", info.codigoEmpresa.ToString())
                };

                var Sectoken = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Issuer"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                return Ok(new
                {
                    access_token = token,
                    expiration = 120,
                    type = "bearer"
                });
            } else
            {
                return Unauthorized();
            }

        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("GreenPlanet")]
        public IActionResult GreenPlanet(int codigoUnidade, string dataInicio, string dataTermino, string? itemMedicao = "") 
        {

            InterfaceApi interfaceApi = new InterfaceApi(sCon: _config["ConnectionString"].ToString());

            return Ok(interfaceApi.GreenPlanet(iCodigoEmpresa: Convert.ToInt32(User.FindFirst("codigoEmpresa").Value.ToString()),
                                               codigoUnidade: codigoUnidade,
                                               dStartDate: dataInicio,
                                               dEndDate: dataTermino,
                                               sItemMedicao: itemMedicao));

        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("ResumoUnidades")]
        public IActionResult ResumoUnidades()
        {

            InterfaceApi interfaceApi = new InterfaceApi(sCon: _config["ConnectionString"].ToString());

            return Ok(interfaceApi.ResumoUnidades(iCodigoEmpresa: Convert.ToInt32(User.FindFirst("codigoEmpresa").Value.ToString())));

        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("PrincipaisOcorrencias")]
        public IActionResult PrincipaisOcorrencias(string hotelId, string dataInicio, string dataTermino, int modulo = 1)
        {

            InterfaceApi interfaceApi = new InterfaceApi(sCon: _config["ConnectionString"].ToString());

            return Ok(interfaceApi.PrincipaisOcorrencias(codigoEmpresa: Convert.ToInt32(User.FindFirst("codigoEmpresa").Value.ToString()),
                                                         hotelId: hotelId,
                                                         codigoModulo: modulo,
                                                         startDate: dataInicio,
                                                         endDate: dataTermino));

        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("RankingSLA")]
        public IActionResult RankingSLA(string hotelId, string dataInicio, string dataTermino, int modulo = 1)
        {

            InterfaceApi interfaceApi = new InterfaceApi(sCon: _config["ConnectionString"].ToString());

            return Ok(interfaceApi.PrincipaisOcorrencias(codigoEmpresa: Convert.ToInt32(User.FindFirst("codigoEmpresa")?.Value.ToString()),
                                                         hotelId: hotelId,
                                                         codigoModulo: modulo,
                                                         startDate: dataInicio,
                                                         endDate: dataTermino));

        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("OrdemServico")]
        public IActionResult OrdemServico(int page,
                                          int? codigoUnidade = null,
                                          string? dataAberturaOSInicio = null,
                                          string? dataAberturaOSTermino = null,
                                          string? dataConclusaoOSInicio = null,
                                          string? dataConclusaoOSTermino = null,
                                          string? status = null)
        {
            
            InterfaceApi interfaceApi = new InterfaceApi(sCon: _config["ConnectionString"].ToString());

            return Ok(interfaceApi.OrdemServico(codigoEmpresa: Convert.ToInt32(User.FindFirst("codigoEmpresa")?.Value),
                                                codigoUnidade: codigoUnidade ?? -1,
                                                dataAberturaOSInicio: dataAberturaOSInicio,
                                                dataAberturaOSTermino: dataAberturaOSTermino,
                                                dataConclusaoOSInicio: dataConclusaoOSInicio,
                                                dataConclusaoOSTermino: dataConclusaoOSTermino,
                                                status: status ?? string.Empty,
                                                page: page));

        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("Governanca")]
        public IActionResult Governanca(int page,
                                        int? codigoUnidade = null,
                                        string? dataInicio = null,
                                        string? dataTermino = null)
        {

            InterfaceApi interfaceApi = new InterfaceApi(sCon: _config["ConnectionString"].ToString());

            return Ok(interfaceApi.Governanca(codigoEmpresa: Convert.ToInt32(User.FindFirst("codigoEmpresa")?.Value),
                                              codigoUnidade: codigoUnidade ?? -1,
                                              dataInicio: dataInicio,
                                              dataTermino: dataTermino,
                                              page: page));

        }

        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("Rotina")]
        public IActionResult Rotina(int page,
                                    int? codigoUnidade = null,
                                    string? dataInicio = null,
                                    string? dataTermino = null)
        {

            InterfaceApi interfaceApi = new InterfaceApi(sCon: _config["ConnectionString"].ToString());

            try
            {

                return Ok(interfaceApi.Rotina(codigoEmpresa: Convert.ToInt32(User.FindFirst("codigoEmpresa")?.Value),
                                              codigoUnidade: codigoUnidade ?? -1,
                                              dataInicio: dataInicio,
                                              dataTermino: dataTermino,
                                              page: page));
            } catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }


        }

        [HttpPost]
       // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("StatusUH")]
        public IActionResult StatusUH(string hotelId)
        {

            InterfaceApiOracle interfaceApi = new InterfaceApiOracle(sCon: _config["ConnectionStringOracle"].ToString());

            return Ok(interfaceApi.StatusUH(sHotelId: hotelId));

        }

    }

}
