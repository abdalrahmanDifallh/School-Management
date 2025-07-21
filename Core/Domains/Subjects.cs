// Models/Subjects.cs
using Core;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Models
{
    public class Subjects : BaseEntity
    {
       

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<Grades> Grades { get; set; } = new List<Grades>();
    }
}