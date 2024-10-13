using Airport_Ticket_Booking_System;

public class FlightRepository
{
    private const string CSVFilePath = "flights.csv";
    private readonly FlightService _flightService;
    private List<Flight> _flights = new List<Flight>();
    private readonly object _lock = new object();

    public FlightRepository()
    {
        LoadFlightsFromCSVFile();
    }

    public void AddFlight(Flight flight)
    {
        lock (_lock)
        {
            _flights.Add(flight);
            SaveFlightsToCSVFile();
        }
    }

    public IEnumerable<Flight> GetAllFlights()
    {
        lock (_lock)
        {
            return _flights.ToList();
        }
    }

    public Flight? GetFlightByNumber(string flightNumber)
    {
        lock (_lock)
        {
            return _flights.FirstOrDefault(f => f.FlightNumber.Equals(flightNumber, StringComparison.OrdinalIgnoreCase));
        }
    }

    public void UpdateFlight(Flight updatedFlight)
    {
        lock (_lock)
        {
            var flight = GetFlightByNumber(updatedFlight.FlightNumber);
            if (flight != null)
            {
                _flights.Remove(flight);
                _flights.Add(updatedFlight);
                SaveFlightsToCSVFile();
            }
        }
    }

    public void RemoveFlight(string flightNumber)
    {
        lock (_lock)
        {
            var flight = GetFlightByNumber(flightNumber);
            if (flight != null)
            {
                _flights.Remove(flight);
                SaveFlightsToCSVFile();
            }
        }
    }

    private void LoadFlightsFromCSVFile()
    {
        lock (_lock)
        {
            if (File.Exists(CSVFilePath))
            {
                var lines = File.ReadAllLines(CSVFilePath);
                foreach (var line in lines.Skip(1))
                {
                    var parts = line.Split(',');
                    if (parts.Length != 18)
                    {
                        Console.WriteLine($"Invalid format in line: {line}");
                        continue;
                    }

                    try
                    {
                        string flightNumber = parts[0].Trim();
                        if (!Enum.TryParse<Airlines>(parts[1].Trim(), true, out Airlines airline))
                        {
                            Console.WriteLine($"Invalid airline in line: {line}");
                            continue;
                        }
                        string departureAirport = parts[2].Trim();
                        string arrivalAirport = parts[3].Trim();
                        DateTime departureDateTime = DateTime.Parse(parts[4].Trim());
                        DateTime arrivalDateTime = DateTime.Parse(parts[5].Trim());
                        decimal economyPriceAdult = decimal.Parse(parts[6].Trim());
                        decimal economyPriceChild = decimal.Parse(parts[7].Trim());
                        decimal economyPriceBaby = decimal.Parse(parts[8].Trim());
                        decimal premiumPriceAdult = decimal.Parse(parts[9].Trim());
                        decimal premiumPriceChild = decimal.Parse(parts[10].Trim());
                        decimal premiumPriceBaby = decimal.Parse(parts[11].Trim());
                        decimal businessPriceAdult = decimal.Parse(parts[12].Trim());
                        decimal businessPriceChild = decimal.Parse(parts[13].Trim());
                        decimal businessPriceBaby = decimal.Parse(parts[14].Trim());
                        decimal firstClassPriceAdult = decimal.Parse(parts[15].Trim());
                        decimal firstClassPriceChild = decimal.Parse(parts[16].Trim());
                        decimal firstClassPriceBaby = decimal.Parse(parts[17].Trim());

                        var flightPrice = new FlightPrice();
                        flightPrice.UpdatePrices(airline, FlightClass.Economy, economyPriceAdult, economyPriceChild, economyPriceBaby);
                        flightPrice.UpdatePrices(airline, FlightClass.Premium, premiumPriceAdult, premiumPriceChild, premiumPriceBaby);
                        flightPrice.UpdatePrices(airline, FlightClass.Business, businessPriceAdult, businessPriceChild, businessPriceBaby);
                        flightPrice.UpdatePrices(airline, FlightClass.First, firstClassPriceAdult, firstClassPriceChild, firstClassPriceBaby);

                        var flight = new Flight(flightNumber, airline, departureAirport, arrivalAirport, departureDateTime, arrivalDateTime, flightPrice);
                        _flights.Add(flight);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading flight from line: {line}, Error: {ex.Message}");
                    }
                }
            }
        }
    }

    private void SaveFlightsToCSVFile()
    {
        lock (_lock)
        {
            var lines = new List<string>
            {
                "FlightNumber, Airlines, DepartureAirport, ArrivalAirport, DepartureDateTime, ArrivalDateTime, EconomyPrice, PremiumPrice, BusinessPrice, FirstClassPrice"
            };
            lines.AddRange(_flights.Select(flight =>
                $"{flight.FlightNumber}, {flight.Airlines}, {flight.DepartureAirport}, {flight.ArrivalAirport}," +
                $"{flight.DepartureDateTime:yyyy-MM-ddTHH:mm}, {flight.ArrivalDateTime:yyyy-MM-ddTHH:mm}," +
                $"{flight.PricePerPerson.GetPrice(flight.Airlines, FlightClass.Economy, PassengerType.Adult)}," +
                $"{flight.PricePerPerson.GetPrice(flight.Airlines, FlightClass.Economy, PassengerType.Child)}," +
                $"{flight.PricePerPerson.GetPrice(flight.Airlines, FlightClass.Economy, PassengerType.Baby)}," +
                $"{flight.PricePerPerson.GetPrice(flight.Airlines, FlightClass.Premium, PassengerType.Adult)}," +
                $"{flight.PricePerPerson.GetPrice(flight.Airlines, FlightClass.Premium, PassengerType.Child)}," +
                $"{flight.PricePerPerson.GetPrice(flight.Airlines, FlightClass.Premium, PassengerType.Baby)}," +
                $"{flight.PricePerPerson.GetPrice(flight.Airlines, FlightClass.Business, PassengerType.Adult)}," +
                $"{flight.PricePerPerson.GetPrice(flight.Airlines, FlightClass.Business, PassengerType.Child)}," +
                $"{flight.PricePerPerson.GetPrice(flight.Airlines, FlightClass.Business, PassengerType.Baby)}," +
                $"{flight.PricePerPerson.GetPrice(flight.Airlines, FlightClass.First, PassengerType.Adult)}," +
                $"{flight.PricePerPerson.GetPrice(flight.Airlines, FlightClass.First, PassengerType.Child)}," +
                $"{flight.PricePerPerson.GetPrice(flight.Airlines, FlightClass.First, PassengerType.Baby)}"));
            File.WriteAllLines(CSVFilePath, lines);
        }
    }
}
