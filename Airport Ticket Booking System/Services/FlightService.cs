namespace AirportTicketBookingSystem;

public class FlightService
{
    private readonly FlightRepository _flightRepository;

    public FlightService(FlightRepository flightRepository)
    {
        _flightRepository = flightRepository ?? throw new ArgumentNullException(nameof(flightRepository));
    }

    public void AddFlight(Flight flight)
    {
        if (flight == null)
            throw new ArgumentNullException(nameof(flight));

        _flightRepository.AddFlight(flight);
        Console.WriteLine($"Flight {flight.FlightNumber} has been successfully added.");
    }

    public void UpdateFlight(Flight updatedFlight)
    {
        if (updatedFlight == null)
            throw new ArgumentNullException(nameof(updatedFlight));

        var existingFlight = _flightRepository.GetFlightByNumber(updatedFlight.FlightNumber);
        if (existingFlight != null)
        {
            _flightRepository.UpdateFlight(updatedFlight);
            Console.WriteLine($"Flight {updatedFlight.FlightNumber} has been successfully updated.");
        }
        else
            Console.WriteLine($"Flight {updatedFlight.FlightNumber} does not exist.");
    }

    public void RemoveFlight(string flightNumber)
    {
        if (string.IsNullOrEmpty(flightNumber))
            throw new ArgumentNullException(nameof(flightNumber));

        var existingFlight = _flightRepository.GetFlightByNumber(flightNumber);
        if (existingFlight != null)
        {
            _flightRepository.RemoveFlight(flightNumber);
            Console.WriteLine($"Flight {flightNumber} has been successfully removed.");
        }
        else
            Console.WriteLine($"Flight {flightNumber} does not exist.");
    }

    public bool CheckAvailability(string flightNumber, FlightClass flightClass, int requestedSeats)
    {
        if (string.IsNullOrEmpty(flightNumber))
            throw new ArgumentNullException(nameof(flightNumber));

        var flight = _flightRepository.GetFlightByNumber(flightNumber);
        if (flight != null)
        {
            int availableSeats = flight.GetAvailableSeats(flightClass);
            return availableSeats >= requestedSeats;
        }

        Console.WriteLine($"Flight {flightNumber} does not exist.");
        return false;
    }

    public IEnumerable<Flight> SearchFlights(string departureAirport, string arrivalAirport, DateTime? departureDate,
        PassengerType passengerType, Airlines airline, FlightClass? flightClass = null, decimal? maxPrice = null)
    {
        var allFlights = _flightRepository.GetAllFlights();

        return allFlights.Where(flight =>
            (string.IsNullOrEmpty(departureAirport) || flight.DepartureAirport.Equals(departureAirport,
            StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(arrivalAirport) || flight.ArrivalAirport.Equals(arrivalAirport,
            StringComparison.OrdinalIgnoreCase)) &&
            (!departureDate.HasValue || flight.DepartureDateTime.Date == departureDate.Value.Date) &&
            (!flightClass.HasValue || flight.GetAvailableSeats(flightClass.Value) > 0) &&
            (!maxPrice.HasValue || flightClass.HasValue && flight.PricePerPerson.GetPrice(airline, flightClass.Value,
            passengerType) <= maxPrice.Value)
        );
    }
}

