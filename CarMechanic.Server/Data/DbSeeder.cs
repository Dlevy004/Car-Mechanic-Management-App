using CarMechanic.Server;
using Microsoft.AspNetCore.Identity;

namespace CarMechanic.Server.Data
{
    public static class DbSeeder
    {
        public static async Task Initialize(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            string[] roleNames = { "Admin", "Mechanic" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Fő admin felhasználó létrehozása
            var adminEmail = "admin@carmechanic.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "AdminPass123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            var mechanicEmail = "mechanic@carmechanic.com";
            var mechanicUser = await userManager.FindByEmailAsync(mechanicEmail);

            if (mechanicUser == null)
            {
                mechanicUser = new AppUser
                {
                    UserName = mechanicEmail,
                    Email = mechanicEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(mechanicUser, "MechanicPass123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(mechanicUser, "Mechanic");
                }
            }
        }
    }
}
