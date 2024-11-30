namespace Infrastructure.Data
{
    using Core.Entities;
    using Microsoft.EntityFrameworkCore;

    public class StoreContext(DbContextOptions<StoreContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
    }
}