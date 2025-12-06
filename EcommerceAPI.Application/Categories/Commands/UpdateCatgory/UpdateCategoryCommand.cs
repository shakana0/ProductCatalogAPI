using MediatR;
using EcommerceAPI.Application.Categories.Dtos;

namespace EcommerceAPI.Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<CategoryDto>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
