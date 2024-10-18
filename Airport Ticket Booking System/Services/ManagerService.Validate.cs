namespace Airport_Ticket_Booking_System;

public partial class ManagerService
{
    public async Task ValidateImportedFlightDataAsync()
    {
        Console.WriteLine("Validate Imported Flight Data");
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
            var validationErrors = new List<string>();

            foreach (var line in lines)
            {
                var parts = line.Split(',');

                if (parts.Length != 6)
                {
                    validationErrors.Add($"Invalid format in line: {line}");
                    continue;
                }

                string flightNumber = parts[0].Trim();
                string departureAirport = parts[1].Trim();
                string arrivalAirport = parts[2].Trim();
                DateTime departureDateTime;
                DateTime arrivalDateTime;
                decimal price;

                if (!DateTime.TryParse(parts[3].Trim(), out departureDateTime))
                {
                    validationErrors.Add($"Invalid Departure Date and Time format in line: {line}");
                    continue;
                }

                if (!DateTime.TryParse(parts[4].Trim(), out arrivalDateTime))
                {
                    validationErrors.Add($"Invalid Arrival Date and Time format in line: {line}");
                    continue;
                }

                if (!decimal.TryParse(parts[5].Trim(), out price))
                {
                    validationErrors.Add($"Invalid Price format in line: {line}");
                    continue;
                }
            }

            if (validationErrors.Any())
            {
                Console.WriteLine("Validation errors encountered:");
                foreach (var error in validationErrors)
                    Console.WriteLine(error);
            }
            else
                Console.WriteLine("Flight data validated successfully.");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error validating flight data: {ex.Message}");
        }
    }
}

