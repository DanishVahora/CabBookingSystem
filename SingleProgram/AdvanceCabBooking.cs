using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedCabBookingSystem
{
    // Enum for cab types
    public enum CabType
    {
        Sedan,
        SUV,
        Hatchback,
        Luxury
    }

    // Enum for payment status
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }

    // Class for User
    public class User
    {
        public int UserId { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public List<Review> Reviews { get; private set; } = new List<Review>();
        public List<Booking> BookingHistory { get; private set; } = new List<Booking>();

        public User(int userId, string name, string email)
        {
            UserId = userId;
            Name = name;
            Email = email;
        }

        public void AddReview(Review review)
        {
            Reviews.Add(review);
        }

        public void AddBooking(Booking booking)
        {
            BookingHistory.Add(booking);
        }

        public override string ToString()
        {
            return $"{UserId}: {Name}, Email: {Email}";
        }
    }

    // Class for Driver
    public class Driver
    {
        public int DriverId { get; private set; }
        public string Name { get; private set; }
        public string LicenseNumber { get; private set; }
        public CabType CabType { get; private set; }
        public double Rating { get; private set; }
        public int TotalTrips { get; private set; }

        public Driver(int driverId, string name, string licenseNumber, CabType cabType)
        {
            DriverId = driverId;
            Name = name;
            LicenseNumber = licenseNumber;
            CabType = cabType;
            Rating = 0;
            TotalTrips = 0;
        }

        public void RateDriver(double rating)
        {
            Rating = (Rating * TotalTrips + rating) / (TotalTrips + 1);
            TotalTrips++;
        }

        public override string ToString()
        {
            return $"{DriverId}: {Name}, License: {LicenseNumber}, Cab Type: {CabType}, Rating: {Rating:F2}";
        }
    }

    // Class for Review
    public class Review
    {
        public int ReviewId { get; private set; }
        public User User { get; private set; }
        public string Comment { get; private set; }
        public double Rating { get; private set; }

        public Review(int reviewId, User user, string comment, double rating)
        {
            ReviewId = reviewId;
            User = user;
            Comment = comment;
            Rating = rating;
        }

        public override string ToString()
        {
            return $"Review by {User.Name}: {Comment}, Rating: {Rating}";
        }
    }

    // Class for Cab
    public class Cab
    {
        public int CabId { get; private set; }
        public CabType Type { get; private set; }
        public string LicensePlate { get; private set; }
        public bool IsAvailable { get; set; }
        public Driver AssignedDriver { get; set; }

        public Cab(int cabId, CabType type, string licensePlate)
        {
            CabId = cabId;
            Type = type;
            LicensePlate = licensePlate;
            IsAvailable = true;
        }

        public override string ToString()
        {
            return $"{CabId}: {Type}, License Plate: {LicensePlate}, Available: {IsAvailable}, Driver: {(AssignedDriver != null ? AssignedDriver.Name : "None")}";
        }
    }

    // Class for Booking
    public class Booking
    {
        public int BookingId { get; private set; }
        public User User { get; private set; }
        public Cab Cab { get; private set; }
        public DateTime BookingDate { get; private set; }
        public double Fare { get; private set; }
        public PaymentStatus PaymentStatus { get; private set; }
        public string PickupLocation { get; private set; }
        public string DropoffLocation { get; private set; }

        public Booking(int bookingId, User user, Cab cab, double fare, string pickupLocation, string dropoffLocation)
        {
            BookingId = bookingId;
            User = user;
            Cab = cab;
            Fare = fare;
            BookingDate = DateTime.Now;
            PaymentStatus = PaymentStatus.Pending;
            PickupLocation = pickupLocation;
            DropoffLocation = dropoffLocation;
        }

        public void CompletePayment()
        {
            PaymentStatus = PaymentStatus.Completed;
        }

        public override string ToString()
        {
            return $"{BookingId}: {User.Name} booked {Cab.LicensePlate} from {PickupLocation} to {DropoffLocation} on {BookingDate}, Fare: {Fare}, Payment Status: {PaymentStatus}";
        }
    }

    // Class for managing Cabs
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

        public void BookCab(int cabId, Driver driver)
        {
            var cab = cabs.FirstOrDefault(c => c.CabId == cabId);
            if (cab != null && cab.IsAvailable)
            {
                cab.IsAvailable = false;
                cab.AssignedDriver = driver;
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
                cab.AssignedDriver = null;
            }
        }

        public double CalculateFare(Cab cab, double distance)
        {
            double baseFare = 50; // Base fare
            double perKmRate = cab.Type switch
            {
                CabType.Sedan => 10,
                CabType.SUV => 15,
                CabType.Hatchback => 8,
                CabType.Luxury => 20,
                _ => 0
            };
            return baseFare + (perKmRate * distance);
        }
    }

    // Class for managing Users
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

        public void LeaveReview(User user, Review review)
        {
            user.AddReview(review);
        }
    }

    // Class for managing Drivers
    public class DriverManager
    {
        private List<Driver> drivers;

        public DriverManager()
        {
            drivers = new List<Driver>
            {
                new Driver(1, "David White", "DL123456", CabType.Sedan),
                new Driver(2, "Eva Green", "DL654321", CabType.SUV),
                new Driver(3, "Frank Castle", "DL987654", CabType.Hatchback)
            };
        }

        public List<Driver> GetAvailableDrivers(CabType cabType)
        {
            return drivers.Where(d => d.CabType == cabType).ToList();
        }

        public void RateDriver(int driverId, double rating)
        {
            var driver = drivers.FirstOrDefault(d => d.DriverId == driverId);
            if (driver != null)
            {
                driver.RateDriver(rating);
            }
        }
    }

    // Main Program
    class Program
    {
        static void Main(string[] args)
        {
            CabManager cabManager = new CabManager();
            UserManager userManager = new UserManager();
            DriverManager driverManager = new DriverManager();

            Console.WriteLine("Welcome to the Advanced Cab Booking System!");

            // User interaction to book a cab
            var user = userManager.GetUserById(1); // Get user by ID
            Console.WriteLine($"\nUser: {user}");

            Console.WriteLine("\nAvailable Cabs:");
            var availableCabs = cabManager.GetAvailableCabs();
            foreach (var cab in availableCabs)
            {
                Console.WriteLine(cab);
            }

            // Book a cab
            if (availableCabs.Count > 0)
            {
                var driver = driverManager.GetAvailableDrivers(availableCabs.First().Type).FirstOrDefault();
                if (driver != null)
                {
                    cabManager.BookCab(availableCabs.First().CabId, driver);
                    double fare = cabManager.CalculateFare(availableCabs.First(), 10); // Calculate fare for 10 km
                    var booking = new Booking(1, user, availableCabs.First(), fare, "Location A", "Location B");
                    user.AddBooking(booking);
                    booking.CompletePayment();
                    Console.WriteLine($"\nBooking Details: {booking}");
                }
            }

            // Leaving a review
            var review = new Review(1, user, "Great service!", 5);
            userManager.LeaveReview(user, review);
            Console.WriteLine($"\nUser Reviews:");
            foreach (var r in user.Reviews)
            {
                Console.WriteLine(r);
            }
        }
    }
}
