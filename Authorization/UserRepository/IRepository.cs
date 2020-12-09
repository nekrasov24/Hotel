using Authorization.Dal;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Authorization.UserRepository
{
    public interface IRepository<T, Tkey> where T : class where Tkey : struct
    { 
        Task AddUserAsync(T obj);

        Task UpdateUser(T obj);

        Task SaveChangeAsync();

        Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
        public User GetUserById(Tkey id);
    }
}