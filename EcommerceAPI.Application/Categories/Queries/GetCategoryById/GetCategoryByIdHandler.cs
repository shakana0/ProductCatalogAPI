using AutoMapper;
using EcommerceAPI.Application.Categories.Dtos;
using EcommerceAPI.Domain.Interfaces;
using MediatR;

namespace EcommerceAPI.Application.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoryByIdHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto?> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(query.Id, cancellationToken);

            if (category == null) return null;

            return _mapper.Map<CategoryDto>(category);
        }
    }
}
