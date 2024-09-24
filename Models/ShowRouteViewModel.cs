namespace CabBookingSystem.Models
{
    public class ShowRouteViewModel
    {
        public string PickupLocation { get; set; }
        public string DropLocation { get; set; }
        public DateTime BookingTime { get; set; }
        public int Price { get; set; }
        public double PickupLatitude { get; set; } // Add this
        public double PickupLongitude { get; set; } // Add this
        public double DropLatitude { get; set; } // Add this
        public double DropLongitude { get; set; } // Add this
    }

}
