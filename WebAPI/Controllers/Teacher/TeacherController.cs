using Core;
using Core.Domains;
using Core.Enums;
using Data.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.WebAPI.Filters;
using Services.StudentService;
using Services.TeacherService;
using Syncfusion.EJ2.Base;
using static Data.DTOs.Teacher.TeacherDto;

namespace WebAPI.Controllers.Teacher
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
       
        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
          
        }

        [HttpGet("get-all-teachers")]
        public async Task<PagedListResult<AppUserViewDTO>> GetAllStudentDm(DataManagerRequest dm)
        {

            var result = await _teacherService.GetTeachersAsync(dm);
            return result;
        }


        [HttpGet("get-number-Teacher")]
        [Permission(nameof(Auth.PermissionsAdmin.Users_Get))]
        public async Task<ResponseResult<int>> GetNumberOfStudentAsync()
        {

            var result = await _teacherService.GetNumberOfTeacherAsync();
            return result;
        }

    }
}
