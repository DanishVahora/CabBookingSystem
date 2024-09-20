using Microsoft.AspNetCore.Mvc;
using CabBookingSystem.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
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
        var user = _context.Users.SingleOrDefault(u => u.Email == email);

        if (user != null)
        {
            // Directly compare the provided password with the stored password (plaintext)
            if (user.PasswordHash == password)
            {
                // Login successful, store user session
                HttpContext.Session.SetInt32("UserId", user.UserId);
                TempData["LoginMessage"] = "Login Successful!";
                return RedirectToAction("Index", "Home"); // Redirect to Home/Booking page
            }
        }

        // If credentials are invalid, show error message
        ViewBag.Message = "Invalid credentials";
        return View();
    }

    // GET: Signup
    public IActionResult Signup()
    {
        return View();
    }

    // POST: Signup
    [HttpPost]
    public IActionResult Signup(User user, string password)
    {
        User use = _context.Users.SingleOrDefault(u => u.Email == user.Email);
        if (use != null)
        {
            TempData["userExist"] = "User Exist";
            return RedirectToAction("Signup", "Account");
        }
        // Directly store the password as plaintext in the PasswordHash field
        user.PasswordHash = password;

        // Add the new user to the database
        _context.Users.Add(user);
        _context.SaveChanges();

        // Store users session after signup
        HttpContext.Session.SetInt32("UserId", user.UserId);
        TempData["LoginMessage"] = "Account Created Successfully!";

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Profile()
    {
        if (HttpContext.Session.GetInt32("UserId") == null)
        {
            TempData["logFirst"] = "Login First";
            return RedirectToAction("Login", "Account");
        }

        int userId = HttpContext.Session.GetInt32("UserId").Value;

        // Fetch the user and their related bookings
        var user = _context.Users
            .Include(u => u.Bookings)
            .SingleOrDefault(u => u.UserId == userId);

        if (user == null)
        {
            return NotFound(); // Handle case when user not found
        }

        return View(user); // Pass the 'user' object to the view
    }



    // Logout
    public IActionResult Logout()
    {
        // Clear the session and redirect to login page
        HttpContext.Session.Clear();
        return RedirectToAction("Index","Home");
    }
}
