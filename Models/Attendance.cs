using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coursesSystem.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        public string studentName { get; set; }
        public bool isAttendance { get; set; }
        public string reason { get; set; }
        public DateTime classTime { get; set; }
        public Class Class { get; set; }
        public int ClassId { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }

    }
}
