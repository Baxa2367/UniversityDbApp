
using Microsoft.AspNetCore.Mvc;

namespace UniversityDbApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return Content("Жүйеде қате пайда болды.");
        }
    }
}
