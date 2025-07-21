using SchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Student
{
    public class StudentDto
    {
        public class StudentAllViewDto
        {
            public string Id { get; set; } = Guid.NewGuid().ToString(); // Changed to string

            public string FullName { get; set; } 

            public string? PhoneNumber { get; set; }

            public string? Address { get; set; }

            public bool Gender { get; set; }

            public string? Image { get; set; }

  
           
            public string? ClassRoomName { get; set; }
           

            [NotMapped] // هذا يخبر Entity Framework أن هذا الحقل غير موجود في قاعدة البيانات
            public double AverageGrade { get; set; }
            //{
            //    get
            //    {
            //        if (Grades == null || !Grades.Any())
            //            return 0;

            //        // حساب مجموع العلامات مقسوم على عدد المواد (10)
            //        var totalGrades = Grades.Sum(g => g.StudentGrad); // افتراض أن Grade هو اسم الحقل
            //        return (double)(totalGrades / 10); // القسمة على 10 مواد
            //    }
            //}

        }
      
    }
}
