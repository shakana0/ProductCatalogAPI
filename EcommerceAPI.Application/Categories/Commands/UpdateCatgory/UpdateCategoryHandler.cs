using MediatR;
using AutoMapper;
using EcommerceAPI.Domain.Interfaces;
using EcommerceAPI.Application.Categories.Dtos;

namespace EcommerceAPI.Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto?>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public UpdateCategoryHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto?> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var updatedCategory = await _categoryRepository.UpdateAsync(request.Id, request.Name, request.Description, cancellationToken);
            if (updatedCategory == null) return null;

            return _mapper.Map<CategoryDto>(updatedCategory);
        }
    }
}
