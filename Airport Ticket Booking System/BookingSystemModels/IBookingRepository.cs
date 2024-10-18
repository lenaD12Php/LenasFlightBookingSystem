namespace Airport_Ticket_Booking_System;

public interface IBookingRepository
{
    Task AddBookingAsync(Booking booking);
    Task CancelBookingAsync(string bookingID);
    Task<IEnumerable<Booking>> GetAllBookingsAsync();
    Task<Booking?> GetBookingByIDAsync(string bookingID);
}

