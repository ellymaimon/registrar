using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Registrar.Models;

namespace Registrar.Tests
{
    [TestClass]
    public class CourseTest : IDisposable
    {

        public void Dispose()
        {
            Course.DeleteAll();
        }

        public CourseTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=registrar_test;";
        }

        [TestMethod]
        public void GetSet_GetsSetsProperties_True()
        {
            Course newCourse = new Course ("Geology 101", "GEO300", 1);
            Assert.AreEqual("Geology 101", newCourse.Name);
            Assert.AreEqual("GEO300", newCourse.CourseNumber);
            Assert.AreEqual(1, newCourse.Id);
        }

        [TestMethod]
        public void Equals_ReturnsTrueIfPropertiesAreSame_True()
        {
            Course newCourse = new Course ("Geology 101", "GEO300", 1);
            Course newCourse2 = new Course ("Geology 101", "GEO300", 1);
            Assert.AreEqual(newCourse, newCourse2);
        }

        [TestMethod]
        public void GetAll_MakesSureDbIsEmpty_0()
        {
            int result = Course.GetAll().Count;
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Save_SavesToDatabase_ListCourses()
        {
            Course newCourse = new Course ("Geology 101", "GEO300");
            newCourse.Save();
            List<Course> actualList = Course.GetAll();
            List<Course> expectedList = new List<Course> () { newCourse };

            CollectionAssert.AreEqual(expectedList, actualList);
        }

        [TestMethod]
        public void Find_ReturnsCourseFromDatabase_Course()
        {
            Course newCourse = new Course ("Geology 101", "GEO300");
            newCourse.Save();
            Course foundCourse = Course.Find(newCourse.Id);
            Assert.AreEqual(newCourse, foundCourse);
        }

        [TestMethod]
        public void DeleteAll_DeletesCoursesFromDataBase_ListCourses()
        {
            Course newCourse = new Course ("Geology 101", "GEO300");
            newCourse.Save();
            Course.DeleteAll();
            List<Course> actualList = Course.GetAll();
            List<Course> expectedList = new List<Course> () { };
            CollectionAssert.AreEqual(expectedList, actualList);
        }
    }
}
