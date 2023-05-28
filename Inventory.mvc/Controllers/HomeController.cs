using Inventory.mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Inventory.mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHostedService hostedService;

        public HomeController(ILogger<HomeController> logger, IHostedService hostedService)
        {
            _logger = logger;
            this.hostedService = hostedService;
        }

        public async Task<IActionResult> Index()
        {
             await hostedService.StartAsync(default);
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