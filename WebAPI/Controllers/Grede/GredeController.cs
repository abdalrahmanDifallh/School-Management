using Core;
using Core.Domains;
using Core.Enums;
using Data.DTOs.Grade;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.WebAPI.Filters;
using Services.Grade;
using Syncfusion.EJ2.Base;
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
        [Permission(nameof(Auth.PermissionsStudent.Grades_Get_by_studentId))]
        public async Task<ResponseResult<List<GradeViewDto>>> GetAllGredsForStudent(string studentId)
            
         {
            var greds = await _gradeService.GetAllGredesForStudentAsync(studentId);
            return greds;
         }



        [HttpGet("get-all-grades-by-teacherId")]
        [Permission(nameof(Auth.PermissionsTeacher.Grades_Get_by_TeacherId))]
        public async Task<PagedListResult<GradeAllViewDto>> GetAllGredesForTeacher(DataManagerRequest dm , string teacharId)

        {
            var greds = await _gradeService.GetAllGradesForTeacherIdAsync(dm ,teacharId);
            return greds;
        }

        [HttpGet("get-all-grades-by-teacherId-v2")]
        [Permission(nameof(Auth.PermissionsTeacher.Grades_Get_by_TeacherId))]
        public async Task<ResponseResult<List<GradeAllViewDto>>> GetAllGredesForTeacher( string teacharId)

        {
            var greds = await _gradeService.GetAllGradesForTeacherIdAsync( teacharId);
            return greds;
        }


        [HttpGet("get-all-grades-by-teacherId-And-subjectId")]
        [Permission(nameof(Auth.PermissionsTeacher.Grades_Get_by_TeacherId_and_subjectId))]
        public async Task<ResponseResult<List<GradeAllViewDto>>> GetAllGredesForTeacherAndSubject(string teacherId, int subjectId)

        {
            var greds = await _gradeService.GetAllGradesForTeacherIdAndSubjectIdAsync(teacherId, subjectId);
            return greds;
        }


        [HttpPut("put-grade-by-gradeI")]
        [Permission(nameof(Auth.PermissionsTeacher.Grades_Update))]
        public async Task<ResponseResult<GradeNewDto>> PutGrade(GradeEditDto gradeEditDto)
        {
            var gradeEdited = await _gradeService.PutGredesAsync(gradeEditDto);
            return gradeEdited;
        }

        [HttpGet("get-average-score")]
        [Permission(nameof(Auth.PermissionsAdmin.Subjects_Get))]
        public async Task<ResponseResult<float>> GetAverageScore(int year)
        {
            var resulte = await _gradeService.GetAverageScoreByYear(year);
            return resulte;
        }

        [HttpGet("get-average-score-by-teacherId")]
        [Permission(nameof(Auth.PermissionsTeacher.Grades_Get_by_TeacherId))]
        public async Task<ResponseResult<float>> GetAverageScoreByTeacherId(string teacherId , int year)
        {
            var resulte = await _gradeService.GetAverageScoreByTeacherId(teacherId , year);
            return resulte;
        }

        //GetNumberOfClassesAsync



    }
}
