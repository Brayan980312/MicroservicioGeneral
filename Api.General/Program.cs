using Api.General.Middlewares;
using Domain.General.Interfaces.External;
using Infrastructure.General.Extensions;
using Infrastructure.General.External;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Net;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Configuración de controladores y serialización JSON
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// Configuración global de CORS (permitir cualquier origen, método y encabezado)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = $"API General - {builder.Environment.EnvironmentName}",
        Version = "V1"
    });

    // Configuración para usar un Bearer Token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "Ingresa el token en el siguiente formato: Bearer {tu token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

//Configuracion de peticiones para auditoria en BD
builder.Services.AddTransient<RequestAuditingMiddleware>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle.
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton(builder.Configuration);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Inyección de dependencias.
builder.Services.AddDependencyInjection();

// Configuración del Contexto.
builder.Services.AddDatabaseContext(builder.Configuration);

// Configuración de peticiones a APIs externas
builder.Services.AddExternalServices(builder.Configuration);

// Configuración de la autenticación JWT.
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                // Evita que el sistema genere la respuesta por defecto
                context.HandleResponse();

                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";

                var problem = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.Unauthorized,
                    Type = "Unauthorized",
                    Title = "No autorizado",
                    Detail = "No tiene permisos para acceder a este recurso o su sesión ha expirado."
                };

                var json = JsonSerializer.Serialize(problem);
                await context.Response.WriteAsync(json);
            },
            OnForbidden = async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.Response.ContentType = "application/json";

                var problem = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.Forbidden,
                    Type = "Forbidden",
                    Title = "Acceso denegado",
                    Detail = "No tiene permisos suficientes para realizar esta acción."
                };

                var json = JsonSerializer.Serialize(problem);
                await context.Response.WriteAsync(json);
            }
        };
    });

builder.Services.AddAuthorization();

// Configuración de redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!)
);

// Construir la aplicación
var app = builder.Build();

// Configurar zona horaria predeterminada (Colombia)
var colombiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
app.Logger.LogInformation($"Zona horaria configurada: {colombiaTimeZone.DisplayName}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Configuración de los Cors.
app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseSwagger();
// Configuración Swagger.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "General API v1");
    c.RoutePrefix = string.Empty;
});

app.UseMiddleware<RequestAuditingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();