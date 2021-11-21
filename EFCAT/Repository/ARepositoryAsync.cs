using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace EFCAT.Repository {
    public abstract class ARepositoryAsync<TEntity, TKey> : IRepositoryAsync<TEntity, TKey> where TEntity : class {
        protected DbContext _context;
        protected DbSet<TEntity> _entitySet;

        protected ARepositoryAsync(DbContext context) {
            _context = context;
            _entitySet = _context.Set<TEntity>();
        }

        public async Task<TEntity> ReadAsync(TKey Id) => await _entitySet.FindAsync(Id);

        public async Task<IEnumerable<TEntity>> ReadAllAsync() =>  await _entitySet.ToListAsync();

        public async Task<IEnumerable<TEntity>> ReadAllAsync(int start, int count) => await _entitySet.Skip(start).Take(count).ToListAsync();

        public async Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> filter) => await _entitySet.AsQueryable<TEntity>().Where(filter).ToListAsync();

        public async Task<TEntity> CreateAsync(TEntity entity) {
            _entitySet.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(TEntity entity) {
            _entitySet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TKey Id) => DeleteAsync(await ReadAsync(Id));
        
        public async Task DeleteAsync(TEntity entity) {
            _entitySet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
