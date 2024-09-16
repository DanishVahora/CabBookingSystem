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

    }
}
