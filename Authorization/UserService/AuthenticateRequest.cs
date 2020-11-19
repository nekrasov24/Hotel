using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.UserService
{
    public class AuthenticateRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Email is not valid+")]
        public string Email { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Password is required", MinimumLength = 4)]
        public string Password { get; set; }
    }
}
