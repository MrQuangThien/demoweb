using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebQuanLiCuaHangBanOto.Models;

namespace WebQuanLiCuaHangBanOto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult viewadmin()
        {
            return View(viewadmin);
        }
        public IActionResult viewindex()
        {
            return View(viewindex);
        }
        public IActionResult viewcontact()
        {
            return View(viewcontact);
        }
        public IActionResult viewblox()
        {
            return View(viewblox);
        }
        public IActionResult viewshop()
        {
            return View(viewshop);
        }
        public IActionResult viewlogdetal()
        {
            return View(viewlogdetal);
        }
        public IActionResult viewcheckout()
        {
            return View(viewcheckout);
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
