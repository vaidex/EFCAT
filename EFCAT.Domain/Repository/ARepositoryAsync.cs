using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EFCAT.Domain.Repository;

public abstract class ARepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class {
    protected DbContext _context;
    protected DbSet<TEntity> _set;

    protected ARepositoryAsync(DbContext context) {
        _context = context;
        _set = _context.Set<TEntity>();
    }

    public async Task<TEntity> ReadAsync(params object[] keys) => await _set.FindAsync(keys);

    public async Task<IEnumerable<TEntity>> ReadAllAsync() => await _set.ToListAsync();

    public async Task<IEnumerable<TEntity>> ReadAllAsync(int start, int count) => await _set.Skip(start).Take(count).ToListAsync();

    public async Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> filter) => await _set.AsQueryable<TEntity>().Where(filter).ToListAsync();

    public async Task<TEntity> CreateAsync(TEntity entity) {
        _set.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(TEntity entity) {
        if (entity == null) return;
        _set.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(params object[] keys) => await DeleteAsync(await ReadAsync(keys));

    public async Task DeleteAsync(TEntity entity) {
        if (entity == null) return;
        _set.Remove(entity);
        await _context.SaveChangesAsync();
    }
}