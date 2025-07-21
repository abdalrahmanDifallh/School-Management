using Core.Domains;
using Data.DTOs;
using SchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.DTOs.Classe.ClasseDto;
using static Data.DTOs.Student.StudentDto;

namespace Services.Classes
{
    public interface IClasseService
    {
        Task<ResponseResult<List<ClasseDTO>>> GetAllClassesAsync();
        Task<ResponseResult<ClasseCreatDTO>> CreatClasseAsync(ClasseCreatDTO request);

        Task<ResponseResult<ClasseDTO>> GetClasseByIdAsync(int classeId);

        Task<ResponseResult<ClasseDTO>> DeleteClasseAsync(int classeId);

        Task<ResponseResult<ClasseUpdateDTO>> UpdateClassetAsync(ClasseUpdateDTO classe);


    }
}
