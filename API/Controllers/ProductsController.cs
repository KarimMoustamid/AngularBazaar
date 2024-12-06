namespace API.Controllers
{
    using Core.Entities;
    using Core.Interfaces;
    using Infrastructure.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IGenericRepository<Product> genericRepository) : ControllerBase
    {
        #region Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            // return this.Ok(await _productRepository.GetProductsAsync(brand, type, sort));
            // We don't have the existing functionality of sorting and filtering products.
             return this.Ok(await genericRepository.GetAllAsync());
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await  genericRepository.GetByIdAsync(id);

            if (product is null) return this.NotFound();

            return this.Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            genericRepository.AddEntity(product);

            if (await genericRepository.SaveChangesAsync())
            {
                return CreatedAtRoute("GetProduct", new {id = product.Id}, product);
            }

            return this.BadRequest("Could not create the product due to invalid input or server error.");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id))
                return this.BadRequest("Cannot update this product as it does not exist.");

            genericRepository.UpdateEntity(product);
            if (await genericRepository.SaveChangesAsync())
            {
                return this.NoContent();
            }

            return this.BadRequest("Could not update the product due to invalid input or server error.");
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await genericRepository.GetByIdAsync(id);

            if (product is null)
                return this.NotFound("Product not found.");

            genericRepository.DeleteEntity(product);
            if (await genericRepository.SaveChangesAsync())
            {
                return this.NoContent();
            }

            return this.BadRequest("Could not delete the product due to invalid input or server error.");
        }
        #endregion

        #region Brands
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            // TODO : Implement Generic method
            // return this.Ok(await _productRepository.GetBrandsAsync());
            return this.Ok();
        }
        #endregion

        #region Types
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> Gettypes()
        {
            // TODO : Implement Generic method
            // return this.Ok(await _productRepository.GetTypesAsync());
            return this.Ok();
        }
        #endregion

        #region Helpers
        private bool ProductExists(int id)
        {
            return genericRepository.EntityExists(id);
        }
        #endregion
    }
}