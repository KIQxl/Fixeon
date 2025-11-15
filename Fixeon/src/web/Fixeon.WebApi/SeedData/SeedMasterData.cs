using Fixeon.Auth.Infraestructure.Data;
using Fixeon.Auth.Infraestructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fixeon.WebApi.SeedData
{
    public class SeedMasterData
    {
        private static readonly List<string> _baseRoles = new List<string> { "Admin", "Analista", "Usuario" };
        public static async Task SeedData(IServiceProvider provider)
        {
            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            var authDbContext = provider.GetRequiredService<DataContext>();

            authDbContext.IgnoreTenantInterceptor = true;

            const string masterRole = "MasterAdmin";

            if (!await roleManager.RoleExistsAsync(masterRole))
            {
                await roleManager.CreateAsync(new IdentityRole(masterRole));
            }

            string masterEmail = "admin@fixeon.com.br";
            var masterUser = await userManager.Users.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Email == masterEmail);

            if (masterUser is null)
            {
                masterUser = new ApplicationUser(masterEmail, "Fixeon");

                var createUserResult = await userManager.CreateAsync(masterUser, "F1X3oN@2025");
                if (createUserResult.Succeeded)
                    await userManager.AddToRoleAsync(masterUser, masterRole);
                else
                    throw new Exception($"Erro ao criar usuário master: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
            }

            foreach (var role in _baseRoles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
