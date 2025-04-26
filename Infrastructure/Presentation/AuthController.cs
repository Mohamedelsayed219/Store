using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IServiceManager serviceManager) : ControllerBase
    {

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto) 
        {
            var result = await serviceManager.AuthService.LoginAsunc(loginDto);

            return Ok(result);

        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await serviceManager.AuthService.RegisterAsunc(registerDto);

            return Ok(result);

        }



    }
}
