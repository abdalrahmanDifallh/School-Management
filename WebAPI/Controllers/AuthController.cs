using Core.Domains;
using Core.Enums;
using Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.WebAPI.Filters;
using SchoolManagement.Models;
using Services.AuthenticationService;
using Services.AuthenticationService.UserService;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class AuthController : ControllerBase
    {
        private readonly IUserService _authService;
        private readonly IAuthenticationService _authenticationService;
        public AuthController(IUserService authService , IAuthenticationService authenticationService)
        {
            _authService = authService;
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
       

        public async Task<ActionResult<TokenResponseDto>> Login(LoginDto request)
        {
            // إضافة logging للتشخيص
            Console.WriteLine($"Received request: Email={request?.Email}, PasswordHash={request?.PasswordHash}");

            if (request == null)
            {
                return BadRequest("Request body is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.PasswordHash))
            {
                return BadRequest("Email and password are required.");
            }

            var tokenResult = await _authenticationService.Login(request);

            if (tokenResult == null)
            {
                return BadRequest("Invalid Email or password.");
            }

            return Ok(tokenResult);
        }

        //[HttpPost("refrsh-token")]
        //public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        //{
        //    var result =  await _authService.RefreshTokensAsync(request);
        //    if (result is null || result.AccessToken is null || result.RefreshToken is null)
        //    {
        //        return Unauthorized("Invaliad refresh Token");
        //    }
        //    return Ok(result); 
        //}


        [HttpPost("register-student")]
        [Permission(nameof(Auth.PermissionsAdmin.Users_Create))] 
        public async Task<ActionResult<ApplicationUser>> RegisterStudent(AppUserDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.PasswordHash))
            {
                return BadRequest("Email and password are required.");
            }

            var user = await _authService.CreateStudentAsync(request);
            if (user is null)
            {
                return BadRequest("Email already exists.");
            }
            return Ok(user);
        }


        [HttpPost("register-teacher")]
        [Permission(nameof(Auth.PermissionsAdmin.Users_Create))]
        public async Task<ActionResult<ApplicationUser>> RegisterTeacher(AppUserDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.PasswordHash))
            {
                return BadRequest("Email and password are required.");
            }

            var user = await _authService.CreateTeacherAsync(request);
            if (user is null)
            {
                return BadRequest("Email already exists.");
            }
            return Ok(user);
        }

        [HttpPost("change-password")]
        [Permission(nameof(Auth.PermissionsAdmin.Users_ChangePassword))]
        public async Task<ResponseResult<bool>> ChangePassword(string userId, string oldPassword, string newPassword)
        {
            var result = await _authService.ChangePasswordAsync(userId, oldPassword, newPassword);
            return result;
        }

        [HttpGet("get-user-by-id")]
        [Permission(nameof(Auth.PermissionsAdmin.Users_Get))]
        public async Task<ResponseResult<AppUserDTO>> GetUserById(string userId)
        {
            var user = await _authService.GetUserByIdAsync(userId);
            return user;
        }

        [HttpPut("update-user")]
        [Permission(nameof(Auth.PermissionsAdmin.Users_Update))]
        public async Task<ResponseResult<AppUserUpdateDTO>> UpdateUser(AppUserUpdateDTO user)
        {
            var updatedUser = await _authService.UpdateUserAsync(user);
            return updatedUser;
        }

        [HttpDelete("delete-user")]
        [Permission(nameof(Auth.PermissionsAdmin.Users_Delete))]
        public async Task<ResponseResult<AppUserDTO>> DeleteUser(string userId)
        {
            var deletedUser = await _authService.DeleteUserAsync(userId);
            return deletedUser;
        }
    }
}
