using Core.Domains;
using Data.DTOs.Grade;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using Services._Base;
using Services.LoggerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.DTOs.Classe.ClasseDto;
using static Data.DTOs.Grade.GradeDTO;

namespace Services.Grade
{
    public class GradeService : BaseService<GradeService>, IGradeService
    {
        private readonly SchoolManagementContext _context;
        public GradeService(SchoolManagementContext dbContext, ILoggerService<GradeService> logger, IHttpContextAccessor httpAccessor) : base(dbContext, logger, httpAccessor)
        {
            _context = dbContext;
        }

        public async Task<ResponseResult<List<GradeViewDto>>> GetAllGredesForStudentAsync(string userId)
        {
            try
            {
                var gredes = await _context.Grades
                    .Where(u => u.IsActive == true)
                    .Where(u => u.ApplicationUserId == userId)
                    .Include(s => s.Subject)
                    .Select(s => new GradeViewDto
                    {
                        SubjectName = s.Subject.Name,
                        AssignedDate = s.AssignedDate,
                        StudentGrad = s.StudentGrad,
                    })
                    .ToListAsync();

                return Success(gredes);
            }
            catch (Exception ex)
            {
                return Error<List<GradeViewDto>>(ex);
            }
        }

        public async Task<ResponseResult<List<GradeAllViewDto>>> GetAllStudentForTeacherIdAndSubjectIdAsync(string teacherId, int subjectId)
        {
            try
            {
                var grades = await _context.Grades
                    .Where(g => g.IsActive == true)
                    .Where(g => g.Classroom.TeacherUserId == teacherId)
                    .Where(g => g.SubjectId == subjectId)
                    .Include(g => g.ApplicationUser) // لجلب بيانات الطالب
                    .Include(g => g.Classroom)      // لجلب بيانات الفصل
                    .Select(g => new GradeAllViewDto
                    {
                        Id = g.ApplicationUser.Id,
                        StudentName = g.ApplicationUser.FullName,
                        Classe = g.Classroom.Name,
                        Gender = g.ApplicationUser.Gender,
                        image = g.ApplicationUser.Image,
                        AverageGrade = (int)g.StudentGrad // تعيين StudentGrad مباشرة
                    })
                    .ToListAsync();

                return Success(grades);
            }
            catch (Exception ex)
            {
                return Error<List<GradeAllViewDto>>(ex);
            }
        }

        public async Task<ResponseResult<List<GradeAllViewDto>>> GetAllStudentForTeacherIdAsync(string teacharId)
        {
            try
            {
                var grades = await _context.Grades
                    .Where(g => g.IsActive == true)
                    .Where(g => g.Classroom.TeacherUserId == teacharId)
                    .GroupBy(g => new { g.ApplicationUser.Id, g.ApplicationUser.FullName, g.ApplicationUser.Gender, g.Classroom.Name, g.ApplicationUser.Image })
                    .Select(g => new GradeAllViewDto
                    {
                        image = g.Key.Image,
                        StudentName = g.Key.FullName,
                        Id = g.Key.Id,
                        Classe = g.Key.Name,
                        Gender = g.Key.Gender,
                        AverageGrade = (int)(g.Any() ? (double)g.Sum(x => x.StudentGrad) / 10.0 : 0) // Calculate average grade
                    })
                    .ToListAsync();

                return Success(grades);
            }
            catch (Exception ex)
            {
                return Error<List<GradeAllViewDto>>(ex);
            }
        }

        public async Task<ResponseResult<GradeNewDto>> PutGredesAsync(GradeEditDto gradeEditDto)
        {
            try
            {
                var existingGrade = await _context.Grades.FirstOrDefaultAsync(x => x.Id == gradeEditDto.Id);
                if (existingGrade is null)
                    return Error<GradeNewDto>("Grade Not Found");


                existingGrade.StudentGrad = gradeEditDto.StudentGrad;
                existingGrade.AssignedDate = DateTime.UtcNow;




                _context.Grades.Update(existingGrade); // Mark entity as modified
                var result = await _context.SaveChangesAsync();


                if (result <= 0)
                {
                    return Error<GradeNewDto>("Failed to update Grade");
                }



                return Success(new GradeNewDto
                {

                    Id = existingGrade.Id,
                    StudentGrad = existingGrade.StudentGrad,
                    NowDate = existingGrade.AssignedDate,


                });
            }
            catch (Exception ex)
            {
                return Error<GradeNewDto>(ex);
            }
        }
    }


}
