using Microsoft.EntityFrameworkCore;

namespace CabBookingSystem.Models
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)  // Pass options to the base class constructor
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Cab> Cabs { get; set; }
        public DbSet<Booking> Bookings { get; set; }


    }
}
