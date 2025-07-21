using SchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Grade
{
    public class GradeDTO
    {
        public class GradeViewDto
        {

            public decimal StudentGrad { get; set; }

            public DateTime AssignedDate { get; set; }

            public string SubjectName { get; set; }

        }
        public class GradeEditDto
        {
            public int Id { get; set; }

            public int StudentGrad { get; set; }
      
        }
        public class GradeNewDto
        {
            public int Id { get; set; }
            public int StudentGrad { get; set; }
            public DateTime NowDate { get; set; }
        }
        public class GradeAllViewDto
        {
            public string image { get; set; }
            public string StudentName { get; set; }
            public string Id { get; set; }
            public string Classe { get; set; }
            public bool Gender { get; set; }
            public int AverageGrade { get; set; }
    
        }
    }
}
