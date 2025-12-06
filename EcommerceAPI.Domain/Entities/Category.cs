using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Domain.Entities;

public class Category
{
    public int Id { get; private set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; private set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; private set; } = string.Empty;

    public Category() { }

    public Category(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void UpdateDetails(string? name, string? description)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name;

        if (!string.IsNullOrWhiteSpace(description))
            Description = description;
    }
}