using Azure.Core;
using Fixeon.Auth.Application.Dtos;
using Fixeon.Auth.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Fixeon.Auth.Infraestructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ApplicationUser> CreateAccount(CreateAccountRequest request)
        {
            var IdentityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(IdentityUser, request.Password);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                var roles = await _userManager.GetRolesAsync(user);

                return new ApplicationUser(user.Id, user.UserName, user.Email, roles);
            }

            return new ApplicationUser(result.Errors.Select(e => e.Description).ToList());
        }

        public async Task<ApplicationUser> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var loginResult = await _signInManager.PasswordSignInAsync(user, password, true, true);

            if (loginResult.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);

                return new ApplicationUser(user.Id, user.UserName, user.Email, roles);
            }

            return null;
        }

        public async Task<bool> FindByEmail(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user is null)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
