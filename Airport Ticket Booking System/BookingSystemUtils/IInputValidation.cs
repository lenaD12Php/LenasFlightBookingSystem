namespace AirportTicketBookingSystem;

public interface IInputValidation
{
    ValidationErrorType IsValidFlightNumber(string flightNumber);
    ValidationErrorType IsValidDepartureAndArrivalDates(DateTime departureDate, DateTime arrivalDate);
    ValidationErrorType IsValidEmail(string email);
    ValidationErrorType IsValidPhoneNumber(string phone);
    ValidationErrorType IsValidName(string name);
}