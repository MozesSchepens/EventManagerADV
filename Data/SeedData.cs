using EventManagerADV.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace EventManagerADV.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Voeg standaardrollen toe
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // Voeg een standaardbeheerder toe
            var user = new ApplicationUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                Voornaam = "Admin",
                Achternaam = "User",
                EmailConfirmed = true
            };

            if (await userManager.FindByNameAsync(user.UserName) == null)
            {
                await userManager.CreateAsync(user, "Admin@123");
                await userManager.AddToRoleAsync(user, "Admin");
            }

            var defaultUser = new ApplicationUser
            {
                UserName = "mib",
                Email = "mib@gmail.com",
                Voornaam = "mib",
                Achternaam = "mib",
                EmailConfirmed = true
            };

            if (await userManager.FindByNameAsync(defaultUser.UserName) == null)
            {
                await userManager.CreateAsync(defaultUser, "User@123");
                await userManager.AddToRoleAsync(defaultUser, "User");
            }
        }
    }
}
