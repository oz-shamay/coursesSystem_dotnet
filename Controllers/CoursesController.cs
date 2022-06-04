using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coursesSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using coursesSystem.repository;
using coursesSystem.Dtos;
using AutoMapper;




namespace coursesSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private UserManager<AppUser> userManager;
        //private IRepository ctxr;
        private IAttendancesRepository AttRepository;
        private IClassesRepository ClsRepository;
        private ICoursesRepository CoursesRepository;
        private IAccountRepository AccountRepository;
        public CoursesController(UserManager<AppUser> usrManager, IAttendancesRepository AttReposy,
            IClassesRepository clsReposy, ICoursesRepository crsReposy, IAccountRepository accreposy)
        {
            userManager = usrManager;
            //ctxr = Ctxr;
            AttRepository = AttReposy;
            ClsRepository = clsReposy;
            CoursesRepository = crsReposy;
            AccountRepository = accreposy;
        }

        [HttpGet()]
        [Route("{courseName}")]
        public async Task<IActionResult> GetCourse(string name)
        {
            Course c = await CoursesRepository.GetCourseAsync(name);
            if(c.courseName != null && c.courseName != "")
            {
                return Ok(c);
            }
            return BadRequest();
        }
        //------------------------------------------------------------------------------

        [Authorize(Roles = UserRoles.Professor)]
       [HttpGet()]
       [Route("{courseId}/students")]
       public async Task<IActionResult> getStudentsOfCourse(int courseId)
        {
            
            List<AppUserDto> students = await CoursesRepository.GetCourseStudentsListAsync(courseId);

            if (students.Count != 0)
            {
                return Ok(students);
            }
            return Ok(new Message("No students are signed to this course."));
        }
        //------------------------------------------------------------------------------

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCourses()
        {
            List<Course> courses = await CoursesRepository.GetCoursesAsync();
            if(courses.Count == 0)
            {
                return BadRequest("there is no courses yet.");
            }
            return Ok(courses);
        }
        //------------------------------------------------------------------------------

        [Authorize(Roles = UserRoles.Professor)]
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] newCourseData newCourse)
        {
            if (ModelState.IsValid)
            {
               
                 Course course = await CoursesRepository.CreateNewCourse(newCourse);

                if (course != null)
                {
                    if (!(await ClsRepository.AddClassesToNewCourseAsync(course.courseId, newCourse.Times.ToList())))
                    {
                        return Ok(new Message("The course was create but no classes added."));
                    }
                    CourseDto courseDto = new CourseDto
                    {
                        courseStartAt = newCourse.courseStartAt,
                        courseEndAt = newCourse.courseEndAt,
                        courseName = newCourse.courseName
                    };
                    return Ok(new Message("Success"));
                }
                return Ok(new Message("There is already course with this name"));
            }
            return Ok(new Message("Course details was invalid"));
        }
        //------------------------------------------------------------------------------

        [Authorize(Roles = UserRoles.Professor)]
        [HttpPut]
        public async Task<IActionResult> PlaceStudentInCourse([FromBody] placeStudentInCourse studentToCourse )
        {
            Course course = CoursesRepository.GetCourseIncludeStudentsById(studentToCourse.courseId);
            AppUser student = await AccountRepository.GetUserByIdAsync(studentToCourse.studentId);

            if (course != null && student != null && student.isProfessor == false)
            {
                
                if (await CoursesRepository.PlaceStudentInCourseAsync(studentToCourse.courseId, studentToCourse.studentId))
                {
                    await AttRepository.AddAttendancesOfStudentToCourseAsync(studentToCourse.studentId, studentToCourse.courseId);

                    return Ok(new Message("success"));
                }
                return Ok(new Message("This student are already placed in this course."));
            }
            return Ok(new Message("Course or student are not valid."));
        }

        //------------------------------------------------------------------------------

        [Authorize(Roles = UserRoles.Professor)]
        [HttpDelete("studentRemove")]
        public async Task<IActionResult> RemoveStudentFromCourse([FromBody] placeStudentInCourse studentCourse) 
        {
            AppUser student = await AccountRepository.GetUserByIdAsync(studentCourse.studentId);

            if(await CoursesRepository.DeleteAppUserCoursesAsync(studentCourse.studentId, studentCourse.courseId))
            {
               if(await AttRepository.DeleteAttendancesOfStudentFromCourseAsync(student.Id , studentCourse.courseId))
                {
                    return Ok(new Message("success"));
                }
            }
            return Ok(new Message("fail"));
        }

        //------------------------------------------------------------------------------

        [Authorize(Roles = UserRoles.Professor)]
        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            if(await CoursesRepository.DeleteCourse(courseId))
            {
                return Ok(new Message("success")); 
            }
            return Ok(new Message("please make sure the course id is correct"));
        }
        //------------------------------------------------------------------------------

        [Authorize]
        [HttpGet("studentAttendances")]
        public async Task<IActionResult> GetStudentAttendances([FromQuery] string studentId , int courseId)
        {
            var currentUser = await AccountRepository.GetCurrentLoggedInUserAsync(User);
            if(!currentUser.isProfessor && currentUser.Id != studentId)
            {
                return Unauthorized();
            }

            var attendances = await AttRepository.GetStudentAttendancesAsync(studentId ,courseId);
            
            if(attendances.Count == 0)
            {
                return Ok(new Message("Please make sure this student enrolls to some course"));
            }

            return Ok(attendances);
        }
        //----------------------------------------------------------------------------

        [Authorize]
        [HttpGet("student")]
        public async Task<IActionResult> GetStudentCourses(string studentId)
        {
            AppUser user = await AccountRepository.GetCurrentLoggedInUserAsync(User);
            if (user.Id != studentId && !user.isProfessor)
            {
                return Unauthorized();
            }

            List<CourseDto> courses = await CoursesRepository.GetStudentCoursesAsync(studentId);
            
            if (courses != null && courses.Count == 0)
            {
                return Ok();
            }
            return Ok(courses);
        }

        //----------------------------------------------------------------------------

        [Authorize]
        [HttpPut("Attendance")]
        public async Task<IActionResult> EditAttendance([FromBody] AttendanceDto attendance)
        {
            if (ModelState.IsValid)
            {
                if (!await AttRepository.EditAttendanceAsync(attendance.AttendanceId, attendance.isAttendance, attendance.reason, User))
                {
                    return BadRequest("fff");
                }
                
                return Ok();
            }
            return BadRequest("model not valid");
        }

        //----------------------------------------------------------------------------

        [Authorize]
        [HttpGet("courseClasses")]
        public async Task<IActionResult> GetCourseClasses([FromQuery] int courseId)
        {
            var Classes = await ClsRepository.GetCourseClassesAsync(courseId);
            if (Classes.Count == 0)
            {
                return Ok(new Message("Please make sure the course Id is correct"));
            }
            return Ok(Classes);
        }
        //-----------------------------------------------------------------------------
        
        [Authorize(Roles = UserRoles.Professor)]
        [HttpGet("ClassAttendances")]
        public async Task<IActionResult> GetClassAttendances([FromQuery] int classId)
        {
            var Attendances = await AttRepository.GetClassAttendances(classId);

            if(Attendances.Count == 0)
            {
                return Ok(new Message("Please make sure the class Id is correct and there are student in this course."));
            }
            return Ok(Attendances);
        }
        //----------------------------------------------------------------------------




    }
}
