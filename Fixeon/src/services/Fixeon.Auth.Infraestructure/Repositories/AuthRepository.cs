using Fixeon.Auth.Infraestructure.Data;
using Fixeon.Auth.Infraestructure.Entities;
using Fixeon.Auth.Infraestructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        public async Task<IdentityResult> AssociateRoles(ApplicationUser user, List<string> roles)
        {
            try
            {
                var result = await _userManager.AddToRolesAsync(user, roles);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IdentityResult> RemoveRoles(ApplicationUser user, List<string> roles)
        {
            try
            {
                var result = await _userManager.RemoveFromRolesAsync(user, roles);

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

        public async Task<IdentityResult> UpdateAccount(ApplicationUser applicationUser)
        {
            try
            {
                var result = await _userManager.UpdateAsync(applicationUser);
                return result;
            }
            catch(Exception ex)
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

        public async Task<List<ApplicationUser>> GetAllUsers(bool masterAdmin, Guid? id, string? email, Guid? organization, string? username)
        {
            if(masterAdmin)
                return await _userManager.Users.IgnoreQueryFilters().AsNoTracking().ToListAsync();

            var query = _userManager.Users.AsQueryable();

            if (id.HasValue)
                query = query.Where(u => u.Id == id.Value.ToString());

            if (!string.IsNullOrEmpty(email))
                query = query.Where(u => u.Email == email);

            if (organization.HasValue)
                query = query.Where(u => u.OrganizationId == organization.Value);

            if (!string.IsNullOrEmpty(username))
                query = query.Where(u => u.UserName == username);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<List<IdentityRole>> GetAllRoles()
        {
            return await _roleManager.Roles.ToListAsync();
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

        public async Task<Dictionary<string, List<string>>> GetRolesForUsers(IEnumerable<string> userIds)
        {
            var userRolesData = await _dataContext.UserRoles
                .Where(ur => userIds.Contains(ur.UserId.ToString()))
                .Join(_dataContext.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
                .ToListAsync();

            return userRolesData
                .GroupBy(ur => ur.UserId)
                .ToDictionary(g =>g.Key.ToString(), g => g.Select(ur => ur.Name).ToList());
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

        public async Task<List<IdentityRole>> GetRolesByName(List<string> rolesName)
        {
            try
            {
                return await _roleManager.Roles.Where(r => rolesName.Contains(r.Name)).ToListAsync();
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
                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

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
                var user = await _userManager.Users.AsNoTracking().IgnoreQueryFilters().Where(x => x.Email == email).FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ApplicationUser>> GetUsersByOrganization(Guid organizationId)
        {
            var result = await _userManager.Users.AsNoTracking().Where(u => u.OrganizationId == organizationId).ToListAsync();

            return result.ToList();
        }
    }
}
