using MediatR;
using EcommerceAPI.Application.Products.Dtos;

namespace EcommerceAPI.Application.Products.Queries.GetProducts
{
    public class GetAllProductsQuery : IRequest<PagedResult<ProductDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
