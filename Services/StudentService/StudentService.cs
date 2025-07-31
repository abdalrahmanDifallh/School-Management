using Core;
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
using Services.SyncGridOperations;
using Syncfusion.EJ2.Base;
using Syncfusion.EJ2.Linq;
using static Data.DTOs.Student.StudentDto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

public class StudentService : BaseService<StudentService>, IStudentService
{

    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    public StudentService(SchoolManagementContext dbContext, ILoggerService<StudentService> logger, IHttpContextAccessor httpAccessor, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager) : base(dbContext, logger, httpAccessor)
    {

        _roleManager = roleManager;
        _userManager = userManager;
    }



    public async Task<ResponseResult<List<AppUserViewDTO>>> GetAllStudentAsync()
    {
        var studentRole = _roleManager.FindByNameAsync("Student").Result;


        //var students = await _context.Users
        //    .Where(u => u.RoleId == "7fb258f3-1976-4b86-8bc8-1e6976a34d91")// افتراض أن هناك حقل Role لتحديد نوع المستخدم
        //    .Where(u => u.IsActive == true)
        //    .Include(s => s.Classroom) // تضمين بيانات الصف
        //    .Include(s => s.Grades) // تضمين بيانات العلامات
        //    .Select(s => new StudentAllViewDto
        //    {
        //        Id = s.Id,
        //        FullName = s.FullName,
        //        PhoneNumber = s.PhoneNumber,
        //        Address = s.Address,
        //        Gender = s.Gender,
        //        Image = s.Image,
        //        ClassRoomName = s.Classroom.Name,
        //        AverageGrade = s.Grades.Any() ? (double)s.Grades.Sum(g => g.StudentGrad) / 10.0 : 0
        //    })
        //    .ToListAsync();

        try
        {
            var query = await _userManager.Users
                //.Where(u => u.RoleId == AdminRole.Id)
                .Where(u => u.IsActive == true)
                .Where(u => u.RoleId == studentRole.Id)
                .Include(s => s.Classroom)
                .Include(s => s.Grades)
                .Select(s => new AppUserViewDTO
                {
                    Id = s.Id,
                    FullName = s.FullName,
                    DateOfBirth = s.DateOfBirth,
                    PhoneNumber = s.PhoneNumber,
                    Address = s.Address,
                    Gender = s.Gender,
                    Image = s.Image,
                    ClassName = s.Classroom.Name,
                    AverageGrade = s.Grades != null && s.Grades.Any()
                                    ? s.Grades.Average(g => (double)g.StudentGrad)
                                    : 0
                }).ToListAsync();


            return Success(query);
        }
        catch (Exception ex)
        {
            return Error<List<AppUserViewDTO>>(ex);
        }
    }

