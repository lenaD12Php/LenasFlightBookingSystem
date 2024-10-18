namespace Airport_Ticket_Booking_System;

public  class BookingRepository : IBookingRepository
{
    private readonly List<Booking> _bookings = new List<Booking>();
    private const string CSVFilePath = "bookings.csv";
    private readonly object _lock = new object();

    public BookingRepository()
    {
        LoadBookingsFromCSVFileAsync().Wait();
    }

    public async Task AddBookingAsync(Booking booking)
    {
        lock (_lock)
            _bookings.Add(booking);

        await SaveBookingsToCSVFileAsync();
        Console.WriteLine("Booking added successfully.");
    }

    public async Task CancelBookingAsync(string bookingID)
    {
        lock (_lock)
        {
            var booking = _bookings.FirstOrDefault(b => b.bookingID == bookingID);
            if (booking != null)
            {
                _bookings.Remove(booking);
            }
            else
            {
                Console.WriteLine($"Booking with ID: {bookingID} not found.");
                return;
            }
        }

        await SaveBookingsToCSVFileAsync();
        Console.WriteLine($"Booking with ID: {bookingID} has been canceled.");
    }

    private async Task LoadBookingsFromCSVFileAsync()
    {
        if (File.Exists(CSVFilePath))
        {
            try
            {
                var lines = await File.ReadAllLinesAsync(CSVFilePath).ConfigureAwait(false);

                foreach (var line in lines)
                {
                    var parts = line.Split(',');

                    if (parts.Length != 18)
                    {
                        Console.WriteLine($"Invalid format in line: {line}");
                        continue;
                    }

                    try
                    {
                        var validation = new InputValidation();
                        var bookingID = parts[0];
                        var passengers = new List<Passenger> { new Passenger( ID: parts[1].Trim(), FirstName: parts[2].Trim(),
                            LastName: parts[3].Trim(), Email: parts[4].Trim(), Phone: parts[5].Trim(),
                            PassengerType: Enum.Parse<PassengerType>(parts[6].Trim(), true))};

                        string flightNumber = parts[7].Trim();

                        if (!Enum.TryParse<Airlines>(parts[8].Trim(), true, out Airlines airline))
                        {
                            Console.WriteLine($"Invalid airline in line: {line}");
                            continue;
                        }

                        string departureAirport = parts[9].Trim();
                        string arrivalAirport = parts[10].Trim();
                        DateTime departureDateTime = DateTime.Parse(parts[11].Trim());
                        DateTime arrivalDateTime = DateTime.Parse(parts[12].Trim());
                        decimal price = decimal.Parse(parts[13].Trim());

                        var flightPrice = new FlightPrice();

                        var flightClass = Enum.Parse<FlightClass>(parts[14].Trim(), true);

                        flightPrice.UpdatePrices(airline, flightClass, price);

                        var flight = new Flight(flightNumber, airline, departureAirport, arrivalAirport, departureDateTime, arrivalDateTime, flightPrice);

                        var paymentType = Enum.Parse<PaymentType>(parts[15].Trim(), true);
                        var totalPrice = decimal.Parse(parts[16].Trim());
                        var bookingDate = DateTime.Parse(parts[17].Trim());

                        var booking = new Booking(bookingID: Guid.NewGuid().ToString(), Passengers: passengers,
                            airline: airline, flight: flight, flightClass: flightClass, paymentType: paymentType,
                            totalPrice: totalPrice, bookingDate: bookingDate);

                        _bookings.Add(booking);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing line: {line}. Exception: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading bookings: {ex.Message}");
            }
        }
    }

    private async Task SaveBookingsToCSVFileAsync()
    {
        IEnumerable<string> lines;

        lock (_lock)
        {
            lines = _bookings.Select(b =>
            {
                var passengerDetails = string.Join(";", b.Passengers.Select(p =>
                    $"{p.ID},{p.FirstName},{p.LastName},{p.Email},{p.Phone},{p.PassengerType}"));

                return $"{b.bookingID},{passengerDetails},{b.airline},{b.flight.FlightNumber},{b.flight.DepartureAirport}," +
                       $"{b.flight.ArrivalAirport},{b.flight.DepartureDateTime},{b.flight.ArrivalDateTime}," +
                       $"{b.flightClass},{b.paymentType},{b.totalPrice},{b.bookingDate}";
            }).ToList();
        }

        await File.WriteAllLinesAsync(CSVFilePath, lines);
    }

    public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
    {
        List<Booking> bookingsCopy;
        lock (_lock)
        {
            bookingsCopy = _bookings.ToList();
        }
        return await Task.FromResult(bookingsCopy.AsEnumerable());
    }

    public  Task<Booking?> GetBookingByIDAsync(string bookingID)
    {
        lock (_lock)
            return  Task.FromResult(_bookings.FirstOrDefault(b => b.bookingID == bookingID));

    }
}

