using Microsoft.AspNetCore.Mvc;
using Registrar.Models;
using Registrar.ViewModels;

namespace Registrar.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
