using Microsoft.AspNetCore.Mvc;
using Registrar.Models;
using System;

namespace Registrar.Controllers
{
    public class StudentsController : Controller
    {
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
            return RedirectToAction("Index", "HomeController");
        }
    }
}
