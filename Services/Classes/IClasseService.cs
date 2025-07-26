using Core;
using Core.Domains;
using Data.DTOs;
using SchoolManagement.Models;
using Syncfusion.EJ2.Base;
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
        Task<PagedListResult<ClasseDTO>> GetAllClassesAsync(DataManagerRequest dm);
        Task<ResponseResult<int>> GetNumberOfClassesAsync();
        Task<ResponseResult<ClasseCreatDTO>> CreatClasseAsync(ClasseCreatDTO request);

        Task<ResponseResult<ClasseDTO>> GetClasseByIdAsync(int classeId);
        Task<ResponseResult<string>> GetClasseNameByTeacherIdAsync(string teacherId);

        Task<ResponseResult<ClasseDTO>> DeleteClasseAsync(int classeId);

        Task<ResponseResult<ClasseUpdateDTO>> UpdateClassetAsync(ClasseUpdateDTO classe);


    }
}
