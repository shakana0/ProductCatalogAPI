using EcommerceAPI.Domain.Interfaces;
using EcommerceAPI.Infrastructure.Context;
using EcommerceAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using EcommerceAPI.Application.AutoMapper;
using EcommerceAPI.Application.Categories.Queries.GetCategoryById;

var builder = WebApplication.CreateBuilder(args);

var licenseKey = builder.Configuration["LuckyPenny:LicenseKey"];

// Registrera DbContext
builder.Services.AddDbContext<EcommerceApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrera repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    cfg.LicenseKey = licenseKey;
    cfg.RegisterServicesFromAssembly(typeof(GetCategoryByIdHandler).Assembly);
});


// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = licenseKey;
    cfg.AddMaps(typeof(AutoMapperProfile).Assembly);
});


var app = builder.Build();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
