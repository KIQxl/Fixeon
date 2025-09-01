using Fixeon.Auth.Infraestructure.Data;
using Fixeon.Auth.Infraestructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fixeon.Auth.Infraestructure.Services
{
    public class SeedMasterData
    {
        private static readonly List<string> _baseRoles = new List<string> { "Admin", "Analista", "Usuario"};
        public static async Task SeedData(IServiceProvider provider)
        {
            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            var dbContext = provider.GetRequiredService<DataContext>();

            dbContext.IgnoreTenantInterceptor = true;

            const string masterRole = "MasterAdmin";

            if(!await roleManager.RoleExistsAsync(masterRole))
            {
                await roleManager.CreateAsync(new IdentityRole(masterRole));
            }

            var companyName = "Fixeon Master Company";

            var masterCompany = await dbContext.companies.AsNoTracking().FirstOrDefaultAsync(x => x.Name == companyName);

            if(masterCompany is null)
            {
                masterCompany = new Company(companyName, "00000000000000", "fixeon.company@email.com");
                await dbContext.companies.AddAsync(masterCompany);
                await dbContext.SaveChangesAsync();
            }

            string masterEmail = "fixeon-company@fixeon.com.br";
            var masterUser = await userManager.Users.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Email == masterEmail);

            if(masterUser is null)
            {
                masterUser = new ApplicationUser(masterEmail, "Fixeon");
                masterUser.AssignCompany(masterCompany.Id);

                var createUserResult = await userManager.CreateAsync(masterUser, "F1X3oN@2025");
                if (createUserResult.Succeeded)
                    await userManager.AddToRoleAsync(masterUser, masterRole);
                else
                    throw new Exception($"Erro ao criar usuário master: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
            }

            foreach(var role in _baseRoles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
