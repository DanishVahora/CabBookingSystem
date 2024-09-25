using Microsoft.AspNetCore.Mvc;
using CabBookingSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace CabBookingSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Details(int id, string pickupLocation, string dropLocation, string BookingTime, double Distance, int TotalPersons, double pickupLat, double pickupLng, double dropLat, double dropLng)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["logFirst"] = "Login First";
                return RedirectToAction("Login", "Account");
            }

            var Price = (int)Distance * 30;
            var cab = _context.Cabs.Find(id);

            ViewBag.PickupLocation = pickupLocation;
            ViewBag.DropLocation = dropLocation;
            ViewBag.BookingTime = BookingTime;
            ViewBag.Distance = Distance;
            ViewBag.Price = Price;
            ViewBag.TotalPersons = TotalPersons;
            ViewBag.PickupLat = pickupLat;
            ViewBag.PickupLng = pickupLng;
            ViewBag.DropLat = dropLat;
            ViewBag.DropLng = dropLng;

            return View(cab);
        }


        [HttpGet]
        public IActionResult GetStatus(int bookingId)
        {
            var booking = _context.Bookings.Find(bookingId);
            if (booking == null)
            {
                return Json(new { status = "not_found" });
            }

            return Json(new { status = booking.Status.ToLower() }); // Ensure consistency in status comparison
        }




        public IActionResult Buffer(int bookingId)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["logFirst"] = "Login First";
                return RedirectToAction("Login", "Account");
            }
            var booking = _context.Bookings.Find(bookingId);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking); // Ensure you have a corresponding Buffer.cshtml view
        }


        [HttpPost]
        public IActionResult ConfirmBooking(int cabId, string pickupLocation, string dropLocation, DateTime bookingTime, double distance, int numberOfPass, int price, double pickupLat, double pickupLng, double dropLat, double dropLng)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["logFirst"] = "Login First";
                return RedirectToAction("Login", "Account");
            }

            int userId = HttpContext.Session.GetInt32("UserId").Value;

            // Generate a random 6-digit OTP
            Random random = new Random();
            int otp = random.Next(100000, 999999);

            Booking booking = new Booking
            {
                UserId = userId,
                CabId = cabId,
                PickupLocation = pickupLocation,
                DropLocation = dropLocation,
                BookingTime = bookingTime,
                Distance = distance,
                Price = price,
                NumberOfPass = numberOfPass,
                Status = "pending", // Status is set to pending initially
                PickupLatitude = pickupLat,
                PickupLongitude = pickupLng,
                DropLatitude = dropLat,
                DropLongitude = dropLng,
                Otp = otp // Store the generated OTP
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            // Redirect to the buffering page
            return RedirectToAction("Buffer", new { bookingId = booking.BookingId });
        }

        [HttpGet]
        public IActionResult Confirmation(int bookingId)
        {

            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["logFirst"] = "Login First";
                return RedirectToAction("Login", "Account");
            }
            var booking = _context.Bookings.Find(bookingId);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking); // This should match the view name and pass the booking model to the view.
        }




    }
}
