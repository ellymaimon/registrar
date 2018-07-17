using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Registrar;

namespace Registrar.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CourseNumber { get; set; }

        public Course(string name, string courseNumber, int id = 0)
        {
            Id = id;
            Name = name;
            CourseNumber = courseNumber;
        }

        public override bool Equals(System.Object otherCourse)
        {
            if (!(otherCourse is Course))
            {
                return false;
            }
            else
            {
                Course newCourse = (Course) otherCourse;
                bool idEquality = (this.Id == newCourse.Id);
                bool nameEquality = (this.Name == newCourse.Name);
                bool courseNumberEquality = (this.CourseNumber == newCourse.CourseNumber);
                return (idEquality && nameEquality && courseNumberEquality);
            }
        }
        
        public static List<Course> GetAll()
        {
            List<Course> allCourses = new List<Course> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM students;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int studentId = rdr.GetInt32(0);
                string studentName = rdr.GetString(1);
                DateTime studentDate = rdr.GetDateTime(2);
                Course newCourse = new Course (studentName, studentDate, studentId);
                allCourses.Add(newCourse);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCourses;
        }

        // public static void DeleteAll()
        // {
        //     List<Course> allCourses = new List<Course> { };
        //     MySqlConnection conn = DB.Connection();
        //     conn.Open();
        //     MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
        //     cmd.CommandText = @"DELETE FROM students;";
        //     cmd.ExecuteNonQuery();
        //     conn.Close();
        //     if (conn != null)
        //     {
        //         conn.Dispose();
        //     }
        // }
        //
        // public void Save()
        // {
        //     MySqlConnection conn = DB.Connection();
        //     conn.Open();
        //     MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
        //     cmd.CommandText = @"INSERT INTO students (name, date) VALUES (@studentName, @studentDate);";
        //     cmd.Parameters.AddWithValue("@studentName", this.Name);
        //     cmd.Parameters.AddWithValue("@studentDate", this.Date);
        //     cmd.ExecuteNonQuery();
        //
        //     Id = (int) cmd.LastInsertedId;
        //
        //     conn.Close();
        //     if(conn != null)
        //     {
        //         conn.Dispose();
        //     }
        // }
        //
        // public static Course Find(int id)
        // {
        //     MySqlConnection conn = DB.Connection();
        //     conn.Open();
        //     MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
        //     cmd.CommandText = @"SELECT * FROM students WHERE id = @studentId;";
        //     cmd.Parameters.AddWithValue("@studentId", id);
        //     MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
        //
        //     int studentId = 0;
        //     string studentName = "";
        //     DateTime studentDate = new DateTime();
        //
        //     while (rdr.Read())
        //     {
        //         studentId = rdr.GetInt32(0);
        //         studentName = rdr.GetString(1);
        //         studentDate = rdr.GetDateTime(2);
        //     }
        //
        //     Course foundCourse = new Course(studentName, studentDate, studentId);
        //     conn.Close();
        //     if(conn != null)
        //     {
        //         conn.Dispose();
        //     }
        //     return foundCourse;
        // }
    }
}
