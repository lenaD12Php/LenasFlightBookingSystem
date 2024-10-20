namespace AirportTicketBookingSystem;

public interface IPriceService
{
    decimal CalculateTotalPrice(Airlines airline, FlightClass flightClass, int numberOfAdults, int numberOfChildren,
        int numberOfBabies, Currency targetCurrency, DateTime bookingDate, DateTime flightDate);
    
    decimal AdditionalCharges(decimal basePrice, DateTime bookingDate, DateTime flightDate);

    void UpdateBasePrice(Airlines airline, FlightClass flightClass, decimal? priceAdult = null, decimal? priceChild = null,
        decimal? priceBaby = null, Currency? currency = null);

}

