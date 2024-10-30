using AutoFixture;

namespace AirportTicketBookingSystem.Tests.BookingSystemModelsTests;

public class FlightShould
{
    [Fact]
    public void ReserveSeat_WhenSeatIsNotBooked_ShouldReturnTrue()
    {
        // Arrange
        var fixture = new Fixture();

        var flight = new Flight("LH30", Airlines.TurkishAirlines, "FRA", "IST",fixture.Create<DateTime>() , fixture.Create<DateTime>(), new FlightPrice());
        var flightClass = fixture.Create<FlightClass>();
        Flight.BusinessBookedSeats = 15;
        Flight.FirstBookedSeats = 9 ;
        Flight.PremiumBookedSeats = 20;
        Flight.EconomyBookedSeats = 100;

        // Act
        var result = flight.ReserveSeat(flightClass);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ReserveSeat_WhenSeatIsBooked_ShouldReturnFalse()
    {
        // Arrange
        var fixture = new Fixture();

        var flight = new Flight("LH30", Airlines.TurkishAirlines, "FRA", "IST", fixture.Create<DateTime>(), fixture.Create<DateTime>(), new FlightPrice());
        var flightClass = fixture.Create<FlightClass>();
        Flight.BusinessBookedSeats = 16;
        Flight.FirstBookedSeats = 10;
        Flight.PremiumBookedSeats = 30;
        Flight.EconomyBookedSeats = 120;

        // Act
        var result = flight.ReserveSeat(flightClass);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData (50)]
    [InlineData(100)]
    [InlineData(110)]
    public void GetAvailableSeats_WhenNotFull(int bookedSeats)
    {
        // Arrange
        var flight = new Flight();
        var flightClass = FlightClass.Economy;
        Flight.EconomyBookedSeats = bookedSeats;
        var totalEconomySeats = Flight.EconomyClassSeatsNumber;

        // Act
        var availableSeats = flight.GetAvailableSeats(flightClass);

        // Assert
        var expectedAvailableSeats = totalEconomySeats - bookedSeats;

        Assert.Equal(expectedAvailableSeats, availableSeats);
    }

    [Theory]
    [InlineData(120)]
    public void IsAvailable_WhenFullShouldReturnFalse(int bookedSeats)
    {
        // Arrange
        var flight = new Flight();
        var flightClass = FlightClass.Economy;
        var BusinessFlightClass = FlightClass.Business;
        Flight.EconomyBookedSeats = bookedSeats;

        // Act
        var isAvailable = flight.IsAvailable(flightClass);
        var isAvailableBusiness = flight.IsAvailable(BusinessFlightClass);

        // Assert
        Assert.False(isAvailable);
        Assert.True(isAvailableBusiness);

    }

    [Theory]
    [InlineData(12)]
    public void IsAvailable_WhenNotFullShouldReturnTrue(int bookedSeats)
    {
        // Arrange
        var flight = new Flight();
        var EconomyFlightClass = FlightClass.Economy;
        Flight.EconomyBookedSeats = bookedSeats;


        // Act
        var isAvailableEconomy = flight.IsAvailable(EconomyFlightClass);

        // Assert
        Assert.True(isAvailableEconomy);

    }


}
