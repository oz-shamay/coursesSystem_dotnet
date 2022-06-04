using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using coursesSystem.Models;
using coursesSystem.Dtos;

namespace coursesSystem
{
    public class UserProfile : Profile
    {
       public UserProfile()
        {
            CreateMap<AppUser, AppUserDto>();
            CreateMap<Attendance, AttendanceDto>();
            CreateMap<Course, CourseDto>();
        }
    }
}
