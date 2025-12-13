using MediatR;
using AutoMapper;
using EcommerceAPI.Domain.Interfaces;
using EcommerceAPI.Application.Products.Dtos;

namespace EcommerceAPI.Application.Products.Queries.GetProducts
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, PagedResult<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetAllProductsHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var totalCount = await _productRepository.CountAsync(cancellationToken);
            var products = await _productRepository.GetPagedAsync(request.Page, request.PageSize, cancellationToken);

            return new PagedResult<ProductDto>
            {
                Items = _mapper.Map<IEnumerable<ProductDto>>(products),
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
