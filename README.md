# Cab Booking System  - **🎥 [video](https://drive.google.com/file/d/1AbQkwSvbSVTchpWJRIfwrH6tQ2HTL2fQ/view)


## Project Overview

The **Cab Booking System** is a web-based application built using **ASP.NET Core MVC** and **Entity Framework Core**. This system allows users to book cabs by providing their pickup and drop locations, and drivers can accept or reject the bookings. The application provides an easy-to-use interface for both users and drivers to interact with the system.

## Features

- **User Registration & Authentication**: Users can sign up, log in, and manage their profile.
- **Driver Registration & Authentication**: Drivers can register, log in, and manage their profile.
- **Cab Booking**: Users can book cabs by providing pickup and drop locations.
- **Driver Assignment**: Drivers can accept or reject cab bookings.
- **Booking History**: Users can view their past bookings.
- **Real-Time Location**: Displays driver’s current location.
- **Booking Status Management**: Manage booking status as pending, completed, or canceled.
- **Responsive Design**: The application is responsive and works on various screen sizes.

## Tech Stack

- **Frontend**: Razor Pages, HTML, CSS, Bootstrap
- **Backend**: ASP.NET Core MVC
- **Database**: SQL Server (Entity Framework Core for database interaction)
- **ORM**: Entity Framework Core
- **IDE**: Microsoft Visual Studio 
- **API Integration**: Leaflet JS API for location services

## Database Structure

Here’s a brief overview of the main database tables:

1. **Users**: Stores user data such as name, email, password hash, and phone number.
2. **Cabs**: Stores information about cabs including cab number and type.
3. **Drivers**: Stores driver information such as name, email, password, phone number, and assigned cab.
4. **Bookings**: Stores booking details including user ID, cab ID, pickup and drop locations, distance, price, and booking time.
5. **RejectedBookings**: Tracks bookings that were rejected by drivers.

## Prerequisites

Before you start, make sure you have the following installed:

- .NET 6 SDK or newer
- SQL Server
- Visual Studio (Community or Professional)

## Getting Started

Follow these instructions to get a copy of the project up and running on your local machine.

### Clone the Repository

```bash
git clone https://github.com/DanishVahora/CabBookingSystem.git
cd CabBookingSystem
```

### Setup the Database

- Open SQL Server Management Studio (SSMS).
- Create a new database called CabBookingSystem.
- Update the connection string in appsettings.json to match your SQL Server instance:

```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=CabBookingSystem;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

- Open the terminal in Visual Studio and run the following command to apply the migrations:

```bash
dotnet ef database update
```

### Running the Application

- Run following command in terminal:

```bash
dotnet run
```
- It will run on [http://localhost:5026](http://localhost:5026)


## Usage

### For Users:
1. Register and log in.
2. Book a cab by entering your pickup and drop locations.
3. View the status of your bookings and check your booking history.

### For Drivers:
1. Register and log in.
2. View booking requests from users.
3. Accept or reject a booking.
4. Navigate to the pickup location once a booking is accepted.


## Future Improvements

- Payment Integration: Add online payment functionality.
- Notifications: Real-time notifications for drivers and users.
- Driver Ratings: Users can rate drivers after completing a ride.
- Admin Panel: An admin interface for managing users, drivers, and bookings.


