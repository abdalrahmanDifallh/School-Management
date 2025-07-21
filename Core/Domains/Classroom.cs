// Models/Classroom.cs
using Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Models
{
    public class Classroom : BaseEntity
    {
       
       

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string AcademicYear { get; set; } = string.Empty;

         
        public string? TeacherUserId { get; set; } = string.Empty;
        [ForeignKey("TeacherUserId")]
        public virtual ApplicationUser Teacher { get; set; } = null!; // المدرس المسؤول عن الفصل

        // Navigation properties
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
        public virtual ICollection<Grades> Grades { get; set; } = new List<Grades>();
    }
}
