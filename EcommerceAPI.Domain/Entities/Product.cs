using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Domain;

public class Product
{
    public int Id {get; private set;}
  
    [Required]
    [MaxLength(100)]
    public string Name { get; private set; } = string.Empty;
    [MaxLength(500)]
    public string Description { get; private set; } = string.Empty;
    [Required]
    public decimal Price { get; private set; }
    public int StockQuantity {get; private set;}
    public int CategoryId {get; private set;}
    public Category Category {get; private set;} = null!;

    protected Product() { }

    public Product(string name, string description, decimal price, int stockQuantity, int categoryId)
    {
        Name = name;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
        CategoryId = categoryId;
    }
}
