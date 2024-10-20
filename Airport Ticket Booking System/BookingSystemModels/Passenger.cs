namespace AirportTicketBookingSystem;


public record Passenger(string ID, string FirstName, string LastName, string Email, string Phone, PassengerType PassengerType)
{
    private string _firstName;
    private string _lastName;
    private string _phone;
    private string _email;
    private PassengerType _type;
    private IInputValidation Validation;

    public string FirstName
    {
        get => _firstName;
        init
        {
            if (Validation.IsValidName(value) != ValidationErrorType.None)
            {
                throw new ArgumentException("Invalid name format.");
            }
            _firstName = value;
        }
    }

    public string LastName
    {
        get => _lastName;
        init
        {
            if (Validation.IsValidName(value) != ValidationErrorType.None)
            {
                throw new ArgumentException("Invalid name format.");
            }
            _lastName = value;
        }
    }

    public string Email
    {
        get => _email;
        init
        {
            if (Validation.IsValidEmail(value) != ValidationErrorType.None)
            {
                throw new ArgumentException("Invalid email format.");
            }
            _email = value;
        }
    }

    public string Phone
    {
        get => _phone;
        init
        {

            if (Validation.IsValidPhoneNumber(value) != ValidationErrorType.None)
            {
                throw new ArgumentException("Invalid phone number format. It should only contain digits and optionally start with '+'.");
            }
            _phone = value;
        }
    }

    public PassengerType PassengerType
    {
        get => _type;
        init
        {
            if (!Enum.IsDefined(typeof(PassengerType), value))
            {
                throw new ArgumentException("Invalid passenger type.");
            }
            _type = value;
        }
    }
}




