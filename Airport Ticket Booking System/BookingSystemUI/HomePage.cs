namespace AirportTicketBookingSystem;

internal static class HomePage
{
    internal static void ShowNavigation(IBookingService bookingService, FlightService flightService, PassengerService passengerService, ManagerService managerService)
    {
        var userRole = LoginService.GetUserRole();

        if (userRole == "Passenger")
            ShowPassengerMenu(passengerService);
        else if (userRole == "Manager")
            ShowManagerMenu(managerService);
    }

    internal static void ShowPassengerMenu(PassengerService passengerService)
    {
        Console.WriteLine("Quick Access:");
        Console.WriteLine("1. Book a Flight");
        Console.WriteLine("2. Manage Bookings");
        Console.WriteLine("3. Search for Available Flights");

        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                passengerService.BookFlightAsync();
                break;
            case "2":
                HomePage.ShowPassengerManageBookingsMenu(passengerService);
                break;
            case "3":
                passengerService.SearchFlightsAsync();
                break;
            default:
                Console.WriteLine("Invalid choice.");
                HomePage.ShowPassengerMenu(passengerService);
                break;
        }
    }

    internal static void ShowPassengerManageBookingsMenu(PassengerService passengerService)
    {
        Console.WriteLine("Manage Bookings:");
        Console.WriteLine("1. Cancel booking");
        Console.WriteLine("2. View personal bookings");

        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                passengerService.CancelABookingAsync();
                break;
            case "2":
                passengerService.ViewPersonalBookingsAsync();
                break;
            default:
                Console.WriteLine("Invalid choice.");
                HomePage.ShowPassengerMenu(passengerService);
                break;
        }
    }

    internal static void ShowManagerMenu(ManagerService managerService)
    {
        Console.WriteLine("Quick Access:");
        Console.WriteLine("1. Filter Bookings");
        Console.WriteLine("2. Batch Flight Upload");
        Console.WriteLine("3. Validate Imported Flight Data");
        Console.WriteLine("4. Dynamic Model Validation Details");

        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                managerService.FilterBookingsAsync();
                break;
            case "2":
                managerService.BatchFlightUploadAsync();
                break;
            case "3":
                managerService.ValidateImportedFlightDataAsync();
                break;
            default:
                Console.WriteLine("Invalid choice.");
                HomePage.ShowManagerMenu(managerService);
                break;
        }
    }
}






