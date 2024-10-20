namespace AirportTicketBookingSystem;

public record Booking(string bookingID, List<Passenger> Passengers, Airlines airline, Flight flight, FlightClass flightClass, PaymentType paymentType, decimal totalPrice, DateTime bookingDate)
{
}



