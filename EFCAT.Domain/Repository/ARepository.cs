using EFCAT.Model.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EFCAT.Domain.Repository;

public abstract class ARepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class {
    protected DbContext _context;
    protected DbSet<TEntity> _entitySet;

    protected ARepository(DbContext context) {
        _context = context;
        _entitySet = _context.Set<TEntity>();
    }

    public TEntity Read(TKey Id) => _entitySet.Find(Id);

    public IEnumerable<TEntity> ReadAll() => _entitySet.ToList();

    public IEnumerable<TEntity> ReadAll(int start, int count) => _entitySet.Skip(start).Take(count).ToList();

    public IEnumerable<TEntity> Filter(Expression<Func<TEntity, bool>> filter) => _entitySet.AsQueryable<TEntity>().Where(filter).ToList();

    public TEntity Create(TEntity entity) {
        _entitySet.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public void Update(TEntity entity) {
        _entitySet.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(TKey Id) => Delete(_entitySet.Find(Id));

    public void Delete(TEntity entity) {
        _entitySet.Remove(entity);
        _context.SaveChanges();
    }
}
