using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using PersonalPortfolio.Models;

namespace PersonalPortfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error([FromQuery(Name = "errorCode")] int errorCode)
        {
            ViewBag.ErrorCode = $"{errorCode} {((HttpStatusCode)errorCode).ToString()}";
            Console.WriteLine(Response.ToString());
            ViewBag.ErrorMessage =
                HttpContext.Items["ErrorMessage"] ?? "An error occurred while processing your request";
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}