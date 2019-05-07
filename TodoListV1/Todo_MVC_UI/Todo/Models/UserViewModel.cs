using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ToDo.Models
{
        public class UserViewModel
        {     
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Event> Events { get; set; }
        }

    public partial class User
    {      
        public User()
        {
            this.Events = new HashSet<Event>();
        }     
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }       
        public virtual ICollection<Event> Events { get; set; }
    }
}