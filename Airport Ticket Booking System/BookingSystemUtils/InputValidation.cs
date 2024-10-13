namespace Airport_Ticket_Booking_System;

public class InputValidation : IInputValidation
{
    public ValidationErrorType IsValidFlightNumber(string flightNumber)
    {
        if (string.IsNullOrEmpty(flightNumber))
        {
            return ValidationErrorType.RequiredField;
        }

        if (flightNumber.Length < 7 || !char.IsLetter(flightNumber[0]) || !char.IsLetter(flightNumber[1]))
        {
            return ValidationErrorType.InvalidFormat;
        }

        string numericPart = flightNumber.Substring(2);
        if (numericPart.Length < 5 || numericPart.Length > 6 || !numericPart.All(char.IsDigit))
        {
            return ValidationErrorType.InvalidFormat;
        }

        return ValidationErrorType.None;
    }

    public ValidationErrorType IsValidDepartureAndArrivalDates(DateTime departureDate, DateTime arrivalDate)
    {
        if (departureDate <= DateTime.Now)
        {
            return ValidationErrorType.OutOfRange;
        }

        if (arrivalDate <= departureDate)
        {
            return ValidationErrorType.OutOfRange;
        }

        return ValidationErrorType.None;
    }

    public ValidationErrorType IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return ValidationErrorType.RequiredField;
        }

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email ? ValidationErrorType.None : ValidationErrorType.InvalidFormat;
        }
        catch
        {
            return ValidationErrorType.InvalidFormat;
        }
    }

    public ValidationErrorType IsValidPhoneNumber(string phone)
    {
        if (string.IsNullOrEmpty(phone))
        {
            return ValidationErrorType.RequiredField;
        }

        if (phone.StartsWith("+"))
        {
            phone = phone.Substring(1);
        }

        if (!phone.All(char.IsDigit) || phone.Length < 7 || phone.Length > 15)
        {
            return ValidationErrorType.InvalidFormat;
        }

        return ValidationErrorType.None;
    }

    public ValidationErrorType IsValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return ValidationErrorType.RequiredField;
        }

        if (!name.All(char.IsLetter))
        {
            return ValidationErrorType.InvalidFormat;
        }

        return ValidationErrorType.None;
    }
}