using System.Text;
using ConMirellaApi.Data;
using ConMirellaApi.Utils.Options;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(x =>
    x.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JWT"));
string key = builder.Configuration["JWT:Key"] ?? throw new InvalidOperationException();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", b =>
    {
        b.AllowAnyOrigin() 
            .AllowAnyHeader() 
            .AllowAnyMethod();
    });
});

builder.Services
    .AddAuthorization()
    .AddFastEndpoints()
    .AddSwaggerGen();

builder.Services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.Title = "Con Mirella Api";
        s.Version = "v1";
    };
});

var app = builder.Build();

app.UseCors("AllowAllOrigins");

app
    .UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints(c => c.Endpoints.RoutePrefix = "api")
    .UseSwaggerGen();

app.UseHttpsRedirection();

await app.RunAsync();
