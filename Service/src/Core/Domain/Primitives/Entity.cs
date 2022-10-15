namespace Domain.Primitives;

public abstract class Entity : IEquatable<Entity>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Entity" /> class.
    /// </summary>
    /// <param name="id">The entity identifier.</param>
    protected Entity(Guid id)
    {
        Id = id;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Entity" /> class.
    /// </summary>
    /// <remarks>
    ///     Required by EF Core.
    /// </remarks>
    protected Entity()
    {
    }

    /// <summary>
    ///     Gets the entity identifier.
    /// </summary>
    public Guid Id { get; }

    public static bool operator ==(Entity? first, Entity? second)
    {
        return first is not null && second is not null && first.Equals(second);
    }

    public static bool operator !=(Entity? first, Entity? second)
    {
        return !(first == second);
    }

    /// <inheritdoc />
    public bool Equals(Entity? other)
    {
        if (other is null) return false;
        if (other.GetType() != GetType())
            return false;

        return other.Id == Id;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType())
            return false;
        if (obj is not Entity entity)
            return false;
        return entity.Id == Id;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Id.GetHashCode() * 41;
    }
}