namespace Airport_Ticket_Booking_System;

public class PassengerService
{
    private readonly BookingService _bookingService;
    private readonly BookingRepository _bookingRepository;
    private readonly FlightService _flightService;
    private readonly FlightRepository _flightRepository;
    private readonly PassengerService _passengerService;
    private readonly ManagerService _managerService;



    public PassengerService(BookingService bookingService, BookingRepository bookingRepository, FlightService flightService, FlightRepository flightRepository)
    {
        _bookingService = bookingService;
        _bookingRepository = bookingRepository;
        _flightService = flightService;
        _flightRepository = flightRepository;
    }

    public async Task BookFlightAsync()
    {
        Console.WriteLine("Select airlines: (1) TurkishAirlines, (2) BritishAirways, (3) Lufthansa, (4) AustrianAirlines");
        if (!int.TryParse(Console.ReadLine(), out int airlineChoice) || airlineChoice < 1 || airlineChoice > 4)
        {
            Console.WriteLine("Invalid airline. Please try again.");
            return;
        }
        Airlines airlines = airlineChoice switch
        {
            1 => Airlines.TurkishAirlines,
            2 => Airlines.BritishAirways,
            3 => Airlines.Lufthansa,
            4 => Airlines.AustrianAirlines,
            _ => throw new ArgumentException("Invalid choice.")
        };

        Console.WriteLine("Enter the departure airport:");
        var departureAirport = Console.ReadLine().Trim();

        Console.WriteLine("Enter the arrival airport:");
        var arrivalAirport = Console.ReadLine().Trim();

        Console.WriteLine("Enter the departure date (yyyy-MM-dd):");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime departureDate))
        {
            Console.WriteLine("Invalid date format. Please use yyyy-MM-dd.");
            HomePage.ShowNavigation(_bookingService, _flightService, _passengerService, _managerService);
            return;
        }

        var availableFlights = _flightRepository.GetAllFlights()
            .Where(f => f.Airlines == airlines &&
                        f.DepartureAirport.Equals(departureAirport, StringComparison.OrdinalIgnoreCase) &&
                        f.ArrivalAirport.Equals(arrivalAirport, StringComparison.OrdinalIgnoreCase) &&
                        f.DepartureDateTime.Date == departureDate.Date)
            .ToList();

        if (availableFlights.Count == 0)
        {
            Console.WriteLine("No matching flights found for the provided criteria.");
            HomePage.ShowNavigation(_bookingService, _flightService, _passengerService, _managerService);
            return;
        }

        Console.WriteLine("Available flights:");
        for (int i = 0; i < availableFlights.Count; i++)
        {
            var flight = availableFlights[i];
            Console.WriteLine($"{i + 1}. Flight Number: {flight.FlightNumber}, Airlines: {flight.Airlines},Departure: {flight.DepartureDateTime}, Arrival: {flight.ArrivalDateTime}");
        }

        Console.WriteLine("Select a flight by entering it's number:");
        if (!int.TryParse(Console.ReadLine(), out int flightChoice) || flightChoice < 1 || flightChoice > availableFlights.Count)
        {
            Console.WriteLine("Invalid selection.");
            HomePage.ShowNavigation(_bookingService, _flightService, _passengerService, _managerService);
            return;
        }

        var selectedFlight = availableFlights[flightChoice - 1];

        Console.WriteLine("Enter the number of adults:");
        var numberOfAdults = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter the number of children:");
        var numberOfChildren = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter the number of babies:");
        var numberOfBabies = int.Parse(Console.ReadLine());

        var passengers = new List<Passenger>();
        for (int i = 0; i < numberOfAdults; i++)
        {
            Console.WriteLine($"Enter details for Adults (First Name, Last Name, Email and Phone) {i + 1}:");
            passengers.Add(CreatePassenger(PassengerType.Adult));
        }

        for (int i = 0; i < numberOfChildren; i++)
        {
            Console.WriteLine($"Enter details for Children (First Name, Last Name,  Parent Email and Parent Phone) {i + 1}:");
            passengers.Add(CreatePassenger(PassengerType.Child));
        }

        for (int i = 0; i < numberOfBabies; i++)
        {
            Console.WriteLine($"Enter details for Babies (First Name, Last Name,  Parent Email and Parent Phone) {i + 1}:");
            passengers.Add(CreatePassenger(PassengerType.Baby));
        }

        Console.WriteLine("Select flight class: (1) Economy, (2) Premium, (3) Business, (4) First");
        if (!int.TryParse(Console.ReadLine(), out int classChoice) || classChoice < 1 || classChoice > 4)
        {
            Console.WriteLine("Invalid choice.");
            HomePage.ShowNavigation(_bookingService, _flightService, _passengerService, _managerService);
            return;
        }

        FlightClass flightClass = classChoice switch
        {
            1 => FlightClass.Economy,
            2 => FlightClass.Premium,
            3 => FlightClass.Business,
            4 => FlightClass.First,
            _ => throw new ArgumentException("Invalid choice.")
        };

        Console.WriteLine("Enter payment type: (1) PayPal, (2) Klarna, (3) DebitCard, (4) CreditCard, (5) AmericanExpress");
        if (!int.TryParse(Console.ReadLine(), out int paymentChoice) || paymentChoice < 1 || paymentChoice > 5)
        {
            Console.WriteLine("Invalid choice.");
            HomePage.ShowNavigation(_bookingService, _flightService, _passengerService, _managerService);
            return;
        }

        PaymentType paymentType = paymentChoice switch
        {
            1 => PaymentType.Paypal,
            2 => PaymentType.Klarna,
            3 => PaymentType.DebitCard,
            4 => PaymentType.CreditCard,
            5 => PaymentType.AmericanExpress,
            _ => throw new ArgumentException("Invalid choice.")
        };

        decimal totalPrice = selectedFlight.PricePerPerson.GetTotalPrice(
            selectedFlight.Airlines, flightClass, numberOfAdults, numberOfChildren, numberOfBabies, Currency.EUR);

        var booking = new Booking(bookingID: Guid.NewGuid().ToString(), Passengers: passengers, airline: airlines,
            flight: selectedFlight, flightClass: flightClass, paymentType: paymentType, bookingDate: DateTime.Now,
            totalPrice: totalPrice);
        await _bookingRepository.AddBookingAsync(booking);


        Console.WriteLine($"Booking confirmed. Total price: {totalPrice} EUR");
    }

    private Passenger CreatePassenger(PassengerType type)
    {
        Console.WriteLine("First Name:");
        string firstName = Console.ReadLine();

        Console.WriteLine("Last Name:");
        string lastName = Console.ReadLine();

        Console.WriteLine("Email:");
        string email = Console.ReadLine();

        Console.WriteLine("Phone:");
        string phone = Console.ReadLine();

        return new Passenger(Guid.NewGuid().ToString(), firstName, lastName, email, phone, type);
    }

    public async Task CancelABookingAsync()
    {
        Console.WriteLine("Enter booking ID to cancel:");
        string bookingID = Console.ReadLine();
        var bookings = await _bookingRepository.GetAllBookingsAsync();
        var booking = bookings.FirstOrDefault(b => b.bookingID == bookingID);
        if (booking != null)
            await _bookingRepository.CancelBookingAsync(bookingID);
        else
        {
            Console.WriteLine("Booking not found.");
            HomePage.ShowNavigation(_bookingService, _flightService, _passengerService, _managerService);
        }
    }

    public async Task ViewPersonalBookingsAsync()
    {
        Console.WriteLine("Enter booking ID to view it's personal bookings:");
        string bookingID = Console.ReadLine();

        var booking = await _bookingRepository.GetBookingByIDAsync(bookingID);

        if (booking == null)
        {
            Console.WriteLine("No booking found for the provided ID.");
            HomePage.ShowNavigation(_bookingService, _flightService, _passengerService, _managerService);
            return;
        }

        Console.WriteLine("Booking Details:");
        Console.WriteLine($"Booking ID: {booking.bookingID}");
        foreach (var passenger in booking.Passengers)
            Console.WriteLine($"Passenger: {passenger.FirstName} {passenger.LastName}");
        Console.WriteLine($"Flight: {booking.flight.FlightNumber}");
        Console.WriteLine($"Departure Airport: {booking.flight.DepartureAirport}");
        Console.WriteLine($"Arrival Airport: {booking.flight.ArrivalAirport}");
        Console.WriteLine($"Departure Date: {booking.flight.DepartureDateTime}");
        Console.WriteLine($"Flight Class: {booking.flightClass}");
        Console.WriteLine($"Total Price: {booking.totalPrice}");
    }

    public async Task SearchFlightsAsync()
    {
        Console.WriteLine("Enter departure airport:");
        string departureAirport = Console.ReadLine();

        Console.WriteLine("Enter arrival airport:");
        string arrivalAirport = Console.ReadLine();

        var availableFlights = _flightRepository.GetAllFlights();
        var filteredFlights = availableFlights
            .Where(f => f.DepartureAirport.Equals(departureAirport, StringComparison.OrdinalIgnoreCase) &&
                        f.ArrivalAirport.Equals(arrivalAirport, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (!filteredFlights.Any())
        {
            Console.WriteLine("No available flights found.");
            HomePage.ShowNavigation(_bookingService, _flightService, _passengerService, _managerService);
        }
        else
            foreach (var f in filteredFlights)
                Console.WriteLine($"Flight: {f.FlightNumber}, Airline: {f.Airlines}, Departure: {f.DepartureDateTime}, Arrival: {f.ArrivalDateTime}");
    }
}

