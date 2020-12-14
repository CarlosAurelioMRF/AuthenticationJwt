using AuthenticationJwt.Domain.Dtos;
using AuthenticationJwt.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AuthenticationJwt.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register(UserForRegisterDto userForRegister)
        {
            var userId = _authService.Register(userForRegister);

            return Ok(new { userId });
        }

        [HttpPost("login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {
            var accessToken = _authService.Login(userForLogin);

            if (string.IsNullOrEmpty(accessToken))
                    return Unauthorized(new { message = "Credentials invalid" } );
            
            return Ok(new { accessToken });
        }
    }
}
