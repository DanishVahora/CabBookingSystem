using Microsoft.AspNetCore.Mvc;
using CabBookingSystem.Models; // Assuming you have these models created
namespace CabBookingSystem.Controllers
{

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;


        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Search(string PickupLocation, string DropLocation, DateTime BookingTime, double Distance, int TotalPersons, double pickupLat, double pickupLng, double dropLat, double dropLng)
        {

            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["logFirst"] = "Login First"; // Use TempData to persist message across redirect
                return RedirectToAction("Login", "Account");
            }


            var availableCabs = _context.Cabs.ToList();

            ViewBag.PickupLocation = PickupLocation;
            ViewBag.DropLocation = DropLocation;
            ViewBag.Distance = Distance;
            ViewBag.BookingTime = BookingTime;
            ViewBag.TotalPersons = TotalPersons;
            ViewBag.PickupLat = pickupLat;
            ViewBag.PickupLng = pickupLng;
            ViewBag.DropLat = dropLat;
            ViewBag.DropLng = dropLng;



            return View("SearchResults", availableCabs); // Show the cab search results
        }
    }
}
