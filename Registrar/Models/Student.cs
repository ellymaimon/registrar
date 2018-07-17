using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Registrar;

namespace Registrar.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public Student(string name, DateTime date, int id = 0)
        {
            Id = id;
            Name = name;
            Date = date;
        }

        public override bool Equals(System.Object otherStudent)
        {
            if (!(otherStudent is Student))
            {
                return false;
            }
            else
            {
                Student newStudent = (Student) otherStudent;
                bool idEquality = (this.Id == newStudent.Id);
                bool nameEquality = (this.Name == newStudent.Name);
                bool dateEquality = (this.Date == newStudent.Date);
                return (idEquality && nameEquality && dateEquality);
            }
        }

        public static List<Student> GetAll()
        {
            List<Student> allStudents = new List<Student> { };
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
                Student newStudent = new Student (studentName, studentDate, studentId);
                allStudents.Add(newStudent);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allStudents;
        }

        public static void DeleteAll()
        {
            List<Student> allStudents = new List<Student> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM students; DELETE FROM enrollments";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO students (name, date) VALUES (@studentName, @studentDate);";
            cmd.Parameters.AddWithValue("@studentName", this.Name);
            cmd.Parameters.AddWithValue("@studentDate", this.Date);
            cmd.ExecuteNonQuery();

            Id = (int) cmd.LastInsertedId;

            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
        }

        public static Student Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM students WHERE id = @studentId;";
            cmd.Parameters.AddWithValue("@studentId", id);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            int studentId = 0;
            string studentName = "";
            DateTime studentDate = new DateTime();

            while (rdr.Read())
            {
                studentId = rdr.GetInt32(0);
                studentName = rdr.GetString(1);
                studentDate = rdr.GetDateTime(2);
            }

            Student foundStudent = new Student(studentName, studentDate, studentId);
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
            return foundStudent;
        }

        public void AddCourse(Course course)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO enrollments (student_id, course_id) VALUES (@studentId, @courseId);";
            cmd.Parameters.AddWithValue("@studentId", this.Id);
            cmd.Parameters.AddWithValue("@courseId", course.Id);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Course> GetCourses()
        {
            List<Course> courses = new List<Course> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT courses.* FROM students
                JOIN enrollments ON (students.id = enrollments.student_id)
                JOIN courses ON (enrollments.course_id = courses.id)
                WHERE students.id = @studentId;";
            cmd.Parameters.AddWithValue("@studentId", this.Id);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int courseId = rdr.GetInt32(0);
                string courseName = rdr.GetString(1);
                string courseNumber = rdr.GetString(2);
                Course course = new Course(courseName, courseNumber, courseId);
                courses.Add(course);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return courses;
        }
    }
}
