using Domain.Primitives;

namespace Domain.Errors;

public static partial class DomainErrors
{
    public static class Contact
    {
        public static Error DuplicateInternalNumber(short internalNumber)
        {
            return new Error("InternalNumber.Duplicate", $"Internal number '{internalNumber}' exists in database.");
        }
    }
}