
using Core.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SchoolManagement.Data;
using SchoolManagement.Models;
using static Core.Enums.Auth;

namespace Data.Context
{
    public static class Seeder
    {
        public static void SeedData(this IApplicationBuilder applicationBuilder)
        {
            SeedPermissions(applicationBuilder);
            SeedIdentity(applicationBuilder);
        }

        //===============================================================================
        private static void SeedIdentity(IApplicationBuilder applicationBuilder)
        {
            using var scope = applicationBuilder.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SchoolManagementContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            ApplicationRole AdminRole = null;
            // إنشاء دور Admin
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                roleManager.CreateAsync(new ApplicationRole("Admin")).Wait(); // إنشاء الدور

                 AdminRole = roleManager.FindByNameAsync("Admin").Result; // الحصول على الدور

                // الحصول على جميع الصلاحيات من قاعدة البيانات
                var permissions = context.Permissions.ToList();

                foreach (var permission in permissions)
                {
                    if (!context.RolePermissionMappings.Any(rp => rp.RoleId == AdminRole.Id && rp.PermissionId == permission.Id))
                    {
                        context.RolePermissionMappings.Add(new RolePermissionMapping
                        {
                            RoleId = AdminRole.Id,
                            PermissionId = permission.Id
                        });
                    }
                }
               
            }
           
            // إنشاء دور Teacher
            if (!roleManager.RoleExistsAsync("Teacher").Result)
            {
                roleManager.CreateAsync(new ApplicationRole("Teacher")).Wait();
                var teacherRole = roleManager.FindByNameAsync("Teacher").Result;

                if (teacherRole == null)
                {
                    Console.WriteLine("Error: Teacher role was not created.");
                    return;
                }

                var allPermissions = context.Permissions.ToList();
                string[] teacherPermissionNames = Enum.GetNames(typeof(PermissionsTeacher));

                foreach (var permissionName in teacherPermissionNames)
                {
                    var permission = allPermissions
                        .FirstOrDefault(p => p.DisplayName.Equals(permissionName, StringComparison.OrdinalIgnoreCase));

                    if (permission != null)
                    {
                        if (!context.RolePermissionMappings.Any(rp => rp.RoleId == teacherRole.Id && rp.PermissionId == permission.Id))
                        {
                            context.RolePermissionMappings.Add(new RolePermissionMapping
                            {
                                RoleId = teacherRole.Id,
                                PermissionId = permission.Id
                            });
                            Console.WriteLine($"Added permission: {permissionName} for Teacher role");
                        }
                        else
                        {
                            Console.WriteLine($"Permission {permissionName} already exists for Teacher role");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Warning: Permission '{permissionName}' not found in database");
                    }
                }
            }

            // إنشاء دور Student
            if (!roleManager.RoleExistsAsync("Student").Result)
            {
                roleManager.CreateAsync(new ApplicationRole("Student")).Wait();
                var studentRole = roleManager.FindByNameAsync("Student").Result;

                if (studentRole == null)
                {
                    Console.WriteLine("Error: Student role was not created.");
                    return;
                }

                var allPermissions = context.Permissions.ToList();
                string[] studentPermissionNames = Enum.GetNames(typeof(PermissionsStudent));

                foreach (var permissionName in studentPermissionNames)
                {
                    var permission = allPermissions
                        .FirstOrDefault(p => p.DisplayName.Equals(permissionName, StringComparison.OrdinalIgnoreCase));

                    if (permission != null)
                    {
                        if (!context.RolePermissionMappings.Any(rp => rp.RoleId == studentRole.Id && rp.PermissionId == permission.Id))
                        {
                            context.RolePermissionMappings.Add(new RolePermissionMapping
                            {
                                RoleId = studentRole.Id,
                                PermissionId = permission.Id
                            });
                            Console.WriteLine($"Added permission: {permissionName} for Student role");
                        }
                        else
                        {
                            Console.WriteLine($"Permission {permissionName} already exists for Student role");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Warning: Permission '{permissionName}' not found in database");
                    }
                }
            }

            // حفظ التغييرات مرة واحدة
            try
            {
                context.SaveChanges();
                Console.WriteLine("Changes saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving changes: {ex.Message}");
            }

            // إنشاء مستخدم Admin
            List<string> users = new() { "Admin" };
            users.ForEach(username =>
            {
                if (userManager.FindByNameAsync(username).Result == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = username,
                        Email = $"{username}@Admin.com",
                        FullName = $"Admin",
                        PhoneNumber = "0123456789",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        IsActive = true,
                        RoleId = AdminRole.Id,
                    };

                    var result = userManager.CreateAsync(user, "Admin@123").Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Admin").Wait();
                    }
                }
            });
        }

        //===============================================================================
        private static void SeedPermissions(IApplicationBuilder applicationBuilder)
        {
            using var scope = applicationBuilder.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SchoolManagementContext>();
            context.Database.EnsureCreated();
            string[] permissions = Enum.GetNames(typeof(PermissionsAdmin));

            foreach (var permission in permissions)
            {
                if (!context.Permissions.Any(p => p.Key == permission))
                    context.Permissions.Add(new Permission { Key = permission, DisplayName = permission });

                context.SaveChanges();
            }
        }
    }
}