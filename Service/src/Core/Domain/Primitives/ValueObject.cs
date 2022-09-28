namespace Domain.Primitives;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public static bool operator ==(ValueObject? a, ValueObject? b)
    {
        if (a is null && b is null) return true;

        if (a is null || b is null) return false;

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject a, ValueObject b)
    {
        return !(a == b);
    }

    /// <inheritdoc />
    public bool Equals(ValueObject? other)
    {
        return other is not null && ValuesAreEquals(other);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;

        if (GetType() != obj.GetType()) return false;

        return obj is ValueObject other && ValuesAreEquals(other);
    }

    private bool ValuesAreEquals(ValueObject other)
    {
        return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return GetAtomicValues()
            .Aggregate(default(HashCode), (hashCode, obj) =>
            {
                hashCode.Add(obj.GetHashCode());

                return hashCode;
            }).ToHashCode();
    }

    /// <summary>
    ///     Gets the atomic values of the value object.
    /// </summary>
    /// <returns>The collection of objects representing the value object values.</returns>
    protected abstract IEnumerable<object> GetAtomicValues();
}