
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppServiceForHotel.UserService
{
    public interface IUserService
    {
        string Register(RegistrationRequest model);
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        //string GenerateJwtToken(User user);
    }
}
