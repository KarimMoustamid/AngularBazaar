namespace Infrastructure.Data.Repositories
{
    using Core.Entities;
    using Core.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class ProductRepository(StoreContext context) : IProductRepository
    {
        #region Products
        public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
        {
            var query = context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(brand))
                query = query.Where(p => p.Brand == brand);
            if (!string.IsNullOrEmpty(type))
                query = query.Where(p => p.Type == type);

            query = sort switch
            {
                "priceAsc" => query.OrderBy(p => p.Price),
                "priceDesc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            };

            return await query.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public void AddProduct(Product product)
        {
            context.Products.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            context.Entry(product).State = EntityState.Modified;
        }

        public void DeleteProduct(Product product)
        {
            context.Products.Remove(product);
        }

        public bool ProductExists(int id)
        {
            return context.Products.Any(e => e.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
        #endregion

        #region Brands
        public async Task<IReadOnlyList<string>> GetBrandsAsync()
        {
            return await context.Products.Select(p => p.Brand).Distinct().ToListAsync();
        }
        #endregion

        #region Types
        public async Task<IReadOnlyList<string>> GetTypesAsync()
        {
            return await context.Products.Select(p => p.Type).Distinct().ToListAsync();
        }
        #endregion
    }
}