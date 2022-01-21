using System.Linq.Expressions;

namespace EFCAT.Domain.Repository;

public interface IRepository<TEntity> where TEntity : class {
    TEntity Read(params object[] keys);
    IEnumerable<TEntity> ReadAll();
    IEnumerable<TEntity> ReadAll(int start, int count);
    IEnumerable<TEntity> Filter(Expression<Func<TEntity, bool>> filter);
    TEntity Create(TEntity entity);
    void Update(TEntity entity);
    void Delete(params object[] keys);
    void Delete(TEntity entity);
}
