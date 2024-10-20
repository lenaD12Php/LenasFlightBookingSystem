namespace AirportTicketBookingSystem;

public partial class ManagerService
{
    private readonly FlightService _flightService;
    private readonly IBookingService _bookingService;
    private readonly IBookingRepository _bookingRepository;
    private readonly FlightRepository _flightRepository;


    public ManagerService(FlightService flightService, IBookingService bookingService, IBookingRepository bookingRepository, FlightRepository flightRepository)
    {
        _flightService = flightService;
        _bookingService = bookingService;
        _bookingRepository = bookingRepository;
        _flightRepository = flightRepository;
    }
}

