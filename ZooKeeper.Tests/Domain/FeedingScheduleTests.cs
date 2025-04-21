using FluentAssertions;
using ZooKeeper.Domain.Entities;
using ZooKeeper.Domain.Enums;
using ZooKeeper.Domain.ValueObjects;
using ZooKeeper.Tests.TestHelpers;

namespace ZooKeeper.Tests.Domain;

public class FeedingScheduleTests : TestBase
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var id = FeedingScheduleId.Create();
        var animalId = AnimalId.Create();
        var feedingTime = DateTime.UtcNow.AddHours(1);
        var foodType = FoodType.Meat;

        // Act
        var schedule = new FeedingSchedule(id, animalId, feedingTime, foodType);

        // Assert
        schedule.Id.Should().Be(id);
        schedule.AnimalId.Should().Be(animalId);
        schedule.FeedingTime.Should().Be(feedingTime);
        schedule.FoodType.Should().Be(foodType);
        schedule.IsCompleted.Should().BeFalse();
    }

    [Fact]
    public void ScheduleFeeding_WhenNotCompleted_ShouldReturnEvent()
    {
        // Arrange
        var animal = TestDataFactory.CreateAnimal();
        var schedule = TestDataFactory.CreateFeedingSchedule(animal);

        // Act
        var result = schedule.ScheduleFeeding();

        // Assert
        result.Should().NotBeNull();
        result.AnimalId.Should().Be(animal.Id);
        result.FoodType.Should().Be(schedule.FoodType);
        result.ScheduledTime.Should().Be(schedule.FeedingTime);
    }

    [Fact]
    public void ScheduleFeeding_WhenCompleted_ShouldThrowException()
    {
        // Arrange
        var animal = TestDataFactory.CreateAnimal();
        var schedule = TestDataFactory.CreateFeedingSchedule(animal, DateTime.UtcNow.AddHours(-1));
        schedule.MarkAsCompleted();

        // Act
        var act = () => schedule.ScheduleFeeding();

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Нельзя запланировать уже выполненное кормление");
    }

    [Fact]
    public void MarkAsCompleted_AfterFeedingTime_ShouldMarkCompleted()
    {
        // Arrange
        var animal = TestDataFactory.CreateAnimal();
        var schedule = TestDataFactory.CreateFeedingSchedule(animal, DateTime.UtcNow.AddHours(-1));

        // Act
        schedule.MarkAsCompleted();

        // Assert
        schedule.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public void MarkAsCompleted_BeforeFeedingTime_ShouldThrowException()
    {
        // Arrange
        var animal = TestDataFactory.CreateAnimal();
        var schedule = TestDataFactory.CreateFeedingSchedule(animal, DateTime.UtcNow.AddHours(1));

        // Act
        var act = () => schedule.MarkAsCompleted();

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Нельзя отметить кормление как выполненное до назначенного времени");
    }
}