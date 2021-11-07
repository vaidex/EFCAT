using System.Linq.Expressions;

namespace EFCAT.Repository {
    public interface IRepository<TEntity, TKey> where TEntity : class {
        
        TEntity Read(TKey Id);
        IEnumerable<TEntity> ReadAll();
        IEnumerable<TEntity> ReadAll(int start, int count);
        IEnumerable<TEntity> Filter(Expression<Func<TEntity, bool>> filter);
        TEntity Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TKey Id);
        void Delete(TEntity entity);
    }
}
