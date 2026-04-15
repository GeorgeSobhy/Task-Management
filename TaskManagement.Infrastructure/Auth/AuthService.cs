using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TaskManagement.BusinessLogic.DTOs;
using TaskManagement.Domain.Entities.Identity;
using TaskManagement.Infrastructure.JWT;
using TaskManagement.Shared.Enums;
using TaskManagement.Shared.Exceptions;

namespace TaskManagement.Infrastructure.Auth
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto request);
        Task<string> LoginAsync(LoginDto request);
    }

    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtService _jwtService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            JwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        public async Task<string> RegisterAsync(RegisterDto request)
        {

            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
                throw new UserAlreadyExistsException(request.Email);

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new GenericException(errors, StatusCodes.Status400BadRequest);
            }

            return _jwtService.GenerateToken(user.Id, user.Email!,new List<string>() { DefaultRoles.NewUser.ToString() });
        }

        public async Task<string> LoginAsync(LoginDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                throw new InvalidCredentialsException();

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                request.Password,
                false
            );

            if (!result.Succeeded)
                throw new InvalidCredentialsException();

            var roles = await _userManager.GetRolesAsync(user);
            return _jwtService.GenerateToken(user.Id, user.Email!, roles);
        }
    }
}
