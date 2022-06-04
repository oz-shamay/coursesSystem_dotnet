using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using coursesSystem.Models;
using coursesSystem.Dtos;
using AutoMapper;

namespace coursesSystem.repository
{
    public class CoursesRepository : ICoursesRepository
    {
        public CoursesRepository(Context ctx, UserManager<AppUser> usrmngr ,IMapper _mapper)
        {
            mapper = _mapper;
            context = ctx;
            userManager = usrmngr;
        }
        private IMapper mapper;
        private UserManager<AppUser> userManager;
        private Context context;

        public async Task<Course> GetCourseAsync(string name)
        {
            Course c = await context.Courses.SingleOrDefaultAsync(c => c.courseName == name);
            if (c != null)
            {
                return c;
            }
            return new Course();
        }

        public async Task<List<Course>> GetCoursesAsync()
        {
            List<Course> courses = await context.Courses.ToListAsync();
            if (courses == null || courses.Count == 0)
            {
                return new List<Course>();
            }

            return courses;
        }

        public async Task<Course> CreateNewCourse(newCourseData newCourse)
        {
            Course check = await context.Courses.SingleOrDefaultAsync(c => c.courseName == newCourse.courseName);
            if (check == null)
            {
                TimeSpan ts = new TimeSpan(00, 00, 0);
                newCourse.courseStartAt = newCourse.courseStartAt.Date + ts;
                ts = new TimeSpan(23, 59, 0);
                newCourse.courseEndAt = newCourse.courseEndAt.Date + ts;

                Course course = new Course()
                {
                    courseName = newCourse.courseName,
                    courseStartAt = newCourse.courseStartAt,
                    courseEndAt = newCourse.courseEndAt
                };

                await context.Courses.AddAsync(course);
                await context.SaveChangesAsync();
                course = await context.Courses.SingleOrDefaultAsync(c => c.courseName == newCourse.courseName);
                return course;
            }
            return check;
        }

        public async Task<bool> DeleteCourse(int id)
        {
            Course course = await context.Courses.SingleOrDefaultAsync(c => c.courseId == id);
            if (course != null)
            {
                context.Remove(course);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<List<AppUserDto>> GetCourseStudentsListAsync(int courseId)
        {
            Course course = await context.Courses.SingleOrDefaultAsync(c => c.courseId == courseId);

            if (course != null)
            {
                var studentsId = await context.UserCourses.Where(c => c.CourseId == courseId).ToListAsync();
                List<AppUserDto> students = new List<AppUserDto>();
                foreach (var std in studentsId)
                {
                    AppUser stud = await userManager.FindByIdAsync(std.AppUserId);
                    AppUserDto student = mapper.Map<AppUserDto>(stud);
                    students.Add(student);
                }
                return students;
            }
            return new List<AppUserDto>();
        }

        public Course GetCourseIncludeStudentsByName(string courseName)
        {
            Course course = context.Courses.Include(c => c.StudentsList).SingleOrDefault(c => c.courseName == courseName);
            return course;
        }

        public Course GetCourseIncludeStudentsById(int courseId)
        {
            Course course = context.Courses.Include(c => c.StudentsList).SingleOrDefault(c => c.courseId == courseId);
            return course;
        }

        public async Task<bool> PlaceStudentInCourseAsync(int courseId, string studentId)
        {
            AppUserCourse placed = await context.UserCourses.SingleOrDefaultAsync(a => a.AppUserId == studentId && a.CourseId == courseId);
            if(placed == null)
            {
                Course course = await context.Courses.SingleOrDefaultAsync(c => c.courseId == courseId);
                AppUser student = await context.appUsers.SingleOrDefaultAsync(u => u.Id == studentId);
                course.StudentsList.Add(
                        new AppUserCourse
                        {
                            Course = course,
                            AppUser = student
                        });
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAppUserCoursesAsync(string userId, int courseId)
        {
            AppUserCourse appUserCourse = await context.UserCourses.SingleOrDefaultAsync(a => a.CourseId == courseId && a.AppUserId == userId);
            if(appUserCourse == null)
            {
                return false;
            }
            context.UserCourses.Remove(appUserCourse);
            context.SaveChanges();
            return true;
        }

        public async Task<List<CourseDto>> GetStudentCoursesAsync(string studentId)
        {
            var coursesId = await context.UserCourses.Where(c => c.AppUserId == studentId).ToListAsync();
            List<CourseDto> courses = new List<CourseDto>();

            foreach (var courseId in coursesId)
            {
                Course course = await context.Courses.SingleOrDefaultAsync(c => c.courseId == courseId.CourseId);
                CourseDto courseDto = mapper.Map<CourseDto>(course);
                courses.Add(courseDto);
            }
            return courses;
        }

        public void saveChanges()
        {
            context.SaveChanges();
        }

    }
}
