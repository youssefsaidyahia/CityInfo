using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace City_info.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthanticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthanticationController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public class AuthanticationRequestBody
        {
           public string? UserName { get; set; }   
           public string? Password { get; set; }   
        }
        public class CityInfoUser
        {
            public int UserId { get; set; }
            public string UserName { get; set; }    
            
            public string FirstName { get; set; }    
            public string LastName { get; set; }    
            public string City { get; set; }
            public CityInfoUser(int userId, string userName, string firstName, string lastName, string city)
            {
                UserId = userId;
                UserName = userName;
                FirstName = firstName;
                LastName = lastName;
                City = city;
            }
        }
        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthanticationRequestBody authanticationRequestBody)
        {
            // step 1 validate username and password

            var user = ValidateUserCredentials(authanticationRequestBody.UserName, authanticationRequestBody.Password);
            if (user == null)
            {
                return Unauthorized();
            }
            // Step 2: create a token
            var securitytoken = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));
            var signingCredentials=new SigningCredentials(securitytoken, SecurityAlgorithms.HmacSha256);
            var claimsForToken = new List<Claim>();

            // give information about the user using claims
            claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("given_name", user.FirstName));
            claimsForToken.Add(new Claim("family_name", user.LastName));
            claimsForToken.Add(new Claim("city", user.City));
            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],_configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler()
               .WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);

        }

        private CityInfoUser ValidateUserCredentials(string? userName, string? password)
        {
            return new CityInfoUser(
                1,
                userName ?? "",
                "Kevin",
                "Dockx",
                "Antwerp");
        }
    }
}
