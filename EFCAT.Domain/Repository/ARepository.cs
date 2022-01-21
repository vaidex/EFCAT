using EFCAT.Model.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EFCAT.Domain.Repository;

public abstract class ARepository<TEntity> : IRepository<TEntity> where TEntity : class {
    protected DbContext _context;
    protected DbSet<TEntity> _set;

    protected ARepository(DbContext context) {
        _context = context;
        _set = _context.Set<TEntity>();
    }

    public TEntity Read(params object[] keys) => _set.Find(keys);

    public IEnumerable<TEntity> ReadAll() => _set.ToList();

    public IEnumerable<TEntity> ReadAll(int start, int count) => _set.Skip(start).Take(count).ToList();

    public IEnumerable<TEntity> Filter(Expression<Func<TEntity, bool>> filter) => _set.AsQueryable<TEntity>().Where(filter).ToList();

    public TEntity Create(TEntity entity) {
        _set.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public void Update(TEntity entity) {
        if (entity == null) return;
        _set.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(params object[] keys) => Delete(_set.Find(keys));

    public void Delete(TEntity entity) {
        if (entity == null) return;
        _set.Remove(entity);
        _context.SaveChanges();
    }
}
