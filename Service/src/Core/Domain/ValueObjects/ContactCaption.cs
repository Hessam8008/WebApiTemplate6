using Domain.Errors;
using Domain.Primitives;
using Domain.Primitives.Result;

namespace Domain.ValueObjects;

public sealed class ContactCaption : ValueObject
{
    public const int MaxLength = 50;

    private ContactCaption(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<ContactCaption> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return DomainErrors.General.ValueIsRequired();

        value = value.Trim();

        return value.Length > MaxLength
            ? DomainErrors.General.TooLong()
            : new ContactCaption(value);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static implicit operator string(ContactCaption value)
    {
        return value.Value;
    }
}