using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        Task AddAsync(TEntity obj);
        IEnumerable<TEntity> GetAll();
        Task Update(TEntity obj);
        Task SaveChangeAsync();
        Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null);
        TEntity GetById(TKey id);
        Task Delete(TKey id);
    }
}
