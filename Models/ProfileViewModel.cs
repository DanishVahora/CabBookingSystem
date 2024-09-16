namespace CabBookingSystem.Models
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public List<Booking> PastBookings { get; set; }
    }
}
