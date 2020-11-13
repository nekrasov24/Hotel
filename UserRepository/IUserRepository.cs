
using DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserRepository
{
    public interface IUserRepository
    {
        Task AddUser(User user);
        IEnumerable<User> GetAll();
        void UpdateUser(User user);
        User GetUser();
    }
}
