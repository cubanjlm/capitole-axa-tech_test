using Newtonsoft.Json;
using ProvideService.Domain.Primitives;
using ProvideService.Domain.Shared;

namespace ProvideService.Domain.ValueObjects;

public class PostalAddress : ValueObject
{
    [JsonConstructor]
    private PostalAddress(string addressLine1, string? city)
    {
        AddressLine1 = addressLine1;
        City = city;
    }

    public string AddressLine1 { get; private init; }
    public string? City { get; private init; }

    public static PostalAddress Create(string fullAddress)
    {
        var addressLineAndCountry = fullAddress.Split(",");
        return new PostalAddress(addressLineAndCountry.First(), addressLineAndCountry.LastOrDefault());
    }

    public override string ToString()
    {
        return $"{AddressLine1},{City}";
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return new[] { AddressLine1, City ?? string.Empty };
    }
}