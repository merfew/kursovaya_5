using kursovaya_auth1.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace kursovaya_auth1.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController: ControllerBase
    {
        AppDbContext db = new AppDbContext();
        [HttpGet]
        public IActionResult Login()
        {
            var headers = HttpContext.Request.Headers;
            HashPassword hashPassword = new HashPassword();
            if (headers.TryGetValue("email", out var emailValues) && (headers.TryGetValue("password", out var passwordValues)))
            {
                string email = emailValues.FirstOrDefault();
                string password = passwordValues.FirstOrDefault();
                User user = db.User.FirstOrDefault(c => c.email == email);
                if (user != null)
                {
                    var result = hashPassword.Verify(password, user.password);
                    if (result == false)
                    {
                        return BadRequest("Неправильный пароль");
                    }
                    else
                    {
                        JwtTokenGenerator tokenGenerator = new JwtTokenGenerator();
                        var jwt = tokenGenerator.GenerateToken(email);
                        Response.Cookies.Append("jwtCookie", jwt);
                        Response.Cookies.Append("id_user", user.user_id.ToString());
                        return Ok(new { jwt });
                    }

                }
            }
            return BadRequest("Ошибка");
        }


        [HttpPost]
        public IActionResult Registr([FromBody] User user)
        {
            HashPassword hashPassword = new HashPassword();
            string passHash = hashPassword.Generate(user.password);
            db.User.Add(new User
            {
                name = user.name,
                surname = user.surname,
                phone_number = user.phone_number,
                email = user.email,
                password = passHash
            });
            db.SaveChanges();
            JwtTokenGenerator tokenGenerator = new JwtTokenGenerator();
            var jwt = tokenGenerator.GenerateToken(user.name);
            Response.Cookies.Append("jwtCookie", jwt);
            Response.Cookies.Append("user_id", user.user_id.ToString());
            return Ok(new { jwt });
        }

        [Authorize]
        [HttpGet]
        public IActionResult Enter()
        {
            var headers = HttpContext.Request.Headers;
            return Ok("Welcome!");

        }
    }
}
