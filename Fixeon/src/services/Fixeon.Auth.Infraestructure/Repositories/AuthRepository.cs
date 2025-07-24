using Fixeon.Auth.Infraestructure.Data;
using Fixeon.Auth.Infraestructure.Entities;
using Fixeon.Auth.Infraestructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fixeon.Auth.Infraestructure.Repositories
{
    public class AuthRepository : IIdentityRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _dataContext;

        public AuthRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, DataContext dataContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _dataContext = dataContext;
        }

        public async Task<IdentityResult> AssociateRole(ApplicationUser user, string roleName)
        {
            try
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IdentityResult> CreateAccount(ApplicationUser request, string password, bool ignoreTenantInterceptor = false)
        {
            try
            {
                if (ignoreTenantInterceptor)
                    _dataContext.IgnoreTenantInterceptor = true;

                return await _userManager.CreateAsync(request, password);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IdentityResult> CreateRole(string roleName)
        {
            try
            {
                return await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApplicationUser> FindByEmail(string email)
        {
            try
            {
                var user = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GenerateResetPasswordToken(ApplicationUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return token;
        }

        public async Task<List<ApplicationUser>> GetAllUsers(bool masterAdmin)
        {
            if(masterAdmin)
                return await _userManager.Users.IgnoreQueryFilters().AsNoTracking().ToListAsync();

            return await _userManager.Users.AsNoTracking().ToListAsync();
        }

        public async Task<ApplicationUser> GetUser(string email)
        {
            return await FindByEmail(email);
        }

        public async Task<List<ApplicationUser>> GetUsersByRoleName(string roleName)
        {
            var result = await _userManager.GetUsersInRoleAsync(roleName);

            return result.ToList();
        }

        public async Task<SignInResult> Login(ApplicationUser user, string password)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(user, password, false, true);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IdentityResult> ResetPassword(ApplicationUser user, string token, string newPassword)
        {
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            return result;
        }

        public async Task<List<string>> GetRolesByUser(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            return roles.ToList();
        }

        public async Task<IdentityRole> GetRole(string roleName)
        {
            try
            {
                return await _roleManager.FindByNameAsync(roleName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApplicationUser> FindById(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApplicationUser> FindByEmailWithoutFilter(string email)
        {
            try
            {
                var user = await _userManager.Users.AsNoTracking().IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Email == email);

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
