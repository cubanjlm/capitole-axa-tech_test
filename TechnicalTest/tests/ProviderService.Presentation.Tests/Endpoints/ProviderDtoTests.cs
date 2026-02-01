using System.Text.Json;
using ProviderService.Presentation.Endpoints;
using Shouldly;

namespace ProviderService.Presentation.Tests.Endpoints;

public class ProviderDtoTests
{
    [Fact]
    public void ProviderOutputDto_WhenConstructedWithRequiredProperties_ShouldSucceed()
    {
        // Arrange & Act
        var dto = new ProviderOutputDto
        {
            Id = 1,
            Name = "Test Provider"
        };

        // Assert
        dto.Id.ShouldBe(1);
        dto.Name.ShouldBe("Test Provider");
    }

    [Fact]
    public void ProviderDto_WhenConstructedWithRequiredProperties_ShouldSucceed()
    {
        // Arrange & Act
        var dto = new ProviderDto
        {
            Id = 1,
            Name = "Test Provider",
            PostalAddress = "medio del monte, Betera",
            CreatedAt = new DateTime(2025, 2, 1, 10, 0, 0, DateTimeKind.Utc)
        };

        // Assert
        dto.Id.ShouldBe(1);
        dto.Name.ShouldBe("Test Provider");
        dto.PostalAddress.ShouldBe("medio del monte, Betera");
        dto.CreatedAt.ShouldBe(new DateTime(2025, 2, 1, 10, 0, 0, DateTimeKind.Utc));
    }

    [Fact]
    public void ProviderInputDto_WhenConstructedWithRequiredProperties_ShouldSucceed()
    {
        // Arrange & Act
        var dto = new ProviderInputDto
        {
            Id = 1,
            Name = "Test Provider",
            PostalAddress = "medio del monte, Betera",
            CreatedAt = new DateTime(2025, 2, 1, 10, 0, 0, DateTimeKind.Utc),
            Type = "rental"
        };

        // Assert
        dto.Id.ShouldBe(1);
        dto.Name.ShouldBe("Test Provider");
        dto.PostalAddress.ShouldBe("medio del monte, Betera");
        dto.Type.ShouldBe("rental");
    }

    [Fact]
    public void ProviderOutputDto_WhenSerializedToJson_ShouldUseCorrectPropertyNames()
    {
        // Arrange
        var dto = new ProviderOutputDto { Id = 42, Name = "Serialized Provider" };

        // Act
        var json = JsonSerializer.Serialize(dto);

        // Assert
        json.ShouldContain("provider_id");
        json.ShouldContain("42");
        json.ShouldContain("Name");
        json.ShouldContain("Serialized Provider");
    }

    [Fact]
    public void ProviderInputDto_WhenDeserializedFromJson_ShouldPopulateRequiredProperties()
    {
        // Arrange
        var json = """
            {
                "provider_id": 1,
                "Name": "Deserialized Provider",
                "postal_address": "Calle Principal 123",
                "created_at": "2025-02-01T10:00:00Z",
                "type": "rental"
            }
            """;

        // Act
        var dto = JsonSerializer.Deserialize<ProviderInputDto>(json);

        // Assert
        dto.ShouldNotBeNull();
        dto.Id.ShouldBe(1);
        dto.Name.ShouldBe("Deserialized Provider");
        dto.PostalAddress.ShouldBe("Calle Principal 123");
        dto.Type.ShouldBe("rental");
    }

    [Fact]
    public void ProviderDto_WhenDeserializedFromJson_ShouldPopulateRequiredProperties()
    {
        // Arrange
        var json = """
            {
                "provider_id": 99,
                "Name": "Full Provider",
                "postal_address": "Address Line",
                "created_at": "2025-02-01T12:00:00Z"
            }
            """;

        // Act
        var dto = JsonSerializer.Deserialize<ProviderDto>(json);

        // Assert
        dto.ShouldNotBeNull();
        dto.Id.ShouldBe(99);
        dto.Name.ShouldBe("Full Provider");
        dto.PostalAddress.ShouldBe("Address Line");
    }
}
