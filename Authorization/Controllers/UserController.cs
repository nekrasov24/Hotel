using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authorization.HeaderService;
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
                       
                return Ok(response);
            }
            catch(AuthenticateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message/*"Something went wrong"*/);
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
            catch(RegisterException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var obUsers = _userService.GetUser(id);
            return Ok(obUsers);
        }

        [HttpPut("edituser")]
        public async Task<IActionResult> EditRoomAsync(EditUserRequestModel model)
        {
            try
            {
                var resp = await _userService.EditUserAsync(model);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            try
            {
                var resp = "log out";
                return Ok(resp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("topup")]
        public async Task<IActionResult> TopUp(BalanceModel summ)
        {
            try
            {
                var response = await _userService.TopUp(summ);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
