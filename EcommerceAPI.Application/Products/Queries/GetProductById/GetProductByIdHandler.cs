using MediatR;
using AutoMapper;
using EcommerceAPI.Domain.Interfaces;
using EcommerceAPI.Application.Products.Dtos;

namespace EcommerceAPI.Application.Products.Queries.GetProductById
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductByIdHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

            if (product == null)
                return null;

            return _mapper.Map<ProductDto>(product);
        }
    }
}
