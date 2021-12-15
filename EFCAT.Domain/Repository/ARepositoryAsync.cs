using EFCAT.Model.Annotation;
using EFCAT.Model.Configuration;
using EFCAT.Util;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.ComponentModel.DataAnnotations;

namespace EFCAT.Domain.Repository;

public abstract class ARepositoryAsync<TEntity, TKey> : IRepositoryAsync<TEntity, TKey> where TEntity : class {
    protected DatabaseContext _context;
    protected DbSet<TEntity> _entitySet;

    protected ARepositoryAsync(DatabaseContext context) {
        _context = context;
        _entitySet = _context.Set<TEntity>();
    }

    public async Task<TEntity> ReadAsync(TKey Id) => await _entitySet.FindAsync(Id);

    public async Task<IEnumerable<TEntity>> ReadAllAsync() => await _entitySet.ToListAsync();

    public async Task<IEnumerable<TEntity>> ReadAllAsync(int start, int count) => await _entitySet.Skip(start).Take(count).ToListAsync();

    public async Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> filter) => await _entitySet.AsQueryable<TEntity>().Where(filter).ToListAsync();

    public async Task<TEntity> CreateAsync(TEntity entity) {
        foreach(var property in entity.GetType().GetProperties()) {
            if (property.HasAttribute<UniqueAttribute>()) if (await _entitySet.Where($"{property.Name} == @0", property.GetValue(entity)).CountAsync() > 0) throw new ValidationException(property.GetAttribute<UniqueAttribute>().ErrorMessage == null ? $"{property.Name} needs to be unique." : property.GetAttribute<UniqueAttribute>().ErrorMessage);
        }
        _entitySet.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(TEntity entity) {
        _entitySet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TKey Id) => await DeleteAsync(await ReadAsync(Id));

    public async Task DeleteAsync(TEntity entity) {
        _entitySet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}