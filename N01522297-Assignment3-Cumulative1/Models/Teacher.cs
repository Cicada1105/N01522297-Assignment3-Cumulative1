using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace N01522297_Assignment3_Cumulative1.Models
{
    // Class for modeling teacher database data into a formatted object
    public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeNumber { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
    }
}