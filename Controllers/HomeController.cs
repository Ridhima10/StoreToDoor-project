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
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var artists = _userManager.GetUsersInRoleAsync("Artist").Result.Take(3);

            ViewBag.Artists = artists;

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
        [Authorize(Roles = "User")]
        public IActionResult Wishlist()
        {
            return View();
        }

        [Route("/Home/Collection/{id}")]
        public IActionResult Collection1(int id)
        {
            var artistCollection = _context.ArtistCollection.Find(id);

            if (artistCollection == null)
            {
                return RedirectToAction("Error");
            }

            var artist = _userManager.FindByNameAsync(artistCollection.Artist).Result;

            ViewBag.artistCollection = artistCollection;
            ViewBag.artist = artist;

            return View();
        }

        [Route("/Home/Artist/{id}")]
        public IActionResult Artist1(string id)
        {
            var artist = _userManager.Users.FirstOrDefault(u => u.AccountName == id);

            var artistCollections = _context.ArtistCollection.Where(a => a.Artist == artist.UserName);

            ViewBag.Artist = artist;
            ViewBag.ArtistCollections = artistCollections;

            return View();
        }
        [Authorize(Roles = "User")]
        public IActionResult CustomArt()
        {
            return View();
        }
        [Authorize(Roles = "Artist")]
        public IActionResult CustomArtRequest()
        {
            return View();
        }
        [Authorize(Roles = "User")]
        public IActionResult YourCustomRequest()
        {
            return View();
        }
        [Authorize(Roles = "User")]
        public IActionResult Payment()
        {
            return View();
        }
        [Authorize(Roles = "User")]
        public IActionResult OrderPlaced()
        {
            return View();
        }
        [Authorize(Roles = "User")]
        public IActionResult CancelOrderRequest()
        {
            return View();
        }
        [Authorize(Roles = "User")]
        public IActionResult CancelOrder()
        {
            return View();
        }
        [Authorize(Roles = "User")]
        public IActionResult ReviewOrder()
        {
            return View();
        }
        public IActionResult Error()
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
        public async Task<IActionResult> EditArtistProfile([FromForm] EditArtist artist)
        {
            try
            {
                var loggedInUser = await _userManager.FindByNameAsync(User.Identity.Name);

                if (artist.ProfileImage != null)
                {

                    var fileName = Guid.NewGuid().ToString() + "_" + artist.ProfileImage.FileName;
                    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profiles", fileName);

                    // Check if folder "profiles" exists in wwwroot if not create it
                    if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profiles")))
                    {
                        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profiles"));
                    }

                    artist.ProfileImage.CopyTo(new FileStream(filepath, FileMode.Create));

                    // Save Image to Database
                    loggedInUser.ProfileImage = fileName;
                }

                loggedInUser.AccountName = artist.AccountName;
                loggedInUser.UserName = artist.Email;
                loggedInUser.Email = artist.Email;
                loggedInUser.PhoneNumber = artist.PhoneNumber;
                loggedInUser.Bio = artist.Bio;

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

            var artistCollection = _context.ArtistCollection.Where(a => a.Artist == currentUserName).ToList();

            ViewBag.artistCollection = artistCollection;

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Artist")]
        public IActionResult UploadArtwork()
        {
            return View();
        }

        // handles Artist Artwork upload
        [HttpPost]
        [Authorize(Roles = "Artist")]
        public IActionResult UploadArtwork([FromForm] FileUpload formFile)
        {
            try
            {
                var fileName = Guid.NewGuid().ToString() + "_" + formFile.File.FileName;

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

                // check if folder "uploads" exists in wwwroot if not create it
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads"));
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    formFile.File.CopyTo(stream);
                }

                ArtistCollection artistCollection = new ArtistCollection()
                {
                    Title = formFile.Title,
                    File = fileName,
                    Size = formFile.Size,
                    Category = formFile.Category,
                    Price = formFile.Price,
                    Artist = User.Identity.Name,
                };

                _context.ArtistCollection.Add(artistCollection);
                _context.SaveChanges();

                // empty formFile
                formFile = new FileUpload();

                return RedirectToAction("ArtistProfile");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToAction("Error", "Index");
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult Cart(int id)
        {
            var item = _context.ArtistCollection.Find(id);

            var artist = _userManager.FindByNameAsync(item.Artist).Result;
            
            ViewBag.item = item;
            ViewBag.artist = artist;

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
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
