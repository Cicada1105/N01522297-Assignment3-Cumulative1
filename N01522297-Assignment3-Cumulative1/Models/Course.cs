using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace N01522297_Assignment3_Cumulative1.Models
{
    // Class for modeling course database table data into a formatted object
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ClassCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
    }
}