using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using coursesSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace coursesSystem.repository
{
    public interface IRepository
    {
        void saveChanges();
        List<AppUserCourse> studentsIdsListByCourseId(int courseId);
        List<Course> GetAllCourses();
        void AddCourse(Course course);
        Course GetCourseIncludeClassesByName(string courseName);
        Course GetCourseIncludeStudentsByName(string courseName);
        Task<List<Class>> GetClassesIncludeAttendancesByCourseIdAsync(int courseId);
        void RemoveCourse(Course course);
        Task<List<Attendance>> GetAttendancesIncludeClassesByStudentNameAsync(string studentName);
        Task<Attendance> GetAttendanceByIdAsync(int id);
        Task<List<Class>> GetClassesByCourseNameAsync(string courseName);
        Task<List<Attendance>> GetAttendancesIncludeClassByClassIdAsync(int id);
        Task<List<string>> GetCourseStudents(string courseName);

        Task<AppUser> GetUserByNameAsync(string name);
        Task<IdentityResult> CreateUserAndGetResultAsync(AppUser user, string password);
        Task RemoveUserPasswordAsync(AppUser user);
        Task<IdentityResult> AddPasswordAsync(AppUser user, string password);
        Task<AppUser> GetCurrentLoggedInUserAsync(ClaimsPrincipal User);
        Task<AppUser> GetUserByIdAsync(string id);
        Task<Boolean> CheckPasswordAsync(AppUser user, string password);
        IEnumerable<AppUser> GetAllUsers();
        Task<IdentityResult> DeleteUserAndGetResult(AppUser user);

    }
}
