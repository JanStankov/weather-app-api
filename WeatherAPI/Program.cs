using Microsoft.EntityFrameworkCore;
using WeatherAPI.Database;
using WeatherAPI.Database.Repositories;
using WeatherAPI.Database.Repositories.Interfaces;
using WeatherAPI.Services;
using WeatherAPI.Utilities;
using WeatherAPI.Utilities.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WeatherAPI.Services.Interfaces;
using WeatherAPI.Models;

var builder = WebApplication.CreateBuilder(args);


var settings = new Settings();
builder.Configuration.Bind("Settings", settings);
builder.Services.AddSingleton(settings);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.BearerKey)),
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false
    };
});

builder.Services.AddAuthorization();

// Add services to the container.

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IOpenWeatherService, OpenWeatherService>();
builder.Services.AddHttpClient();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var connectionString = builder.Configuration.GetConnectionString("Db");
builder.Services.AddDbContext<WeatherAPIDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200")
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                      });
});

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
