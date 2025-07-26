using SchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTOs
{
    public class AppUserDTO1
    {

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string Token { get; set; }

        

    }
    public class AppUserDTO
    {

        
        public string FullName { get; set; } 
        public string Email { get; set; } 
        public string PasswordHash { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool Gender { get; set; }
        public string? Image { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public int? ClassRoomId { get; set; }
        public string? UserName { get; set; }
        //public string RoleId { get; set; }

    }

    public class AppUserViewDTO
    {

        public string Id { get; set; } 

        public string FullName { get; set; }
       
        
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public string? Image { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string ClassName { get; set; }
        
       public double  AverageGrade { get; set; }

     

    }



    public class AppUserUpdateDTO
    {
        public string Id { get; set; } 
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool Gender { get; set; }
        public string? Image { get; set; }
        public DateOnly? DateOfBirth { get; set; }
    }
    //public class AppUserCreateTeacherDTO
    //{

    //    public string Id { get; set; } // Changed to string
    //    public string FullName { get; set; } = string.Empty;
    //    public string Email { get; set; } = string.Empty;
    //    public string PasswordHash { get; set; } = string.Empty;
    //    public string? PhoneNumber { get; set; }
    //    public string? Address { get; set; }
    //    public DateTime CreatedOnUtc { get; set; }


    //}

    public class LoginDto
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }


    public sealed class RoleCreateDTO
    {
        
        public string Name { get; set; }

        public List<int> Permissions { get; set; }

    }

    public sealed class RoleUpdateDTO
    {
        public  string Id { get; set; } 
        public string Name { get; set; }

        public List<int> Permissions { get; set; }

    }
    public sealed class RoleDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<PermissionDTO> Permissions { get; set; }
    }
    public sealed class PermissionDTO
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string DisplayName { get; set; }
    }

}
