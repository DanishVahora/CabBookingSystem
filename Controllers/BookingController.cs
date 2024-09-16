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
        public IActionResult Details(int id, string pickupLocation, string dropLocation, string BookingTime, double Distance, int TotalPersons)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["logFirst"] = "Login First"; // Use TempData to persist message across redirect
                return RedirectToAction("Login", "Account");
            }


            // Create new booking entry
            var Price = (int)Distance * 30;
            // For now, fetch dummy cab data
            var cab = _context.Cabs.Find(id);

            ViewBag.PickupLocation = pickupLocation;
            ViewBag.DropLocation = dropLocation;
            ViewBag.BookingTime = BookingTime;
            ViewBag.Distance = Distance;
            ViewBag.Price = Price;
            ViewBag.TotalPersons = TotalPersons;


            // Pass the cab information to the view
            return View(cab);
        }

        [HttpPost]
        public IActionResult ConfirmBooking(int cabId, string pickupLocation, string dropLocation, DateTime bookingTime, double distance, int numberOfPass, int price)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["logFirst"] = "Login First";
                return RedirectToAction("Login", "Account");
            }

            int userId = HttpContext.Session.GetInt32("UserId").Value;

            // Create a new booking
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
                Status = "Confirmed"
                // No OTP assignment here
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return View("BookingConfirmation", booking);
        }


    }
}