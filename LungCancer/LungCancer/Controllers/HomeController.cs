using Microsoft.AspNetCore.Mvc;

namespace LungTumorWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult About()
        {
            
            return View();
        }

    }
}
