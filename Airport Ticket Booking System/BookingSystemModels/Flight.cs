namespace Airport_Ticket_Booking_System;

/// <summary>
/// To simplify the implementation, all airlines will initially have the same number of seats available for each flight class.
/// This approach allows for a consistent reservation system, though it can be extended to customize seat availability per airline in the future.
/// </summary>

public class Flight
{
    private const int _economyClassSeatsNumber = 120;
    private const int _premiumClassSeatsNumber = 30;
    private const int _businessClassSeatsNumber = 16;
    private const int _firstClassSeatsNumber = 10;

    public string FlightNumber { get; init; }
    public Airlines Airlines { get; init; }
    public string DepartureAirport { get; set; }
    public string ArrivalAirport { get; set; }
    public DateTime DepartureDateTime { get; set; }
    public DateTime ArrivalDateTime { get; set; }
    public List<FlightClass> BookedSeats { get; private set; } = new List<FlightClass>();
    public FlightPrice PricePerPerson { get; set; }

    public Flight() { }

    public Flight(string flightNumber, Airlines airlines, string departureAirport, string arrivalAirport,
        DateTime departureDateTime, DateTime arrivalDateTime, FlightPrice price)
    {
        FlightNumber = flightNumber;
        DepartureAirport = departureAirport;
        ArrivalAirport = arrivalAirport;
        DepartureDateTime = departureDateTime;
        ArrivalDateTime = arrivalDateTime;
        PricePerPerson = price;
        BookedSeats = new List<FlightClass>();
    }

    public bool ReserveSeat(FlightClass flightClass)
    {
        if (BookedSeats.Contains(flightClass))
            return false;
        else
        {
            BookedSeats.Add(flightClass);
            return true;
        }
    }

    public int GetAvailableSeats(FlightClass flightClass)
    {
        switch (flightClass)
        {
            case FlightClass.Economy:
                return _economyClassSeatsNumber - BookedSeats.Count(s => s == FlightClass.Economy);
            case FlightClass.Premium:
                return _premiumClassSeatsNumber - BookedSeats.Count(s => s == FlightClass.Premium);
            case FlightClass.Business:
                return _businessClassSeatsNumber - BookedSeats.Count(s => s == FlightClass.Business);
            case FlightClass.First:
                return _firstClassSeatsNumber - BookedSeats.Count(s => s == FlightClass.First);
            default:
                return 0;
        }
    }

    public bool IsAvailable(FlightClass flightClass) => GetAvailableSeats(flightClass) > 0;

    public override string ToString() => $"Flight number: {FlightNumber}, Airlines: {Airlines}, " +
        $"\nDeparture details: {DepartureAirport}, {DepartureDateTime}, \nArrival details: {ArrivalAirport}, {ArrivalDateTime}";
}
