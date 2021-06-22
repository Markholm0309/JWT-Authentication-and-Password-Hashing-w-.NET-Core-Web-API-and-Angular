using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models
{
    public class SeedData
    {
        public async static Task Seed(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) 
        {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }

        public async static Task SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (await userManager.FindByEmailAsync("admin@content.com") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@content.com"
                };

                var result = await userManager.CreateAsync(user, "Pa$$w0rd1");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Administrator);
                }
            }
        }

        public async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(Roles.Administrator))
            {
                var role = new IdentityRole
                {
                    Name = Roles.Administrator
                };

                await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync(Roles.User))
            {
                var role = new IdentityRole
                {
                    Name = Roles.User
                };

                await roleManager.CreateAsync(role);
            }
        }
    }
}