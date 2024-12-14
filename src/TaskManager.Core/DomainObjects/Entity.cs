namespace TaskManager.Core.DomainObjects;

public abstract class Entity : IEntity
{
    public Guid Id { get; set; }

    public Entity()
    {
        Id = Guid.NewGuid();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity compareTo) return false;
        if (ReferenceEquals(this, compareTo)) return true;

        return Id.Equals(compareTo.Id);
    }

    public static bool operator ==(Entity a, Entity b)
    {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity a, Entity b) => !(a == b);

    public override int GetHashCode() => (GetType().GetHashCode() * 907) + Id.GetHashCode();

    public override string ToString() => $"{GetType().Name} [Id={Id}]";
}