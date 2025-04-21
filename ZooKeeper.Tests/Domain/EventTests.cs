using System.Reflection;
using System.Runtime.CompilerServices;
using FluentAssertions;
using ZooKeeper.Domain.Enums;
using ZooKeeper.Domain.Events;
using ZooKeeper.Domain.ValueObjects;
using ZooKeeper.Tests.TestHelpers;

namespace ZooKeeper.Tests.Domain;

public class EventTests : TestBase
{
    [Fact]
    public void AnimalMovedEvent_Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var animalId = AnimalId.Create();
        var fromEnclosureId = EnclosureId.Create();
        var toEnclosureId = EnclosureId.Create();

        // Act
        var @event = new AnimalMovedEvent(animalId, fromEnclosureId, toEnclosureId);

        // Assert
        @event.AnimalId.Should().Be(animalId);
        @event.FromEnclosureId.Should().Be(fromEnclosureId);
        @event.ToEnclosureId.Should().Be(toEnclosureId);
        @event.MovedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void AnimalMovedEvent_Constructor_WithNullFromEnclosure_ShouldAllowNull()
    {
        // Arrange
        var animalId = AnimalId.Create();
        var toEnclosureId = EnclosureId.Create();

        // Act
        var @event = new AnimalMovedEvent(animalId, null, toEnclosureId);

        // Assert
        @event.AnimalId.Should().Be(animalId);
        @event.FromEnclosureId.Should().BeNull();
        @event.ToEnclosureId.Should().Be(toEnclosureId);
    }

    [Fact]
    public void FeedingTimeEvent_Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var animalId = AnimalId.Create();
        var scheduledTime = DateTime.UtcNow.AddHours(1);
        var foodType = FoodType.Meat;
        var scheduleId = FeedingScheduleId.Create();

        // Act
        var @event = new FeedingTimeEvent(animalId, scheduledTime, foodType, scheduleId);

        // Assert
        @event.AnimalId.Should().Be(animalId);
        @event.ScheduledTime.Should().Be(scheduledTime);
        @event.FoodType.Should().Be(foodType);
        @event.ScheduleId.Should().Be(scheduleId);
    }

    [Fact]
    public void Events_ShouldBeImmutable()
    {
        // Assert
        typeof(AnimalMovedEvent).Should().BeAssignableTo<IEquatable<AnimalMovedEvent>>();
        typeof(AnimalMovedEvent).GetProperties()
            .All(p => p.GetSetMethod(true) == null || p.GetSetMethod(true).IsPrivate)
            .Should().BeTrue("все свойства должны быть только для чтения");

        typeof(FeedingTimeEvent).Should().BeAssignableTo<IEquatable<FeedingTimeEvent>>();
        typeof(FeedingTimeEvent).GetProperties()
            .All(p => p.GetSetMethod(true) == null || p.GetSetMethod(true).IsPrivate)
            .Should().BeTrue("все свойства должны быть только для чтения");
    }
}