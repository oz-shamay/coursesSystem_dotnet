using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace coursesSystem.Models
{
    public class Course
    {
        public int courseId { get; set; }
        public string courseName { get; set; }
        public DateTime courseStartAt { get; set; }
        public DateTime courseEndAt { get; set; }
        public ICollection<Class> classes { get; set; }
        public ICollection<AppUserCourse> StudentsList { get; set; }

        
        
    }
}
