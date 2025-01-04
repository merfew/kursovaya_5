using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace kursovaya_auth1
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly AuthOptions _authOptions;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _authOptions = new AuthOptions(configuration);
        }

        public class AuthOptions
        {
            public string? ISSUER { get; set; }
            public string? AUDIENCE { get; set; }
            public string? KEY { get; set; }

            public AuthOptions(IConfiguration configuration)
            {
                ISSUER = configuration.GetValue<string>("AuthOptions:Issuer");
                AUDIENCE = configuration.GetValue<string>("AuthOptions:Audience");
                KEY = configuration.GetValue<string>("AuthOptions:Key");
            }

            public SymmetricSecurityKey GetSymmetricSecurityKey()
            {
                return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY!));
            }
        }

        public string GenerateToken(string? username)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

            var token = new JwtSecurityToken(
                issuer: _authOptions.ISSUER,
                audience: _authOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: new SigningCredentials(_authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    public interface IJwtTokenGenerator
    {
        string GenerateToken(string? username);
    }
}
