﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAppServiceForHotel.UserService;

namespace WebAppServiceForHotel.Controlles
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
        public IActionResult Authenticate([FromBody] AuthenticateRequest model)
        {
            try
            {
                var response = _userService.Authenticate(model);
                if (response == null)
                    return BadRequest(new { message = "Email or password is incorrect" });
                return Ok(response);
            }
            catch
            {
                throw;
            }

        }
        [HttpPost("/register")]
        public IActionResult Register([FromBody] RegistrationRequest model)
        {
            try
            {
                var response = _userService.Register(model);
                if (response == null)
                    return BadRequest(new { message = "User already exists" });
                return Ok(response);
            }
            catch
            {
                throw;
            }
        }
    }
}
