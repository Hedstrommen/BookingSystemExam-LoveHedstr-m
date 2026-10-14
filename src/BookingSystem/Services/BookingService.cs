using BookingSystem.Models;
using BookingSystem.Repositories;

namespace BookingSystem.Services;

public class BookingService(IBookingRepository repository)
{
    private static readonly IReadOnlyDictionary<string, Room> Rooms =
        new Dictionary<string, Room>(StringComparer.OrdinalIgnoreCase)
        {
            ["Aspen"] = new("Aspen", 4),
            ["Birken"] = new("Birken", 8),
            ["Eken"] = new("Eken", 12)
        };

    public IReadOnlyList<Booking> GetActiveBookings()
    {
        //Skapar en enbart läsbar lista av modellen Booking vid namn currentBokkings som jag genom repositoryns metod GetAll hämtar alla booking.
        IReadOnlyList<Booking> currentBookings = repository.GetAll();

        //Skapar en ny lista som jag kan ändra med .Add
        List<Booking> activeBookings = new List<Booking>();
        
        //Loopar igenom alla booking i currentBookings och kollar om IsCancelled propertyn INTE är cancelled (alltså om den är false). Om booking är false så lägger jag till den till min activeBookings.Lista. 
        foreach (var booking in currentBookings)
        {
            if (!booking.IsCancelled)
            {
                activeBookings.Add(booking);
            }
        }

        var sortedActiveBookings = SortBookings(activeBookings);

        //Skickar tillbaka sorterade listan. Medveten om att det är en List och inte en IReadOnly men det funkar ändå.
        return sortedActiveBookings;
    }

    //Skapar egen metod som sorterar bookings. 
    public List<Booking> SortBookings(IReadOnlyList<Booking> bookingsToSort)
    {
        //Sorterar enligt kraven genom LINQ. Inte helt med på hur bakomliggandekod ser ut men det som sker är:
        //Vi skapar en List som vi tilldelar värdet av listan bookingsToSort som vi skickade in i metoden. Genom LINQ skriver vi en ny lista så vi sorterar under den ny skrivningen, därför behöver vi en ToList i slutet annars hade ingenting hänt. Jag använder LINQ för att den inte påverkar originallistan utan skapar en ny som visar utåt. 
        List<Booking> sortedBookings = bookingsToSort
            .OrderBy(booking => booking.Date)
            .ThenBy(booking => booking.StartTime)
            .ToList();

        return sortedBookings;
    }


    public IReadOnlyList<Booking> Search(string searchTerm)
    {
        //gör sökningen case insensitive genom ToLowermetoden.
        string searchTermLower = searchTerm.ToLower();
        
        //.Where kollar om den INTE är cancelled samt Contains kollar delar av namn i sökningen. Jag lägger till To.Lower till customerName för att göra den case insensitive som söktermen.
        return repository.GetAll()
            .Where(booking =>
                !booking.IsCancelled &&
                booking.CustomerName.ToLower().Contains(searchTermLower))
            .ToList();
    }

    public BookingResult Create(Booking booking)
    {
        var validationResult = Validate(booking);

        if (!validationResult.IsSuccessful)
        {
            return validationResult;
        }

        booking.Id = repository.GetAll().Select(item => item.Id).DefaultIfEmpty(1000).Max() + 1;
        repository.Add(booking);
        repository.Save();

        return BookingResult.Success($"Booking {booking.Id} was created.");
    }

    public BookingResult Cancel(int bookingId)
    {
        throw new NotImplementedException();
    }


    private BookingResult Validate(Booking booking)
    {
        if (string.IsNullOrWhiteSpace(booking.CustomerName))
        {
            return BookingResult.Failure("Customer name is required.");
        }

        if (!Rooms.TryGetValue(booking.RoomName, out var room))
        {
            return BookingResult.Failure("The selected room does not exist.");
        }

        if (booking.StartTime == booking.EndTime)
        {
            return BookingResult.Failure("The start time must be earlier than the end time.");
        }

        if (booking.Participants < 1)
        {
            return BookingResult.Failure("The number of participants must be at least 1.");
        }

        if (booking.Participants > room.Capacity)
        {
            return BookingResult.Failure($"{room.Name} has a maximum capacity of {room.Capacity}.");
        }

        var hasOverlap = repository.GetAll().Any(existing =>
            !existing.IsCancelled &&
            existing.Date == booking.Date &&
            existing.RoomName.Equals(booking.RoomName, StringComparison.OrdinalIgnoreCase) &&
            booking.StartTime >= existing.StartTime &&
            booking.StartTime < existing.EndTime);

        if (hasOverlap)
        {
            return BookingResult.Failure("The room is already booked during the selected time.");
        }

        return BookingResult.Success("The booking is valid.");
    }
}
