namespace AirportTicketBookingSystem;

public partial class ManagerService
{
    public async Task BatchFlightUploadAsync()
    {
        Console.WriteLine("Batch Flight Upload");
        Console.WriteLine("---------------------------------------------------");

        Console.WriteLine("Enter the path to the CSV file:");
        string CSVFilePath = Console.ReadLine();

        if (!File.Exists(CSVFilePath))
        {
            Console.WriteLine("Invalid file path.");
            return;
        }

        try
        {
            var lines = File.ReadAllLines(CSVFilePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');

                if (parts.Length != 6)
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

                    decimal price = decimal.Parse(parts[6].Trim());

                    var flightPrice = new FlightPrice();

                    Console.WriteLine("Select flight class: (1) Economy, (2) Premium, (3) Business, (4) First");
                    var classChoice = int.Parse(Console.ReadLine());
                    FlightClass flightClass = classChoice switch
                    {
                        1 => FlightClass.Economy,
                        2 => FlightClass.Premium,
                        3 => FlightClass.Business,
                        4 => FlightClass.First,
                        _ => throw new ArgumentException("Invalid choice.")
                    };

                    flightPrice.UpdatePrices(airline, flightClass, price);

                    var flight = new Flight(flightNumber, airline, departureAirport, arrivalAirport, departureDateTime, arrivalDateTime, flightPrice);
                    _flightService.AddFlight(flight);

                    Console.WriteLine("Flights imported successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error importing flight from line: {line}, Error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }
    }
}

