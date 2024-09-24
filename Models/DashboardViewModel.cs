using CabBookingSystem.Models;
using System.Collections.Generic;

namespace CabBookingSystem.Models
{
    public class DashboardViewModel
    {
        public Driver Driver { get; set; }
        public List<Booking> CurrentBookings { get; set; }
    }
}
