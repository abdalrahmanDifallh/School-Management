using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Required for IdentityDbContext
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;

namespace SchoolManagement.Data
{
    public class SchoolManagementContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public SchoolManagementContext(DbContextOptions<SchoolManagementContext> options)
            : base(options)
        {
        }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermissionMapping> RolePermissionMappings { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Subjects> Subjects { get; set; }
        public DbSet<Grades> Grades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Important: Call base to configure Identity entities
            modelBuilder.ApplyConfiguration(new PermissionRoleMappingConfiguration());

            // Configure ApplicationUser-Classroom relationship
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(au => au.Classroom)
                .WithMany(c => c.ApplicationUsers)
                .HasForeignKey(au => au.ClassRoomId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure Classroom-Teacher relationship
            modelBuilder.Entity<Classroom>()
                .HasOne(c => c.Teacher)
                .WithMany()
                .HasForeignKey(c => c.TeacherUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}