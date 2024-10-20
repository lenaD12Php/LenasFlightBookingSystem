namespace AirportTicketBookingSystem;

public interface IBookingService
{
    Task CreateBookingAsync(List<Passenger> passengers, Airlines airline, Flight flight, FlightClass flightClass, PaymentType paymentType,
        DateTime bookingDate, DateTime flightDate, int numberOfAdults, int numberOfChildren, int numberOfBabies, Currency targetCurrency);
}