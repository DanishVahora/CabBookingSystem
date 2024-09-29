using Microsoft.AspNetCore.Mvc;
using CabBookingSystem.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace CabBookingSystem.Controllers
{
    public class CabController : Controller
    {
        public IActionResult Index()
        {
            // You can fetch the list of cabs from the database (for now, a static list)
            List<Cab> cabs = new List<Cab>
            {
                new Cab { CabId = 1, CabNumber = "ABC123", CabType = "Sedan" },
                new Cab { CabId = 2, CabNumber = "XYZ456", CabType = "SUV" }
            };
            return View(cabs);
        }

        public IActionResult Details(int id)
        {
            // Fetch the cab details using id
            Cab cab = new Cab { CabId = id, CabNumber = "ABC123",  CabType = "Sedan" };
            return View(cab);
        }
    }
}
