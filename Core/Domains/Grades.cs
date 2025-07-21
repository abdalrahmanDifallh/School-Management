// Models/Grades.cs
using Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Models
{
    public class Grades : BaseEntity
    {
       

        public int StudentGrad { get; set; }

        public DateTime AssignedDate { get; set; }

        public string ApplicationUserId { get; set; } = string.Empty;

        public int SubjectId { get; set; }

        public int ClassroomId { get; set; }

        // Navigation properties
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        [ForeignKey("SubjectId")]
        public virtual Subjects Subject { get; set; } = null!;

        [ForeignKey("ClassroomId")]
        public virtual Classroom Classroom { get; set; } = null!;
    }
}