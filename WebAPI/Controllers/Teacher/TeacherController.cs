//using Core.Domains;
//using Data.DTOs;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Services.StudentService;
//using Services.TeacherService;
//using Services.User;
//using static Data.DTOs.Teacher.TeacherDto;

//namespace WebAPI.Controllers.Teacher
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TeacherController : ControllerBase
//    {
//        private readonly ITeacherService _teacherService;
//        private readonly IUserService _userService;
//        public TeacherController(ITeacherService teacherService , IUserService userService)
//        {
//            _teacherService = teacherService;
//            _userService = userService;
//        }

//        [HttpGet("get-all-teachers")]
//        public async Task<ResponseResult<List<TeacherAllViewDto>>> GetAllTeachers()
//        {

//            var result = await _teacherService.GetAllTeacherAsync();

//            return result;
//        }

//        [HttpGet("get-teacher-by-id")]
//        public async Task<ResponseResult<AppUserDTO>> GetTeacherById(string userId)
//        {
//            var user = await _userService.GetUserByIdAsync(userId);
//            return user;
//        }

//        [HttpDelete("delete-teacher")]

//        public async Task<ResponseResult<AppUserDTO>> DeleteStudent(string userId)
//        {
//            var deletedUser = await _userService.DeleteUserAsync(userId);
//            return deletedUser;
//        }


//        [HttpPut("update-teacher")]

//        public async Task<ResponseResult<AppUserUpdateDTO>> UpdateTeacher(AppUserUpdateDTO user)
//        {
//            var updatedUser = await _userService.UpdateUsertAsync(user);
//            return updatedUser;
//        }

//    }
//}
