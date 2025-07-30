using System.Diagnostics;
using KeyRent.Models;
using Microsoft.AspNetCore.Mvc;

namespace KeyRent.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Akcja odpowiadaj�ca za stron� g��wn�
        public IActionResult Index()
        {
            // Mo�esz doda� dane do widoku, np. wiadomo�� powitaln�
            var model = new { Message = "Witaj na stronie startowej!" };
            return View(model);
        }

        // Akcja dla strony "Privacy" (je�li masz taki widok)
        public IActionResult Privacy()
        {
            return View();
        }

        // Akcja dla strony b��du
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
