using Microsoft.AspNetCore.Authorization;
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
        public IActionResult About()
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

        [Authorize(Roles = "User")]
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
        
        [Authorize(Roles = "Artist")]
        public IActionResult UploadArtwork()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View();
        }
        [Authorize(Roles = "User")]
        public IActionResult Cart()
        {
            return View();
        }
        public IActionResult ContactUs()
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