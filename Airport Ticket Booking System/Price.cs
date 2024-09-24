namespace Airport_Ticket_Booking_System.General;

public class Price
{
    public double TicketPrice { get; set; }
    public Currency Currency { get; set; }

    public override string ToString()
    {
        return $"{TicketPrice} {Currency}";
    }
    public Price() { }
    public Price(double price, Currency currency)
    {
        TicketPrice = price;
        Currency = currency;
    }
}


