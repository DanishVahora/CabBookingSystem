using Microsoft.AspNetCore.Mvc;
using CabBookingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace CabBookingSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;


        public BookingController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
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


        [HttpPost]
        [Route("Booking/InitiatePayment")]
        public IActionResult InitiatePayment(int bookingId)
        {
            var booking = _context.Bookings.Find(bookingId);
            if (booking == null) return NotFound();

            // Generate a Razorpay Order
            var client = new Razorpay.Api.RazorpayClient(_config["Razorpay:Key"], _config["Razorpay:Secret"]);
            var options = new Dictionary<string, object>
            {
                { "amount", booking.Price * 100}, // Amount in paise
                { "currency", "INR" },
                { "receipt", $"order_rcptid_{bookingId}" }
            };
            var order = client.Order.Create(options);

            // Save the order ID for verification after payment
            TempData["RazorpayOrderId"] = order["id"].ToString();

            ViewBag.OrderId = order["id"].ToString();
            ViewBag.RazorpayKey = _config["Razorpay:Key"];
            ViewBag.Amount = booking.Price * 100; // Pass amount in paise
            ViewBag.BookingId = bookingId;

            ViewBag.PickupLocation = booking.PickupLocation;
            ViewBag.DropLocation = booking.DropLocation;
            ViewBag.BookingTime= booking.BookingTime;
            ViewBag.Distance = booking.Distance;
            ViewBag.Price = booking.Price;
            ViewBag.TotalPersons = booking.NumberOfPass;

            return View("PaymentPage"); // Create a "PaymentPage.cshtml" for Razorpay form
        }



        [HttpPost]
        public IActionResult PaymentSuccess(string razorpay_payment_id, string razorpay_order_id, string razorpay_signature, int bookingId)
        {
            var booking = _context.Bookings.Find(bookingId);
            if (booking == null) return NotFound();

            // Concatenate order ID and payment ID
            var payload = razorpay_order_id + "|" + razorpay_payment_id;

            // Create HMACSHA256 hash using the secret key
            var secret = _config["Razorpay:Secret"];
            var secretBytes = Encoding.UTF8.GetBytes(secret);
            var payloadBytes = Encoding.UTF8.GetBytes(payload);

            string computedSignature;
            using (var hmac = new HMACSHA256(secretBytes))
            {
                var hashBytes = hmac.ComputeHash(payloadBytes);
                computedSignature = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }

            // Compare the generated signature with the received signature
            if (computedSignature == razorpay_signature)
            {
                booking.Status = "pending";
                _context.SaveChanges();
                return RedirectToAction("Buffer", new { bookingId = bookingId });
            }
            else
            {
                TempData["PaymentError"] = "Payment verification failed.";
                return RedirectToAction("Buffer", new { bookingId = bookingId });
            }
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
        public IActionResult ConfirmBooking(int cabId, string pickupLocation, string dropLocation, DateTime bookingTime, double distance, int TotalPersons, int price, double pickupLat, double pickupLng, double dropLat, double dropLng)
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
                NumberOfPass = TotalPersons,
                Status = "pending", // Status is set to pending initially
                PickupLatitude = pickupLat,
                PickupLongitude = pickupLng,
                DropLatitude = dropLat,
                DropLongitude = dropLng,
                Otp = otp // Store the generated OTP
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            // Directly call InitiatePayment method with the new booking ID
            return InitiatePayment(booking.BookingId);
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

            var cabId = booking?.CabId;
            var cab = _context.Cabs.Find(cabId);

            ViewBag.cabType = cab.CabType;


            if (booking == null)
            {
                return NotFound();
            }

            return View(booking); // This should match the view name and pass the booking model to the view.
        }




    }
}