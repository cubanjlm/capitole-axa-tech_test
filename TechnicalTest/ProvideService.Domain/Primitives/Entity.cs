

using Newtonsoft.Json;

namespace ProvideService.Domain.Primitives;

public abstract class Entity :IEquatable<Entity>, IAuditable
{
    protected Entity(int id)
    {
        Id = $"{Collection}/{id}";
        CreatedAt = DateTimeOffset.Now;
    }

    [JsonIgnore]
    protected abstract string Collection { get; }
    public string Id { get; private init; }
    public DateTimeOffset CreatedAt { get; init; }
    

    public bool Equals(Entity? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Collection == other.Collection && Id == other.Id && CreatedAt.Equals(other.CreatedAt);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Entity)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Collection, Id, CreatedAt);
    }
}