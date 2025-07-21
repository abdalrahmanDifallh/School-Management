using Core.Domains;
using Data.DTOs.Subject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;
using Services._Base;
using Services.LoggerService;
using static Data.DTOs.Classe.ClasseDto;



namespace Services.Subject
{
    public class SubjectService : BaseService<SubjectService>, ISubjectService
    {
        private readonly SchoolManagementContext _context;

        public SubjectService(SchoolManagementContext dbContext, ILoggerService<SubjectService> logger, IHttpContextAccessor httpAccessor) : base(dbContext, logger, httpAccessor)
        {
            _context = dbContext;
        }

        public   async Task<ResponseResult<SubjectCreatDTO>> CreatSubjectAsync(SubjectCreatDTO request)
        {
            if (await _context.Subjects.AnyAsync(c => c.Name == request.Name))
            {
                return Error<SubjectCreatDTO>("Name Subject already exists. ");
            }

            try
            {
                var subject = new Subjects
                {
                    Name = request.Name,
                  
                };

                await _context.Subjects.AddAsync(subject);
                await _context.SaveChangesAsync();

                return Success(request);

            }
            catch (Exception ex)
            {
                return Error<SubjectCreatDTO>(ex);
            }
        }

        public async Task<ResponseResult<SubjectDTO>> DeleteSubjectAsync(int subjectId)
        {
            try
            {
                var existingSubject = await _context.Subjects.FirstOrDefaultAsync(x => x.Id == subjectId);
                if (existingSubject is null)
                    return Error<SubjectDTO>("Subject Not Found");

                existingSubject.IsActive = false;
                existingSubject.IsDeleted = true;
              


                _context.Subjects.Update(existingSubject); 
                var result = await _context.SaveChangesAsync();



                if (result <= 0)
                {
                    return Error<SubjectDTO>("Failed to update Subject");
                }
                return Success(new SubjectDTO
                {
                    Id = existingSubject.Id,
                    Name = existingSubject.Name,
                   
                });
            }
            catch (Exception ex)
            {
                return Error<SubjectDTO>(ex);
            }
        }

        public async Task<ResponseResult<List<SubjectDTO>>> GetAllSubjectAsync()
        {
            try
            {

                var subject = await _context.Subjects
                    .Where(u => u.IsActive == true)

                    .Select(s => new SubjectDTO
                    {
                        Id = s.Id,
                        Name = s.Name,
                    })
                    .ToListAsync();

                return Success(subject);
            }
            catch (Exception ex)
            {
                return Error<List<SubjectDTO>>(ex);
            }
        }

        public async Task<ResponseResult<SubjectDTO>> GetSubjectByIdAsync(int subjectId)
        {

            try
            {
                var subject = await _context.Subjects
                              // تضمين المستخدمين
                             .FirstOrDefaultAsync(x => x.Id == subjectId);

                if (subject is null)
                    return Error<SubjectDTO>("subject Not Found");



                var result = new SubjectDTO
                {
                    Id = subject.Id,
                    Name = subject.Name,
                 };

                return Success(result);

            }
            catch (Exception ex)
            {
                return Error<SubjectDTO>(ex);
            }
        }

        public async Task<ResponseResult<SubjectDTO>> UpdateSubjectAsync(SubjectDTO subject)
        {
            try
            {
                var existingSubject = await _context.Subjects.FirstOrDefaultAsync(x => x.Id == subject.Id);
                if (existingSubject is null)
                    return Error<SubjectDTO>("Subject Not Found");


                existingSubject.Name = subject.Name;
               




                _context.Subjects.Update(existingSubject); // Mark entity as modified
                var result = await _context.SaveChangesAsync();


                if (result <= 0)
                {
                    return Error<SubjectDTO>("Failed to update Subject");
                }



                return Success(new SubjectDTO
                {

                    Id = existingSubject.Id,
                    Name = existingSubject.Name,
                   

                });
            }
            catch (Exception ex)
            {
                return Error<SubjectDTO>(ex);
            }
        }
    }
}