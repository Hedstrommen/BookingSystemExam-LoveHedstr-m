# Booking System

A console application for managing meeting room bookings, built with .NET.

## Features

- Show active bookings
- Search bookings by customer name
- Create bookings with validation (room capacity, time format, double bookings)
- Cancel bookings
- Bookings are saved to file and persist between runs

## Getting started

### Requirements

- [.NET SDK](https://dotnet.microsoft.com/download) 10.0 or later

### Run the application

From the repository root, run:

```bash
dotnet run --project src/BookingSystem
```

The application is a console app. Use the on-screen menu to show active
bookings, search, create and cancel bookings. Select `0` to exit.

Booking data is stored in `src/BookingSystem/Data/bookings.json` and is
loaded automatically when the application starts.

### Run the tests

From the repository root, run:

```bash
dotnet test
```

The test project (`tests/BookingSystem.Tests`) uses xUnit and an in-memory
repository, so the tests run without touching the booking data file.
