using Microsoft.AspNetCore.Mvc;

namespace SistemaParqueo.MVC.Controllers
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

        // Método Error simple sin vista ni modelo
        public IActionResult Error()
        {
            return Content("Ocurrió un error en el sistema.");
        }
    }
}