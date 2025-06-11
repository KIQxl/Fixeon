using Fixeon.Auth.Application.Dtos;
using Fixeon.Auth.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fixeon.Auth.Infraestructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<ApplicationUser> GetUser(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                    return new ApplicationUser(new List<string> { "Usuário não encontrado." });

                var roles = await _userManager.GetRolesAsync(user);

                return new ApplicationUser(user.Id, user.UserName, user.Email, roles);
            }
            catch (Exception ex)
            {
                return new ApplicationUser(new List<string> { $"Ocorreu um erro ao buscar pelo usuário.", $"{ex.Message}." });
            }
        }

        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();

                if (users == null)
                    return null;

                var appUsers = new List<ApplicationUser>();

                foreach(var u in users)
                {
                    var roles = await _userManager.GetRolesAsync(u);

                    var appUser = new ApplicationUser(u.Id, u.UserName, u.Email, roles);

                    appUsers.Add(appUser);
                };


                return appUsers;
            }
            catch (Exception ex)
            {
                return null;
            }
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

        public async Task<bool> CreateRole(string role)
        {
            try
            {
                var exists = await _roleManager.RoleExistsAsync(role);

                if (!exists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<ApplicationUser> AssociateRole(string userId, string role)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if(user != null)
                {
                    var roleExists = await _roleManager.RoleExistsAsync(role);
                    if (roleExists)
                    {
                        await _userManager.AddToRoleAsync(user, role);
                        var userRoles = await _userManager.GetRolesAsync(user);

                        return new ApplicationUser(user.Id, user.UserName, user.Email, userRoles);
                    }

                    return new ApplicationUser(new List<string> { $"O perfil informado não existe." });
                }

                return new ApplicationUser(new List<string> { $"Não foi possivel associar o perfil: {role} ao usuário.", "Usuário não encontrado." });
            }
            catch (Exception ex)
            {
                return new ApplicationUser(new List<string> { $"Ocorreu um erro ao associar o perfil ao usuário.", $"{ex.Message}." });
            }
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
