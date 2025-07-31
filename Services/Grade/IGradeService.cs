using Core;
using Core.Domains;
using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.DTOs.Grade.GradeDTO;

namespace Services.Grade
{
    public interface IGradeService
    {
        Task<ResponseResult<List<GradeViewDto>>> GetAllGredesForStudentAsync(string userId);

        Task<PagedListResult<GradeAllViewDto>> GetAllGradesForTeacherIdAsync(DataManagerRequest dm, string teacharId);
        Task<ResponseResult<List<GradeAllViewDto>>> GetAllGradesForTeacherIdAsync(string teacharId);
        Task<ResponseResult<List<GradeAllViewDto>>> GetAllGradesForTeacherIdAndSubjectIdAsync(string teacherId, int subjectId);
        Task<ResponseResult<GradeNewDto>> PutGredesAsync(GradeEditDto gradeEditDto);

        Task<ResponseResult<float>> GetAverageScoreByYear(int year);
        Task<ResponseResult<float>> GetAverageScoreByTeacherId(string teacharId, int year);


    }
}