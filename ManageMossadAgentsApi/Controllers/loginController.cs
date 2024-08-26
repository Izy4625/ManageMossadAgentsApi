using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ManageMossadAgentsApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ManageMossadAgentsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class loginController : ControllerBase
    {
        
        private string GenerateToken(string userIP)
        {
            // token handler can create token
            var tokenHandler = new JwtSecurityTokenHandler();

            string secretKey = "1234zdfxcghvjbhy7t67r54rtsdfxcghjugio7t6r5etrdfcghygu78t6redtfcghjgui78t6retdfty675r5678"; //TODO: remove this from code
            byte[] key = Encoding.ASCII.GetBytes(secretKey);

            // token descriptor describe HOW to create the token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // things to include in the token
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                    new Claim(ClaimTypes.Name, userIP),
                    }
                ),
                // expiration time of the token
                Expires = DateTime.Now.AddMinutes(10),
                // the secret key of the token
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                    )
            };

            // creating the token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            // converting the token to string
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;


        }

        [HttpPost]
        public IActionResult Login(Login loginObject)
        {
            if (loginObject.id == "SimulationServer")

            {
                //string UserIp = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

                return StatusCode(200
                    , new { token = "hidksjghskj"/*GenerateToken(UserIp)*/ }
                    ); 
              
                
            }
            return StatusCode(StatusCodes.Status401Unauthorized,
                    new { error = "invalid credentials" });
        }
    }
}

