using Core.Domains;
using Data.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;
using Services._Base;
using Services.LoggerService;
using Services.StudentService;

using static Data.DTOs.Student.StudentDto;

public class StudentService : BaseService<StudentService> , IStudentService
{
    private readonly SchoolManagementContext _context;

    public StudentService(SchoolManagementContext dbContext, ILoggerService<StudentService> logger, IHttpContextAccessor httpAccessor) : base(dbContext, logger, httpAccessor)
    {
        _context = dbContext;
    }

   

    public async Task<ResponseResult<List<StudentAllViewDto>>> GetAllStudentAsync()
    {
        try
        {
           
            var students = await _context.Users
                .Where(u => u.RoleId == "3")// افتراض أن هناك حقل Role لتحديد نوع المستخدم
                .Where(u => u.IsActive == true)
                .Include(s => s.Classroom) // تضمين بيانات الصف
                .Include(s => s.Grades) // تضمين بيانات العلامات
                .Select(s => new StudentAllViewDto
                {
                    Id = s.Id,
                    FullName = s.FullName,
                    PhoneNumber = s.PhoneNumber,
                    Address = s.Address,
                    Gender = s.Gender,
                    Image = s.Image,
                    ClassRoomName = s.Classroom.Name,
                    AverageGrade = s.Grades.Any() ? (double)s.Grades.Sum(g => g.StudentGrad) / 10.0 : 0
                })
                .ToListAsync();

            return Success(students);
        }
        catch (Exception ex)
        {
            return Error<List<StudentAllViewDto>>(ex);
        }
    }

    

    
}

