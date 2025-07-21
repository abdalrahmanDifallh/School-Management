// Models/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
       

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Address { get; set; }

        public bool Gender { get; set; }

        public string? Image {  get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }



        //-------------------------------------------------------
        [ForeignKey("ClassRoomId")]
        public virtual Classroom? Classroom { get; set; }
        public int? ClassRoomId { get; set; }



        [ForeignKey("RoleId")]
        public virtual ApplicationRole? Role { get; set; } // تم إضافة Navigation Property

        public string? RoleId { get; set; }  // This seems to be a string in your ERD




        public virtual ICollection<Grades> Grades { get; set; } = new List<Grades>();
    }
} 