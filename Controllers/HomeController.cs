using Microsoft.AspNetCore.Mvc;
using StoreToDoor.Models;
using System.Diagnostics;

namespace StoreToDoor.Controllers
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

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Shopbyartist()
        {
            return View();
        }
        public IActionResult Shopbymedium()
        {
            return View();
        }
        public IActionResult Painting()
        {
            return View();
       }
        public IActionResult Wishlist()
        {
            return View();
        }
        public IActionResult Collection1()
        {
            return View();
        }
        public IActionResult Artist1()
        {
            return View();
        }
        public IActionResult CustomArt()
        {
            return View();
        }
        public IActionResult EditUserProfile()
        {
            return View();
        }
        public IActionResult EditArtistProfile()
        {
            return View();
        }
        public IActionResult ArtistProfile()
        {
            return View();
        }
        public IActionResult UploadArtwork()
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