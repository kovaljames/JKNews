using JKNews.Models;
using Microsoft.AspNetCore.Identity;

namespace JKNews.Services;

public static class AppDbInitializer
{
    public static void SeedRolesAndUsers(
        UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
        bool adminRoleReady1 = false;
        bool adminRoleReady2 = false;
        bool adminRoleReady3 = false;

        // Seed Roles
        if (roleManager.FindByNameAsync("Admin").Result == null)
        {
            var role = new Role()
            {
                Name = "Admin",
            };

            var res = roleManager.CreateAsync(role).Result;
            adminRoleReady1 = res.Succeeded;
        }
        if (roleManager.FindByNameAsync("Editor").Result == null)
        {
            var role = new Role()
            {
                Name = "Editor",
            };

            var res = roleManager.CreateAsync(role).Result;
            adminRoleReady2 = res.Succeeded;
        }
        if (roleManager.FindByNameAsync("User").Result == null)
        {
            var role = new Role()
            {
                Name = "User",
            };

            var res = roleManager.CreateAsync(role).Result;
            adminRoleReady3 = res.Succeeded;
        }

        // Roles created, then Seed Users
        if (adminRoleReady1 && adminRoleReady2 && adminRoleReady3)
        {
            if (userManager.FindByEmailAsync("mon@treal.com").Result == null)
            {
                var user = new User
                {
                    UserName = "mon@treal.com",
                    Email = "mon@treal.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "Montreal31*").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
        }
    }
}
