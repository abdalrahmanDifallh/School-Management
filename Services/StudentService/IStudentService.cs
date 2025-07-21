

using Core.Domains;
using Data.DTOs;
using static Data.DTOs.Student.StudentDto;

namespace Services.StudentService
{
    public interface IStudentService
    {
        Task<ResponseResult<List<StudentAllViewDto>>> GetAllStudentAsync();

       
    }
}
