using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Domain.Entities;

public class Product
{
    public int Id {get; set;}
  
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    [Required]
    public decimal Price { get; set; }
    public int StockQuantity {get; set;}
    public int CategoryId {get; set;}
    public Category Category {get; set;} = null!;

    public Product() { }

    public Product(string name, string description, decimal price, int stockQuantity, int categoryId)
    {
        Name = name;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
        CategoryId = categoryId;
    }
}
