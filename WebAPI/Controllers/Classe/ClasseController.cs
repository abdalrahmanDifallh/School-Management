using Core;
using Core.Domains;
using Core.Enums;
using Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.WebAPI.Filters;
using SchoolManagement.Models;
using Services.Classes;
using Services.StudentService;
using Syncfusion.EJ2.Base;
using static Data.DTOs.Classe.ClasseDto;
using static Data.DTOs.Student.StudentDto;

namespace WebAPI.Controllers.Classe
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClasseController : ControllerBase
    {
        private readonly IClasseService _classeService;
        public ClasseController(IClasseService classeService)
        {
            _classeService = classeService;
        }
        //|| string.IsNullOrWhiteSpace(request.TeacherUserId))
        [HttpPost("create-classe")]
        [Permission(nameof(Auth.PermissionsAdmin.Classrooms_Create))]
        public async Task<ActionResult<Classroom>> CreateStudent(ClasseCreatDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) )
              
            {
                return BadRequest("Name are  required ");
            }
            if (string.IsNullOrWhiteSpace(request.AcademicYear))

            {
                return BadRequest("AcademicYear are required ");
            }
            
            var user = await _classeService.CreatClasseAsync(request);
            if (user is null)
            {
                return BadRequest("Clsses already exists.");
            }
            return Ok(user);
        }

        [HttpGet("get-all-classes")]
        [Permission(nameof(Auth.PermissionsAdmin.Classrooms_Get))]
        public async Task<PagedListResult<ClasseDTO>> GetAllClasses(DataManagerRequest dm)
        {

            var result = await _classeService.GetAllClassesAsync(dm);
            return result;
        }

        [HttpDelete("delete-classe")]
        [Permission(nameof(Auth.PermissionsAdmin.Classrooms_Delete))]
        public async Task<ResponseResult<ClasseDTO>> DeleteClasse(int classeId)
        {
            var deletedClasse = await _classeService.DeleteClasseAsync(classeId);
            return deletedClasse;
        }

        [HttpGet("get-classe-by-id")]
        [Permission(nameof(Auth.PermissionsAdmin.Classrooms_Get_by_Id))]
        public async Task<ResponseResult<ClasseDTO>> GetClasseById(int classeId)
        {
            var classe = await _classeService.GetClasseByIdAsync(classeId);
            return classe;
        }

        [HttpPut("update-classe")]
        [Permission(nameof(Auth.PermissionsAdmin.Classrooms_Update))]
        public async Task<ResponseResult<ClasseUpdateDTO>> UpdateClasse(ClasseUpdateDTO classe)
        {
            var updatedClasse = await _classeService.UpdateClassetAsync(classe);
            return updatedClasse;
        }


        [HttpGet("get-number-classes")]
        [Permission(nameof(Auth.PermissionsAdmin.Classrooms_Get))]
        public async Task<ResponseResult<int>> GetNumberOfClasses( )
        {

            var result = await _classeService.GetNumberOfClassesAsync();
            return result;
        }

        [HttpGet("get-name-class-by-TeacherId")]
        [Permission(nameof(Auth.PermissionsTeacher.Grades_Get_by_TeacherId))]
        public async Task<ResponseResult<string>> GetNameOfClassesByTeacherId(string teacherId)
        {

            var result = await _classeService.GetClasseNameByTeacherIdAsync(teacherId);
            return result;
        }

    }
}
