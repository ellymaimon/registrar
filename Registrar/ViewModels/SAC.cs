using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Registrar;
using Registrar.Models;

namespace Registrar.ViewModels
{
    public class SAC
    {
        public List<Student> AllStudents { get; set; }
        public List<Course> AllCourses { get; set; }
        public Student CurrentStudent { get; set; }
        public Course CurrentCourse { get; set; }

        public SAC()
        {
            AllStudents = Student.GetAll();
            AllCourses = Course.GetAll();
        }

        public void FindStudent(int id)
        {
            CurrentStudent = Student.Find(id);
        }

        public void FindCourse (int id)
        {
            CurrentCourse = Course.Find(id);
        }
    }
}
