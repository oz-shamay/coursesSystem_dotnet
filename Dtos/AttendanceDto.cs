using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coursesSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace coursesSystem.Dtos
{
    public class AttendanceDto
    {
        [Required]
        public int AttendanceId { get; set; }

        public string studentName { get; set; }
        public bool isAttendance { get; set; }
        public string reason { get; set; }
        public DateTime classTime { get; set; }
    }
}
