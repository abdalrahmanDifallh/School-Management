// Models/ApplicationRole.cs
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Models
{
    public class ApplicationRole  : IdentityRole
    {
        public ApplicationRole()   // EF يحتاجه
        {
        }
        public ApplicationRole(string roleName) : base(roleName)
        {
        }

        public bool IsDeleted { get; set; }
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
        public virtual ICollection<RolePermissionMapping> RolePermissionMappings { get; set; } = new List<RolePermissionMapping>();
    }
}