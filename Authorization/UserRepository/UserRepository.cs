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
            _userContext.Users.Add(user);
            await SaveChangeAsync();
        }

        /*public IEnumerable<User> GetAll()
        {
            return _userContext.Users.ToArray();
        }*/

        public async Task UpdateUser(User user)
        {
            _userContext.Users.Update(user);
            await SaveChangeAsync();
        }
        public async Task SaveChangeAsync()
        {
            await _userContext.SaveChangesAsync();

        }
   
        public async Task<IQueryable<User>> GetAllAsync(Expression<Func<User, bool>> predicate = null,
            Func<IQueryable<User>, IIncludableQueryable<User, object>> includes = null)
        {
           
            
                var result = _userContext.Users.AsQueryable();

                if (includes != null)
                    result = includes(result);

                return await Task.FromResult(predicate != null ? result.Where(predicate) : result);
        }

        public User GetUserById(Guid id)
        {
            return _userContext.Users.FirstOrDefault(x => x.Id.Equals(id));
        }
    }
}
