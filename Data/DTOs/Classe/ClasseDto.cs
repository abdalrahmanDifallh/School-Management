using SchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Classe
{
    public class ClasseDto
    {
        public class ClasseDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public string AcademicYear { get; set; }

            public string ClassTeacher { get; set; }

            [NotMapped]
            public int StudentNumber { get; set; }
            public string ImageTeacher { get; set; }
        }
        public class ClasseCreatDTO
        {
            public string Name { get; set; }

            public string AcademicYear { get; set; }

            public string TeacherUserId { get; set; } 
         
        }


        public class ClasseUpdateDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public string AcademicYear { get; set; }

            public string TeacherUserId { get; set; }

        }
    }
}