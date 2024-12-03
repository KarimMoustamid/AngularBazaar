using System.Collections.Generic;
using System.Threading.Tasks;
namespace Core.Interfaces
{
    using Entities;

    public interface IProductRepository
    {
        #region Products
        Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort);
        Task<Product?> GetProductByIdAsync(int id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        bool ProductExists(int id);
        Task<bool> SaveChangesAsync();
        #endregion

        #region Brands
        Task<IReadOnlyList<string>> GetBrandsAsync();
        Task<IReadOnlyList<string>> GetTypesAsync();
        #endregion
    }
}