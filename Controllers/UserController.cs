using CabBookingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CabBookingSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }
        public IActionResult Details(int id)
        {
            

            // Assuming you fetch the cab from the database based on id
            User user = _context.Users.Find(id);
            // Pass the cab information to the view
            return View(User);
        }
        // Other actions like Register, Login can go here
    }
}
