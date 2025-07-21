using Core.Domains;
using Data.DTOs.Grade;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Grade;
using static Data.DTOs.Grade.GradeDTO;

namespace WebAPI.Controllers.Gred
{
    [Route("api/[controller]")]
    [ApiController]
    public class GredeController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GredeController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }
        
       

        [HttpGet("get-all-grades-by-id-student")]
        public async Task<ResponseResult<List<GradeViewDto>>> GetAllGredsForStudent(string studentId)
            
         {
            var greds = await _gradeService.GetAllGredesForStudentAsync(studentId);
            return greds;
         }

        [HttpGet("get-all-grades-by-teacherId")]
        public async Task<ResponseResult<List<GradeAllViewDto>>> GetAllGredesForTeacher(string teacharId)

        {
            var greds = await _gradeService.GetAllStudentForTeacherIdAsync(teacharId);
            return greds;
        }

        [HttpGet("get-all-grades-by-teacherId-And-subjectId")]
        public async Task<ResponseResult<List<GradeAllViewDto>>> GetAllGredesForTeacherAndSubject(string teacherId, int subjectId)

        {
            var greds = await _gradeService.GetAllStudentForTeacherIdAndSubjectIdAsync(teacherId, subjectId);
            return greds;
        }

        [HttpPut("put-grade-by-gradeI")]
        public async Task<ResponseResult<GradeNewDto>> PutGrade(GradeEditDto gradeEditDto)
        {
            var gradeEdited = await _gradeService.PutGredesAsync(gradeEditDto);
            return gradeEdited;
        }
    }
}
