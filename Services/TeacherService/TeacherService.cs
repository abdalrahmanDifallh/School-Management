using Core.Domains;
using Data.DTOs.Teacher;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using Services._Base;
using Services.LoggerService;
using Services.TeacherService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.DTOs.Student.StudentDto;
using static Data.DTOs.Teacher.TeacherDto;



public class TeacherService : BaseService<TeacherService>, ITeacherService
{
    private readonly SchoolManagementContext _context;

    public TeacherService(SchoolManagementContext dbContext, ILoggerService<TeacherService> logger, IHttpContextAccessor httpAccessor) : base(dbContext, logger, httpAccessor)
    {
        _context = dbContext;
    }

    public async Task<ResponseResult<List<TeacherAllViewDto>>> GetAllTeacherAsync()
    {
        try
        {

            var teachers = await _context.Users
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
}

