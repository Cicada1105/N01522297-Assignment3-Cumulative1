using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
// Access School DB Context class
using N01522297_Assignment3_Cumulative1.Models;
// Access MySqlClient for database access
using MySql.Data.MySqlClient;

namespace N01522297_Assignment3_Cumulative1.Controllers
{
    public class TeacherDataController : ApiController
    {
        // Create a new instance of the School Db Context to access instance method of DatabaseConnection
        private SchoolDbContext ctxt = new SchoolDbContext();
        /// <summary>
        /// Method for returning all teachers stored in the school database
        /// </summary>
        /// <example>
        /// GET api/TeacherData/ListTeachers
        /// </example>
        /// <returns>List of Teacher objects containing all data from the database</returns>
        [HttpGet]
        [Route("api/TeacherData/ListTeachers")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public List<Teacher> ListTeachers()
        {
            // Retrieve connection to database through its context
            MySqlConnection conn = ctxt.DatabaseConnection();

            // Open connection to be 
            conn.Open();

            // Establish an sql query command
            MySqlCommand query = conn.CreateCommand();

            // Define the sql command through its property
            query.CommandText = "SELECT * FROM teachers";

            // Execute command
            MySqlDataReader reader = query.ExecuteReader();

            // Create an empty list of teachers to add to once the data has been retrieved from the database
            List<Teacher> teacherList = new List<Teacher>();

            // While there is still data to be read from the data reader, add the result set items to the teacher list
            while (reader.Read())
            {
                // Instantiate a new teacher object to then add database properties to
                Teacher currTeacher = new Teacher();

                // Access data from data reader and add it to the current teacher object
                currTeacher.Id = (int)reader["teacherid"];
                currTeacher.FirstName = reader["teacherfname"].ToString();
                currTeacher.LastName = reader["teacherlname"].ToString();
                currTeacher.EmployeeNumber = reader["employeenumber"].ToString();
                // HireDate property of the Teacher class is defined as a DateTime and must be converted to one as such
                currTeacher.HireDate = Convert.ToDateTime(reader["hiredate"]);
                currTeacher.Salary = (decimal)reader["salary"];

                // Add newly updated teacher to the list of teachers
                teacherList.Add(currTeacher);
            }

            // Close connection once we are done retrieving values from the data reader
            conn.Close();

            // Return list of teachers
            return teacherList;
        }
        /// <summary>
        /// Given an id, the method will search the database for the teacher with that id and
        /// return a Teacher object containing the respective info
        /// </summary>
        /// <param name="teacherId">integer id of a teacher in the database</param>
        /// <returns>Teacher object containing data related to teacher with id of teacherId</returns>
        [HttpGet]
        [Route("api/TeacherData/FindTeacher/{teacherId}")]
        // This API method will be called by the List teacher view when
        // a user clicks on a teacher to find more information about a teacher
        public Teacher FindTeacher(int teacherId)
        {
            // Create an sql connection with the school database
            MySqlConnection conn = ctxt.DatabaseConnection();

            // Open up the connection for query execution
            conn.Open();

            // Create MySqlCommand object to write and execute commands
            MySqlCommand query = conn.CreateCommand();
            // Set CommandText property of the MySqlCommand created instance
            query.CommandText = $"SELECT * FROM teachers WHERE teacherid = {teacherId}";

            // Execute command using data reader
            MySqlDataReader reader = query.ExecuteReader();

            // Create a new Teacher instance to store retrieved values from database
            Teacher foundTeacher = new Teacher();
            // While there is data from the database to be read, keep reading
            while (reader.Read())
            {
                foundTeacher.Id = (int)reader["teacherid"];
                foundTeacher.FirstName = reader["teacherfname"].ToString();
                foundTeacher.LastName = reader["teacherlname"].ToString();
                foundTeacher.EmployeeNumber = reader["employeenumber"].ToString();
                // HireDate property of the Teacher class is defined as a DateTime and must be converted to one as such
                foundTeacher.HireDate = Convert.ToDateTime(reader["hiredate"]);
                foundTeacher.Salary = (decimal)reader["salary"];
            }

            // Close connection to the database
            conn.Close();

            // Return found teacher
            return foundTeacher;
        }
        /// <summary>
        /// Given an integer id, this method will search for the teacher with the id of teacherId
        /// and the courses they teach, returning a TeacherCourses object containing both teacher
        /// an dcourses data
        /// </summary>
        /// <param name="teacherId">Integer id of teacher to find</param>
        /// <returns>TeacherCourses object containing teacher and courses data</returns>
        [HttpGet]
        [Route("api/TeacherData/ListTeacherCourses/{teacherId}")]
        public TeacherCourses ListTeacherCourses(int teacherId)
        {
            // Retrieve a connection to the database from the school db context
            MySqlConnection conn = ctxt.DatabaseConnection();

            // Open up the connection for communication
            conn.Open();

            // Create a command object for later querying the database
            MySqlCommand command = conn.CreateCommand();
            // Set the CommandText property to the respective join query
            /*
             * Left join is used because there may be teachers that are not teaching any courses
             * In this case, you still want to retrieve the teacher data even if the course data is NULL (left join)
             */
            command.CommandText = $"SELECT * FROM teachers t LEFT JOIN classes c ON t.teacherid = c.teacherid WHERE t.teacherid = {teacherId}";
            // Simplified version of above query since both tables share same PK/FK connecton
            //command.CommandText = $"SELECT * FROM teachers JOIN classes USING(teacherid) WHERE teacherid = {teacherId}";

            // Execute the MySqlCommand, retrieving a DataReader
            MySqlDataReader reader = command.ExecuteReader();

            // Create a list of courses that are taught by the teacher with an id of teacherId
            TeacherCourses coursesTaught = new TeacherCourses();
            // While there are courses the teacher teaches, loop through them all, retrieving them from db, and storing them in the course list
            while (reader.Read())
            {
                // Extract values from reader, storing them into a local Course object, then adding it to the coursesTaught list

                // Set attributes of the teacher object in the TeacherCourses instance
                // Note: The teacher is going to be the same within each loop
                coursesTaught.Teacher.Id = (int)reader["teacherid"];
                coursesTaught.Teacher.FirstName = reader["teacherfname"].ToString();
                coursesTaught.Teacher.LastName = reader["teacherlname"].ToString();
                coursesTaught.Teacher.EmployeeNumber = reader["employeenumber"].ToString();
                // HireDate property of the Teacher class is defined as a DateTime and must be converted to one as such
                coursesTaught.Teacher.HireDate = Convert.ToDateTime(reader["hiredate"]);
                coursesTaught.Teacher.Salary = (decimal)reader["salary"];

                // Check if class info is available (not empty)
                if (reader["classid"].ToString() != "")
                {
                    // Create a new course object
                    Course currCourse = new Course();
                    // Set attributes of course
                    currCourse.Name = reader["classname"].ToString();
                    currCourse.ClassCode = reader["classcode"].ToString();
                    currCourse.StartDate = Convert.ToDateTime(reader["startdate"]);
                    currCourse.FinishDate = Convert.ToDateTime(reader["finishdate"]);

                    // Add current course to the course list of the TeacherCourses instance
                    coursesTaught.Courses.Add(currCourse);
                }
            }

            // Close connection
            conn.Close();

            // Return list of courses taught
            return coursesTaught;
        }
        /// <summary>
        /// Adds a new teacher to the database
        /// </summary>
        /// <param name="teacher">Teacher instance that contains data to add to the teachers database</param>
        [HttpPost]
        [Route("api/TeacherData/Add")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void Add(Teacher teacher)
        {
            // Get access to a connection to the database from the context
            MySqlConnection conn = ctxt.DatabaseConnection();

            // Open up communication to the connection
            conn.Open();

            // Create an instance of a MySqlCommand through the connection method CreateCommand
            MySqlCommand command = conn.CreateCommand();

            // Define the query statement for adding a new teacher to the database
            command.CommandText = "INSERT INTO teachers (teacherfname,teacherlname,employeenumber,hiredate,salary) VALUES(@f_name,@l_name,@employee_num,@hire_date,@salary)";

            // Replace keys in SQL statement with respective values
            command.Parameters.AddWithValue("@f_name", teacher.FirstName);
            command.Parameters.AddWithValue("@l_name", teacher.LastName);
            command.Parameters.AddWithValue("@employee_num", teacher.EmployeeNumber);
            command.Parameters.AddWithValue("@hire_date", teacher.HireDate);
            command.Parameters.AddWithValue("@salary", teacher.Salary);

            // Prepare/sanitize parameter data
            command.Prepare();

            // Execute non query statement
            command.ExecuteNonQuery();

            // Close connection to database
            conn.Close();
        }
        /// <summary>
        /// Remove a teacher from the database
        /// </summary>
        /// <param name="teacherId">ID of teacher to be removed from the database</param>
        [HttpGet]
        [Route("api/TeacherData/Delete/{teacherId}")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void Delete(int teacherId)
        {
            // Get access to a connection to the database from the context
            MySqlConnection conn = ctxt.DatabaseConnection();

            // Open up communication to the connection
            conn.Open();

            // Create an instance of a MySqlCommand through the connection method CreateCommand
            MySqlCommand command = conn.CreateCommand();

            // Define the query to execute for removing a teacher from the database
            command.CommandText = "DELETE FROM teachers WHERE teacherid = @id";

            // Replace id key with respective teacherId value
            command.Parameters.AddWithValue("@id", teacherId);
            // Prepare/sanitize data
            command.Prepare();

            // Execute the non query statement
            command.ExecuteNonQuery();

            // Close connection to the database
            conn.Close();
        }

        /// <summary>
        /// Searches for teacher with id property and updates additional properties in database accordingly
        /// </summary>
        /// <param name="teacher">Teacher object containing id for Primary Key access and additional info related to a teacher</param>
        [HttpPost]
        [Route("api/TeacherData/Update")]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public bool Update(Teacher teacher)
        {
            // Check if there is any missing information
            if (teacher.FirstName == "")
                return false;
            if (teacher.LastName == "")
                return false;
            // Employee number can be skipped for now as a  pattern is used to make sure it follows "T###" format
            // Employee hiredate can be ignored as an error is already thrown if not provided
            if (teacher.Salary == 0)
                return false;

            // Create a new connection to the databse through the data controller
            MySqlConnection conn = ctxt.DatabaseConnection();

            // Open up communication to this connection
            conn.Open();

            // Create a new instnace of a MySQL command object for setting queries
            MySqlCommand command = conn.CreateCommand();
            Debug.WriteLine(teacher.HireDate);
            // Write the sql query for updating a teacher based on it's id Primary Key, including parameterization
            command.CommandText = "UPDATE teachers SET teacherfname=@fName, teacherlname=@lName, employeenumber=@number, hiredate=@hiredate, salary=@salary WHERE teacherid=@id";

            // Replace parameters with respective values
            command.Parameters.AddWithValue("@fName", teacher.FirstName);
            command.Parameters.AddWithValue("@lName", teacher.LastName);
            command.Parameters.AddWithValue("@number", teacher.EmployeeNumber);
            command.Parameters.AddWithValue("@hiredate", teacher.HireDate);
            command.Parameters.AddWithValue("@salary", teacher.Salary);
            command.Parameters.AddWithValue("@id", teacher.Id);

            // Sanitize data
            command.Prepare();

            // Execute non query sql statement
            command.ExecuteNonQuery();

            // Once execution is done, close connection
            conn.Close();

            // Return true now that the update functionality has been a success
            return true;
        }
    }
}