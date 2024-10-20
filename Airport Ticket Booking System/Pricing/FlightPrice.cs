namespace AirportTicketBookingSystem;

public class FlightPrice
{
    private readonly Dictionary<(Airlines Airline, FlightClass Class), (decimal PriceAdult, decimal PriceChild, decimal PriceBaby)>
        _priceData = new Dictionary<(Airlines, FlightClass), (decimal, decimal, decimal)>();

    public Currency Currency { get; private set; } = Currency.EUR;

    public FlightPrice()
    {
        SetDefaultPrices();
    }
    private void SetDefaultPrices()
    {
        foreach (Airlines airline in Enum.GetValues(typeof(Airlines)))
        {
            foreach (FlightClass flightClass in Enum.GetValues(typeof(FlightClass)))
            {
                _priceData[(airline, flightClass)] = flightClass switch
                {
                    FlightClass.Economy => (600, 500, 150),
                    FlightClass.Premium => (800, 700, 200),
                    FlightClass.Business => (1000, 900, 300),
                    FlightClass.First => (1500, 1300, 400),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    }

    public void UpdatePrices(Airlines airline, FlightClass flightClass, decimal? priceAdult = null, decimal? priceChild = null, decimal? priceBaby = null, Currency? currency = null)
    {
        if (_priceData.ContainsKey((airline, flightClass)))
        {
            var currentPrices = _priceData[(airline, flightClass)];
            _priceData[(airline, flightClass)] = (
                priceAdult ?? currentPrices.PriceAdult,
                priceChild ?? currentPrices.PriceChild,
                priceBaby ?? currentPrices.PriceBaby);

            if (currency.HasValue)
                Currency = currency.Value;
        }
        else
            throw new ArgumentException("The specified airline and flight class combination does not exist.");

    }
    public decimal GetPrice(Airlines airline, FlightClass flightClass, PassengerType passengerType)
    {
        if (_priceData.TryGetValue((airline, flightClass), out var prices))
        {
            return passengerType switch
            {
                PassengerType.Adult => prices.PriceAdult,
                PassengerType.Child => prices.PriceChild,
                PassengerType.Baby => prices.PriceBaby,
                _ => throw new ArgumentException("Invalid passenger type.")
            };
        }

        throw new ArgumentException("The specified airline and flight class combination does not exist.");
    }

    public decimal GetTotalPrice(Airlines airline, FlightClass flightClass, int numberOfAdults, int numberOfChildren, int numberOfBabies, Currency targetCurrency)
    {
        if (_priceData.TryGetValue((airline, flightClass), out var prices))
        {
            decimal totalPrice = (prices.PriceAdult * numberOfAdults) +
                                (prices.PriceChild * numberOfChildren) +
                                (prices.PriceBaby * numberOfBabies);

            return ((decimal)totalPrice).ConvertToCurrency(Currency, targetCurrency);
        }

        throw new ArgumentException("The specified airline and flight class combination does not exist.");
    }

    public string GetFormattedTotalPrice(Airlines airline, FlightClass flightClass, int numberOfAdults, int numberOfChildren, int numberOfBabies, Currency targetCurrency)
    {
        decimal totalPrice = GetTotalPrice(airline, flightClass, numberOfAdults, numberOfChildren, numberOfBabies, targetCurrency);

        return targetCurrency switch
        {
            Currency.EUR => $"{totalPrice} €",
            Currency.USD => $"{totalPrice} $",
            Currency.ILS => $"{totalPrice} ₪",
            Currency.JOD => $"{totalPrice} JD",
            _ => "Currency is not supported"
        };
    }
}
