using Authorization.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Authorization.UserRepository
{
    public class Repository<T, Tkey> : IRepository<T, Tkey> where T : class where Tkey : struct
    {
        private readonly UserContext _userContext;
        private readonly DbSet<T> _dbSet;

        public Repository(UserContext userContext)
        {
            _userContext = userContext;
            _dbSet = _userContext.Set<T>();
        }

        public async Task AddUserAsync(T obj)
        {
            _dbSet.Add(obj);
            await SaveChangeAsync();
        }

        public async Task EditUser(T obj)
        {
            _dbSet.Update(obj);
            await SaveChangeAsync();
        }
        public async Task SaveChangeAsync()
        {
            await _userContext.SaveChangesAsync();

        }
   
        public async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
           
            
                var result = _dbSet.AsQueryable();

                if (includes != null)
                    result = includes(result);

                return await Task.FromResult(predicate != null ? result.Where(predicate) : result);
        }

        public User GetUserById(Tkey id)
        {
            return _userContext.Users.FirstOrDefault(x => x.Id.Equals(id));
        }
    }
}
