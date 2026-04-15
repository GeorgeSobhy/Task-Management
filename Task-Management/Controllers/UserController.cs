using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.BusinessLogic.DTOs;
using TaskManagement.Domain.Entities.Identity;
using TaskManagement.Infrastructure.Auth;
namespace TaskManagement.API.Controllers
{

    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin/users")]
    public class AdminUsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthService _authService;

        public AdminUsersController(UserManager<ApplicationUser> userManager, IAuthService authService, Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _authService = authService;
            _roleManager = roleManager;
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
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {

            var users = _userManager.Users
                .Select(u => new
                {
                    u.Id,
                    u.Email,
                    u.UserName,
                    u.EmailConfirmed
                })
                .ToList();


            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound("User not found");

            return Ok(new
            {
                user.Id,
                user.Email,
                user.UserName,
                user.EmailConfirmed
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound("User not found");

            user.IsDeleted = true;
            user.DeletionDate = DateTime.Now;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("User deleted successfully");
        }



        [HttpPut("{id}/role")]
        public async Task<IActionResult> AssignRole(string id, [FromBody] string role)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound("User not found");

            if(await _roleManager.FindByNameAsync(role) == null)
                return NotFound("Role not found");

            var result = await _userManager.AddToRoleAsync(user, role);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok($"Role '{role}' assigned");
        }
    }
}
