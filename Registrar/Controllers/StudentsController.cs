using Microsoft.AspNetCore.Mvc;
using Registrar.Models;
using Registrar.ViewModels;
using System;
using System.Collections.Generic;

namespace Registrar.Controllers
{
    public class StudentsController : Controller
    {
        [HttpGet("/students")]
        public ActionResult Index()
        {
            List<Student> allStudents = Student.GetAll();
            return View(allStudents);
        }

        [HttpGet("/students/new")]
        public ActionResult StudentForm()
        {
            return View();
        }

        [HttpPost("/students/new")]
        public ActionResult Create(string name)
        {
            DateTime date = DateTime.Now;
            Student newStudent = new Student (name, date);
            newStudent.Save();
            return RedirectToAction("Index");
        }

        [HttpPost("/students/delete")]
        public ActionResult DeleteAll()
        {
            Student.DeleteAll();
            return RedirectToAction("Index");
        }

        [HttpGet("/students/{id}")]
        public ActionResult Details(int id)
        {
            SAC newSAC = new SAC();
            newSAC.FindStudent(id);
            return View(newSAC);
        }

        [HttpPost("/students/{id}/add-course")]
        public ActionResult AddCourse(int id, int courseId)
        {
            Student foundStudent = Student.Find(id);
            Course foundCourse = Course.Find(courseId);
            foundStudent.AddCourse(foundCourse);
            return RedirectToAction("Details");
        }
    }
}
