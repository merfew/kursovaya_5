using kursovaya_auth1.Model;
using kursovaya_auth1.Object;
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
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginObj loginObj)
        {
            AnswerObj answerObj = new AnswerObj();
            answerObj = await _authService.Login(loginObj);
            Response.Cookies.Append("jwtCookie", answerObj.jwt);
            Response.Cookies.Append("id_user", answerObj.id.ToString());
            return Ok(answerObj.jwt);
        }


        [HttpPost]
        public async Task<IActionResult> Registr([FromBody] UserObj userObj)
        {
            AnswerObj answerObj = new AnswerObj();
            answerObj = await _authService.CreateUser(userObj);
            Response.Cookies.Append("jwtCookie", answerObj.jwt);
            Response.Cookies.Append("id_user", answerObj.id.ToString());
            return Ok( answerObj.jwt );
        }

        [Authorize]
        [HttpGet]
        public IActionResult Enter()
        {
            return Ok("Welcome!");
        }
    }
}
