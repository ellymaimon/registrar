using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Registrar.Models;

namespace Registrar.Tests
{
    [TestClass]
    public class StudentTest : IDisposable
    {

        public void Dispose()
        {
            Student.DeleteAll();
        }

        public StudentTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=registrar_test;";
        }

        [TestMethod]
        public void GetSet_GetsSetsProperties_True()
        {
            DateTime date = new DateTime(2008, 10, 17);
            Student newStudent = new Student ("Joe", date, 1);
            Assert.AreEqual("Joe", newStudent.Name);
            Assert.AreEqual(date, newStudent.Date);
            Assert.AreEqual(1, newStudent.Id);
        }

        [TestMethod]
        public void Equals_ReturnsTrueIfPropertiesAreSame_True()
        {
            DateTime date = new DateTime(2008, 10, 17);
            Student newStudent = new Student ("Joe", date, 1);
            Student newStudent2 = new Student ("Joe", date, 1);
            Assert.AreEqual(newStudent, newStudent2);
        }

        [TestMethod]
        public void GetAll_MakesSureDbIsEmpty_0()
        {
            int result = Student.GetAll().Count;
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Save_SavesToDatabase_ListStudents()
        {
            DateTime date = new DateTime(2008, 10, 17);
            Student newStudent = new Student ("Matt", date);
            newStudent.Save();
            List<Student> actualList = Student.GetAll();
            List<Student> expectedList = new List<Student> () { newStudent };

            CollectionAssert.AreEqual(expectedList, actualList);
        }

        [TestMethod]
        public void Find_ReturnsStudentFromDatabase_Student()
        {
            DateTime date = new DateTime(2008, 10, 17);
            Student newStudent = new Student ("Matt", date);
            newStudent.Save();
            Student foundStudent = Student.Find(newStudent.Id);
            Assert.AreEqual(newStudent, foundStudent);
        }

        [TestMethod]
        public void DeleteAll_DeletesStudentsFromDataBase_ListStudents()
        {
            DateTime date = new DateTime(2008, 10, 17);
            Student newStudent = new Student ("Matt", date);
            newStudent.Save();
            Student.DeleteAll();
            List<Student> actualList = Student.GetAll();
            List<Student> expectedList = new List<Student> () { };
            CollectionAssert.AreEqual(expectedList, actualList);
        }

        [TestMethod]
        public void AddCourse_AddsCourseToStudent_ListCourses()
        {
            DateTime date = new DateTime(2008, 10, 17);
            Student newStudent = new Student("Matt", date);
            newStudent.Save();
            Course newCourse = new Course("Geology 101", "GEO300");
            newCourse.Save();
            newStudent.AddCourse(newCourse);
            List<Course> expectedList = new List<Course> { newCourse };
            List<Course> actualList = newStudent.GetCourses();
            CollectionAssert.AreEqual(expectedList, actualList);
        }

        [TestMethod]
        public void GetCourses_GetsCoursesAssociatedWithStudent_ListCourses()
        {
            DateTime date = new DateTime(2008, 10, 17);
            Student newStudent = new Student("Matt", date);
            newStudent.Save();
            Course newCourse = new Course("Geology 101", "GEO300");
            Course newCourse2 = new Course("Geology 101", "GEO300");
            newCourse.Save();
            newStudent.AddCourse(newCourse);
            List<Course> expectedList = new List<Course> { newCourse };
            List<Course> actualList = newStudent.GetCourses();
            CollectionAssert.AreEqual(expectedList, actualList);
        }
    }
}
