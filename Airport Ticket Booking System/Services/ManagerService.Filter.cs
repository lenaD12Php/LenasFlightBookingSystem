using System.Runtime.CompilerServices;

namespace Airport_Ticket_Booking_System;

public partial class ManagerService
{
    public async Task FilterBookingsAsync()
    {
        Console.WriteLine("Filter Bookings");
        Console.WriteLine("--------------------------------------------");

        Console.WriteLine("Enter flight number:");
        string flightNumber = Console.ReadLine();

        Console.WriteLine("Enter minimum price:");
        if (!decimal.TryParse(Console.ReadLine(), out decimal minPrice))
        {
            Console.WriteLine("Invalid minimum price.");
            return;
        }

        Console.WriteLine("Enter maximum price:");
        if (!decimal.TryParse(Console.ReadLine(), out decimal maxPrice))
        {
            Console.WriteLine("Invalid maximum price.");
            return;
        }

        Console.WriteLine("Enter departure country:");
        string departureCountry = Console.ReadLine();

        Console.WriteLine("Enter destination country:");
        string destinationCountry = Console.ReadLine();

        Console.WriteLine("Enter departure date:");
        DateTime? departureDate = null;
        string departureDateInput = Console.ReadLine();
        if (string.IsNullOrEmpty(departureDateInput) || !DateTime.TryParse(departureDateInput, out DateTime parsedDate))
        {
            Console.WriteLine("Invalid date format.");
            return;
        }
        departureDate = parsedDate;

        Console.WriteLine("Enter departure airport:");
        string departureAirport = Console.ReadLine();

        Console.WriteLine("Enter arrival airport:");
        string arrivalAirport = Console.ReadLine();

        Console.WriteLine("Enter passenger name:");
        string passengerName = Console.ReadLine();

        Console.WriteLine("Enter flight class:");
        string classInput = Console.ReadLine();
        FlightClass? flightClass = null;
        if (!string.IsNullOrEmpty(classInput) && Enum.TryParse(classInput, true, out FlightClass parsedClass))
        {
            flightClass = parsedClass;
        }

        Console.WriteLine("Enter passenger type: (1) Adult, (2) Child, (3) Baby");
        if (!int.TryParse(Console.ReadLine(), out int passengerChoice) || !Enum.IsDefined(typeof(PassengerType), passengerChoice))
        {
            Console.WriteLine("Invalid passenger type choice.");
            return;
        }
        PassengerType passengerType = (PassengerType)passengerChoice;

        Console.WriteLine("Enter airline: (1) TurkishAirlines, (2) BritishAirways, (3) Lufthansa, (4) AustrianAirlines");
        Airlines? airline = null;
        if (int.TryParse(Console.ReadLine(), out int airlineChoice))
        {
            airline = airlineChoice switch
            {
                1 => Airlines.TurkishAirlines,
                2 => Airlines.BritishAirways,
                3 => Airlines.Lufthansa,
                4 => Airlines.AustrianAirlines,
                _ => null
            };
        }


        var filteredBookings = (await _bookingRepository.GetAllBookingsAsync())
            .Where(b => (string.IsNullOrEmpty(flightNumber) || b.flight.FlightNumber == flightNumber)
                        && (b.totalPrice >= minPrice && b.totalPrice <= maxPrice)
                        && (string.IsNullOrEmpty(departureCountry) || b.flight.DepartureAirport.Equals(departureCountry, StringComparison.OrdinalIgnoreCase))
                        && (string.IsNullOrEmpty(destinationCountry) || b.flight.ArrivalAirport.Equals(destinationCountry, StringComparison.OrdinalIgnoreCase))
                        && (!departureDate.HasValue || b.flight.DepartureDateTime.Date == departureDate.Value.Date)
                        && (string.IsNullOrEmpty(departureAirport) || b.flight.DepartureAirport.Equals(departureAirport, StringComparison.OrdinalIgnoreCase))
                        && (string.IsNullOrEmpty(arrivalAirport) || b.flight.ArrivalAirport.Equals(arrivalAirport, StringComparison.OrdinalIgnoreCase))
                        && (string.IsNullOrEmpty(passengerName) || b.Passengers.Any(p => (p.FirstName + " " + p.LastName).Equals(passengerName, StringComparison.OrdinalIgnoreCase)))
                        && (flightClass == null || b.flightClass == flightClass)
                        && (airline == null || b.flight.Airlines == airline))
            .ToList();

        if (filteredBookings.Any())
        {
            foreach (var booking in filteredBookings)
            {
                Console.WriteLine($"Booking ID: {booking.bookingID}, Passenger: {string.Join(", ", booking.Passengers.Select(p => $"{p.FirstName} {p.LastName}"))}," +
                    $" Flight: {booking.flight.FlightNumber}, Class: {booking.flightClass}, Total Price: {booking.totalPrice}");
            }
        }
        else
        {
            Console.WriteLine("No bookings found within the specified criteria.");
            FilterBookingsAsync();
        }
    }
}


