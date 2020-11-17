

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAppServiceForHotel.DAL;

namespace WebAppServiceForHotel.UserRepository
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
