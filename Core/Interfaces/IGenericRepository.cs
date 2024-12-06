namespace Core.Interfaces
{
    using Entities;

    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        void AddEntity(T entity);
        void UpdateEntity(T entity);
        void DeleteEntity(T entity);
        Task<bool> SaveChangesAsync();
        bool EntityExists(int id);
    }
}