using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.UserService
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is incorrect")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.DateTime)]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(30, ErrorMessage = "Password is short", MinimumLength = 4)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string PasswordConfirm { get; set; }


    }
}
