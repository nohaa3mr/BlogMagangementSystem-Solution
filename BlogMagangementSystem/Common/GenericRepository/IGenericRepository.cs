using System.Linq.Expressions;

namespace BlogMagangementSystem.Common.GenericRepository
{
    public interface IGenericRepository<T> : IGenericRepositoryNon
    {
        public Task AddAsync(T entity);
        public Task DeleteAsync(T entity);
        public  Task UpdateInclude(T entity, params string[] modifiedProperties);
        public  Task AddRangeAsync(IEnumerable<T> entities);
        public Task<int> SaveChangesAsync();
        public void Dispose();
        public  Task DeleteRangeAsync(IEnumerable<T> entities);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IQueryable<T>> GetAllWithSpecAsync(Expression<Func<T, bool>> criteria);
        Task<T> GetByIdAsync(string id);
        Task<T> GetBySpecAsync(Expression<Func<T, bool>> criteria);

    }
}
