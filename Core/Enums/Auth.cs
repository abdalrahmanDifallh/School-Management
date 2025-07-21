using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public class Auth
    {
        public enum PermissionsAdmin
        {
            // SuperAdmin
            SuperAdmin,


            // Users
            Users_Get,
            Users_Create,
            Users_Update,
            Users_Delete,
            Users_Activate,
            Users_Deactivate,
            Users_ChangePassword,

            // Classrooms
            Classrooms_Get,
            Classrooms_Get_by_Id,
            Classrooms_Create,
            Classrooms_Update,
            Classrooms_Delete,

            // Subjects
            Subjects_Get,
            Subjects_Create,
            Subjects_Update,
            Subjects_Delete,
            Subjects_Get_By_Id,



            // Grades
            Grades_Get,
            Grades_Get_by_studentId,
            Grades_Get_by_TeacherId,
            Grades_Get_by_TeacherId_and_subjectId,
            Grades_Create,
            Grades_Update,
            Grades_Delete
        }

        public enum PermissionsTeacher
        {
            Grades_Get_by_TeacherId,
            Grades_Get_by_TeacherId_and_subjectId,
            Grades_Update,
            Grades_Create,
        }

        public enum PermissionsStudent
        {
            Grades_Get_by_studentId,
        }
    }
}
