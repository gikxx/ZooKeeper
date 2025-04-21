using FluentAssertions;
using ZooKeeper.Domain.Entities;
using ZooKeeper.Domain.Enums;
using ZooKeeper.Domain.ValueObjects;
using ZooKeeper.Tests.TestHelpers;

namespace ZooKeeper.Tests.Domain;

public class AnimalTests : TestBase
{
    [Fact]
    public void Feed_HealthyAnimal_ShouldReturnFeedingEvent()
    {
        // Arrange
        var animal = TestDataFactory.CreateAnimal(status: AnimalStatus.Healthy);

        // Act
        var result = animal.Feed();

        // Assert
        result.Should().NotBeNull();
        result.AnimalId.Should().Be(animal.Id);
        result.FoodType.Should().Be(animal.FavoriteFood);
    }

    [Theory]
    [InlineData(AnimalStatus.Sick, "Нельзя кормить больное животное без назначения врача")]
    [InlineData(AnimalStatus.Treatment, "Животное на лечении, требуется особый режим кормления")]
    [InlineData(AnimalStatus.Quarantine, "Животное на карантине, требуется особый режим кормления")]
    public void Feed_UnhealthyAnimal_ShouldThrowException(AnimalStatus status, string expectedMessage)
    {
        // Arrange
        var animal = TestDataFactory.CreateAnimal();
        animal.UpdateStatus(status);

        // Act
        var act = () => animal.Feed();

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage(expectedMessage);
    }


    [Fact]
    public void Heal_SickAnimal_ShouldSetStatusToHealthy()
    {
        // Arrange
        var animal = TestDataFactory.CreateAnimal();
        animal.UpdateStatus(AnimalStatus.Sick);

        // Act
        animal.Heal();

        // Assert
        animal.Status.Should().Be(AnimalStatus.Healthy);
    }
    
    [Fact]
    public void MoveToEnclosure_ShouldCreateMoveEvent()
    {
        // Arrange
        var animal = TestDataFactory.CreateAnimal();
        var newEnclosureId = EnclosureId.Create();

        // Act
        var result = animal.MoveToEnclosure(newEnclosureId);

        // Assert
        result.Should().NotBeNull();
        result.AnimalId.Should().Be(animal.Id);
        result.ToEnclosureId.Should().Be(newEnclosureId);
    }

    [Fact]
    public void GetAge_ShouldCalculateCorrectAge()
    {
        // Arrange
        var birthDate = DateTime.UtcNow.AddYears(-3);
        var animal = TestDataFactory.CreateAnimal(dateOfBirth: birthDate);

        // Act
        var age = animal.GetAge();

        // Assert
        age.Should().Be(3);
    }
    
    [Fact]
    public void UpdateStatus_ShouldChangeAnimalStatus()
    {
        // Arrange
        var animal = TestDataFactory.CreateAnimal();
        var newStatus = AnimalStatus.Quarantine;

        // Act
        animal.UpdateStatus(newStatus);

        // Assert
        animal.Status.Should().Be(newStatus);
    }
    
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var id = AnimalId.Create();
        var name = new AnimalName("TestAnimal");
        var species = "Lion";
        var dateOfBirth = DateTime.UtcNow.AddYears(-2);
        var gender = Gender.Female;
        var favoriteFood = FoodType.Meat;

        // Act
        var animal = new Animal(
            id,
            name,
            species,
            dateOfBirth,
            gender,
            favoriteFood);

        // Assert
        animal.Id.Should().Be(id);
        animal.Name.Should().Be(name);
        animal.Species.Should().Be(species);
        animal.DateOfBirth.Should().Be(dateOfBirth);
        animal.Gender.Should().Be(gender);
        animal.FavoriteFood.Should().Be(favoriteFood);
        animal.Status.Should().Be(AnimalStatus.Healthy);
        animal.CurrentEnclosureId.Should().BeNull();
    }
    
    [Fact]
    public void Heal_ShouldThrowException_WhenAnimalIsNotSick()
    {
        // Arrange
        var animal = new Animal(
            new AnimalId(Guid.NewGuid()),
            new AnimalName("Лев"),
            "Panthera leo",
            new DateTime(2015, 5, 1),
            Gender.Male,
            FoodType.Meat
        );
        animal.UpdateStatus(AnimalStatus.Healthy);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => animal.Heal());
        Assert.Equal("Животное не нуждается в лечении", exception.Message);
    }
}