using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authorization.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {
            try
            {
                var response = await _userService.Authenticate(model);
                if (response == null)
                    return BadRequest(new { message = "Email or password is incorrect" });
                return Ok(response);
            }
            catch
            {
                throw;
            }

        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            try
            {
                var response = await _userService.Register(model);
                if (response == null)
                    return BadRequest(new { message = "User already exists" });
                return Ok(response);
            }
            catch
            {
                throw;
            }
        }
        [HttpGet("getUsers")]
        public IActionResult Get()
        {
            var obUsers = _userService.GetUsers();
            return Ok(obUsers);
        }
    }
}
