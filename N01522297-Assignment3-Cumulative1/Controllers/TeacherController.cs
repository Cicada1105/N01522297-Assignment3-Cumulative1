using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
// Get access to the models in order to properly give typings to the info retrieved from the API controllers
using N01522297_Assignment3_Cumulative1.Models;

namespace N01522297_Assignment3_Cumulative1.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }
        // GET: /Teacher/List
        public ActionResult List()
        {
            // Get a list of all the authors using the TeacherDataController api that interacts dirrectly with the db
            TeacherDataController controller = new TeacherDataController();
            // Store list of teachers
            List<Teacher> allTeachers = controller.ListTeachers();

            // Send list of teachers to the List view
            return View(allTeachers);
        }
        // GET: /Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            // Get the data associated with a single teacher, as well as the courses they teach,
            // using the TeacherDataController api that interacts directly with the db
            TeacherDataController controller = new TeacherDataController();
            // Store the teacher and their coures into a TeacherCourses object
            TeacherCourses currTeacher = controller.ListTeacherCourses(id);

            // Return the found current teacher to the Show view to be displayed
            return View(currTeacher);
        }
    }
}