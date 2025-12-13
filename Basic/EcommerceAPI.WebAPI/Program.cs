using EcommerceAPI.Application.AutoMapper;
using EcommerceAPI.Application.Behaviors;
using EcommerceAPI.Application.Categories.Commands.CreateCategory;
using EcommerceAPI.Application.Categories.Queries.GetCategoryById;
using EcommerceAPI.Application.Infrastructure.Config;
using EcommerceAPI.Domain.Interfaces;
using EcommerceAPI.Infrastructure.Context;
using EcommerceAPI.Infrastructure.Repositories;
using EcommerceAPI.Infrastructure.Secrets;
using EcommerceAPI.WebAPI.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// // Configuration

//register secrets as singleton
builder.Services.AddSingleton<SecretsProvider>();

builder.Services.AddSingleton(sp => new SecretsConfig
{
    LicenseKey = sp.GetRequiredService<SecretsProvider>().GetLicenseKey(),
    ConnectionString = sp.GetRequiredService<SecretsProvider>().GetConnectionString()
});

// DbContext
builder.Services.AddDbContext<EcommerceApiDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("org_role", "org:admin"));
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
    var context = scope.ServiceProvider.GetRequiredService<EcommerceApiDbContext>();

    await DbSeeder.SeedAsync(context);
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