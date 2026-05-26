using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SemilleroGR3.API.Data;
using SemilleroGR3.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// 1. Configurar Entity Framework Core con SQL Server
builder.Services.AddDbContext<SemilleroContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Inyectar el servicio de generación de Tokens para el AuthController
builder.Services.AddScoped<JwtService>();

// 3. Configurar Autenticación con JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings.GetValue<string>("SecretKey");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
            ValidAudience = jwtSettings.GetValue<string>("Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
        };
    });

// Genera el motor OpenAPI nativo de .NET 9
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Mantiene la generación del JSON en /openapi/v1.json

    // 4. Configurar Swagger UI apuntando al JSON nativo de .NET 9
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Semillero GR3 API v1");
        options.RoutePrefix = "swagger"; // La ruta de acceso será /swagger
    });
}

app.UseHttpsRedirection();

// 5. Habilitar Autenticación y Autorización de Rutas
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();