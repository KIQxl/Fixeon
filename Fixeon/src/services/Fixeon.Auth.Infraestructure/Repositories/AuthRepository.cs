using Fixeon.Auth.Application.Dtos.Requests;
using Fixeon.Auth.Application.Dtos.Responses;
using Fixeon.Auth.Application.Interfaces;
using Fixeon.Auth.Infraestructure.Data;
using Fixeon.Auth.Infraestructure.Entities;
using Fixeon.Shared.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fixeon.Auth.Infraestructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _context;
        private readonly ITenantProvider _tenantProvider;

        public AuthRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ITenantProvider tenantProvider, DataContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tenantProvider = tenantProvider;
            _context = context;
        }

        public async Task<ApplicationUserResponse> GetUser(string email)
        {
            try
            {
                var tenantId = _tenantProvider.GetTenantId();

                var user = await _context.users.FirstOrDefaultAsync(x => x.Email.Equals(email) && x.CompanyId.Equals(tenantId));

                if (user == null)
                    return new ApplicationUserResponse(new List<string> { "Usuário não encontrado." });

                var roles = await _userManager.GetRolesAsync(user);

                return new ApplicationUserResponse(user.Id, user.UserName, user.Email, roles);
            }
            catch (Exception ex)
            {
                return new ApplicationUserResponse(new List<string> { $"Ocorreu um erro ao buscar pelo usuário.", $"{ex.Message}." });
            }
        }

        public async Task<List<ApplicationUserResponse>> GetAllUsers()
        {
            try
            {
                var tenantId = _tenantProvider.GetTenantId();

                var users = await _context.users.Where(x => x.CompanyId == tenantId).ToListAsync();

                if (users == null)
                    return null;

                var appUsers = new List<ApplicationUserResponse>();

                foreach (var u in users)
                {
                    var roles = await _userManager.GetRolesAsync(u);

                    var appUser = new ApplicationUserResponse(u.Id, u.UserName, u.Email, roles);

                    appUsers.Add(appUser);
                }
                ;


                return appUsers;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ApplicationUserResponse> CreateAccount(CreateAccountRequest request)
        {
            var tenantId = _tenantProvider.GetTenantId();

            var IdentityUser = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Email,
                EmailConfirmed = true,
                CompanyId = tenantId
            };

            var result = await _userManager.CreateAsync(IdentityUser, request.Password);

            if (result.Succeeded)
            {
                var user = await _context.users.FirstOrDefaultAsync(x => x.Email == request.Email && x.CompanyId == tenantId);
                var roles = await _userManager.GetRolesAsync(user);

                return new ApplicationUserResponse(user.Id, user.UserName, user.Email, roles);
            }

            return new ApplicationUserResponse(result.Errors.Select(e => e.Description).ToList());
        }

        public async Task<ApplicationUserResponse> Login(string email, string password)
        {
            var tenantId = _tenantProvider.GetTenantId();

            var user = await _context.users.FirstOrDefaultAsync(x => x.Email == email && x.CompanyId == tenantId);

            var loginResult = await _signInManager.PasswordSignInAsync(user, password, true, true);

            if (loginResult.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);

                return new ApplicationUserResponse(user.Id, user.UserName, user.Email, roles);
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

        public async Task<ApplicationUserResponse> AssociateRole(string userId, string role)
        {
            try
            {
                var tenantId = _tenantProvider.GetTenantId();

                var user = await _context.users.FirstOrDefaultAsync(x => x.Id == userId && x.CompanyId == tenantId);

                if (user != null)
                {
                    var roleExists = await _roleManager.RoleExistsAsync(role);
                    if (roleExists)
                    {
                        await _userManager.AddToRoleAsync(user, role);
                        var userRoles = await _userManager.GetRolesAsync(user);

                        return new ApplicationUserResponse(user.Id, user.UserName, user.Email, userRoles);
                    }

                    return new ApplicationUserResponse(new List<string> { $"O perfil informado não existe." });
                }

                return new ApplicationUserResponse(new List<string> { $"Não foi possivel associar o perfil: {role} ao usuário.", "Usuário não encontrado." });
            }
            catch (Exception ex)
            {
                return new ApplicationUserResponse(new List<string> { $"Ocorreu um erro ao associar o perfil ao usuário.", $"{ex.Message}." });
            }
        }

        public async Task<bool> FindByEmail(string email)
        {
            try
            {
                var tenantId = _tenantProvider.GetTenantId();

                var user = await _context.users.FirstOrDefaultAsync(x => x.Email == email && x.CompanyId == tenantId);

                if (user is null)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<string> GenerateResetPasswordToken(string email)
        {
            var tenantId = _tenantProvider.GetTenantId();

            var user = await _context.users.FirstOrDefaultAsync(x => x.Email == email && x.CompanyId == tenantId);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return token;
        }

        public async Task<ApplicationUserResponse> ResetPassword(ResetPasswordRequest request)
        {
            var tenantId = _tenantProvider.GetTenantId();

            var user = await _context.users.FirstOrDefaultAsync(x => x.Email == request.Email && x.CompanyId == tenantId);

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

            if (result.Succeeded)
                return new ApplicationUserResponse(user.Id, user.UserName, user.Email, new List<string>());

            return new ApplicationUserResponse(result
                .Errors
                .Select(e => e.Description)
                .ToList());
        }

        public async Task<List<ApplicationUserResponse>> GetUsersByRoleName(string roleName)
        {
            try
            {
                var tenantId = _tenantProvider.GetTenantId();

                var users = await _userManager.GetUsersInRoleAsync(roleName);

                if (users == null)
                    return null;

                var appUsers = await Task.WhenAll(users.Select(async u =>
                {
                    var roles = await _userManager.GetRolesAsync(u);

                    return new ApplicationUserResponse(u.Id, u.UserName, u.Email, roles);
                }));
                
                return appUsers.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
