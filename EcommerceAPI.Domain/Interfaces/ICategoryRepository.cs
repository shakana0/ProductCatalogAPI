
using EcommerceAPI.Domain.Entities;

namespace EcommerceAPI.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> AddAsync(Category category, CancellationToken cancellationToken);
        Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken);
        Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Category?> UpdateAsync(int id, string name, string description, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
