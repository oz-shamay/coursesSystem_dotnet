using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coursesSystem.Dtos
{
    public class AppUserDto
    {
        public string userName { get; set; }
        public string Email { get; set; }
        public string id { get; set; }
        public bool isProfessor { get; set; }
    }
}
