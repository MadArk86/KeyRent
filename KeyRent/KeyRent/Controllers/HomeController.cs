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

        // Akcja odpowiadaj¹ca za stronê g³ówn¹
        public IActionResult Index()
        {
            // Mo¿esz dodaæ dane do widoku, np. wiadomoœæ powitaln¹
            var model = new { Message = "Witaj na stronie startowej!" };
            return View(model);
        }

        // Akcja dla strony "Privacy" (jeœli masz taki widok)
        public IActionResult Privacy()
        {
            return View();
        }

        // Akcja dla strony b³êdu
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
