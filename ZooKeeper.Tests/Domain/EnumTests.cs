using System.Text.Json;
using FluentAssertions;
using ZooKeeper.Domain.Enums;
using ZooKeeper.Tests.TestHelpers;

namespace ZooKeeper.Tests.Domain;

public class EnumTests : TestBase
{
    [Theory]
    [InlineData(AnimalStatus.Healthy, "Healthy")]
    [InlineData(AnimalStatus.Sick, "Sick")]
    [InlineData(AnimalStatus.Quarantine, "Quarantine")]
    [InlineData(AnimalStatus.Treatment, "Treatment")]
    public void AnimalStatus_ShouldSerializeCorrectly(AnimalStatus status, string expected)
    {
        // Arrange & Act
        var serialized = JsonSerializer.Serialize(status);

        // Assert
        serialized.Should().Be($"\"{expected}\"");
    }

    [Theory]
    [InlineData(EnclosureType.Predator, "Predator")]
    [InlineData(EnclosureType.Herbivore, "Herbivore")]
    [InlineData(EnclosureType.Aviary, "Aviary")]
    [InlineData(EnclosureType.Aquarium, "Aquarium")]
    [InlineData(EnclosureType.Terrarium, "Terrarium")]
    [InlineData(EnclosureType.Insectarium, "Insectarium")]
    public void EnclosureType_ShouldSerializeCorrectly(EnclosureType type, string expected)
    {
        // Arrange & Act
        var serialized = JsonSerializer.Serialize(type);

        // Assert
        serialized.Should().Be($"\"{expected}\"");
    }

    [Theory]
    [InlineData(FoodType.Meat, "Meat")]
    [InlineData(FoodType.Fish, "Fish")]
    [InlineData(FoodType.Vegetables, "Vegetables")]
    [InlineData(FoodType.Fruits, "Fruits")]
    [InlineData(FoodType.Insects, "Insects")]
    [InlineData(FoodType.SpecialFood, "SpecialFood")]
    public void FoodType_ShouldSerializeCorrectly(FoodType type, string expected)
    {
        // Arrange & Act
        var serialized = JsonSerializer.Serialize(type);

        // Assert
        serialized.Should().Be($"\"{expected}\"");
    }

    [Theory]
    [InlineData(Gender.Male, "Male")]
    [InlineData(Gender.Female, "Female")]
    public void Gender_ShouldSerializeCorrectly(Gender gender, string expected)
    {
        // Arrange & Act
        var serialized = JsonSerializer.Serialize(gender);

        // Assert
        serialized.Should().Be($"\"{expected}\"");
    }

    [Theory]
    [InlineData("\"Healthy\"", AnimalStatus.Healthy)]
    [InlineData("\"Predator\"", EnclosureType.Predator)]
    [InlineData("\"Meat\"", FoodType.Meat)]
    [InlineData("\"Male\"", Gender.Male)]
    public void Enums_ShouldDeserializeCorrectly(string json, object expected)
    {
        // Arrange & Act & Assert
        switch (expected)
        {
            case AnimalStatus status:
                JsonSerializer.Deserialize<AnimalStatus>(json).Should().Be(status);
                break;
            case EnclosureType type:
                JsonSerializer.Deserialize<EnclosureType>(json).Should().Be(type);
                break;
            case FoodType food:
                JsonSerializer.Deserialize<FoodType>(json).Should().Be(food);
                break;
            case Gender gender:
                JsonSerializer.Deserialize<Gender>(json).Should().Be(gender);
                break;
        }
    }
}