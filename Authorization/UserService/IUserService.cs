using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.UserService
{
    public interface IUserService
    {
        string Register(RegisterRequest model);
        string Authenticate(AuthenticateRequest model);
        //string GenerateJwtToken(User user);
    }
}
