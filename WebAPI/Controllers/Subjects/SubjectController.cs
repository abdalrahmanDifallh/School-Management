using Core.Domains;
using Core.Enums;
using Data.DTOs.Subject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.WebAPI.Filters;
using SchoolManagement.Models;
using Services.Subject;
using static Data.DTOs.Classe.ClasseDto;

namespace WebAPI.Controllers.Subjects
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet("get-all-subject")]
       // [Permission(nameof(Auth.PermissionsAdmin.Subjects_Get))] 
        public async Task<ResponseResult<List<SubjectDTO>>> GetAllSubject()
        {

            var result = await _subjectService.GetAllSubjectAsync();
            return result;
        }

        [HttpPost("create-subject")]
        public async Task<ActionResult<SubjectCreatDTO>> CreatSubjctAsync(SubjectCreatDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) )
            {
                return BadRequest("Name is  required");
            }

            var user = await _subjectService.CreatSubjectAsync(request);
            if (user is null)
            {
                return BadRequest("subject already exists.");
            }
            return Ok(user);
        }

        [HttpDelete("delete-subject")]

        public async Task<ResponseResult<SubjectDTO>> DeleteSubject(int subjectId)
        {
            var deletedSubject = await _subjectService.DeleteSubjectAsync(subjectId);
            return deletedSubject;
        }

        [HttpGet("get-subject-by-id")]
        public async Task<ResponseResult<SubjectDTO>> GetSubjectById(int subjectId)
        {
            var subject = await _subjectService.GetSubjectByIdAsync(subjectId);
            return subject;
        }

        [HttpPut("update-subject")]

        public async Task<ResponseResult<SubjectDTO>> UpdateClasse(SubjectDTO classe)
        {
            var updatedSubject = await _subjectService.UpdateSubjectAsync(classe);
            return updatedSubject;
        }



        [HttpGet("test")]
        [Authorize]
        [Permission(nameof(Auth.PermissionsAdmin.Subjects_Get))] 
        public ActionResult GetSubjectById()
        {
            var subject = "testasdadsdas";
            return Ok(subject);
        }

    }
}
