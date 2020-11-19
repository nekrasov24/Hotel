using Authorization.Dal;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Authorization.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _userContext;

        public UserRepository(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task AddUserAsync(User user)
        {
            await _userContext.Users.AddAsync(user);
            await SaveChangeAsync();
        }

        public IEnumerable<User> GetAll()
        {
            return _userContext.Users.ToArray();
        }

        public void UpdateUser(User user)
        {
            _userContext.Users.Update(user);
        }

        public User GetUser()
        {
            return _userContext.Users.FirstOrDefault();
        }

        public void SaveChanges()
        {
             _userContext.SaveChanges();
        }
        public async Task SaveChangeAsync()
        {
            await _userContext.SaveChangesAsync();

        }
        public IEnumerable<User> GetAllUsers()
        {
            return _userContext.Users.ToArray();     
        }

        public async Task<IQueryable<User>> GetAllAsync(Expression<Func<User, bool>> predicate = null,
            Func<IQueryable<User>, IIncludableQueryable<User, object>> includes = null)
        {
            return await Task.Run(() =>
            {
                var result = _userContext.Users.AsQueryable();

                if (includes != null)
                    result = includes(result);

                return predicate != null ? result.Where(predicate) : result;
            });
        }
    }
}
