
using Core.Domains;
using Data.DTOs;

namespace Services.AuthenticationService.UserService
{
    public interface IUserService
    {
        //   Task<ResponseResult<bool>> ActivateUserAsync(string userId);
         Task<ResponseResult<bool>> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
         Task<ResponseResult<AppUserDTO>> CreateStudentAsync(AppUserDTO user);
         Task<ResponseResult<AppUserDTO>> CreateTeacherAsync(AppUserDTO user);
     //   Task<ResponseResult<bool>> DeactivateUserAsync(string userId);
          Task<ResponseResult<AppUserDTO>> DeleteUserAsync(string userId);
         Task<ResponseResult<AppUserDTO>> GetUserByIdAsync(string userId);
        //  Task<PagedListResult<AppUserDTO>> GetUsersAsync(DataManagerRequest dm);
        Task<ResponseResult<AppUserUpdateDTO>> UpdateUserAsync(AppUserUpdateDTO user);

    }
}
