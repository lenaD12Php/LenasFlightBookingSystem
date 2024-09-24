namespace Airport_Ticket_Booking_System.BookingSystemUI;

internal class HomePage
{
    internal static void ShowNavigation()
    {
        Console.WriteLine("              Welcome to your best booking system          ");
        Console.WriteLine("***********************************************************");
       
        Console.WriteLine();
       
        Console.WriteLine("Use as:");
            Console.WriteLine("1. Passenger");
            Console.WriteLine("2. Manager");
    }
    internal static void PassengerFeature()
    {
        Console.WriteLine("Quick Access:");
            Console.WriteLine("1. Book a Flight");
            Console.WriteLine("2. Manage Bookings");
            Console.WriteLine("3. Search for Available Flights");
    }
    internal static void ManagerFeatures() 
    {
        Console.WriteLine("Quick Access:");
            Console.WriteLine("1. Filter Bookings");
            Console.WriteLine("2. Batch Flight Upload");
            Console.WriteLine("3. Validate Imported Flight Data");
            Console.WriteLine("4. Dynamic Model Validation Details");
    }

}
