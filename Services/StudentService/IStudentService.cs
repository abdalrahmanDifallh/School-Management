

using Core;
using Core.Domains;
using Data.DTOs;
using Syncfusion.EJ2.Base;
using static Data.DTOs.Student.StudentDto;

namespace Services.StudentService
{
    public interface IStudentService
    {
       // Task<ResponseResult<List<StudentAllViewDto>>> GetAllStudentAsync();
        Task<PagedListResult<AppUserViewDTO>> GetStudentsAsync(DataManagerRequest dm);
        Task<ResponseResult<int>> GetNumberOfStudentsAsync();
        Task<ResponseResult<int>> GetNumberOfStudentsByTeacherIdAsync(string teacherId);
        Task<ResponseResult<int>> GetNumberOfStudentsByGenderAsync(bool gender);
        Task<ResponseResult<int>> GetNumberOfStudentsByGenderAndByTeacherIdAsync(bool gender , string teacherId);
        Task<ResponseResult<int>> GetNumberOfStudentsByStatusAsync(bool status);
        Task<ResponseResult<int>> GetNumberOfStudentsByStatusAndByTeacherIdAsync(bool status, string teacherId);

    }
}
