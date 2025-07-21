// Models/RolePermissionMapping.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Models
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        public string Key { get; set; }
        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<RolePermissionMapping> RolePermissionMappings { get; set; } = new List<RolePermissionMapping>();
    }
    public class RolePermissionMapping
    {
       
        public int PermissionId { get; set; }

       
        public string RoleId { get; set; }

        // Navigation properties
        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; } = null!;

        [ForeignKey("RoleId")]
        public virtual ApplicationRole Role { get; set; } = null!;
    }

    public class PermissionRoleMappingConfiguration : IEntityTypeConfiguration<RolePermissionMapping>
    {
        public void Configure(EntityTypeBuilder<RolePermissionMapping> builder)
        {
            builder.HasKey(od => new { od.RoleId, od.PermissionId });
        }
    }

}