using Core.Domains;
using Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Models;
using Services.Classes;
using Services.StudentService;
using static Data.DTOs.Classe.ClasseDto;
using static Data.DTOs.Student.StudentDto;

namespace WebAPI.Controllers.Classe
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClasseController : ControllerBase
    {
        private readonly IClasseService _classeService;
        public ClasseController(IClasseService classeService)
        {
            _classeService = classeService;
        }

        [HttpPost("create-classe")]
        public async Task<ActionResult<Classroom>> CreateStudent(ClasseCreatDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.AcademicYear) || string.IsNullOrWhiteSpace(request.TeacherUserId))
            {
                return BadRequest("Name and AcademicYear and TeacherUser are required");
            }

            var user = await _classeService.CreatClasseAsync(request);
            if (user is null)
            {
                return BadRequest("Clsses already exists.");
            }
            return Ok(user);
        }

        [HttpGet("get-all-classes")]
        public async Task<ResponseResult<List<ClasseDTO>>> GetAllClasses()
        {

            var result = await _classeService.GetAllClassesAsync();
            return result;
        }

        [HttpDelete("delete-classe")]

        public async Task<ResponseResult<ClasseDTO>> DeleteClasse(int classeId)
        {
            var deletedClasse = await _classeService.DeleteClasseAsync(classeId);
            return deletedClasse;
        }

        [HttpGet("get-classe-by-id")]
        public async Task<ResponseResult<ClasseDTO>> GetClasseById(int classeId)
        {
            var classe = await _classeService.GetClasseByIdAsync(classeId);
            return classe;
        }

        [HttpPut("update-classe")]

        public async Task<ResponseResult<ClasseUpdateDTO>> UpdateClasse(ClasseUpdateDTO classe)
        {
            var updatedClasse = await _classeService.UpdateClassetAsync(classe);
            return updatedClasse;
        }


    }
}
