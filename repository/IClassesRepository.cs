using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coursesSystem.Models;
using coursesSystem.Dtos;

namespace coursesSystem.repository
{
    public interface IClassesRepository
    {
        Task<List<ClassDto>> GetCourseClassesAsync(int courseId);
        Task<bool> AddClassesToNewCourseAsync(int courseId, List<classTime> times);
    }
}
