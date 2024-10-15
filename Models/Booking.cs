using System.ComponentModel;

namespace CabBookingSystem.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        public int UserId { get; set; }

        public int CabId { get; set; }

        public string PickupLocation { get; set; }

        public int NumberOfPass { get; set; }

        public string DropLocation { get; set; }

        public int Price { get; set; }

        public DateTime BookingTime { get; set; }

        public double Distance { get; set; }

        public string? Status { get; set; }

        public User User { get; set; }

        // Adding latitude and longitude for Pickup and Drop locations
        public double PickupLatitude { get; set; } // Latitude for Pickup Location
        public double PickupLongitude { get; set; } // Longitude for Pickup Location
        public double DropLatitude { get; set; } // Latitude for Drop Location
        public double DropLongitude { get; set; } // Longitude for Drop Location

        public int? Otp { get; set; }
        public int? DriverId { get; set; }

    }
}
