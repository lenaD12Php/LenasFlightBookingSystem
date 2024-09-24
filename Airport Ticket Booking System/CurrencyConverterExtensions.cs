namespace Airport_Ticket_Booking_System;

public static class CurrencyConverterExtensions
{
    private static readonly Dictionary<(Currency From, Currency To), decimal> ExchangeRates = new Dictionary<(Currency, Currency), decimal>
    {
        { (Currency.EUR, Currency.USD), 1.0m },
        { (Currency.EUR, Currency.ILS), 3.9m },
        { (Currency.EUR, Currency.JOD), 0.76m },

        { (Currency.USD, Currency.EUR), 1 / 1.0m },
        { (Currency.USD, Currency.ILS), 3.9m / 1.0m },
        { (Currency.USD, Currency.JOD), 0.76m / 1.0m },

        { (Currency.ILS, Currency.EUR), 1 / 3.9m },
        { (Currency.ILS, Currency.USD), 1.0m / 3.9m },
        { (Currency.ILS, Currency.JOD), 0.76m / 3.9m },

        { (Currency.JOD, Currency.EUR), 1 / 0.76m },
        { (Currency.JOD, Currency.USD), 1.0m / 0.76m },
        { (Currency.JOD, Currency.ILS), 3.9m / 0.76m }
    };

    public static decimal ConvertToCurrency(this decimal amount, Currency fromCurrency, Currency toCurrency)
    {
        if (fromCurrency == toCurrency)
        {
            return amount;
        }

        var conversionRate = ExchangeRates[(fromCurrency, toCurrency)];

        return amount * conversionRate;
    }
}
