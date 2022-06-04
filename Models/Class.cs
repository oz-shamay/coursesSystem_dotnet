using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coursesSystem.Models
{
    public class Class
    {
        public int ClassId { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string courseName { get; set; }
        public Course course { get; set; }
        public int CourseId { get; set; }
        public ICollection<Attendance> attendances { get; set; }
        
    }
}
