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
    public class AttendancesRepository : IAttendancesRepository
    {
        public AttendancesRepository(Context ctx , UserManager<AppUser> usrmngr, IMapper _mapper)
        {
            mapper = _mapper;
            context = ctx;
            userManager = usrmngr;
        }
        private UserManager<AppUser> userManager;
        private Context context;
        private IMapper mapper;

        public async Task AddAttendancesOfStudentToCourseAsync(string studentId, int courseId)
        {
            var classes = await context.Classes.Include(c => c.attendances).Where(c => c.CourseId == courseId).ToListAsync();
            AppUser student = await userManager.FindByIdAsync(studentId);
            foreach (Class cls in classes)
            {
                cls.attendances.Add(new Attendance
                {
                    studentName = student.UserName,
                    reason = "",
                    isAttendance = false,
                    AppUserId = student.Id,
                    ClassId = cls.ClassId,
                    classTime = cls.start
                });
            }
            context.SaveChanges();
        }

        public async Task<bool> DeleteAttendancesOfStudentFromCourseAsync(string studentId , int courseId)
        {
            List<Attendance> studentAttendances = await 
                context.Attendances.Include(a => a.Class).Where(a => a.Class.CourseId == courseId && a.AppUserId == studentId).ToListAsync();
            if(studentAttendances.Count == 0)
            {
                return false;
            }
            foreach(Attendance att in studentAttendances)
            {
                context.Attendances.Remove(att);
            }
            context.SaveChanges();
            return true;
        }


        public async Task<bool> EditAttendanceAsync(int AttId, bool Present, string reason , ClaimsPrincipal User )
        {
            var currentUser = await userManager.GetUserAsync(User);
            var attendance = await context.Attendances.FindAsync(AttId);
            if (attendance == null || currentUser.UserName != attendance.studentName )
            {
                return false;
            }
            attendance.isAttendance = Present;
            attendance.reason = reason;
            if (attendance.isAttendance)
            {
                attendance.reason = null;
            }
                context.SaveChanges();
            return true;
        }

        public async Task<List<AttendanceDto>> GetStudentAttendancesAsync(string studentId , int courseId)
        {
            var Attendances = await context.Attendances.Include(a => a.Class).Where(a => a.AppUserId == studentId && a.Class.CourseId == courseId)
                .OrderBy(a => a.classTime).ToListAsync();

            if (Attendances.Count == 0)
            {
                return new List<AttendanceDto>();
            }

            List<AttendanceDto> attendances = new List<AttendanceDto>();
            foreach (var att in Attendances)
            {
                AttendanceDto attd = mapper.Map<AttendanceDto>(att);
                attendances.Add(attd);
            }

            return attendances;
        }

        public async Task<List<AttendanceDto>> GetClassAttendances(int classId)
        {
            List<Attendance> Attendances = new List<Attendance>();

            Attendances = await context.Attendances.Include(a => a.Class).Where(a => a.ClassId == classId).ToListAsync();

            if (Attendances.Count == 0)
            {
                return new List<AttendanceDto>();
            }

            List<AttendanceDto> attendances = new List<AttendanceDto>();

            foreach (var att in Attendances)
            {
                AttendanceDto attDto = mapper.Map<AttendanceDto>(att); 
                attendances.Add(attDto);
            }
            return attendances;
        }

        //public async Task DeleteStudentAttendances(string studentName)
        //{
        //    var studentAttendances = await context.Attendances.Where(a => a.studentName == studentName).ToListAsync();
        //    context.RemoveRange(studentAttendances);
        //    context.SaveChanges();
        //}

        public async Task<AttendanceDto> GetAttendanceAsync(int AttendanceId)
        {
            Attendance attendance = await context.Attendances.FindAsync(AttendanceId);
            AttendanceDto attDto = mapper.Map<AttendanceDto>(attendance); 
            return attDto;
        }
    }
}
