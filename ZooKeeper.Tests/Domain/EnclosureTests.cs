using FluentAssertions;
using ZooKeeper.Domain.Entities;
using ZooKeeper.Domain.Enums;
using ZooKeeper.Domain.ValueObjects;
using ZooKeeper.Tests.TestHelpers;

namespace ZooKeeper.Tests.Domain;

public class EnclosureTests : TestBase
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var id = EnclosureId.Create();
        var type = EnclosureType.Predator;
        var capacity = new EnclosureCapacity(5);

        // Act
        var enclosure = new Enclosure(id, type, capacity);

        // Assert
        enclosure.Id.Should().Be(id);
        enclosure.Type.Should().Be(type);
        enclosure.Capacity.Should().Be(capacity);
        enclosure.Animals.Should().BeEmpty();
    }

    [Fact]
    public void AddAnimal_WhenSpaceAvailable_ShouldAddAnimal()
    {
        // Arrange
        var enclosure = TestDataFactory.CreateEnclosure(capacity: 2);
        var animalId = AnimalId.Create();

        // Act
        enclosure.AddAnimal(animalId);

        // Assert
        enclosure.Animals.Should().Contain(animalId);
    }

    [Fact]
    public void AddAnimal_WhenEnclosureFull_ShouldThrowException()
    {
        // Arrange
        var enclosure = TestDataFactory.CreateEnclosure(capacity: 1);
        enclosure.AddAnimal(AnimalId.Create());

        // Act
        var act = () => enclosure.AddAnimal(AnimalId.Create());

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Вольер переполнен");
    }

    [Fact]
    public void RemoveAnimal_ExistingAnimal_ShouldRemoveAnimal()
    {
        // Arrange
        var enclosure = TestDataFactory.CreateEnclosure();
        var animalId = AnimalId.Create();
        enclosure.AddAnimal(animalId);

        // Act
        enclosure.RemoveAnimal(animalId);

        // Assert
        enclosure.Animals.Should().NotContain(animalId);
    }

    [Fact]
    public void RemoveAnimal_NonExistingAnimal_ShouldThrowException()
    {
        // Arrange
        var enclosure = TestDataFactory.CreateEnclosure();
        var animalId = AnimalId.Create();

        // Act
        var act = () => enclosure.RemoveAnimal(animalId);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Животное не найдено в вольере");
    }

    [Fact]
    public void Clean_EmptyEnclosure_ShouldThrowException()
    {
        // Arrange
        var enclosure = TestDataFactory.CreateEnclosure();

        // Act
        var act = () => enclosure.Clean();

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Нет необходимости убирать пустой вольер");
    }

    [Fact]
    public void Clean_DirtyEnclosure_ShouldCleanSuccessfully()
    {
        // Arrange
        var enclosure = TestDataFactory.CreateEnclosure();
        var animalId = AnimalId.Create();
        enclosure.AddAnimal(animalId);
        enclosure.MarkDirty();

        // Act
        enclosure.Clean();

        // Assert
        enclosure.NeedsCleaning().Should().BeFalse();
    }

    [Fact]
    public void Clean_AlreadyCleanEnclosure_ShouldThrowException()
    {
        // Arrange
        var enclosure = TestDataFactory.CreateEnclosure();
        var animalId = AnimalId.Create();
        enclosure.AddAnimal(animalId);
        enclosure.Clean();

        // Act
        var act = () => enclosure.Clean();

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Вольер уже чистый и не требует уборки");
    }

    [Fact]
    public void NeedsCleaning_AfterTwelveHours_ShouldReturnTrue()
    {
        // Arrange
        var enclosure = TestDataFactory.CreateEnclosure();
        var animalId = AnimalId.Create();
        enclosure.AddAnimal(animalId);
        enclosure.Clean();

        // Устанавливаем время последней уборки на 13 часов назад через рефлексию
        var lastCleaningTime = DateTime.UtcNow.AddHours(-13);
        typeof(Enclosure)
            .GetField("_lastCleaningTime",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(enclosure, lastCleaningTime);

        // Act & Assert
        enclosure.NeedsCleaning().Should().BeTrue();
    }

    [Fact]
    public void MarkDirty_ShouldRequireCleaning()
    {
        // Arrange
        var enclosure = TestDataFactory.CreateEnclosure();
        var animalId = AnimalId.Create();
        enclosure.AddAnimal(animalId);
        enclosure.Clean();

        // Act
        enclosure.MarkDirty();

        // Assert
        enclosure.NeedsCleaning().Should().BeTrue();
    }
}