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

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(authOptions);

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