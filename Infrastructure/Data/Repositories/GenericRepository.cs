namespace Infrastructure.Data.Repositories
{
    using Core.Entities;
    using Core.Interfaces;
    using Microsoft.EntityFrameworkCore;


    public class GenericRepository<T>(StoreContext context) : IGenericRepository<T> where T : BaseEntity
    {
        public async Task<T?> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public void AddEntity(T entity)
        {
            context.Set<T>().Add(entity);
        }

        public void UpdateEntity(T entity)
        {
            context.Set<T>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void DeleteEntity(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public bool EntityExists(int id)
        {
            return context.Set<T>().Any(e => e.Id == id);
        }
    }
}