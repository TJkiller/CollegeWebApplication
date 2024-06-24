using CollegeWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementLibrary;
using System.Diagnostics;

namespace CollegeWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Display()
        {
            DateTime currentDate = DateTime.Now;
            string formattedDate = Utility.GetFormattedDate(currentDate);
            ViewBag.FormattedDate = formattedDate;
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
