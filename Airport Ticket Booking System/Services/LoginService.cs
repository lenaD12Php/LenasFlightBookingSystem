namespace Airport_Ticket_Booking_System;

public static class LoginService
{
    public static string GetUserRole()
    {
        Console.WriteLine("              Welcome to your best booking system          ");
        Console.WriteLine("***********************************************************");

        Console.WriteLine();

        Console.WriteLine("Please select your role:");
        Console.WriteLine("1. Passenger");
        Console.WriteLine("2. Manager");


        var userChoice = Console.ReadLine();
        switch (userChoice)
        {
            case "1":
                return "Passenger";
            case "2":
                return "Manager";
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                return GetUserRole();
        }
    }
}

