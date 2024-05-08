using H6_WiseWatt_Backend.Api.Utils;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.Domain.Services;
using H6_WiseWatt_Backend.MongoData;
using H6_WiseWatt_Backend.MongoData.Utils;
using H6_WiseWatt_Backend.Security;
using H6_WiseWatt_Backend.Security.Interfaces;
using H6_WiseWatt_Backend.Security.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure Serilog
Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs/logs.txt", rollingInterval: RollingInterval.Month)
            .CreateLogger();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// API Specific
builder.Services.AddTransient<AuthenticationUtility>();
builder.Services.AddTransient<DeviceDTOMapper>();
builder.Services.AddTransient<UserDTOMapper>();

// Data Specific
builder.Services.AddTransient<MongoDbContext>();
builder.Services.AddTransient<IUserRepo, UserRepo>();
builder.Services.AddTransient<UserDbMapper>();
builder.Services.AddTransient<IDeviceRepo, DeviceRepo>();
builder.Services.AddTransient<DeviceDbMapper>();
builder.Services.AddTransient<IElectricityPriceRepo, ElectricityPriceRepo>();
builder.Services.AddTransient<ElectricityPriceDbMapper>();

// Domain Specific Services
builder.Services.AddTransient<IUserManager, UserManager>();
builder.Services.AddSingleton<IDeviceFactory, DeviceFactoryService>();
builder.Services.AddTransient<IDeviceManager, DeviceManager>();
builder.Services.AddTransient<IConsumptionCalculator, ConsumptionCalculator>();
builder.Services.AddTransient<IElectricPriceService, ElectricPriceService>();
builder.Services.AddHttpClient();


// Security services
builder.Services.AddTransient<ITokenGenerator, TokenGenerator>();
builder.Services.AddTransient<IPasswordHasher, PasswordHasher>();

// Authentication schema
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var tokenSettings = builder.Configuration.GetSection("JWT").Get<JwtSettingModel>();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(tokenSettings.TokenSecret)),
            ValidateIssuer = true,
            ValidIssuer = tokenSettings.TokenIssuer,
            ValidateAudience = true,
            ValidAudience = tokenSettings.TokenAudience,
            ValidateLifetime = true,
        };
    });

// Disable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
