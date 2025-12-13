using EcommerceAPI.Domain.Entities;
using EcommerceAPI.Domain.Interfaces;
using EcommerceAPI.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;


namespace EcommerceAPI.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly EcommerceApiDbContext _context;

        public ProductRepository(EcommerceApiDbContext context)
        {
            _context = context;
        }

        public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);
            return product;
        }

        public async Task<IEnumerable<Product>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return await _context.Products.CountAsync(cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync(cancellationToken);
        }

        public async Task<Product?> UpdateAsync(int id, string name, string description, decimal price, int stockQuantity, int categoryId, CancellationToken cancellationToken)
        {
            var product = await GetByIdAsync(id, cancellationToken);
            if (product == null) return null;

            product.UpdateDetails(name, description, price, stockQuantity, categoryId);

            await _context.SaveChangesAsync(cancellationToken);
            return product;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var product = await GetByIdAsync(id, cancellationToken);
            if (product is null) return false;
            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
            return true;

        }
    }
}
