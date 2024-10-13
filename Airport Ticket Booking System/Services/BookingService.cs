namespace Airport_Ticket_Booking_System;

public class BookingService
{
    private readonly BookingRepository _bookingRepository;
    private readonly PriceService _priceService;

    public BookingService(BookingRepository bookingRepository, PriceService priceService)
    {
        _bookingRepository = bookingRepository;
        _priceService = priceService;
    }

    public void CreateBooking(List<Passenger> passengers, Airlines airline, Flight flight, FlightClass flightClass, PaymentType paymentType,
        DateTime bookingDate, DateTime flightDate, int numberOfAdults, int numberOfChildren, int numberOfBabies, Currency targetCurrency)
    {
        if (numberOfAdults <= 0)
            throw new ArgumentException("Number of adults must be at least 1.");
        if (numberOfChildren < 0 || numberOfBabies < 0)
            throw new ArgumentException("Number of children and babies cannot be negative.");
        if (flightDate < bookingDate)
            throw new ArgumentException("Flight date cannot be earlier than booking date.");

        decimal totalPrice = _priceService.CalculateTotalPrice(airline, flightClass, numberOfAdults, numberOfChildren,
            numberOfBabies, targetCurrency, bookingDate, flightDate);

        string bookingID = Guid.NewGuid().ToString();
        var newBooking = new Booking(bookingID: bookingID, Passengers: passengers, airline,
            flight, flightClass, paymentType, totalPrice, bookingDate);

        _bookingRepository.AddBookingAsync(newBooking);

        Console.WriteLine($"Booking successfully created with ID: {bookingID}");
    }
}
