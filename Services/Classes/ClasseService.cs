
using Core.Domains;

using Microsoft.AspNetCore.Http;

using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;
using Services._Base;
using Services.LoggerService;

using static Data.DTOs.Classe.ClasseDto;


namespace Services.Classes
{
    public class ClasseService : BaseService<ClasseService>, IClasseService
    {
        private readonly SchoolManagementContext _context;
        public ClasseService(SchoolManagementContext dbContext, ILoggerService<ClasseService> logger, IHttpContextAccessor httpAccessor) : base(dbContext, logger, httpAccessor)
        {
            _context = dbContext;
        }

        public async Task<ResponseResult<ClasseCreatDTO>> CreatClasseAsync(ClasseCreatDTO request)
        {

            if (await _context.Classrooms.AnyAsync(c => c.Name == request.Name))
            {
                return Error<ClasseCreatDTO>("Name Classe already exists. ");
            }

            // فحص إذا كانت السنة الأكاديمية موجودة
            if (await _context.Classrooms.AnyAsync(c => c.AcademicYear == request.AcademicYear))
            {
                return Error<ClasseCreatDTO>("AcademicYear Classe already exists. ");
            }

            // فحص إذا كان الأستاذ مربوط بصف آخر
            if (await _context.Classrooms.AnyAsync(c => c.TeacherUserId == request.TeacherUserId ))
            {
                return Error<ClasseCreatDTO>("TeacherUser Classe already exists. ");
            }
            try
            {
                var classe = new Classroom
                {
                    Name = request.Name,
                    AcademicYear = request.AcademicYear,
                    TeacherUserId = request.TeacherUserId,

                };
                await _context.Classrooms.AddAsync(classe);
                await _context.SaveChangesAsync();
                return Success(request);

            }
            catch (Exception ex)
            {
                return Error<ClasseCreatDTO>(ex);
            }
        }

        public async Task<ResponseResult<ClasseDTO>> DeleteClasseAsync(int classeId)
        {
            try
            {
                var existingClass = await _context.Classrooms.FirstOrDefaultAsync(x => x.Id == classeId);
                if (existingClass is null)
                    return Error<ClasseDTO>("Classe Not Found");

                existingClass.IsActive = false;
                existingClass.IsDeleted = true;
                existingClass.TeacherUserId = null;
              

                _context.Classrooms.Update(existingClass); // Mark entity as modified
                var result = await _context.SaveChangesAsync();

               

                if (result <= 0)
                {
                    return Error<ClasseDTO>("Failed to update class");
                }
                return Success(new ClasseDTO
                {
                    Id =existingClass.Id,
                    Name = existingClass.Name,
                    AcademicYear = existingClass.AcademicYear,
                    ClassTeacher = existingClass.TeacherUserId,
                    StudentNumber = existingClass.ApplicationUsers.Count == 0 ? 0 : existingClass.ApplicationUsers.Count - 1,

                });
            }
            catch (Exception ex)
            {
                return Error<ClasseDTO>(ex);
            }
        }

        public async Task<ResponseResult<List<ClasseDTO>>> GetAllClassesAsync() 
        {
            try
            {
                var classes = await _context.Classrooms
                    .Where(u => u.IsActive == true)
                    .Include(s => s.Teacher)
                    .Include(s => s.ApplicationUsers) // تضمين بيانات المستخدمين
                    .Select(s => new ClasseDTO
                    {
                        Id = s.Id,
                        Name = s.Name,
                        AcademicYear = s.AcademicYear,
                        ClassTeacher = s.Teacher.FullName,
                        ImageTeacher = s.Teacher.Image,
                        StudentNumber = s.ApplicationUsers.Count == 0 ? 0 : s.ApplicationUsers.Count - 1

                    })
                    .ToListAsync();

                return Success(classes);
            }
            catch (Exception ex)
            {
                return Error<List<ClasseDTO>>(ex);
            }
        }

        public async Task<ResponseResult<ClasseDTO>> GetClasseByIdAsync(int classeId)
        {
            try
            {
                var classe = await _context.Classrooms
                             .Include(x => x.Teacher) // تضمين المعلم
                             .Include(x => x.ApplicationUsers) // تضمين المستخدمين
                             .FirstOrDefaultAsync(x => x.Id == classeId);

                if (classe is null)
                    return Error<ClasseDTO>("UserNotFound");



                var result = new ClasseDTO
                {
                    Id  = classe.Id,
                    Name = classe.Name,
                    AcademicYear = classe.AcademicYear,
                    ClassTeacher = classe.TeacherUserId == null ? null : classe.Teacher.FullName,
                    ImageTeacher = classe.TeacherUserId == null ? null : classe.Teacher.Image,
                    StudentNumber = classe.ApplicationUsers.Count == 0 ? 0 : classe.ApplicationUsers.Count - 1
                };

                return Success(result);

            }
            catch (Exception ex)
            {
                return Error<ClasseDTO>(ex);
            }
        }

        public async Task<ResponseResult<ClasseUpdateDTO>> UpdateClassetAsync(ClasseUpdateDTO classe)
        {
            try
            {
                var existingClasse = await _context.Classrooms.FirstOrDefaultAsync(x => x.Id == classe.Id);
                if (existingClasse is null)
                    return Error<ClasseUpdateDTO>("Class Not Found");


                existingClasse.Name = classe.Name;
                existingClasse.AcademicYear = classe.AcademicYear;
                existingClasse.TeacherUserId = classe.TeacherUserId;
                
               


                _context.Classrooms.Update(existingClasse); // Mark entity as modified
                var result = await _context.SaveChangesAsync();


                if (result <= 0)
                {
                    return Error<ClasseUpdateDTO>("Failed to update class");
                }



                return Success(new ClasseUpdateDTO
                {
                    
                    Id = existingClasse.Id,
                    Name = existingClasse.Name,
                    AcademicYear  = existingClasse.AcademicYear,
                    TeacherUserId = existingClasse.TeacherUserId,
                   

                });
            }
            catch (Exception ex)
            {
                return Error<ClasseUpdateDTO>(ex);
            }
        }
    }
}
