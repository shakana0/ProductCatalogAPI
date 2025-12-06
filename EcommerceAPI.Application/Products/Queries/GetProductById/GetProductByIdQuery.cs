using MediatR;
using EcommerceAPI.Application.Products.Dtos;

namespace EcommerceAPI.Application.Products.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<ProductDto?>
    {
        public int Id { get; }

        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
    }
}
