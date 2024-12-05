using EventManagerADV.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace EventManagerADV.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Rollen
                string[] roles = { "Admin", "User" };
                foreach (var role in roles)
                {
                    if (!roleManager.RoleExistsAsync(role).Result)
                    {
                        roleManager.CreateAsync(new IdentityRole(role)).Wait();
                    }
                }

                // Admin-gebruiker
                if (userManager.FindByEmailAsync("admin@example.com").Result == null)
                {
                    var admin = new ApplicationUser
                    {
                        UserName = "admin@example.com",
                        Email = "admin@example.com",
                        FirstName = "Admin",
                        LastName = "User"
                    };
                    userManager.CreateAsync(admin, "Admin123!").Wait();
                    userManager.AddToRoleAsync(admin, "Admin").Wait();
                }
            }
        }
    }
}