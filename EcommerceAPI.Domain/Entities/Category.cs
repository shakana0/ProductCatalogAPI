using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Domain.Entities;

public class Category
{
    public int Id {get; set;}
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public Category() { }

    public Category(string name, string description, int? parentCategoryId = null)
    {
        Name = name;
        Description = description;
    }
}