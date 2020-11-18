using Authorization.Dal;
using Authorization.UserRepository;
using Elskom.Generic.Libs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.UserService
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
        public string Register(RegisterRequest model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.FirstName))
                    return null;
                var adminEmail = "admin@gmail.com";
                var adminFirstName = "admin22";
                if (model.Email == adminEmail && model.Password == adminFirstName)
                {
                    var newAdminUser = new User()
                    {
                        Id = Guid.NewGuid(),
                        Password = Encrypt(adminFirstName),
                        Email = adminEmail,
                        Roles = Roles.Admin,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        DateOfBirth = model.DateOfBirth
                    };
                    _userRepository.AddUser(newAdminUser);
                    _userRepository.SaveChanges();
                    var adminToken = GenerateJwtToken(newAdminUser);
                    return adminToken;
                }
                var findUser = _userRepository.GetAll();
                var user = findUser.FirstOrDefault(u => u.Email == model.Email && u.FirstName == model.FirstName);
                if (user != null) return null;
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
        public string RegisterAdmin(RegisterRequest model)
        {
            try
            {
                var adminEmail = "admin@gmail.com";
                var adminPassword = "admin22";
                var findUser = _userRepository.GetAll();
                var user = findUser.FirstOrDefault(u => u.Email == model.Email && u.FirstName == model.FirstName);
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
        public string Authenticate(AuthenticateRequest model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Email))                
                return null;    
                var findUser = _userRepository.GetAll();
                var user = findUser.FirstOrDefault(u => u.Email == model.Email && u.FirstName == model.FirstName);
                if (user == null) return null;
                var token = GenerateJwtToken(user);
                return token;
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
            
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hash = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return hash;
        }
    }
}
