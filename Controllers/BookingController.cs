﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]


        [HttpPost]
        public IActionResult ConfirmBooking(int cabId, string pickupLocation, string dropLocation, DateTime bookingTime, double distance, int numberOfPass, int price, double pickupLat, double pickupLng, double dropLat, double dropLng)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["logFirst"] = "Login First";
                return RedirectToAction("Login", "Account");
            }

            int userId = HttpContext.Session.GetInt32("UserId").Value;

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
                Status = "pending",
                PickupLatitude = pickupLat,
                PickupLongitude = pickupLng,
                DropLatitude = dropLat,
                DropLongitude = dropLng
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return View("BookingConfirmation", booking);
        }
    }
}
