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
    public class ClassesRepository : IClassesRepository
    {
        public ClassesRepository(Context ctx, UserManager<AppUser> usrmngr)
        {
            context = ctx;
            userManager = usrmngr;
        }
        private UserManager<AppUser> userManager;
        private Context context;

        public async Task<List<ClassDto>> GetCourseClassesAsync(int courseId)
        {
            var Classes = await context.Classes.Where(c => c.CourseId == courseId).OrderBy(c => c.start).ToListAsync();
            if (Classes.Count == 0)
            {
                return new List<ClassDto>();
            }

            List<ClassDto> classes = new List<ClassDto>();
            foreach (var cls in Classes)
            {
                ClassDto clsDetails = new ClassDto
                {
                    courseName = cls.courseName,
                    start = cls.start,
                    end = cls.end,
                    ClassId = cls.ClassId
                };

                classes.Add(clsDetails);
            }

            return classes;
        }

        public async Task<bool> AddClassesToNewCourseAsync(int courseId, List<classTime> times)
        {
            Course c = await context.Courses.Include(c => c.classes).SingleOrDefaultAsync(c => c.courseId == courseId);
            if (c == null)
            {
                return false;
            }

            for (int st = 0; st < times.Count; st++)
            {
                double delta = 0;
                DateTime ct = times[st].start;
                for (DateTime t = c.courseStartAt; t < c.courseEndAt; t = t.AddMinutes(1))
                {
                    if (t.DayOfWeek == ct.DayOfWeek && t.TimeOfDay == ct.TimeOfDay)
                    {
                        DateTime ce = times[st].end;
                        ce = ce.AddDays(delta);
                        c.classes.Add(new Class
                        {
                            start = t,
                            end = ce,
                            courseName = c.courseName,
                            CourseId = c.courseId
                        });

                        delta = delta + 7;
                    }
                }
            }
            context.SaveChanges();
            return true;
        }
    }
    
}
