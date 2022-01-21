using System.Linq.Expressions;

namespace EFCAT.Domain.Repository;

public interface IRepositoryAsync<TEntity> where TEntity : class {
    Task<TEntity> ReadAsync(params object[] keys);
    Task<IEnumerable<TEntity>> ReadAllAsync();
    Task<IEnumerable<TEntity>> ReadAllAsync(int start, int count);
    Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> filter);
    Task<TEntity> CreateAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(params object[] keys);
    Task DeleteAsync(TEntity entity);
}