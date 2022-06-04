using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using coursesSystem.Models;


namespace coursesSystem.Models
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            //coursesList = new List<Course>();
        }
       // public IEnumerable<Course> coursesList { get; set; }
        public ICollection<AppUserCourse> CoursesList { get; set; }
        public bool isProfessor { get; set; }
        
    }
}
