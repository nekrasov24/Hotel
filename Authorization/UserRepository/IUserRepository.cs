using Authorization.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.UserRepository
{
    public interface IUserRepository : IRepository<User, Guid> 
    {

    }
}
