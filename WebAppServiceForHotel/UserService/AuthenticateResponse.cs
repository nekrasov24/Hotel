
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppServiceForHotel.UserService
{
    public class AuthenticateResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string JwtToken { get; set; }
        public AuthenticateResponse(User user, string jwtToken)
        {
            Id = user.Id;
            Name = user.FirstName;
            JwtToken = jwtToken;
        }
    }
}
