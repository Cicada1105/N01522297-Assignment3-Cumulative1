using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace N01522297_Assignment3_Cumulative1.Models
{
    // Classes for storing courses taught by a teacher and the teacher associated with those courses
    public class TeacherCourses
    {
        // Private members for storing TeacherCourses data
        private Teacher _teacher;
        private List<Course> _courses;
        // Instantiate a Teacher and List of Course object s
        public TeacherCourses()
        {
            _teacher = new Teacher();
            _courses = new List<Course>();
        }
        public Teacher Teacher {
            get { return _teacher; }
        }
        public List<Course> Courses
        {
            get { return _courses; }
        }    
    }
}