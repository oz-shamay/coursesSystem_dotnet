using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coursesSystem.Dtos
{
    public class ClassDto
    {
        public int ClassId { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string courseName { get; set; }
    }
}
