# Cab Booking System

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
- **IDE**: Visual Studio
- **API Integration**: Google Maps API for location services

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
- Postman (for API testing, optional)

## Getting Started

Follow these instructions to get a copy of the project up and running on your local machine.

### Clone the Repository

```bash
git clone https://github.com/your-username/cab-booking-system.git
cd cab-booking-system
