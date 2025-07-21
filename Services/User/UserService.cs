//using Core.Domains;
//using Data.DTOs;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using SchoolManagement.Data;
//using SchoolManagement.Models;
//using Services._Base;
//using Services.LoggerService;
//using Services.StudentService;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Services.User
//{
//    public class UserService : BaseService<UserService>, IUserService
//    {
//        private readonly SchoolManagementContext _context;

//        public UserService(SchoolManagementContext dbContext, ILoggerService<UserService> logger, IHttpContextAccessor httpAccessor) : base(dbContext, logger, httpAccessor)
//        {
//            _context = dbContext;
//        }

//        public async Task<ResponseResult<AppUserDTO>> GetUserByIdAsync(string userId)
//        {
//            try
//            {
//                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

//                if (user is null)
//                    return Error<AppUserDTO>("UserNotFound");



//                var result = new AppUserDTO
//                {

//                    Email = user.Email,
//                    Gender = user.Gender,
//                    PhoneNumber = user.PhoneNumber,
//                    FullName = user.FullName,
//                    DateOfBirth = user.DateOfBirth,
//                    Image = user.Image,
//                    PasswordHash = user.PasswordHash,
//                    Address = user.Address,
//                };

//                return Success(result);

//            }
//            catch (Exception ex)
//            {
//                return Error<AppUserDTO>(ex);
//            }
//        }

//        public async Task<ResponseResult<AppUserDTO>> DeleteUserAsync(string userId)
//        {
//            try
//            {
//                var existingStudent = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
//                if (existingStudent is null)
//                    return Error<AppUserDTO>("UserNotFound");

//                existingStudent.IsActive = false;
//                existingStudent.IsDeleted = true;

//                //   var result = await _context.Update(existingStudent);

//                _context.Users.Update(existingStudent); // Mark entity as modified
//                var result = await _context.SaveChangesAsync();

//                //
//                //if (!result.Succeeded)
//                //{
//                //    return Error<AppUserDTO>(result.Errors.FirstOrDefault().Description);
//                //}

//                if (result <= 0)
//                {
//                    return Error<AppUserDTO>("Failed to update user");
//                }
//                return Success(new AppUserDTO
//                {

//                    Email = existingStudent.Email,
//                    Address = existingStudent.Address,
//                    PhoneNumber = existingStudent.PhoneNumber,
//                    FullName = existingStudent.FullName,

//                });
//            }
//            catch (Exception ex)
//            {
//                return Error<AppUserDTO>(ex);
//            }
//        }

//        public async Task<ResponseResult<AppUserUpdateDTO>> UpdateUsertAsync(AppUserUpdateDTO user)
//        {
//            try
//            {
//                var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
//                if (existingUser is null)
//                    return Error<AppUserUpdateDTO>("UserNotFound");

//                var hashedPassword = new PasswordHasher<ApplicationUser>().HashPassword(existingUser, user.PasswordHash);

//                existingUser.Email = user.Email;
//                existingUser.PhoneNumber = user.PhoneNumber;
//                existingUser.FullName = user.FullName;
//                existingUser.Address = user.Address;
//                existingUser.Gender = user.Gender;
//                existingUser.Image = user.Image;
//                existingUser.PasswordHash = hashedPassword;
//                existingUser.DateOfBirth = user.DateOfBirth;



//                _context.Users.Update(existingUser); // Mark entity as modified
//                var result = await _context.SaveChangesAsync();


//                if (result <= 0)
//                {
//                    return Error<AppUserUpdateDTO>("Failed to update user");
//                }



//                return Success(new AppUserUpdateDTO
//                {
//                    Id = existingUser.Id,
//                    Email = existingUser.Email,
//                    PhoneNumber = existingUser.PhoneNumber,
//                    FullName = existingUser.FullName,
//                    Address = existingUser.Address,
//                    Gender = existingUser.Gender,
//                    Image = existingUser.Image,
//                    PasswordHash = existingUser.PasswordHash,
//                    DateOfBirth = existingUser.DateOfBirth

//                });
//            }
//            catch (Exception ex)
//            {
//                return Error<AppUserUpdateDTO>(ex);
//            }
//        }

//    }
//}
