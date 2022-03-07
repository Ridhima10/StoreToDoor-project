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

        // Edit User Profile Get Controller
        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> EditUserProfile()
        {
            var currentUserName = User.Identity.Name;
            var loggedInUser = await _userManager.FindByEmailAsync(currentUserName);

            ViewBag.loggedInUser = loggedInUser;

            return View();
        }

        // Edit User Profile Post Controller
        // For use when Post Request is sent from EditUserProfile.cshtml
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> EditUserProfile(IFormCollection userCollection)
        {
            try
            {
                var loggedInUser = await _userManager.FindByNameAsync(User.Identity.Name);

                loggedInUser.FirstName = userCollection["FirstName"];
                loggedInUser.LastName = userCollection["LastName"];
                loggedInUser.Email = userCollection["Email"];
                loggedInUser.UserName = userCollection["Email"];
                loggedInUser.PhoneNumber = userCollection["PhoneNumber"];
                loggedInUser.Address = userCollection["Address"];
                loggedInUser.City = userCollection["City"];
                loggedInUser.State = userCollection["State"];
                loggedInUser.PinCode = int.Parse(userCollection["PinCode"]);

                var isSuccess = await _userManager.UpdateAsync(loggedInUser);

                if (isSuccess.Succeeded)
                {
                    _logger.LogInformation("User updated successfully.");
                    return RedirectToAction("EditUserProfile");
                }

                // If Operation Failes, Redirect to Index
                _logger.LogInformation("User Profile Update Failed");
                return RedirectToAction("Error", "Index");
            }
            catch (System.Exception ex)
            {
                // Log Exception
                _logger.LogError(ex.ToString());
                // return Error Page or Index
                return RedirectToAction("Error", "Index");
            }

        }

        // Edit Artist Profile Get Controller
        [Authorize(Roles = "Artist")]
        [HttpGet]
        public async Task<IActionResult> EditArtistProfile()
        {
            var currentUserName = User.Identity.Name;
            var loggedInUser = await _userManager.FindByEmailAsync(currentUserName);

            ViewBag.loggedInUser = loggedInUser;

            return View();
        }

        // Edit Artist Profile Post Controller
        // For use when Post Request is sent from EditArtistProfile.cshtml
        [Authorize(Roles = "Artist")]
        [HttpPost]
        public async Task<IActionResult> EditArtistProfile(IFormCollection userCollection)
        {
            try
            {
                var loggedInUser = await _userManager.FindByNameAsync(User.Identity.Name);

                loggedInUser.UserName = userCollection["AccountName"];
                loggedInUser.Email = userCollection["Email"];
                loggedInUser.PhoneNumber = userCollection["PhoneNumber"];
                loggedInUser.Bio = userCollection["Bio"];

                var isSuccess = await _userManager.UpdateAsync(loggedInUser);

                if (isSuccess.Succeeded)
                {
                    _logger.LogInformation("Artist updated successfully.");
                    return RedirectToAction("ArtistProfile");
                }

                // If Operation Failes, Redirect to Index
                _logger.LogInformation("Artist Profile Update Failed");
                return RedirectToAction("Error", "Index");
            }
            catch (System.Exception ex)
            {
                // Log Exception
                _logger.LogError(ex.ToString());
                // return Error Page or Index
                return RedirectToAction("Error", "Index");
            }

        }

        [Authorize(Roles = "Artist")]
        public async Task<IActionResult> ArtistProfile()
        {
            var currentUserName = User.Identity.Name;
            var loggedInUser = await _userManager.FindByNameAsync(currentUserName);

            ViewBag.loggedInUser = loggedInUser;

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
