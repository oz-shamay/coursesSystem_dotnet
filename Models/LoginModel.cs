﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace coursesSystem.Models
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
   
        [Required]
        public string Password { get; set; }


    }
}
