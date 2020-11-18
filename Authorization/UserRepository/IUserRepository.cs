using Authorization.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.UserRepository
{
    public interface IUserRepository
    {
        Task AddUser(User user);
        IEnumerable<User> GetAll();
        void UpdateUser(User user);
        User GetUser();
        int SaveChanges();
    }
}
