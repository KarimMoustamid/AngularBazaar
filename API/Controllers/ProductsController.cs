namespace API.Controllers
{
    using Core.Entities;
    using Infrastructure.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(StoreContext context) : ControllerBase
    {
        private readonly StoreContext _context = context ?? throw new ArgumentNullException(nameof(context));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return this.Ok(await _context.Products.ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null) return this.NotFound();

            return this.Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id))
                return this.BadRequest("Cannot update this product as it does not exist.");

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return this.NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null)
                return this.NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return this.NoContent();
        }

        #region Helpers
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
        #endregion
    }
}