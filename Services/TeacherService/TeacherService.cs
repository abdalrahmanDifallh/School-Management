using Core;
using Core.Domains;
using Data.DTOs;
using Data.DTOs.Teacher;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;
using Services._Base;
using Services.LoggerService;
using Services.SyncGridOperations;
using Services.TeacherService;
using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.DTOs.Student.StudentDto;
using static Data.DTOs.Teacher.TeacherDto;



public class TeacherService : BaseService<TeacherService>, ITeacherService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    public TeacherService(SchoolManagementContext dbContext, ILoggerService<TeacherService> logger, IHttpContextAccessor httpAccessor, RoleManager<ApplicationRole> roleManager , UserManager<ApplicationUser> userManager) : base(dbContext, logger, httpAccessor)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ResponseResult<List<TeacherAllViewDto>>> GetAllTeacherAsync()
    {
        try
        {

            var teachers = await _userManager.Users
                .Where(u => u.RoleId == "2")// افتراض أن هناك حقل Role لتحديد نوع المستخدم
                .Where(u => u.IsActive == true)
                .Include(s => s.Classroom) // تضمين بيانات الصف
                .Select(s => new TeacherAllViewDto
                {
                    Id = s.Id,
                    FullName = s.FullName,
                    PhoneNumber = s.PhoneNumber,
                    Address = s.Address,
                    Gender = s.Gender,
                    Image = s.Image,
                    ClassRoomName = s.Classroom.Name,
                })
                .ToListAsync();

            return Success(teachers);
        }
        catch (Exception ex)
        {
            return Error<List<TeacherAllViewDto>>(ex);
        }
    }

    public async Task<ResponseResult<int>> GetNumberOfTeacherAsync()
    {
        var studentRole = await _roleManager.FindByNameAsync("Teacher");

        var count = await _userManager.Users
            .Where(s => s.RoleId == studentRole.Id)
            .CountAsync();

        return new ResponseResult<int> { Data = count, IsSuccess = true };
    }

    public async Task<PagedListResult<AppUserViewDTO>> GetTeachersAsync(DataManagerRequest dm)
    {
        var AdminRole = _roleManager.FindByNameAsync("Teacher").Result;
        try
        {
            var query = _userManager.Users
                .Where(u => u.IsActive == true)
                .Where(u => u.RoleId == AdminRole.Id)
                .Include(s => s.Classroom)
                .Include(s => s.Grades)
                .Select(s => new AppUserViewDTO
                {
                    Id = s.Id,
                    FullName = s.FullName,
                    PhoneNumber = s.PhoneNumber,
                    Address = s.Address,
                    Gender = s.Gender,
                    Image = s.Image,
                    ClassName = s.Classroom.Name,
                   });

            // Log initial count
            var initialCount = await query.CountAsync();


            var result = await SyncGridOperations<AppUserViewDTO>.PagingAndFilterAsync(query, dm);


            return result;
        }
        catch (Exception ex)
        {

            return new PagedListResult<AppUserViewDTO>();
        }
    }
}

