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
            cmd.CommandText = @"SELECT * FROM courses;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int courseId = rdr.GetInt32(0);
                string courseName = rdr.GetString(1);
                string courseNumber = rdr.GetString(2);

                Course newCourse = new Course (courseName, courseNumber, courseId);
                allCourses.Add(newCourse);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCourses;
        }

        public static void DeleteAll()
        {
            List<Course> allCourses = new List<Course> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM courses; DELETE FROM enrollments";
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
            cmd.CommandText = @"INSERT INTO courses (name, course_number) VALUES (@courseName, @courseNumber);";
            cmd.Parameters.AddWithValue("@courseName", this.Name);
            cmd.Parameters.AddWithValue("@courseNumber", this.CourseNumber);
            cmd.ExecuteNonQuery();

            Id = (int) cmd.LastInsertedId;

            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
        }

        public static Course Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM courses WHERE id = @courseId;";
            cmd.Parameters.AddWithValue("@courseId", id);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            int courseId = 0;
            string courseName = "";
            string courseNumber = "";

            while (rdr.Read())
            {
                courseId = rdr.GetInt32(0);
                courseName = rdr.GetString(1);
                courseNumber = rdr.GetString(2);
            }

            Course foundCourse = new Course(courseName, courseNumber, courseId);
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
            return foundCourse;
        }

        public void AddStudent(Student newStudent)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO enrollments (student_id, course_id) VALUES (@studentId, @courseId);";
            cmd.Parameters.AddWithValue("@studentId", newStudent.Id);
            cmd.Parameters.AddWithValue("@courseId", this.Id);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Student> GetStudents()
        {
            List<Student> students = new List<Student> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT students.* FROM courses
                JOIN enrollments ON (courses.id = enrollments.course_id)
                JOIN students ON (enrollments.student_id = students.id)
                WHERE courses.id = @courseId;";
            cmd.Parameters.AddWithValue("@courseId", this.Id);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int studentId = rdr.GetInt32(0);
                string studentName = rdr.GetString(1);
                DateTime studentDate = rdr.GetDateTime(2);
                Student student = new Student(studentName, studentDate, studentId);
                students.Add(student);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return students;
        }
    }
}
