using Core.Domains;
using Data.DTOs.Subject;
using SchoolManagement.Models;
using static Data.DTOs.Classe.ClasseDto;


namespace Services.Subject
{
    public interface ISubjectService
    {
        Task<ResponseResult<SubjectCreatDTO>> CreatSubjectAsync(SubjectCreatDTO request);
        Task<ResponseResult<List<SubjectDTO>>> GetAllSubjectAsync();

        Task<ResponseResult<SubjectDTO>> GetSubjectByIdAsync(int subjectId);

        Task<ResponseResult<SubjectDTO>> DeleteSubjectAsync(int subjectId);

        Task<ResponseResult<SubjectDTO>> UpdateSubjectAsync(SubjectDTO subject);

    }
}
