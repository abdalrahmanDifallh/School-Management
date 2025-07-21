using Core.Domains;
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

        Task<ResponseResult<List<GradeAllViewDto>>> GetAllStudentForTeacherIdAsync(string teacharId);

        Task<ResponseResult<List<GradeAllViewDto>>> GetAllStudentForTeacherIdAndSubjectIdAsync(string teacherId, int subjectId);

        Task<ResponseResult<GradeNewDto>> PutGredesAsync(GradeEditDto gradeEditDto);


    }
}