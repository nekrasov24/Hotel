using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.UserService
{
    public class AuthenticateException : Exception
    {
        public AuthenticateException(string message) : base(message)
        {

        }
    }
}
