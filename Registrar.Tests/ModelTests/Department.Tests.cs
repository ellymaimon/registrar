using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Registrar.Models;

namespace Registrar.Tests
{
    [TestClass]
    public class DepartmentTest : IDisposable
    {

        public void Dispose()
        {
            Department.DeleteAll();
        }

        public DepartmentTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=registrar_test;";
        }

        [TestMethod]
        public void GetSet_GetsSetsProperties_True()
        {
            Department newDepartment = new Department ("Department of Humanities", 1);
            Assert.AreEqual("Department of Humanities", newDepartment.Name);
            Assert.AreEqual(1, newDepartment.Id);
        }

        [TestMethod]
        public void Equals_ReturnsTrueIfPropertiesAreSame_True()
        {
            Department newDepartment = new Department ("Department of Humanities", 1);
            Department newDepartment2 = new Department ("Department of Humanities", 1);
            Assert.AreEqual(newDepartment, newDepartment2);
        }

        [TestMethod]
        public void GetAll_MakesSureDbIsEmpty_0()
        {
            int result = Department.GetAll().Count;
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Save_SavesToDatabase_ListDepartments()
        {
            Department newDepartment = new Department ("Department of Humanities");
            newDepartment.Save();
            List<Department> actualList = Department.GetAll();
            List<Department> expectedList = new List<Department> () { newDepartment };

            CollectionAssert.AreEqual(expectedList, actualList);
        }

        [TestMethod]
        public void Find_ReturnsDepartmentFromDatabase_Department()
        {
            Department newDepartment = new Department ("Department of Humanities");
            newDepartment.Save();
            Department foundDepartment = Department.Find(newDepartment.Id);
            Assert.AreEqual(newDepartment, foundDepartment);
        }

        [TestMethod]
        public void DeleteAll_DeletesDepartmentsFromDataBase_ListDepartments()
        {
            Department newDepartment = new Department ("Department of Humanities");
            newDepartment.Save();
            Department.DeleteAll();
            List<Department> actualList = Department.GetAll();
            List<Department> expectedList = new List<Department> () { };
            CollectionAssert.AreEqual(expectedList, actualList);
        }
    }
}
