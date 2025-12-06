
using EcommerceAPI.Application.Categories.Dtos;
using MediatR;

namespace EcommerceAPI.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<CategoryDto>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public CreateCategoryCommand(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
