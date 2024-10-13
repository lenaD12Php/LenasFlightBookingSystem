namespace Airport_Ticket_Booking_System;

public partial class ManagerService
{
    private readonly FlightService _flightService;
    private readonly BookingService _bookingService;
    private readonly BookingRepository _bookingRepository;
    private readonly FlightRepository _flightRepository;


    public ManagerService(FlightService flightService, BookingService bookingService, BookingRepository bookingRepository, FlightRepository flightRepository)
    {
        _flightService = flightService;
        _bookingService = bookingService;
        _bookingRepository = bookingRepository;
        _flightRepository = flightRepository;
    }
}

