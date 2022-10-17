using Domain.Primitives;

namespace Domain.Errors;

public static partial class DomainErrors
{
    public static class Person
    {
        public static readonly Error BlockedNationality =
            new("Person.Nationality", "Nationality has been blocked.");

        public static readonly Error EmptyFirstName = new("FirstName.Empty", "First name is empty.");
        public static readonly Error TooLongFirstName = new("FirstName.MaxLength", "First name is too long.");
        public static readonly Error EmptyLastName = new("LastName.Empty", "Last name is empty.");
        public static readonly Error TooLongLastName = new("LastName.MaxLength", "Last name is too long.");
        public static readonly Error EmptyNationalCode = new("NationalCode.Empty", "National Code is empty.");
        public static readonly Error InvalidBirthDay = new("BirthDay.Invalid", "Invalid day for birthday.");

        public static readonly Error WrongLengthNationalCode =
            new("NationalCode.Length", "Length of the nation code is unacceptable.");

        public static readonly Error DigitOnlyCharacterNationalCode =
            new("NationalCode.IllegalCharacter", "Only digits allowed for nation code.");

        public static readonly Error UnacceptableNationalCode =
            new("NationalCode.Unacceptable", "Value is not acceptable.");

        public static readonly Error InvalidNationalCode =
            new("NationalCode.Invalid", "Nation code is invalid.");
    }
}
