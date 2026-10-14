using BookingSystem.Models;
using BookingSystem.Repositories;

namespace BookingSystem.Tests;

public class InMemoryBookingRepository(params Booking[] bookings) : IBookingRepository
{
    //Ett fält av lista av booking som får värdet av befintliga bokningar i listan som skickats in till repon.
    private readonly List<Booking> _bookings = [.. bookings];

    public int SaveCount { get; private set; }

    public IReadOnlyList<Booking> GetAll() => _bookings;

    public void Add(Booking booking) => _bookings.Add(booking);

    public void Save() => SaveCount++;
}
