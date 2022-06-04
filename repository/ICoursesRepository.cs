using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using coursesSystem.Models;
using coursesSystem.Dtos;

namespace coursesSystem.repository

{
    public interface ICoursesRepository
    {
        Task<Course> GetCourseAsync(string name);
        Task<List<Course>> GetCoursesAsync();
        Task<Course> CreateNewCourse(newCourseData newCourse);
        Task<bool> DeleteCourse(int id);
        Task<List<AppUserDto>> GetCourseStudentsListAsync(int courseId);
        Task<bool> DeleteAppUserCoursesAsync(string userId, int courseId);
        Course GetCourseIncludeStudentsByName(string courseName);
        Task<bool> PlaceStudentInCourseAsync(int courseId, string studentId);
        Course GetCourseIncludeStudentsById(int courseId);
        void saveChanges();
        Task<List<CourseDto>> GetStudentCoursesAsync(string studentId);
    }
}
