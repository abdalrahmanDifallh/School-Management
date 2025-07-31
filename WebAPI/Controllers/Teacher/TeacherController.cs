using Core;
using Core.Domains;
using Core.Enums;
using Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.WebAPI.Filters;
using Services.TeacherService;
using Syncfusion.EJ2.Base;


namespace WebAPI.Controllers.Teacher
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
       
        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
          
        }

        [HttpGet("get-all-teachers")]
        [Permission(nameof(Auth.PermissionsAdmin.Users_Get))]
        public async Task<PagedListResult<AppUserViewDTO>> GetAllStudentDm(DataManagerRequest dm)
        {

            var result = await _teacherService.GetTeachersAsync(dm);
            return result;
        }


        [HttpGet("get-all-teachers-v2")]
        [Permission(nameof(Auth.PermissionsAdmin.Users_Get))]
        public async Task<ResponseResult<List<AppUserViewDTO>>> GetAllStudent()
        {

            var result = await _teacherService.GetAllTeacherAsync();
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
