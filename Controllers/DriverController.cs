using Microsoft.AspNetCore.Mvc;
using CabBookingSystem.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class DriverController : Controller
{
    private readonly ApplicationDbContext _context;

    public DriverController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Login
    public IActionResult Login()
    {
        return View();
    }

    // POST: Login
    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        // Find the user by email
        var driver = _context.Drivers.SingleOrDefault(d => d.Email == email);

        if (driver != null)
        {
            // Directly compare the provided password with the stored password (plaintext)
            if (driver.Password == password)
            {
                // Login successful, store user session
                HttpContext.Session.SetInt32("Id", driver.Id);
                TempData["LoginMessage"] = "Login Successful!";
                return RedirectToAction("Dashboard", "Driver"); // Redirect to Home/Booking page
            }
        }

        // If credentials are invalid, show error message
        TempData["Message"] = "Invalid Credential.";
        return View();
    }

    // GET: Signup
    public IActionResult Signup()
    {
        return View();
    }

    // POST: Signup
    [HttpPost]
    public IActionResult Signup(string driverName, string location, string email, string password, string confirmPassword)
    {
        // Check if a driver with the same email already exists
        var existingDriver = _context.Drivers.SingleOrDefault(d => d.Email == email);
        if (existingDriver != null)
        {
            TempData["driverExist"] = "Driver already exists.";
            return RedirectToAction("Signup", "Driver");
        }

        // Check if the passwords match
        if (password != confirmPassword)
        {
            TempData["driverExist"] = "Passwords do not match.";
            return RedirectToAction("Signup", "Driver");
        }

        // Create a new driver instance
        var newDriver = new Driver
        {
            Name = driverName,
            Email = email,
            Location = location,
            Password = password, // Consider hashing the password
            CabId = 1,
            IsAvailable = false,
            PhoneNumber = 21144 // Replace with actual phone number logic if needed
        };

        // Add the new driver to the database
        _context.Drivers.Add(newDriver);
        _context.SaveChanges();

        // Store user session after signup
        HttpContext.Session.SetInt32("Id", newDriver.Id);
        TempData["LoginMessage"] = "Account Created Successfully!";

        return RedirectToAction("Dashboard", "Driver");
    }


    public IActionResult Dashboard()
    {
       

        // Retrieve the driver ID from the session
        int? driverId = HttpContext.Session.GetInt32("Id");

        if (driverId == null)
        {
            TempData["LoginMessage"] = "Please log in first.";
            return RedirectToAction("Login");
        }

        // Get the driver information from the database
        var driver = _context.Drivers
            .Include(d => d.Cab) // Assuming a relation to Cab exists
            .SingleOrDefault(d => d.Id == driverId);

        if (driver == null)
        {
            return NotFound();
        }

        // Fetch current bookings where the pickup location matches the driver's location
        var currentBookings = _context.Bookings.ToList();

        // Create the view model
        var dashboardViewModel = new DashboardViewModel
        {
            Driver = driver,
            CurrentBookings = currentBookings
        };

        return View(dashboardViewModel); // Pass the view model to the view
    }

    [HttpPost]
    public IActionResult UpdateBookingStatus(int bookingId, string status)
    {
        // Fetch the booking by its ID
        var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);

        if (booking != null && booking.Status == "pending")
        {
            if (status == "Accepted")
            {
                // Update the status to 'Confirmed'
                booking.Status = "Confirmed";

                // Save the changes to the database
                _context.SaveChanges();

                // Redirect to ShowRoute action, passing the bookingId
                return RedirectToAction("ShowRoute", new { bookingId = booking.BookingId });
            }
            else if (status == "Rejected")
            {
                // Logic for rejection (only remove for driver, not from booking table)
                TempData[$"RejectedBooking_{bookingId}"] = true;  // Flag this booking as rejected for this driver
            }
        }

        // Redirect back to the driver's dashboard after handling the status
        return RedirectToAction("Dashboard");
    }



    public IActionResult ShowDropRoute(int bookingId)
    {
        int? driverId = HttpContext.Session.GetInt32("Id");

        if (driverId == null)
        {
            TempData["LoginMessage"] = "Please log in first.";
            return RedirectToAction("Login");
        }

        var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);

        if (booking != null)
        {
            var viewModel = new ShowRouteViewModel
            {
                PickupLocation = booking.PickupLocation,
                DropLocation = booking.DropLocation,
                BookingTime = booking.BookingTime,
                Price = booking.Price,
                PickupLatitude = booking.PickupLatitude,
                PickupLongitude = booking.PickupLongitude,
                DropLatitude = booking.DropLatitude,
                DropLongitude = booking.DropLongitude,
            };

            return View(viewModel); // Create a corresponding view for ShowDropRoute
        }

        return RedirectToAction("Dashboard");
    }



    [HttpPost]
    public IActionResult VerifyOtp(int otp, int bookingId)
    {
        // Retrieve driver ID from session
        int? driverId = HttpContext.Session.GetInt32("Id");

        if (driverId == null)
        {
            TempData["LoginMessage"] = "Please log in first.";
            return RedirectToAction("Login");
        }

        // Retrieve the booking using the bookingId
        var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);

        // Check if the booking is null
        if (booking == null)
        {
            // Booking not found, handle the error and redirect to an appropriate page
            TempData["OtpError"] = "Booking not found.";
            return RedirectToAction("Dashboard");
        }

        // Now check if the OTP matches
        if (booking.Otp == otp)
        {
            // OTP is correct, redirect to ShowDropRoute action
            return RedirectToAction("ShowDropRoute", new { bookingId = bookingId });
        }
        else
        {
            // OTP mismatch
            TempData["OtpError"] = "Invalid OTP. Please try again.";
            return RedirectToAction("ShowRoute", new { bookingId = bookingId });
        }
    }








    public IActionResult ShowRoute(int bookingId)
    {
        int? driverId = HttpContext.Session.GetInt32("Id");

        if (driverId == null)
        {
            TempData["LoginMessage"] = "Please log in first.";
            return RedirectToAction("Login");
        }

        // Log the bookingId being processed
        System.Diagnostics.Debug.WriteLine($"ShowRoute called with bookingId: {bookingId}");

        var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
        if (booking == null)
        {
            TempData["OtpError"] = "Booking not found.";
            return RedirectToAction("Dashboard");
        }

        ViewBag.bId = bookingId;

        if (booking != null)
        {
            var viewModel = new ShowRouteViewModel
            {
                PickupLocation = booking.PickupLocation,
                DropLocation = booking.DropLocation,
                BookingTime = booking.BookingTime,
                Price = booking.Price,
                PickupLatitude = booking.PickupLatitude,
                PickupLongitude = booking.PickupLongitude,
                DropLatitude = booking.DropLatitude,
                DropLongitude = booking.DropLongitude,
            };

            return View(viewModel);
        }

        TempData["OtpError"] = "Booking not found.";
        return RedirectToAction("Dashboard");
    }


    public IActionResult RouteMap(int bookingId)
    {
        // You can retrieve the booking details again if needed
        var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);

        if (booking != null)
        {
            // Prepare any necessary data for the route map view
            var viewModel = new ShowRouteViewModel
            {
                // Populate with necessary data, such as pickup and drop locations
                PickupLatitude = booking.PickupLatitude,
                PickupLongitude = booking.PickupLongitude,
                DropLatitude = booking.DropLatitude,
                DropLongitude = booking.DropLongitude
            };

            return View(viewModel); // Create a corresponding RouteMap view
        }

        return RedirectToAction("ShowDropRoute");
    }





    // Logout
    public IActionResult Logout()
    {
        // Clear the session and redirect to login page
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
