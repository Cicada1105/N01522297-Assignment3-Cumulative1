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
        public ActionResult New()
        {
            return View();
        }
        public ActionResult Create(string f_name, string l_name, string employee_num, DateTime hire_date, decimal salary)
        {
            // Get access to the TeacherData controller api that interacts directly with the db
            TeacherDataController controller = new TeacherDataController();

            // Create an instance of the Teacher class, setting the respective fields to the values of the arguments
            Teacher teacher = new Teacher();

            // Set teacher properties
            teacher.FirstName = f_name;
            teacher.LastName = l_name;
            teacher.EmployeeNumber = employee_num;
            teacher.HireDate = hire_date;
            teacher.Salary = salary;

            // Call the Add method of the TeacherDataController class, passing in the teacher data
            controller.Add(teacher);

            // Redirect to the List view in order for the user to see the updated teacher list
            return RedirectToAction("List");
        }
        // GET: /Teacher/Remove/{id}
        public ActionResult Remove(int id)
        {
            // Get access to the TeacherData controller api that interacts directly with the db
            TeacherDataController controller = new TeacherDataController();
            // Call the Delete method, passing in the respective teacher id
            controller.Delete(id);

            // Redirect to the List view in order for the user to see the updated teacher list
            return RedirectToAction("List");
        }
        // GET: /Teacher/Edit/{id}
        public ActionResult Edit(int id)
        {
            // Get access to the TeacherData controller api in order to retrieve author info with id
            TeacherDataController controller = new TeacherDataController();
            // Retrieve teacher info
            Teacher currTeacher = controller.FindTeacher(id);

            // Send teacher data to the view to be displayed
            return View(currTeacher);
        }
        // POST: /Teacher/Update/{id}
        [HttpPost]
        public ActionResult Update(int id,string f_name, string l_name, string employee_num, DateTime hire_date, decimal salary)
        {
            // Create a new teacher to then send to the Update method of the Teacher Data Controller context
            Teacher teacherToUpdate = new Teacher();

            // Set teacher properties to those passed in through the Edit form submission
            teacherToUpdate.Id = id;
            teacherToUpdate.FirstName = f_name;
            teacherToUpdate.LastName = l_name;
            teacherToUpdate.EmployeeNumber = employee_num;
            teacherToUpdate.HireDate = hire_date;
            teacherToUpdate.Salary = salary;

            // Create a new instance of the Teacher Data Controller to handle databse communicatoin
            TeacherDataController controller = new TeacherDataController();
            // Update teacher (returns success state)
            bool isSuccess = controller.Update(teacherToUpdate);

            // If the update was a success, redirect to viewing Teacher
            if (isSuccess) { 
                ViewBag["errorMsg"] = "";
                return RedirectToAction("Show/" + teacherToUpdate.Id);
            }
            else
            { // Return back to edit form passing in an error message to the ViewBag
                ViewBag["errorMsg"] = "You are missing content in your form";
                return RedirectToAction("Edit/" + teacherToUpdate.Id);
            }

        }
    }
}