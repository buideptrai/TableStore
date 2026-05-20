using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace TableStore.Models
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Create Roles
            string[] roleNames = { SD.Role_Admin, SD.Role_Customer, SD.Role_Company, SD.Role_Employee };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create Admin User
            var adminUser = await userManager.FindByEmailAsync("admin@gmail.com");
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    FullName = "Quản trị viên",
                    EmailConfirmed = true
                };

                var createAdmin = await userManager.CreateAsync(newAdmin, "Admin@123");
                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, SD.Role_Admin);
                }
            }

            // Create Normal User
            var normalUser = await userManager.FindByEmailAsync("user@gmail.com");
            if (normalUser == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = "user@gmail.com",
                    Email = "user@gmail.com",
                    FullName = "Khách Hàng Mẫu",
                    Address = "123 Đường Mẫu, Hà Nội",
                    EmailConfirmed = true
                };

                var createUser = await userManager.CreateAsync(newUser, "User@123");
                if (createUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, SD.Role_Customer);
                }
            }
        }
    }
}