    public async Task<PagedListResult<AppUserViewDTO>> GetStudentsAsync(DataManagerRequest dm)
    {
        var studentRole = _roleManager.FindByNameAsync("Student").Result;
        try
        {
            var query = _userManager.Users
                //.Where(u => u.RoleId == AdminRole.Id)
                .Where(u => u.IsActive == true)
                .Where(u => u.RoleId == studentRole.Id)
                .Include(s => s.Classroom)
                .Include(s => s.Grades)
                .Select(s => new AppUserViewDTO
                {
                    Id = s.Id,
                    FullName = s.FullName,
                    DateOfBirth = s.DateOfBirth,
                    PhoneNumber = s.PhoneNumber,
                    Address = s.Address,
                    Gender = s.Gender,
                    Image = s.Image,
                    ClassName = s.Classroom.Name,
                    AverageGrade = s.Grades != null && s.Grades.Any()
                                    ? s.Grades.Average(g => (double)g.StudentGrad)
                                    : 0
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

    public async Task<ResponseResult<int>> GetNumberOfStudentsByGenderAsync(bool gender)
    {
        var studentRole = await _roleManager.FindByNameAsync("Student");

        var count = await _userManager.Users
            .Where(s => s.RoleId == studentRole.Id)
            .Where(s => s.Gender == gender)
            .CountAsync();

        return new ResponseResult<int> { Data = count, IsSuccess = true };
    }

    public async Task<ResponseResult<int>> GetNumberOfStudentsByStatusAsync(bool status)
    {
        var studentRole = await _roleManager.FindByNameAsync("Student");

        var students = await _userManager.Users
            .Where(s => s.RoleId == studentRole.Id)
            .Include(s => s.Grades)
            .ToListAsync();

        int count = 0;

        foreach (var student in students)
        {
            if (student.Grades == null || !student.Grades.Any())
                continue;

            var average = student.Grades.Average(g => g.StudentGrad); // تأكد أن Grades فيها خاصية اسمها Value

            if (status && average >= 60)
                count++;
            else if (!status && average < 60)
                count++;
        }

        return new ResponseResult<int> { Data = count, IsSuccess = true };
    }

    public async Task<ResponseResult<int>> GetNumberOfStudentsAsync()
    {
        var studentRole = await _roleManager.FindByNameAsync("Student");

        var count = await _userManager.Users
            .Where(s => s.RoleId == studentRole.Id)
            .CountAsync();

        return new ResponseResult<int> { Data = count, IsSuccess = true };
    }

    public async Task<ResponseResult<int>> GetNumberOfStudentsByTeacherIdAsync(string teacherId)
    {
        var studentRole = await _roleManager.FindByNameAsync("Student");

        var count = await _userManager.Users
            .Where(s => s.RoleId == studentRole.Id)
            .Where(s => s.Classroom.TeacherUserId == teacherId)
            .CountAsync();

        return new ResponseResult<int> { Data = count, IsSuccess = true };
    }

    public async Task<ResponseResult<int>> GetNumberOfStudentsByGenderAndByTeacherIdAsync(bool gender, string teacherId)
    {
        var studentRole = await _roleManager.FindByNameAsync("Student");

        var count = await _userManager.Users
            .Where(s => s.RoleId == studentRole.Id)
            .Where(s => s.Gender == gender)
            .Where(s => s.Classroom.TeacherUserId == teacherId)
            .CountAsync();

        return new ResponseResult<int> { Data = count, IsSuccess = true };
    }

    public async Task<ResponseResult<int>> GetNumberOfStudentsByStatusAndByTeacherIdAsync(bool status, string teacherId)
    {
        var studentRole = await _roleManager.FindByNameAsync("Student");

        var students = await _userManager.Users
            .Where(s => s.RoleId == studentRole.Id)
            .Where(s => s.Classroom.TeacherUserId == teacherId)
            .Include(s => s.Grades)
            .ToListAsync();

        int count = 0;

        foreach (var student in students)
        {
            if (student.Grades == null || !student.Grades.Any())
                continue;

            var average = student.Grades.Average(g => g.StudentGrad); // تأكد أن Grades فيها خاصية اسمها Value

            if (status && average >= 60)
                count++;
            else if (!status && average < 60)
                count++;
        }

        return new ResponseResult<int> { Data = count, IsSuccess = true };
    }

    public async Task<ResponseResult<int>> GetPassRateAsync()
    {
        var numberStudentPass = await GetNumberOfStudentsByStatusAsync(true);
        var totalStudents = await GetNumberOfStudentsAsync();

        if (!numberStudentPass.IsSuccess || !totalStudents.IsSuccess || totalStudents.Data == 0)
        {
            return new ResponseResult<int> { IsSuccess = false, Data = 0, Errors =  { "Unable to calculate pass rate." } };
        }

        var passRate = (numberStudentPass.Data * 100) / totalStudents.Data; // Calculate percentage
        return new ResponseResult<int> { Data = passRate, IsSuccess = true };
    }

    public async Task<ResponseResult<int>> GetPassRateByTeacherIdAsync(string teacherId)
    {
        var numberStudentPass = await GetNumberOfStudentsByStatusAndByTeacherIdAsync(true , teacherId);
        var totalStudents = await GetNumberOfStudentsByTeacherIdAsync(teacherId);

        if (!numberStudentPass.IsSuccess || !totalStudents.IsSuccess || totalStudents.Data == 0)
        {
            return new ResponseResult<int> { IsSuccess = false, Data = 0, Errors = { "Unable to calculate pass rate." } };
        }

        var passRate = (numberStudentPass.Data * 100) / totalStudents.Data; // Calculate percentage
        return new ResponseResult<int> { Data = passRate, IsSuccess = true };
    }
}


