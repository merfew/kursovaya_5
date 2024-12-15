using kursovaya_auth1.Model;
using kursovaya_auth1.RabbitMQ;
using kursovaya_auth1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Text;

namespace kursovaya_auth1.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController: ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            Response.Cookies.Append("email", "test@test");
            Response.Cookies.Append("password", "test");

            Request.Cookies.TryGetValue("email", out string? email);
            Request.Cookies.TryGetValue("password", out string? password);
            (int id, string jwt) = await _authService.Login(email, password);
            Response.Cookies.Append("jwtCookie", jwt);
            Response.Cookies.Append("id_user", id.ToString());
            return Ok(jwt);
        }


        [HttpPost]
        public async Task<IActionResult> Registr([FromBody] User user)
        {
            var jwt = await _authService.CreateUser(user);
            Response.Cookies.Append("jwtCookie", jwt);
            Response.Cookies.Append("id_user", user.user_id.ToString());
            return Ok( jwt );
        }

        [Authorize]
        [HttpGet]
        public IActionResult Enter()
        {
            return Ok("Welcome!");
        }
    }
}
