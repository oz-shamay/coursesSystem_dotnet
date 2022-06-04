using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace coursesSystem.Models
{
    public class Context : IdentityDbContext<AppUser> 
    {
        public Context(DbContextOptions<Context> opts) : base(opts) { }
        public DbSet<Course> Courses { get; set; }
        public DbSet<AppUser> appUsers { get; set; }
        public DbSet<AppUserCourse> UserCourses { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Class> Classes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUserCourse>().HasKey(uc => new { uc.AppUserId, uc.CourseId });

            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(x => new { x.ProviderKey, x.UserId , x.LoginProvider});

            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(x => new { x.RoleId, x.UserId } );

            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(x => new {x.UserId , x.LoginProvider,});

            //modelBuilder.Ignore<IdentityUserLogin<string>>();
            //modelBuilder.Ignore<IdentityUserRole<string>>();
            //modelBuilder.Ignore<IdentityUserToken<string>>();
        }
    }
}