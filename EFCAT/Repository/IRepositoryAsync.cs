using System.Linq.Expressions;

namespace EFCAT.Repository {
    public interface IRepositoryAsync<TEntity, TKey> where TEntity : class {
        
        
        Task<TEntity> ReadAsync(TKey Id);
        Task<IEnumerable<TEntity>> ReadAllAsync();
        Task<IEnumerable<TEntity>> ReadAllAsync(int start, int count);
        Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> CreateAsync(TEntity entity);
        void UpdateAsync(TEntity entity);
        void DeleteAsync(TKey Id);
        void DeleteAsync(TEntity entity);
    }
}
