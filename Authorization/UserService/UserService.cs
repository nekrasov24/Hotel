using Authorization.Constants;
using Authorization.Dal;
using Authorization.HeaderService;
using Authorization.Publisher;
using Authorization.Subscriber;
using Authorization.UserRepository;
using AutoMapper;
using Elskom.Generic.Libs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IHeaderService _headerService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IPublisher _publisher;
        private readonly ILogger<UserService> _logger;
        private const decimal accountBalance = 0.00m;
        public UserService(IRepository<User, Guid> repository, IConfiguration configuration, IMapper mapper, IHeaderService headerService,
            IPublisher publisher, ILogger<UserService> logger)
        {
            _userRepository = repository;
            _configuration = configuration;
            _headerService = headerService;
            _mapper = mapper;
            _publisher = publisher;
            _logger = logger;
        }
        public async Task<string> Register(RegisterRequest model)
        {
            try
            {
                
                var user = (await _userRepository.GetAllAsync(u => u.Email.ToUpper().Equals(model.Email.ToUpper()))).FirstOrDefault();
                if (user != null) throw new Exception("User already exists");



                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    Password = Encrypt(model.Password),
                    Email = model.Email,
                    Roles = Roles.User,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    AccountBalance = accountBalance
                };

                await _userRepository.AddUserAsync(newUser);

                var token = GenerateJwtToken(newUser);
                var mes = $" {newUser.FirstName} {newUser.LastName} registred successfully";
                await _publisher.Publish(mes);
                return token;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public string RegisterAdmin(RegisterRequest model)
        {
            try
            {

                var newUser = new User()
                {
                    Id = Guid.NewGuid(),
                    Password = Encrypt(model.Password),
                    Email = model.Email,
                    Roles = Roles.Admin,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    AccountBalance = accountBalance
                };
                _userRepository.AddUserAsync(newUser);
                var token = GenerateJwtToken(newUser);
                return token;
            }
            catch
            {
                throw;
            }
        }
        public async Task<string> Authenticate(AuthenticateRequest model)
        {
            try
            {
                
                var user = (await _userRepository.GetAllAsync(u => u.Email.ToUpper().Equals(model.Email.ToUpper()))).FirstOrDefault();
                if (user == null) throw new Exception("Email or Password is incorrect");
                var verified = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);
                if (!verified) throw new Exception("Email or Password is incorrect");

                var token = GenerateJwtToken(user);
                var mes = $" Id: {user.Id}: {user.FirstName} {user.Email} logged successfully";
                await _publisher.Publish(mes);
                return token;
            }
            catch(Exception ex)
            {
                throw ex;
            }
                
            


        }
        private string GenerateJwtToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, userInfo.FirstName),
            new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
            new Claim(JwtRegisteredClaimNames.Jti, userInfo.Id.ToString()),
            new Claim("scope", userInfo.Roles.ToString())
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

        public ProfileModel GetUser(Guid id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null) throw new Exception($"User was not found");
            var profileModel = new ProfileModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth.GetValueOrDefault().ToString("yyyy-MM-dd"),
                Email = user.Email,
                AccountBalance = user.AccountBalance

            };
            return  profileModel;
        }

        public async Task<string> EditUserAsync(EditUserRequestModel model)
        {
            try
            {
                var userId = _headerService.GetUserId();
                var user = _userRepository.GetUserById(userId);

                if (user == null) throw new Exception("User was not found");

                _mapper.Map(model, user);

                await _userRepository.EditUser(user);
                return "User was edited successfully";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> PayRoom(PaymentModel model)
        {
            try
            {
                var userId = Guid.Parse(model.UserId);


                var amountPaid = Convert.ToDecimal(model.AmountPaid);

                var findUser = _userRepository.GetUserById(userId);

                if (findUser == null)
                {
                    var mmm = "rtrtrtrtrtrtr";
                    return mmm;
                }

                var accountBalance = findUser.AccountBalance;

                if (amountPaid > accountBalance)
                {
                    var errMes = "I'm sorry, but it appears your account has insufficient funds";

                    return errMes;
                }

                accountBalance -= amountPaid;
                findUser.AccountBalance = accountBalance;
                await _userRepository.EditUser(findUser);

                var mes = "Payment has been successfully";
                return mes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail UserService");
                throw;
            }

        }

        public async Task<string> TopUp(BalanceModel summ)
        {
            try
            {
                var userId = _headerService.GetUserId();

                var findUser = _userRepository.GetUserById(userId);
                var userBalance = findUser.AccountBalance;
                var topUpBalance = summ.Balance;

                if (topUpBalance > 0)
                {
                    userBalance += topUpBalance;
                    findUser.AccountBalance = userBalance;
                    await _userRepository.EditUser(findUser);
                    return "Your balance has been credited successfully";
                }
                throw new Exception("Top Up amount can't negative");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

        }
    }
}