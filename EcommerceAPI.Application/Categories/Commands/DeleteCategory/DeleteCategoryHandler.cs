using MediatR;
using EcommerceAPI.Domain.Interfaces;

namespace EcommerceAPI.Application.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var result = await _categoryRepository.DeleteAsync(request.Id, cancellationToken);
            return result;
        }
    }
}
