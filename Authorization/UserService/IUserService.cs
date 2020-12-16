using Authorization.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.UserService
{
    public interface IUserService
    {
        Task<string> Register(RegisterRequest model);
        string RegisterAdmin(RegisterRequest model);
        Task<string> Authenticate(AuthenticateRequest model);
        ProfileModel GetUser(Guid id);
        Task<string> EditUserAsync(EditUserRequestModel model);
    }
}
