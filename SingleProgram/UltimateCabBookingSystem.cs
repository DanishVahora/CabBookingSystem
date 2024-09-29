using System;
using System.Collections.Generic;
using System.Linq;

namespace UltimateCabBookingSystem
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

    // Class for Driver
    public class Driver
    {
        public int DriverId { get; private set; }
        public string Name { get; private set; }
        public string LicenseNumber { get; private set; }
        public CabType CabType { get; private set; }

        public Driver(int driverId, string name, string licenseNumber, CabType cabType)
        {
            DriverId = driverId;
            Name = name;
            LicenseNumber = licenseNumber;
            CabType = cabType;
        }

        public override string ToString()
        {
            return $"{DriverId}: {Name}, License: {LicenseNumber}, Cab Type: {CabType}";
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

        public Booking(int bookingId, User user, Cab cab, double fare)
        {
            BookingId = bookingId;
            User = user;
            Cab = cab;
            Fare = fare;
            BookingDate = DateTime.Now;
            PaymentStatus = PaymentStatus.Pending;
        }

        public void CompletePayment()
        {
            PaymentStatus = PaymentStatus.Completed;
        }

        public override string ToString()
        {
            return $"{BookingId}: {User.Name} booked {Cab.LicensePlate} on {BookingDate}, Fare: {Fare}, Payment Status: {PaymentStatus}";
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
            // In real implementation, you could check if the driver is already assigned to a cab
            return drivers.Where(d => d.CabType == cabType).ToList();
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

            Console.WriteLine("Welcome to the Ultimate Cab Booking System!");

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
                    double fare = cabManager.CalculateFare(availableCabs.First(), 10.0); // Example distance of 10 km
                    Booking booking = new Booking(1, user, availableCabs.First(), fare);
                    Console.WriteLine($"Cab booked successfully for {user.Name} with fare: {fare}");
                    booking.CompletePayment();
                    Console.WriteLine(booking);
                }
            }

            // Release a cab
            Console.WriteLine("\nReleasing a Cab:");
            cabManager.ReleaseCab(1); // Release cab with ID 1
            Console.WriteLine("Cab released successfully.");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
