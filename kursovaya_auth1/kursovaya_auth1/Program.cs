using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using kursovaya_auth1.RabbitMQ;
using kursovaya_auth1.Repository;
using kursovaya_auth1.Services;
using kursovaya_auth1.Model;
using kursovaya_auth1;
using static kursovaya_auth1.JwtTokenGenerator;

var builder = WebApplication.CreateBuilder(args);

var authOptions = new AuthOptions(builder.Configuration);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(options =>
     {
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = true,
             ValidIssuer = authOptions.ISSUER,
             ValidateAudience = true,
             ValidAudience = authOptions.AUDIENCE,
             ValidateLifetime = true,
             IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
             ValidateIssuerSigningKey = true,
         };
     });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddSingleton(authOptions);

builder.Services.AddTransient<IAuthRepository, AuthRepository>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddTransient<IBrokerService, BrokerService>();

//builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

