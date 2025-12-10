using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using EcommerceAPI.Application.AutoMapper;
using EcommerceAPI.Application.Behaviors;
using EcommerceAPI.Application.Categories.Commands.CreateCategory;
using EcommerceAPI.Application.Categories.Queries.GetCategoryById;
using EcommerceAPI.Application.Infrastructure.Config;
using EcommerceAPI.Domain.Interfaces;
using EcommerceAPI.Infrastructure.Context;
using EcommerceAPI.Infrastructure.Repositories;
using EcommerceAPI.WebAPI.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// // Configuration
// Load secrets from Azure Key Vault
var licenseVaultUri = new Uri("https://luckypenny-license-key.vault.azure.net/");
var connstrVaultUri = new Uri("https://connstr-ecomdb.vault.azure.net/");
var credential = new DefaultAzureCredential();

var licenseClient = new SecretClient(licenseVaultUri, credential);
var connstrClient = new SecretClient(connstrVaultUri, credential);

var licenseKey = licenseClient.GetSecret("LicenseKey").Value.Value;
var connectionString = connstrClient.GetSecret("EcommerceDb").Value.Value;

//register secrets as singleton
builder.Services.AddSingleton(new SecretsConfig
{
    LicenseKey = licenseKey,
    ConnectionString = connectionString
});

// DbContext
builder.Services.AddDbContext<EcommerceApiDbContext>((sp, options) =>
{
    var secrets = sp.GetRequiredService<SecretsConfig>();
    options.UseSqlServer(secrets.ConnectionString);
});


// Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddControllers();

//Auth
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = issuer;
        options.Audience = audience;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ecommerce API",
        Version = "v1",
        Description = "API för e‑handel"
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetCategoryByIdHandler).Assembly);
});
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LicenseBehavior<,>));


// AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(AutoMapperProfile).Assembly);


// FluentValidation
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryCommandValidator>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EcommerceApiDbContext>();
}

app.UseRouting();
app.UseCors("AllowFrontend");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce API V1");
    c.RoutePrefix = string.Empty;
});

// Health endpoint för snabb test
app.MapGet("/health", () => "OK");

app.UseValidationExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();