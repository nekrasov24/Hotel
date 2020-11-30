using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.UserService
{
    public class RegisterException : Exception
    {
        public RegisterException(string message) : base(message)
        {

        }
    }
}
