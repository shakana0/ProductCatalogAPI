using Bogus;
using EcommerceAPI.Domain.Entities;
using EcommerceAPI.Infrastructure.Context;
public static class DbSeeder
{
    public static void Seed(EcommerceApiDbContext context)
    {
        if (context.Categories.Any() || context.Products.Any())
            return;

        var categories = new List<Category>
        {
            new Category("Gaming Computers", "High-performance desktops and laptops for gaming"),
            new Category("Headphones & Audio", "Wired and wireless headphones, earbuds, speakers"),
            new Category("Smart Home Devices", "Smart bulbs, plugs, thermostats, assistants"),
            new Category("Mobile Accessories", "Chargers, cases, screen protectors, power banks"),
            new Category("Computer Peripherals", "Keyboards, mice, monitors, webcams, storage")
        };

        context.Categories.AddRange(categories);
        context.SaveChanges();

        var productFaker = new Faker<Product>()
        .CustomInstantiator(f =>
        {
            var category = f.PickRandom(categories);

            decimal price;
            int stock;

            switch (category.Name)
            {
                case "Gaming Computers":
                    price = f.Random.Decimal(1000, 50000);
                    stock = f.Random.Int(1, 15);
                    break;

                case "Headphones & Audio":
                    price = f.Random.Decimal(200, 10000);
                    stock = f.Random.Int(10, 50);
                    break;

                case "Smart Home Devices":
                    price = f.Random.Decimal(300, 9000);
                    stock = f.Random.Int(5, 30);
                    break;

                case "Mobile Accessories":
                    price = f.Random.Decimal(10, 1000);
                    stock = f.Random.Int(50, 200);
                    break;

                case "Computer Peripherals":
                    price = f.Random.Decimal(100, 5000);
                    stock = f.Random.Int(20, 100);
                    break;

                default:
                    price = f.Random.Decimal(50, 500);
                    stock = f.Random.Int(1, 50);
                    break;
            }

            return new Product(
                f.Commerce.ProductName(),
                f.Commerce.ProductDescription(),
                price,
                stock,
                category.Id
            );
        });


        var products = productFaker.Generate(50);
        context.Products.AddRange(products);
        context.SaveChanges();
    }
}
