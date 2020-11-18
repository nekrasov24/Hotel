using Authorization.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task AddUser(User user)
        {
            await _userContext.Users.AddAsync(user);
        }

        public IEnumerable<User> GetAll()
        {
            return _userContext.Users.ToList();
        }

        public void UpdateUser(User user)
        {
            _userContext.Users.Update(user);
        }

        public User GetUser()
        {
            return _userContext.Users.FirstOrDefault();
        }

        public int SaveChanges()
        {
            return _userContext.SaveChanges();
        }
    }
}
