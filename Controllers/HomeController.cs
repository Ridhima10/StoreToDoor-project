using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreToDoor.Data;
using StoreToDoor.Models;
using System.Diagnostics;

namespace StoreToDoor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
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

        [Authorize(Roles = "User")]
        public async IAsyncResult EditUserProfile()
        {
            //var currentUser =  _userManager.FindByNameAsync(User.Identity.Name);
            var currentUserName = User.Identity.Name;
            var loggedInUser = _userManager.FindByEmailAsync(currentUserName);
            if (loggedInUser == null)
            {
                return View("Error");
            }
            else
            {
                ViewBag.loggedInUser = loggedInUser;
                return View();
            }

            return View();
        }

        [Authorize(Roles = "Artist")]
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
        [Authorize(Roles = "User")]
        public IActionResult YourOrders()
        {
            return View();
        }
        [Authorize(Roles = "User")]
        public IActionResult Address()
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