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


        public async Task<IActionResult> Search(string PickupLocation, string DropLocation, DateTime BookingTime, double Distance, int TotalPersons)
        {
            Console.WriteLine($"Distance: {Distance}");

            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["logFirst"] = "Login First"; // Use TempData to persist message across redirect
                return RedirectToAction("Login", "Account");
            }


            // Create new booking entry
            

            // For now, fetch dummy cab data
            var availableCabs = _context.Cabs.ToList();

            // You can modify this to filter the cabs based on user input, for now return all
            ViewBag.PickupLocation = PickupLocation;
            ViewBag.DropLocation = DropLocation;
            ViewBag.Distance = Distance;
            ViewBag.BookingTime = BookingTime;
            ViewBag.TotalPersons = TotalPersons;    



            return View("SearchResults", availableCabs); // Show the cab search results
        }
    }
}
