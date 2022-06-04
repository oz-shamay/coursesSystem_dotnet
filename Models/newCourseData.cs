using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace coursesSystem.Models
{
    public class newCourseData
    {
        [Required]
        public string courseName { get; set; }
        [Required]
        public DateTime courseStartAt { get; set; }
        [Required]
        public DateTime courseEndAt { get; set; }
       
        [Required]
        //public string Times { get; set; }  // Times of the first week classes, "start,end,star,end,....."
        public ICollection<classTime> Times { get; set; }
    }
}
