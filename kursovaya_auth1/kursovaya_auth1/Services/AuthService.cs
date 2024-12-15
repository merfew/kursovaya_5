﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using kursovaya_auth1.Model;
using Microsoft.EntityFrameworkCore;
using kursovaya_auth1.Repository;

namespace kursovaya_auth1.Services
{
    public class AuthService: IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtTokenGenerator _tokenGenerator;

        public AuthService(IAuthRepository authRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _authRepository = authRepository;
            _tokenGenerator = jwtTokenGenerator;
        }

        public async Task<(int, string)> Login(string email, string password)
        {
            User user = await _authRepository.Login(email, password);
            if (user == null) 
            {
                return (-1, "Такого пользователя нет");
            }
            else
            {
                HashPassword hashPassword = new HashPassword();
                var result = hashPassword.Verify(password, user.password);
                if (result == false)
                {
                    return (-1,"Неправильный пароль");
                }
                else
                {
                    var jwt = _tokenGenerator.GenerateToken(user.name);
                    return (user.user_id, jwt);
                }
            }
        }
        public async Task<string> CreateUser(User user)
        {
            HashPassword hashPassword = new HashPassword();
            string passHash = hashPassword.Generate(user.password);
            user.password = passHash;
            await _authRepository.CreateUser(user);
            var jwt = _tokenGenerator.GenerateToken(user.name);
            return jwt;
        }
    }
}