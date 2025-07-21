using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs.Teacher
{
    public class TeacherDto
    {
        public class TeacherAllViewDto
        {
            public string Id { get; set; } 

            public string FullName { get; set; }

            public string? PhoneNumber { get; set; }

            public string? Address { get; set; }

            public bool Gender { get; set; }

            public string? Image { get; set; }

            public string? ClassRoomName { get; set; }
           

            

        }
    }
}
