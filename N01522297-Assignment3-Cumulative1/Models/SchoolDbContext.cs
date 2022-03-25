using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace N01522297_Assignment3_Cumulative1.Models
{
    public class SchoolDbContext
    {
        /*
            Using XAMP so login is as follows:
            User: "root"
            Password: "",
            Database: "school",
            Server: "127.0.0.1",
            Port: "3306"
         */
        // Use basic getter function for retrieving db info
        private static string User { 
            get { 
                return "root"; 
            }
        }
        private static string Password
        {
            get
            {
                return "";
            }
        }
        private static string Database
        {
            get
            {
                return "school";
            }
        }
        private static string Server
        {
            get
            {
                return "127.0.0.1";
            }
        }
        private static string Port
        {
            get
            {
                return "3306";
            }
        }
        // DatabaseCredentialsString properly formats the database credentials for creating a MySqconnection
        // protected access identifier allows subclasses to access this formatted string but not the above private members
        protected static string DatabaseCredentialsString
        {
            get
            {
                return $"server = {Server}; user = {User}; database = {Database}; port = {Port}; password = {Password}";
            }
        }
        /// <summary>
        /// Returns a new MySQL connection to the school database
        /// </summary>
        /// <example>
        /// private SchoolDbContext mySchoolDbContext = new SchoolDbContext();
        /// MySqlConnection schoolConn = mySchoolDbContext.DatabaseConnection();
        /// </example>
        /// <returns>An object of type MySqlconnection</returns>
        public MySqlConnection DatabaseConnection()
        {
            // Create a new instance of the built in MySqlConnection class,
            // using the formatted DatabaseCredentialsString as it's sole constructor argument
            return new MySqlConnection(DatabaseCredentialsString);
        }
    }
}