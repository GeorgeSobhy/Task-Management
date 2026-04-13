using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.BusinessLogic.DTOs;
using TaskManagement.Infrastructure.Auth;

namespace Task_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var token = await _authService.LoginAsync(request);

            return Ok(new
            {
                message = "Login successful",
                token
            });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var token = await _authService.RegisterAsync(request);

            return Ok(new
            {
                message = "User registered successfully",
                token
            });
        }
    }
}
