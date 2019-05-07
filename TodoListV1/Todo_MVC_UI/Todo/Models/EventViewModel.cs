using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Models
{
    public class EventViewModel
    {
        
        public int EventId { get; set; }
     
        public int UserId { get; set; }
        [Required(ErrorMessage = "Description Required")]
        public string Description { get; set; }

        
        [Required(ErrorMessage = "Date Required")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }

        public Nullable<System.DateTime> UpdatedOn { get; set; }

    }


    public partial class Event
    {        
        public int EventId { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
    }
}