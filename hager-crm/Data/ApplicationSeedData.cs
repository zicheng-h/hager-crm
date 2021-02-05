using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace hager_crm.Data
{
    public static class ApplicationSeedData
    {
        public static async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            //Create Roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "Supervisor", "Employee" };
            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            //Create Users
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            if (userManager.FindByEmailAsync("admin1@outlook.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "admin1@outlook.com",
                    Email = "admin1@outlook.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "password").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
            if (userManager.FindByEmailAsync("super1@outlook.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "super1@outlook.com",
                    Email = "super1@outlook.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "password").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Supervisor").Wait();
                }
            }
            if (userManager.FindByEmailAsync("employee1@outlook.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "employee1@outlook.com",
                    Email = "employee1@outlook.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "password").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee").Wait();
                }
            }
        }
    }
}
