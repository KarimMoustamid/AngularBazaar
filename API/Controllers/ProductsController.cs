namespace API.Controllers
{
    using Core.Entities;
    using Core.Interfaces;
    using Infrastructure.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IProductRepository _productRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return this.Ok(await _productRepository.GetProductsAsync());
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product is null) return this.NotFound();

            return this.Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _productRepository.AddProduct(product);

            if (await _productRepository.SaveChangesAsync())
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

            _productRepository.UpdateProduct(product);
            if (await _productRepository.SaveChangesAsync())
            {
                return this.NoContent();
            }

            return this.BadRequest("Could not update the product due to invalid input or server error.");
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product is null)
                return this.NotFound("Product not found.");

            _productRepository.DeleteProduct(product);
            if (await _productRepository.SaveChangesAsync())
            {
                return this.NoContent();
            }

            return this.BadRequest("Could not delete the product due to invalid input or server error.");
        }

        #region Helpers
        private bool ProductExists(int id)
        {
            return _productRepository.ProductExists(id);
        }
        #endregion
    }
}