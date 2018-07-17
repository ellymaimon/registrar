using Microsoft.AspNetCore.Mvc;
using Registrar.Models;
using Registrar.ViewModels;
using System;
using System.Collections.Generic;

namespace Registrar.Controllers
{
    public class CoursesController : Controller
    {
        [HttpGet("/courses")]
        public ActionResult Index()
        {
            List<Course> allCourses = Course.GetAll();
            return View(allCourses);
        }

        [HttpGet("/courses/new")]
        public ActionResult CourseForm()
        {
            return View();
        }

        [HttpPost("/courses/new")]
        public ActionResult Create(string name, string courseNumber)
        {
            Course newCourse = new Course (name, courseNumber);
            newCourse.Save();
            return RedirectToAction("Index");
        }

        [HttpPost("/courses/delete")]
        public ActionResult DeleteAll()
        {
            Course.DeleteAll();
            return RedirectToAction("Index");
        }
    }
}
