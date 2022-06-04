using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using coursesSystem.Models;
using coursesSystem.Dtos;

namespace coursesSystem.repository
{
    public interface IAttendancesRepository
    {
        Task<List<AttendanceDto>> GetStudentAttendancesAsync( string studentId , int courseId);
        Task<bool> EditAttendanceAsync( int AttId, bool Present, string reason, ClaimsPrincipal User);
        Task AddAttendancesOfStudentToCourseAsync(string studentId, int courseId);
        Task<List<AttendanceDto>> GetClassAttendances(int classId);
        //Task DeleteStudentAttendances(string studentName);
        Task<AttendanceDto> GetAttendanceAsync(int AttendanceId);
        Task<bool> DeleteAttendancesOfStudentFromCourseAsync(string studentId, int courseId);


    }
}
