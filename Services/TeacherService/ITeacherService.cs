using Core;
using Core.Domains;
using Data.DTOs;
using Syncfusion.EJ2.Base;
using static Data.DTOs.Teacher.TeacherDto;

namespace Services.TeacherService
{
    public interface ITeacherService
    {
        Task<ResponseResult<List<AppUserViewDTO>>> GetAllTeacherAsync();

        Task<PagedListResult<AppUserViewDTO>> GetTeachersAsync(DataManagerRequest dm);
        Task<ResponseResult<int>> GetNumberOfTeacherAsync();
    }
}
