using kursah_5semestr.Services;
using kursovay_card.Model;
using kursovay_card.RabbitMQ;
using kursovay_card.Repository;
using kursovay_card.Service;
using kursovaya_card;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(options =>
     {
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = true,
             ValidIssuer = AuthOptions.ISSUER,
             ValidateAudience = true,
             ValidAudience = AuthOptions.AUDIENCE,
             ValidateLifetime = true,
             IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
             ValidateIssuerSigningKey = true,
         };
     });

//builder.Services.AddHostedService<RabbitMqListener>();

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddTransient<ICardRepository,CardRepository>();
builder.Services.AddTransient<ICardService, CardService>();

builder.Services.AddTransient<ICardBrokerService, CardBrokerService>();
builder.Services.AddTransient<IDataUpdaterService, RabbitMqListener>();
builder.Services.AddSingleton<IUserDataFunc, UserDataFunc>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.Services.GetService<IDataUpdaterService>()!.Start();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public class AuthOptions
{
    public const string ISSUER = "MyAuthServer";
    public const string AUDIENCE = "MyAuthClient";
    const string KEY = "mysupersecret_secretsecretsecretkey!123";
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}