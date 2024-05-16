using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServProgLab7and8.Models;

namespace ServProgLab7and8.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly VideogamesContext _videogamesContext;

        public AuthController(ILogger<AuthController> logger, VideogamesContext videogamesContext)
        {
            _logger = logger;
            _videogamesContext = videogamesContext;
        }

        [HttpPost(Name = "Login")]
        public IActionResult GenAuthToken(string login, string password)
        {
            Person user = _videogamesContext.Persons.FirstOrDefault(x => x.Login == login && x.Password == password);
            if (user == null)
                return new BadRequestObjectResult("Incorrect login or password");
            var user_claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role) };
            var jwt = new JwtSecurityToken
            (
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: user_claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(60)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), 
                    SecurityAlgorithms.HmacSha256)
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                access_token = encodedJwt,
                username = user.Login
            };
            return new JsonResult(response);
        }
    }
}
