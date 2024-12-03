namespace Infrastructure.Data.Repositories
{
    using Core.Entities;
    using Core.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class ProductRepository(StoreContext context) : IProductRepository
    {
        private readonly StoreContext _context = context;

        #region Products
        public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(brand))
                query = query.Where(p => p.Brand == brand);
            if (!string.IsNullOrEmpty(type))
                query = query.Where(p => p.Type == type);

            return await query.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
        }

        public bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        #endregion

        #region Brands
        public async Task<IReadOnlyList<string>> GetBrandsAsync()
        {
            return await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
        }
        #endregion

        #region Types
        public async Task<IReadOnlyList<string>> GetTypesAsync()
        {
            return await _context.Products.Select(p => p.Type).Distinct().ToListAsync();
        }
        #endregion
    }
}