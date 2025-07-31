
using Core;
using Core.Domains;
using Data.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;
using Services._Base;
using Services.LoggerService;
using Services.SyncGridOperations;
using Syncfusion.EJ2.Base;
using static Data.DTOs.Classe.ClasseDto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace Services.Classes
{
    public class ClasseService : BaseService<ClasseService>, IClasseService
    {
        private readonly SchoolManagementContext _context;
        private readonly UserManager<ApplicationUser>  _userManager;

        public ClasseService(SchoolManagementContext dbContext, ILoggerService<ClasseService> logger, IHttpContextAccessor httpAccessor , UserManager<ApplicationUser> userManager) : base(dbContext, logger, httpAccessor)
        {
            _context = dbContext;
            _userManager = userManager;
        }

        public async Task<ResponseResult<ClasseCreatDTO>> CreatClasseAsync(ClasseCreatDTO request)
        {

            if (await _context.Classrooms.AnyAsync(c => c.Name == request.Name))
            {
                return Error<ClasseCreatDTO>("Name Classe already exists. ");
            }

            
            if (await _context.Classrooms.AnyAsync(c => c.AcademicYear == request.AcademicYear))
            {
                return Error<ClasseCreatDTO>("AcademicYear Classe already exists. ");
            }

            
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
                   

                });
            }
            catch (Exception ex)
            {
                return Error<ClasseDTO>(ex);
            }
        }

        public async Task<PagedListResult<ClasseDTO>> GetAllClassesAsync(DataManagerRequest dm)
        {
           
            try
            {
                var classes = _context.Classrooms
                    .Where(u => u.IsActive == true)

                    .Include(s => s.Teacher)
                    .Include(s => s.ApplicationUsers)
                    .Select(s => new ClasseDTO
                    {
                        Id = s.Id,
                        Name = s.Name,
                        AcademicYear = s.AcademicYear,
                        ClassTeacher = s.Teacher.FullName,
                        ImageOfTeacher = s.Teacher.Image,
                        NumberOfStudent = _userManager.Users.Count(u => u.Classroom.Id == s.Id)
                    });

                var result = await SyncGridOperations<ClasseDTO>.PagingAndFilterAsync(classes, dm);
                return result;
            }
            catch (Exception ex)
            {
                return new PagedListResult<ClasseDTO>();
            }
        }

        public async Task<ResponseResult<List<ClasseDTO>>> GetAllClassesAsync()
        {
            try
            {
                var classes = await _context.Classrooms
                    .Where(u => u.IsActive == true)
                    .Include(s => s.Teacher)
                    .Include(s => s.ApplicationUsers)
                    .Select(s => new ClasseDTO
                    {
                        Id = s.Id,
                        Name = s.Name,
                        AcademicYear = s.AcademicYear,
                        ClassTeacher = s.Teacher.FullName,
                        ImageOfTeacher = s.Teacher.Image,
                        NumberOfStudent = _userManager.Users.Count(u => u.Classroom.Id == s.Id)
                    }).ToListAsync();

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
                    ImageOfTeacher = classe.TeacherUserId == null ? null : classe.Teacher.Image,
                    NumberOfStudent = classe.ApplicationUsers.Count == 0 ? 0 : classe.ApplicationUsers.Count - 1
                };

                return Success(result);

            }
            catch (Exception ex)
            {
                return Error<ClasseDTO>(ex);
            }
        }

        public async Task<ResponseResult<string>> GetClasseNameByTeacherIdAsync(string teacherId)
        {


            var nameClass = await _context.Classrooms
                .Where(s => s.TeacherUserId == teacherId)
                .Select(s => s.Name)
                .FirstOrDefaultAsync();
                
                

            return new ResponseResult<string> { Data = nameClass, IsSuccess = true };
        }

        public async Task<ResponseResult<int>> GetNumberOfClassesAsync()
        {
            

            var count = await _context.Classrooms
                      .CountAsync();

            return new ResponseResult<int> { Data = count, IsSuccess = true };
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
