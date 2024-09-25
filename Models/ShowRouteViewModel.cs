namespace CabBookingSystem.Models
{
    public class ShowRouteViewModel
    {
        public int BookingId { get; set; } // Add this property
        public string PickupLocation { get; set; }
        public string DropLocation { get; set; }
        public DateTime BookingTime { get; set; }
        public decimal Price { get; set; }
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public double DropLatitude { get; set; }
        public double DropLongitude { get; set; }
    }
}
