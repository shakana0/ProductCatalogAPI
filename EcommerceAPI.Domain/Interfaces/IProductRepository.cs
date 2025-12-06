
using EcommerceAPI.Domain.Entities;

namespace EcommerceAPI.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product product, CancellationToken cancellationToken);

        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken);

        Task UpdateAsync(Product product, CancellationToken cancellationToken);

        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
