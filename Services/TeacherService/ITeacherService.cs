using Core.Domains;

using static Data.DTOs.Teacher.TeacherDto;

namespace Services.TeacherService
{
    public interface ITeacherService
    {
        Task<ResponseResult<List<TeacherAllViewDto>>> GetAllTeacherAsync();
    }
}
