//using Core.Domains;
//using Data.DTOs;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.OpenApi.Models;
//using Services.AuthenticationService;
//using Services.StudentService;
////using Services.User;
//using static Data.DTOs.Student.StudentDto;

//namespace WebAPI.Controllers.Student
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [Authorize]
//    public class StudentController : ControllerBase
//    {
//        private readonly IStudentService _studentService;
//     //   private readonly IUserService _userService;

//        public StudentController(IStudentService studentService 
//            //IUserService userService
//            ) {
//            _studentService = studentService;
//          //  _userService = userService;
//        }
//        [HttpGet("get-all-students")]
//        //[Permission(nameof(Permissions.Users_Get))] 
//        public async Task<ResponseResult<List<StudentAllViewDto>>> GetAllStudent()
//        {

//            var result = await _studentService.GetAllStudentAsync();
//            return result;
//        }

//        [HttpDelete("delete-student")]
        
//        public async Task<ResponseResult<AppUserDTO>> DeleteStudent(string userId)
//        {
//            var deletedUser = await _userService.DeleteUserAsync(userId);
//            return deletedUser;
//        }

//        [HttpGet("get-student-by-id")]
//        public async Task<ResponseResult<AppUserDTO>> GetStudentById(string userId)
//        {
//            var user = await _userService.GetUserByIdAsync(userId);
//            return user;
//        }

//        [HttpPut("update-student")]
       
//        public async Task<ResponseResult<AppUserUpdateDTO>> UpdateStudent(AppUserUpdateDTO user)
//        {
//            var updatedUser = await _userService.UpdateUsertAsync(user);
//            return updatedUser;
//        }

//    }
//}
