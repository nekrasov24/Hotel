using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.UserService
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(30, ErrorMessage = "Password is required", MinimumLength = 4)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(30, ErrorMessage = "Password isn't confirm", MinimumLength = 4)]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string PasswordConfirm { get; set; }


    }
}
