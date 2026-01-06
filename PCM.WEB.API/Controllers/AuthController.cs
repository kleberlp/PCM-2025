using Microsoft.IdentityModel.Tokens;
using PCM.WEB.MODELS;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web.Http;

namespace PCM.WEB.API.Controllers
{
    public class AuthController : ApiController
    {
        private const string SecretKey = "your_secret_key_here"; // Chave secreta para assinar o token
        private static readonly byte[] SecretBytes = Encoding.UTF8.GetBytes(SecretKey);

        public static class Settings
        {
            public static string Secret = "pcmaf7d8863b48e197b9287d492b708e";
        }

        [HttpPost]
        [Route("api/login")]
        public IHttpActionResult Login([FromBody] UserCredentials credentials)
        {
            // Verificar se as credenciais são válidas (exemplo simples)
            if (credentials.Username == "admin" && credentials.Password == "admin123")
            {
                var token = GenerateToken(credentials.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }

        [HttpGet]
        [Authorize]
        [Route("api/protected")]
        public IHttpActionResult Protected()
        {
            return Ok("Você acessou a rota protegida!");
        }

        public static string GenerateToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }

        // Função para validar o token
        public static bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(SecretBytes),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // Verifique se o identificador da chave (kid) está presente no token
                var jwtToken = (JwtSecurityToken)validatedToken;
                if (jwtToken.Header.ContainsKey("kid"))
                {
                    // Se o kid estiver presente, você pode usar isso para validar a chave, se necessário
                    // Exemplo: validationParameters.ValidIssuer = jwtToken.Issuer;
                    // Você pode adicionar outras validações aqui conforme necessário
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpGet]
        [Route("api/getweekday")]
        public IHttpActionResult GetWeekday(string date)
        {
            // Validar o token JWT
            string token = Request.Headers.Authorization.Parameter;
            if (!ValidateToken(token))
            {
                return Unauthorized();
            }

            // Retornar o dia da semana
            return Ok(Convert.ToDateTime(date).DayOfWeek.ToString());
        }

    }
}
