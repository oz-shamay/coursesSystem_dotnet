using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using coursesSystem.Models;

namespace coursesSystem.repository
{
    public class Repository : IDisposable,IRepository
    {
        public Repository(Context ctx, UserManager<AppUser> usrmngr)
        {
            context = ctx;
            userManager = usrmngr;
        }
        Context context;
        UserManager<AppUser> userManager;


        public void saveChanges()
        {
            context.SaveChanges();
        }

        public List<AppUserCourse> studentsIdsListByCourseId(int courseId)
        {
            var studentsId = context.UserCourses.Where(s => s.CourseId == courseId).ToList();
            return studentsId;
        }

        public List<Course> GetAllCourses()
        {
            var courses = context.Courses.ToList();
            return courses;
        }

        public void AddCourse(Course course)
        {
            context.Courses.Add(course);
        }

        public Course GetCourseIncludeClassesByName(string courseName)
        {
            Course c = context.Courses.Include(c => c.classes).SingleOrDefault(c => c.courseName == courseName);
            return c;
        }

        public Course GetCourseIncludeStudentsByName(string courseName)
        {
            Course course = context.Courses.Include(c => c.StudentsList).SingleOrDefault(c => c.courseName == courseName);
            return course;
        }

        public async Task<List<Class>> GetClassesIncludeAttendancesByCourseIdAsync(int courseId)
        {
            var classes = await context.Classes.Include(c => c.attendances).Where(c => c.CourseId == courseId).ToListAsync();
            return classes;
        }

        public void RemoveCourse(Course course)
        {
            context.Courses.Remove(course);
        }

        public async Task<List<Attendance>> GetAttendancesIncludeClassesByStudentNameAsync(string studentName)
        {
            var Attendances = await context.Attendances.Include(a => a.Class)
                .Where(a => a.studentName == studentName).OrderBy(a => a.Class.start).ToListAsync();

            return Attendances;
        }

        public async Task<Attendance> GetAttendanceByIdAsync(int id)
        {
            var attendance = await context.Attendances.FindAsync(id);
            return attendance;
        }

        public async Task<List<Class>> GetClassesByCourseNameAsync(string courseName)
        {
            var Classes = await context.Classes.OrderBy(c => c.start).Where(c => c.courseName == courseName).ToListAsync();
            return Classes;
        }

        public async Task<List<Attendance>> GetAttendancesIncludeClassByClassIdAsync(int id)
        {
            List<Attendance> Attendances = await context.Attendances.Include(a => a.Class)
                .Where(a => a.ClassId == id).ToListAsync();
            return Attendances;
        }

        //------------------------------------- userManager --------------------------------------

        public async Task<AppUser> GetUserByNameAsync(string name)
        {
            AppUser user = await userManager.FindByNameAsync(name);
            return user;
        }

        public async Task<IdentityResult> CreateUserAndGetResultAsync(AppUser user, string password)
        {
            IdentityResult result = await userManager.CreateAsync(user, password);
            return result;
        }

        public async Task RemoveUserPasswordAsync(AppUser user)
        {
            await userManager.RemovePasswordAsync(user);
        }

        public async Task<IdentityResult> AddPasswordAsync(AppUser user, string password)
        {
            IdentityResult result = await userManager.AddPasswordAsync(user, password);
            return result;
        }

        public async Task<AppUser> GetCurrentLoggedInUserAsync(ClaimsPrincipal User)
        {
            var currentUser = await userManager.GetUserAsync(User);
            return currentUser;
        }

        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            return user;
        }

        public async Task<Boolean> CheckPasswordAsync(AppUser user, string password)
        {
            Boolean isCorrectPassword = await userManager.CheckPasswordAsync(user, password);
            return isCorrectPassword;
        }

        public IEnumerable<AppUser> GetAllUsers()
        {
            IEnumerable<AppUser> users = userManager.Users;
            return users;
        }

        public async Task<IdentityResult> DeleteUserAndGetResult(AppUser user)
        {
            IdentityResult result = await userManager.DeleteAsync(user);
            return result;
        }

        //----------------------------------------------------------------------------------------
        public async Task<List<string>> GetCourseStudents(string courseName)
        {
            Course course = await context.Courses.SingleOrDefaultAsync(c => c.courseName == courseName);

            if (course != null)
            {
                var studentsId = studentsIdsListByCourseId(course.courseId);
                List<string> students = new List<string>();
                foreach (var std in studentsId)
                {
                    AppUser stud = await GetUserByIdAsync(std.AppUserId);
                    students.Add(stud.UserName);
                }
                return students;
            }
            return new List<string>();
        }


        public void Dispose()
        {
            context.Dispose();
        }
    }
}
