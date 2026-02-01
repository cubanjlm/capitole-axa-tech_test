namespace ProvideService.Domain.Primitives;

public interface IAuditable
{
    public DateTimeOffset CreatedAt { get; init; }
}