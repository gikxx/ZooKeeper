using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ZooKeeper.Domain.Enums;
using ZooKeeper.Domain.Events;
using ZooKeeper.Domain.ValueObjects;
using ZooKeeper.Infrastructure.EventBus;
using ZooKeeper.Infrastructure.InMemory;
using ZooKeeper.Tests.TestHelpers;

namespace ZooKeeper.Tests.Infrastructure.InMemory;

public class InMemoryRepositoryTests : TestBase
{
    [Fact]
    public async Task AnimalRepository_BasicOperations_ShouldWork()
    {
        // Arrange
        var repo = new AnimalRepository();
        var animal = TestDataFactory.CreateAnimal();

        // Act & Assert
        await repo.AddAsync(animal);
        var found = await repo.GetByIdAsync(animal.Id);
        found.Should().BeEquivalentTo(animal);

        animal.UpdateStatus(AnimalStatus.Sick);
        await repo.UpdateAsync(animal);
        found = await repo.GetByIdAsync(animal.Id);
        found!.Status.Should().Be(AnimalStatus.Sick);

        await repo.DeleteAsync(animal.Id);
        found = await repo.GetByIdAsync(animal.Id);
        found.Should().BeNull();
    }

    [Fact]
    public async Task AnimalRepository_SpecializedQueries_ShouldWork()
    {
        // Arrange
        var repo = new AnimalRepository();
        var animal1 = TestDataFactory.CreateAnimal();
        var animal2 = TestDataFactory.CreateAnimal();

        animal2.MoveToEnclosure(animal1.CurrentEnclosureId!);
        animal2.UpdateStatus(AnimalStatus.Sick);

        await repo.AddAsync(animal1);
        await repo.AddAsync(animal2);

        // Act
        var byEnclosure = await repo.GetByEnclosureIdAsync(animal1.CurrentEnclosureId);
        var byStatus = await repo.GetByStatusAsync(AnimalStatus.Sick);

        // Assert
        byEnclosure.Should().HaveCount(2);
        byStatus.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(animal2);
    }

    [Fact]
    public async Task FeedingScheduleRepository_BasicOperations_ShouldWork()
    {
        // Arrange
        var repo = new FeedingScheduleRepository();
        var animal = TestDataFactory.CreateAnimal();
        var dateTime = DateTime.UtcNow;
        var schedule = TestDataFactory.CreateFeedingSchedule(animal, dateTime);

        // Act & Assert
        await repo.AddAsync(schedule);
        var found = await repo.GetByIdAsync(schedule.Id);
        found.Should().BeEquivalentTo(schedule);

        await repo.DeleteAsync(schedule.Id);
        found = await repo.GetByIdAsync(schedule.Id);
        found.Should().BeNull();
    }

    [Fact]
    public async Task FeedingScheduleRepository_SpecializedQueries_ShouldWork()
    {
        // Arrange
        var repo = new FeedingScheduleRepository();
        var animal = TestDataFactory.CreateAnimal();
        var dateTime = DateTime.UtcNow;
        var schedule1 = TestDataFactory.CreateFeedingSchedule(animal, dateTime);
        var schedule2 = TestDataFactory.CreateFeedingSchedule(animal, dateTime.AddDays(1));

        await repo.AddAsync(schedule1);
        await repo.AddAsync(schedule2);

        // Act
        var byAnimal = await repo.GetByAnimalIdAsync(animal.Id);
        var byDate = await repo.GetScheduledForDateAsync(dateTime);

        // Assert
        byAnimal.Should().HaveCount(2);
        byDate.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(schedule1);
    }


    [Fact]
    public async Task EnclosureRepository_BasicOperations_ShouldWork()
    {
        // Arrange
        var repo = new EnclosureRepository();
        var enclosure = TestDataFactory.CreateEnclosure();

        // Act & Assert
        await repo.AddAsync(enclosure);
        var found = await repo.GetByIdAsync(enclosure.Id);
        found.Should().BeEquivalentTo(enclosure);

        await repo.DeleteAsync(enclosure.Id);
        found = await repo.GetByIdAsync(enclosure.Id);
        found.Should().BeNull();
    }

    [Fact]
    public async Task EnclosureRepository_SpecializedQueries_ShouldWork()
    {
        // Arrange
        var repo = new EnclosureRepository();
        var enclosure1 = TestDataFactory.CreateEnclosure(type: EnclosureType.Predator);
        var enclosure2 = TestDataFactory.CreateEnclosure(type: EnclosureType.Herbivore);
        await repo.AddAsync(enclosure1);
        await repo.AddAsync(enclosure2);

        // Act
        var byType = await repo.GetByTypeAsync(EnclosureType.Predator);
        var available = await repo.GetAvailableAsync();

        // Assert
        byType.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(enclosure1);
        available.Should().Contain(enclosure1)
            .And.Contain(enclosure2);
    }
}

public class DomainEventPublisherTests : TestBase
{
    private readonly ILogger<DomainEventPublisher> _logger;
    private readonly IServiceProvider _serviceProvider;

    public DomainEventPublisherTests()
    {
        _logger = Mock.Of<ILogger<DomainEventPublisher>>();
        _serviceProvider = Mock.Of<IServiceProvider>();
    }

    [Fact]
    public async Task DomainEventPublisher_ShouldHandleEvents()
    {
        // Arrange
        var publisher = new DomainEventPublisher(_logger, _serviceProvider);
        var movedEvent = new AnimalMovedEvent(AnimalId.Create(), null, EnclosureId.Create());
        var feedingEvent = new FeedingTimeEvent(
            AnimalId.Create(),
            DateTime.UtcNow,
            FoodType.Meat,
            FeedingScheduleId.Create());

        // Act & Assert
        await publisher.PublishAsync(movedEvent);
        await publisher.PublishAsync(feedingEvent);
        await publisher.PublishAsync(new[] { movedEvent });
        await publisher.PublishAsync(new[] { feedingEvent });
    }
}