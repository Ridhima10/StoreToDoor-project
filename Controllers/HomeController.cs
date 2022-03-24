using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreToDoor.Data;
using StoreToDoor.Models;
using System.Diagnostics;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace StoreToDoor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
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
            var artists = _userManager.GetUsersInRoleAsync("Artist").Result;

            ViewBag.Artists = artists;

            return View();
        }

        [Authorize(Roles = "User")]
        public IActionResult Shopbymedium()
        {
            return View();
        }

        // public IActionResult Painting()
        // {
        //     return View();
        // }

        [Route("/Home/category/{category}")]
        public IActionResult Painting(string category)
        {
            try
            {
                // get collections by category
                var collections = _context.ArtistCollection.Where(x => x.Category == category);

                ViewBag.Collections = collections;
                ViewBag.Count = collections.Count();
                return View();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToAction("Error");
            }



        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult Wishlist()
        {
            var loggedInUser = _userManager.GetUserAsync(User).Result;
            var wishlist = _context.Wishlist.Where(w => w.UserId == loggedInUser.Id);
            var ItemCount = wishlist.Count();

            if (ItemCount != 0)
            {
                // Find all items from wislist in ArtistCollection
                var WishItems = _context.ArtistCollection.Where(a => wishlist.Any(w => w.ItemId == a.Id));

                ViewBag.Wishlist = wishlist;
                ViewBag.ItemCount = ItemCount;
                ViewBag.WishItems = WishItems;

                return View();
            }

            ViewBag.Wishlist = wishlist;
            ViewBag.ItemCount = ItemCount;

            return View();


        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult Wishlist(int id)
        {
            try
            {
                var loggedInUser = _userManager.GetUserAsync(User).Result;

                var item = _context.ArtistCollection.Find(id);

                var itemExist = _context.Wishlist.Where(w => w.UserId == loggedInUser.Id && w.ItemId == item.Id);


                if (itemExist.Count() != 0)
                {
                    return RedirectToAction("Wishlist");
                }

                if (item == null)
                {
                    _logger.LogError("Item not found");
                    return RedirectToAction("Error");
                }

                var wishItem = new Wishlist
                {
                    UserId = loggedInUser.Id,
                    ItemId = item.Id
                };

                _context.Wishlist.Add(wishItem);
                _context.SaveChanges();

                _logger.LogInformation("Item added to wishlist");

                return RedirectToAction("Wishlist");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToAction("Error");
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult DeleteWishItem(int id)
        {
            try
            {
                var loggedInUser = _userManager.GetUserAsync(User).Result;
                var item = _context.Wishlist.FirstOrDefault(w => w.UserId == loggedInUser.Id && w.ItemId == id);


                if (item == null)
                {
                    _logger.LogError("Item not found");
                    return RedirectToAction("Error");
                }

                _context.Wishlist.Remove(item);
                _context.SaveChanges();

                _logger.LogInformation("Item Removed from wishlist");

                return RedirectToAction("Wishlist");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
                return RedirectToAction("Error");
            }
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
        public IActionResult CustomArt(string artistId)
        {
            ViewBag.ArtistId = artistId;
            return View();
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult CustomArt(IFormCollection formCollection, string artistId)
        {
            var loggedInUser = _userManager.GetUserAsync(User).Result;
            var artRequest = new ArtRequest
            {
                UserId = loggedInUser.Id,
                Description = formCollection["Description"],
                ArtistId = formCollection["ArtistId"],
            };

            _context.ArtRequest.Add(artRequest);
            _context.SaveChanges();

            _logger.LogInformation("Art Request Added");

            return RedirectToAction("YourCustomRequest");
        }

        [Authorize(Roles = "Artist")]
        public IActionResult CustomArtRequest()
        {
            var loggedInUser = _userManager.GetUserAsync(User).Result;
            var artRequests = _context.ArtRequest.Where(a => a.ArtistId == loggedInUser.Id).ToList();

            ViewBag.ArtRequests= artRequests;

            return View();
        }

        [Authorize(Roles = "User")]
        public IActionResult YourCustomRequest()
        {
            var loggedInUser = _userManager.GetUserAsync(User).Result;
            var artRequests = _context.ArtRequest.Where(a => a.UserId == loggedInUser.Id).ToList();

            ViewBag.ArtRequests = artRequests;

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
                    Account account = new Account(
                        _configuration["AccountSettings:CloudName"],
                        _configuration["AccountSettings:ApiKey"],
                        _configuration["AccountSettings:ApiSecret"]

                    );

                    Cloudinary cloudinary = new Cloudinary(account);
                    cloudinary.Api.Secure = true;

                    var uploadsparams = new ImageUploadParams()
                    {
                        File = new FileDescription(artist.ProfileImage.FileName, artist.ProfileImage.OpenReadStream()),
                        Folder = "profile"
                    };

                    var uploadResponse = cloudinary.Upload(uploadsparams);

                    _logger.LogInformation("Image Uploaded Successfully");

                    loggedInUser.ProfileImage = uploadResponse.SecureUrl.ToString();
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

            ViewBag.ColCount = artistCollection.Count;

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
                Account account = new Account(
                    _configuration["AccountSettings:CloudName"],
                    _configuration["AccountSettings:ApiKey"],
                    _configuration["AccountSettings:ApiSecret"]

                );

                Cloudinary cloudinary = new Cloudinary(account);
                cloudinary.Api.Secure = true;

                var uploadsparams = new ImageUploadParams()
                {
                    File = new FileDescription(formFile.File.FileName, formFile.File.OpenReadStream()),
                    Folder = "artwork"
                };

                var uploadResponse = cloudinary.Upload(uploadsparams);

                _logger.LogInformation("Image Uploaded Successfully");

                ArtistCollection artistCollection = new ArtistCollection()
                {
                    Title = formFile.Title,
                    File = uploadResponse.SecureUrl.ToString(),
                    Size = formFile.Size,
                    Category = formFile.Category,
                    Price = formFile.Price,
                    Artist = User.Identity.Name,
                    Description = formFile.Description,
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
        [HttpGet]
        public IActionResult Cart()
        {
            ViewBag.checkout = false;
            return View();
        }


        [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult Cart(int id)
        {
            var item = _context.ArtistCollection.Find(id);

            var artist = _userManager.FindByNameAsync(item.Artist).Result;

            ViewBag.checkout = true;
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
