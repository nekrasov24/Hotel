using Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserRepository;

namespace WebAppServiceForHotel.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserContext _userContext;
        private const string Key = "5DBA5C7DBF8C4EB3B74E185AF5C26188";

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public AuthenticateResponse Register(RegistrationRequest model)
        {
            try
            {
                var findUser = _userRepository.GetAll();
                var user = findUser.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null) return null;
                var newUser = new User()
                {
                    Id = Guid.NewGuid(),
                    Password = model.Password,
                    Email = model.Email
                };

                var role = _userContext.Roles.FirstOrDefault(r => r.Name.Equals("User"));
                _userContext.UserRoles.Add(new UserRole()
                { Id = Guid.NewGuid(), Users = newUser, Roles = role});

                
                _userRepository.AddUser(newUser);
                var token = GenerateJwtToken(newUser);
                return new AuthenticateResponse(user, token);
            }
            catch
            {
                throw;
            }
        }
        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            try
            {
                var findUser = _userRepository.GetAll();
                var user = findUser.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                if (user == null) return null;
                var token = GenerateJwtToken(user);
                return new AuthenticateResponse(user, token);
            }
            catch
            {
                throw;
            }

        }
        private string GenerateJwtToken(User user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Key);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.FirstName),
                };

                var claimsIdentity = new ClaimsIdentity(claims);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claimsIdentity,
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch
            {
                throw;
            }
        }
    }
}
