using Authorization.Dal;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Authorization.UserRepository
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        IEnumerable<User> GetAll();
        void UpdateUser(User user);
        User GetUser();
        void SaveChanges();
        Task SaveChangeAsync();
        IEnumerable<User> GetAllUsers();
        Task<IQueryable<User>> GetAllAsync(Expression<Func<User, bool>> predicate = null,
            Func<IQueryable<User>, IIncludableQueryable<User, object>> includes = null);
    }
}