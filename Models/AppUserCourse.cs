using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coursesSystem.Models
{
    public class AppUserCourse
    {
        public string AppUserId { get; set; }
        public int CourseId { get; set; }

        public AppUser AppUser { get; set; }
        public Course Course { get; set; }
    }
}
