using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Association.Models
{
    public class Course
    {
        [Key]
        public int course_id { get; set; }
        public string course_name { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime course_datetime { get; set; }


        public virtual ICollection<Category> Category { get; set; }

    }
}