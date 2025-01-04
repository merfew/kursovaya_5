using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using kursovaya_auth1.Model;
using Microsoft.EntityFrameworkCore;
using kursovaya_auth1.Repository;
using kursovaya_auth1.RabbitMQ;
using kursovaya_auth1.Object;
using System.Text.RegularExpressions;

namespace kursovaya_auth1.Services
{
    public class AuthService: IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtTokenGenerator _tokenGenerator;
        private readonly IBrokerService _brokerService;

        public AuthService(IAuthRepository authRepository, IJwtTokenGenerator jwtTokenGenerator, IBrokerService brokerService)
        {
            _authRepository = authRepository;
            _tokenGenerator = jwtTokenGenerator;
            _brokerService = brokerService;
        }

        public async Task<AnswerObj> Login(LoginObj loginObj)
        {
            AnswerObj answerObj = new AnswerObj();
            User? user = await _authRepository.Login(loginObj.email, loginObj.password);
            if (user == null) 
            {
                answerObj.id = -1;
                answerObj.jwt = "Такого пользователя нет";
                return answerObj;
            }
            else
            {
                HashPassword hashPassword = new HashPassword();
                var result = hashPassword.Verify(loginObj.password, user.password);
                if (result == false)
                {
                    answerObj.id = -1;
                    answerObj.jwt = "Такого пользователя нет";
                    return answerObj;
                }
                else
                {
                    var jwt = _tokenGenerator.GenerateToken(user.name);
                    var message = new UserData() { user_id = user.user_id };
                    await _brokerService.SendMessage("changes", message);
                    answerObj.id = user.user_id;
                    answerObj.jwt = jwt;
                    return answerObj;
                }
            }
        }
        public async Task<AnswerObj> CreateUser(UserObj userObj)
        {
            AnswerObj answerObj = new AnswerObj();
            User user = new User { 
                name = userObj.name,
                surname = userObj.surname,
                phone_number = userObj.phone_number,
                email = userObj.email,
                password = userObj.password,
            };
            if (!Regex.IsMatch(userObj.phone_number, @"^\+7\d{10}$"))
            {
                answerObj.id = -1;
                answerObj.jwt = "Неверный формат номера телефона. Пожалуйста, введите номер в формате +7XXXXXXXXXX";
                return answerObj;
            }
            //if (userObj.email.Contains("@"))
            if (!Regex.IsMatch(userObj.email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                answerObj.id = -1;
                answerObj.jwt = "Неверный формат email. Пожалуйста, введите корректный email адрес.";
                return answerObj;
            }
            HashPassword hashPassword = new HashPassword();
            string passHash = hashPassword.Generate(user.password);
            user.password = passHash;
            await _authRepository.CreateUser(user);
            var jwt = _tokenGenerator.GenerateToken(user.name);
            UserData message = new UserData
            {
                user_id = user.user_id
            };
            await _brokerService.SendMessage("changes", message);
            answerObj.id = user.user_id;
            answerObj.jwt = jwt;
            return answerObj;
        }
    }
}