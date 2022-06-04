using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coursesSystem.Dtos
{
    public class Message
    {
        public string message { set; get; }
        
        public Message(string message) {
            this.message = message;
        } 
    }
}
