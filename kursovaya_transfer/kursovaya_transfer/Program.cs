using kursah_5semestr.Services;
using kursovay_transfer.RabbitMQ;
using kursovaya_transfer;
using kursovaya_transfer.Model;
using kursovaya_transfer.Repository;
using kursovaya_transfer.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddTransient<ITransferRepository, TransferRepository>();
builder.Services.AddTransient<ITransferService, TransferService>();
builder.Services.AddTransient<ITransferBrokerService, TransferBrokerService>();
builder.Services.AddTransient<IDataUpdaterService, RabbitMqListener>();

builder.Services.AddSingleton<IUserDataFunc, UserDataFunc>();

//builder.Services.AddHostedService<RabbitMqListener>();

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