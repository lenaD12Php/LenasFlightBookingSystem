namespace AirportTicketBookingSystem;

public class PriceService : IPriceService
{
    private FlightPrice _flightPrice;

    private const int _lateBooking = 30;
    private const int _normalBooking = 60;
    private const int _earlyBooking = 120;

    private const decimal _feesForLateBooking = 1.2m;
    private const decimal _feesForNormalBooking = 1.1m;
    private const decimal _feesForWeekendBookings = 1.1m;
    private const decimal _feesForHolidaysBooking = 1.3m;
    private const decimal _discountForEarlyBooking = 0.8m;

    public PriceService()
    {
    }

    public decimal CalculateTotalPrice(Airlines airline, FlightClass flightClass, int numberOfAdults, int numberOfChildren,
        int numberOfBabies, Currency targetCurrency, DateTime bookingDate, DateTime flightDate)
    {

        decimal baseTotalPrice = _flightPrice.GetTotalPrice(airline, flightClass, numberOfAdults, numberOfChildren, numberOfBabies, Currency.EUR);

        decimal updatedPrice = AdditionalCharges(baseTotalPrice, bookingDate, flightDate);

        return updatedPrice.ConvertToCurrency(Currency.EUR, targetCurrency);
    }

    public decimal AdditionalCharges(decimal basePrice, DateTime bookingDate, DateTime flightDate)
    {
        int daysUntilFlight = (flightDate - bookingDate).Days;

        if (daysUntilFlight <= _lateBooking)
            basePrice *= _feesForLateBooking;

        else if (daysUntilFlight <= _normalBooking)
            basePrice *= _feesForNormalBooking;

        else if (daysUntilFlight >= _earlyBooking)
            basePrice *= _discountForEarlyBooking;

        if (IsHoliday(flightDate))
            basePrice *= _feesForHolidaysBooking;

        if (IsWeekend(flightDate.DayOfWeek))
            basePrice *= _feesForWeekendBookings;

        return basePrice;
    }

    private bool IsHoliday(DateTime date)
    {
        // This is based on Lower Saxony holidays
        List<(int Month, int StartDay, int EndDay)> holidays = new List<(int, int, int)>
        {
            (12, 22, 31),  // Christmas and New Year's period
            (1, 1, 2),     // New Year holidays
            (4, 7, 17),    // Easter holidays
            (7, 3, 31),    // Summer holidays
            (8, 1, 13),
            (10, 13, 24)   // Fall holidays
        };

        return holidays.Any(h => date.Month == h.Month && date.Day >= h.StartDay && date.Day <= h.EndDay);
    }

    private bool IsWeekend(DayOfWeek day) => (day == DayOfWeek.Friday) || (day == DayOfWeek.Saturday) || (day == DayOfWeek.Sunday);

    public void UpdateBasePrice(Airlines airline, FlightClass flightClass, decimal? priceAdult = null, decimal? priceChild = null,
        decimal? priceBaby = null, Currency? currency = null)
    {
        _flightPrice.UpdatePrices(airline, flightClass, priceAdult, priceChild, priceBaby, currency);
        Console.WriteLine("Base prices have been updated.");
    }
}



