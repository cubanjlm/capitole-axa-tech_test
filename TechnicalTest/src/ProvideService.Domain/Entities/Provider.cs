using System.Text.Json.Serialization;
using ProvideService.Domain.Enums;
using ProvideService.Domain.Primitives;
using ProvideService.Domain.ValueObjects;

namespace ProvideService.Domain.Entities;

public class Provider : Entity
{
    public Provider(int id, string name, PostalAddress postalAddress, ProviderType type) : base(id)
    {
        Name = name;
        PostalAddress = postalAddress;
        Type = type;
    }
    
    private static readonly string _collection = "providers";

    protected override string Collection => _collection;

    public string Name { get; set; }

    public PostalAddress PostalAddress { get; set; }

    public ProviderType Type { get; set; }
    
    public int CleanId() => int.Parse(Id.Replace($"{Collection}/", string.Empty));
    
    public static string BuildId(int id) => $"{_collection}/{id}";
}