using Core;
using Core.Domains;
using Core.Enums;
using Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using ProjectManagement.WebAPI.Filters;
using Services.AuthenticationService;
using Services.StudentService;
using Syncfusion.EJ2.Base;

//using Services.User;
using static Data.DTOs.Student.StudentDto;

namespace WebAPI.Controllers.Student
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        //   private readonly IUserService _userService;

        public StudentController(IStudentService studentService
            //IUserService userService
            )
        {
            _studentService = studentService;
            //  _userService = userService;
        }
        [HttpGet("get-all-students")]
        [Permission(nameof(Auth.PermissionsAdmin.Users_Get))] 
        public async Task<PagedListResult<AppUserViewDTO>> GetAllStudentDm(DataManagerRequest dm)
        {

            var result = await _studentService.GetStudentsAsync(dm);
            return result;
        }


        [HttpGet("get-number-students-by-gender")]
        [Permission(nameof(Auth.PermissionsAdmin.Users_Get))]
        public async Task<ResponseResult<int>> GetNumberOfStudentByGender(bool gender)
        {

            var result = await _studentService.GetNumberOfStudentsByGenderAsync(gender);
            return result;
        }

        [HttpGet("get-number-students-by-gender-and-TeacherId")]
        [Permission(nameof(Auth.PermissionsTeacher.Grades_Get_by_TeacherId))]
        public async Task<ResponseResult<int>> GetNumberOfStudentByGenderAndTeacherId(bool gender , string teacherId)
        {

            var result = await _studentService.GetNumberOfStudentsByGenderAndByTeacherIdAsync(gender , teacherId);
            return result;
        }

        [HttpGet("get-number-students-by-status")]
         [Permission(nameof(Auth.PermissionsAdmin.Users_Get))]
        public async Task<ResponseResult<int>> GetNumberOfStudentByStatusAsync(bool status )
        {

            var result = await _studentService.GetNumberOfStudentsByStatusAsync(status);
            return result;
        }

        [HttpGet("get-number-students-by-status-and-teacherId")]
        [Permission(nameof(Auth.PermissionsTeacher.Grades_Get_by_TeacherId))]
        public async Task<ResponseResult<int>> GetNumberOfStudentByStatusAndTeacherIdAsync(bool status , string teacherId)
        {

            var result = await _studentService.GetNumberOfStudentsByStatusAndByTeacherIdAsync(status, teacherId);
            return result;
        }



        [HttpGet("get-number-students")]
        [Permission(nameof(Auth.PermissionsAdmin.Users_Get))]
        public async Task<ResponseResult<int>> GetNumberOfStudentAsync()
        {

            var result = await _studentService.GetNumberOfStudentsAsync();
            return result;
        }


        //GetNumberOfStudentsByTeacherIdAsync
        [HttpGet("get-number-students-by-teacherId")]
        [Permission(nameof(Auth.PermissionsTeacher.Grades_Get_by_TeacherId))]
        public async Task<ResponseResult<int>> GetNumberOfStudentByTeacherIdAsync(string teacherId)
        {

            var result = await _studentService.GetNumberOfStudentsByTeacherIdAsync(teacherId);
            return result;
        }


    }
}
