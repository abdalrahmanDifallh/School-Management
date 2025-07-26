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
    [Authorize]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet("get-all-subject")]
        [Permission(nameof(Auth.PermissionsAdmin.Subjects_Get))] 
        public async Task<ResponseResult<List<SubjectDTO>>> GetAllSubject()
        {

            var result = await _subjectService.GetAllSubjectAsync();
            return result;
        }

        [HttpPost("create-subject")]
        [Permission(nameof(Auth.PermissionsAdmin.Subjects_Create))]
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
        [Permission(nameof(Auth.PermissionsAdmin.Subjects_Delete))]
        public async Task<ResponseResult<SubjectDTO>> DeleteSubject(int subjectId)
        {
            var deletedSubject = await _subjectService.DeleteSubjectAsync(subjectId);
            return deletedSubject;
        }

        [HttpGet("get-subject-by-id")]
        [Permission(nameof(Auth.PermissionsAdmin.Subjects_Get_By_Id))]
        public async Task<ResponseResult<SubjectDTO>> GetSubjectById(int subjectId)
        {
            var subject = await _subjectService.GetSubjectByIdAsync(subjectId);
            return subject;
        }

        [HttpPut("update-subject")]
        [Permission(nameof(Auth.PermissionsAdmin.Subjects_Update))]
        public async Task<ResponseResult<SubjectDTO>> UpdateClasse(SubjectDTO classe)
        {
            var updatedSubject = await _subjectService.UpdateSubjectAsync(classe);
            return updatedSubject;
        }




    }
}
