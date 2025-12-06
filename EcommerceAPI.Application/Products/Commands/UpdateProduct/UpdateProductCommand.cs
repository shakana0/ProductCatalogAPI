using MediatR;
using EcommerceAPI.Application.Products.Dtos;

namespace EcommerceAPI.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<ProductDto?>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
    }
}
