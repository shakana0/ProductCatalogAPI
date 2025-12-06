using EcommerceAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Infrastructure.Context;

public class EcommerceApiDbContext : DbContext
{
    public EcommerceApiDbContext(DbContextOptions<EcommerceApiDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
        .Property(p => p.Price)
        .HasPrecision(18, 2);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId);
    }
}
