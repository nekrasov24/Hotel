using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class 
    {
        private readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();

        }

        public async Task AddAsync(TEntity obj)
        {
            _dbSet.Add(obj);
            await SaveChangeAsync();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToArray();
        }

        public async Task Update(TEntity obj)
        {
            _dbSet.Update(obj);
            await SaveChangeAsync();
        }
        public Task SaveChangeAsync()
        {
            return _context.SaveChangesAsync();
        }

        public async Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null)
        {


            var result = _dbSet.AsQueryable();

            if (includes != null)
                result = includes(result);

            return await Task.FromResult(predicate != null ? result.Where(predicate) : result);
        }

        public TEntity GetById(TKey id)
        {
            var obj =  _dbSet.Find(id);
            return obj;
        }

        public async Task Delete(TKey id)
        {
            var findRoom = await _dbSet.FindAsync(id);
             _dbSet.Remove(findRoom);
            

        }
    }
}
