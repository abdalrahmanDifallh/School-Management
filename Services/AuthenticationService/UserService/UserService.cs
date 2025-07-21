using Core.Domains;
using Data.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;
using Services._Base;
using Services.LoggerService;
using Syncfusion.EJ2.FileManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AuthenticationService.UserService
{
    public class UserService : BaseService<UserService>, IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UserService(SchoolManagementContext dbContext, ILoggerService<UserService> logger, IHttpContextAccessor httpAccessor, UserManager<ApplicationUser> userManager , RoleManager<ApplicationRole> roleManager) : base(dbContext, logger, httpAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseResult<bool>> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                    return Error<bool>("UserNotFound");

                var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

                if (!result.Succeeded)
                {
                    return Error<bool>(result.Errors.FirstOrDefault().Description);
                }

                return Success(true);
            }
            catch (Exception ex)
            {
                return Error<bool>(ex);
            }
        }

        // create user
        public async Task<ResponseResult<AppUserDTO>> CreateStudentAsync(AppUserDTO user)
        {
            var AdminRole = _roleManager.FindByNameAsync("student").Result;
            try
            {
                var newUser = new ApplicationUser
                {
                    Address = user.Address,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    FullName = user.FullName,
                    IsActive = true,
                    CreatedOnUtc = DateTime.Now,
                    DateOfBirth = user.DateOfBirth,
                    Image = user.Image,
                    RoleId = AdminRole.Id,
                    Gender = user.Gender ,
                    UserName = user.UserName
                    
                };

                var result = await _userManager.CreateAsync(newUser, user.PasswordHash);

                if (!result.Succeeded)
                {
                    return Error<AppUserDTO>(result.Errors.FirstOrDefault().Description);
                }

                var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == AdminRole.Id);
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(newUser, role.Name);
                }

                return Success(new AppUserDTO
                {
                   
                    Email = newUser.Email,
                    PhoneNumber = newUser.PhoneNumber,
                    FullName = newUser.FullName,
                    
                });
            }
            catch (Exception ex)
            {
                return Error<AppUserDTO>(ex);
            }
        }

        public async Task<ResponseResult<AppUserDTO>> CreateTeacherAsync(AppUserDTO user)
        {
            var AdminRole = _roleManager.FindByNameAsync("Teacher").Result;
            try
            {
                var newUser = new ApplicationUser
                {
                    Address = user.Address,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    FullName = user.FullName,
                    IsActive = true,
                    CreatedOnUtc = DateTime.Now,
                    DateOfBirth = user.DateOfBirth,
                    Image = user.Image,
                    RoleId = AdminRole.Id,
                    Gender = user.Gender,
                    UserName = user.UserName

                };

                var result = await _userManager.CreateAsync(newUser, user.PasswordHash);

                if (!result.Succeeded)
                {
                    return Error<AppUserDTO>(result.Errors.FirstOrDefault().Description);
                }

                var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == AdminRole.Id);
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(newUser, role.Name);
                }

                return Success(new AppUserDTO
                {

                    Email = newUser.Email,
                    PhoneNumber = newUser.PhoneNumber,
                    FullName = newUser.FullName,

                });
            }
            catch (Exception ex)
            {
                return Error<AppUserDTO>(ex);
            }
        }

        public async Task<ResponseResult<AppUserDTO>> DeleteUserAsync(string userId)
        {
            try
            {
                var existingUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (existingUser is null)
                    return Error<AppUserDTO>("UserNotFound");

                existingUser.IsActive = false;
                existingUser.IsDeleted = true;

                var result = await _userManager.UpdateAsync(existingUser);

                if (!result.Succeeded)
                {
                    return Error<AppUserDTO>(result.Errors.FirstOrDefault().Description);
                }

                return Success(new AppUserDTO
                {
                   
                    Email = existingUser.Email,
                    UserName = existingUser.UserName,
                    PhoneNumber = existingUser.PhoneNumber,
                    FullName = existingUser.FullName,
                   
                });
            }
            catch (Exception ex)
            {
                return Error<AppUserDTO>(ex);
            }
        }

        public async Task<ResponseResult<AppUserDTO>> GetUserByIdAsync(string userId)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

                if (user is null)
                    return Error<AppUserDTO>("UserNotFound");

                var userRoleId = await _dbContext.UserRoles
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                if (userRoleId is null)
                    return Error<AppUserDTO>("UserRoleNotFound");

                var roleId = await _dbContext.Roles
                    .Where(x => x.Id == userRoleId.RoleId)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();

                var result = new AppUserDTO
                {
                    
                    Email = user.Email,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    FullName = user.FullName,
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth,
                    Image = user.Image,
                    Address = user.Address,
                    
                };

                return Success(result);

            }
            catch (Exception ex)
            {
                return Error<AppUserDTO>(ex);
            }
        }

        public async Task<ResponseResult<AppUserUpdateDTO>> UpdateUserAsync(AppUserUpdateDTO user)
        {
            try
            {
                var existingUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
                if (existingUser is null)
                    return Error<AppUserUpdateDTO>("UserNotFound");


                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.FullName = user.FullName;
                existingUser.Address = user.Address;
                existingUser.Gender = user.Gender;
                existingUser.Image = user.Image;
                existingUser.DateOfBirth = user.DateOfBirth;

                var result = await _userManager.UpdateAsync(existingUser);

                if (!result.Succeeded)
                {
                    return Error<AppUserUpdateDTO>(result.Errors.FirstOrDefault().Description);
                }

               

                return Success(new AppUserUpdateDTO
                {
                    Id = existingUser.Id,
                    Email = existingUser.Email,
                    PhoneNumber = existingUser.PhoneNumber,
                    FullName = existingUser.FullName,
            
                });
            }
            catch (Exception ex)
            {
                return Error<AppUserUpdateDTO>(ex);
            }
        }
    }
}
