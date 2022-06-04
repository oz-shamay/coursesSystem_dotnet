using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coursesSystem.Dtos
{
    public class CourseDto
    {
        public int courseId { get; set; }
        public string courseName { get; set; }
        public DateTime courseStartAt { get; set; }
        public DateTime courseEndAt { get; set; }

    }
}
