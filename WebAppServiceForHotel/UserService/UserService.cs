


using Elskom.Generic.Libs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAppServiceForHotel.DAL;
using WebAppServiceForHotel.UserRepository;

namespace WebAppServiceForHotel.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        public string Register(RegistrationRequest model)
        {
            try
            {
                var findUser = _userRepository.GetAll();
                var user = findUser.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                //if (user != null) return null;
                var newUser = new User()
                {
                    Id = Guid.NewGuid(),
                    Password = Encrypt(model.Password),
                    Email = model.Email,
                    Roles = Roles.User,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth
                }; 
                _userRepository.AddUser(newUser);
                _userRepository.SaveChanges();
                var token = GenerateJwtToken(newUser);
                return token;
            }
            catch
            {
                throw;
            }
        }
        public string RegisterAdmin(RegistrationRequest model)
        {
            try
            {
                var adminEmail = "admin@gmail.com";
                var adminPassword = "admin22";
                var findUser = _userRepository.GetAll();
                var user = findUser.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null) return null;

                var newUser = new User()
                {
                    Id = Guid.NewGuid(),
                    Password = Encrypt(adminPassword),
                    Email = adminEmail,
                    Roles = Roles.Admin,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth
                };
                _userRepository.AddUser(newUser);
                var token = GenerateJwtToken(newUser);
                return token;
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
        private string GenerateJwtToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, userInfo.FirstName),
            new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string Encrypt(string password)
        {
            var cryptKey = "80";
            BlowFish blow = new BlowFish(cryptKey);
            string encrypt = blow.EncryptCBC(password);
            return password;
        }
    }
}
