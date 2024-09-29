using System;
using System.Collections.Generic;
using System.Linq;

namespace CabBookingSystem
{
    // Enum for cab types
    public enum CabType
    {
        Sedan,
        SUV,
        Hatchback,
        Luxury
    }

    // Class for User
    public class User
    {
        public int UserId { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }

        public User(int userId, string name, string email)
        {
            UserId = userId;
            Name = name;
            Email = email;
        }

        public override string ToString()
        {
            return $"{UserId}: {Name}, Email: {Email}";
        }
    }

    // Class for Cab
    public class Cab
    {
        public int CabId { get; private set; }
        public CabType Type { get; private set; }
        public string LicensePlate { get; private set; }
        public bool IsAvailable { get; set; }

        public Cab(int cabId, CabType type, string licensePlate)
        {
            CabId = cabId;
            Type = type;
            LicensePlate = licensePlate;
            IsAvailable = true;
        }

        public override string ToString()
        {
            return $"{CabId}: {Type}, License Plate: {LicensePlate}, Available: {IsAvailable}";
        }
    }

    // Class for Booking
    public class Booking
    {
        public int BookingId { get; private set; }
        public User User { get; private set; }
        public Cab Cab { get; private set; }
        public DateTime BookingDate { get; private set; }

        public Booking(int bookingId, User user, Cab cab)
        {
            BookingId = bookingId;
            User = user;
            Cab = cab;
            BookingDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{BookingId}: {User.Name} booked {Cab.LicensePlate} on {BookingDate}";
        }
    }

    // Class for Cab Manager
    public class CabManager
    {
        private List<Cab> cabs;

        public CabManager()
        {
            cabs = new List<Cab>
            {
                new Cab(1, CabType.Sedan, "ABC123"),
                new Cab(2, CabType.SUV, "XYZ456"),
                new Cab(3, CabType.Hatchback, "DEF789"),
                new Cab(4, CabType.Luxury, "GHI012")
            };
        }

        public List<Cab> GetAvailableCabs() => cabs.Where(c => c.IsAvailable).ToList();

        public void BookCab(int cabId)
        {
            var cab = cabs.FirstOrDefault(c => c.CabId == cabId);
            if (cab != null && cab.IsAvailable)
            {
                cab.IsAvailable = false;
            }
            else
            {
                Console.WriteLine("Cab not available for booking.");
            }
        }

        public void ReleaseCab(int cabId)
        {
            var cab = cabs.FirstOrDefault(c => c.CabId == cabId);
            if (cab != null)
            {
                cab.IsAvailable = true;
            }
        }
    }

    // Class for User Manager
    public class UserManager
    {
        private List<User> users;

        public UserManager()
        {
            users = new List<User>
            {
                new User(1, "Alice Johnson", "alice@example.com"),
                new User(2, "Bob Smith", "bob@example.com"),
                new User(3, "Charlie Brown", "charlie@example.com")
            };
        }

        public User GetUserById(int userId) => users.FirstOrDefault(u => u.UserId == userId);
    }

    // Main Program
    class Program
    {
        static void Main(string[] args)
        {
            CabManager cabManager = new CabManager();
            UserManager userManager = new UserManager();

            Console.WriteLine("Welcome to the Cab Booking System!");
            Console.WriteLine("Available Cabs:");
            var availableCabs = cabManager.GetAvailableCabs();
            foreach (var cab in availableCabs)
            {
                Console.WriteLine(cab);
            }

            Console.WriteLine("\nBooking a Cab:");
            var user = userManager.GetUserById(1); // Get user by ID
            if (user != null && availableCabs.Count > 0)
            {
                cabManager.BookCab(availableCabs.First().CabId);
                Console.WriteLine($"Cab booked successfully for {user.Name}.");
            }

            Console.WriteLine("\nAvailable Cabs after booking:");
            availableCabs = cabManager.GetAvailableCabs();
            foreach (var cab in availableCabs)
            {
                Console.WriteLine(cab);
            }

            Console.WriteLine("\nReleasing a Cab:");
            cabManager.ReleaseCab(1); // Release cab with ID 1
            Console.WriteLine("Cab released successfully.");

            Console.WriteLine("\nAvailable Cabs after release:");
            availableCabs = cabManager.GetAvailableCabs();
            foreach (var cab in availableCabs)
            {
                Console.WriteLine(cab);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
