using System.Linq.Expressions;

namespace EFCAT.Domain.Repository;

public interface IRepositoryAsync<TEntity, TKey> where TEntity : class {
    Task<TEntity> ReadAsync(TKey Id);
    Task<IEnumerable<TEntity>> ReadAllAsync();
    Task<IEnumerable<TEntity>> ReadAllAsync(int start, int count);
    Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> filter);
    Task<TEntity> CreateAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TKey Id);
    Task DeleteAsync(TEntity entity);
}