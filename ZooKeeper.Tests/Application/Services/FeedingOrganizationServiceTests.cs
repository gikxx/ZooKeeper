using FluentAssertions;
using Moq;
using ZooKeeper.Application.Services;
using ZooKeeper.Domain.Entities;
using ZooKeeper.Tests.TestHelpers;

namespace ZooKeeper.Tests.Application.Services;

public class FeedingOrganizationServiceTests : TestBase
{
    private readonly FeedingOrganizationService _service;

    public FeedingOrganizationServiceTests()
        : base()
    {
        _service = new FeedingOrganizationService(
            FeedingScheduleRepositoryMock.Object,
            AnimalRepositoryMock.Object,
            EventPublisherMock.Object);
    }

    [Fact]
    public async Task OrganizeFeedingAsync_ShouldCreateSchedule()
    {
        // Arrange
        var animal = TestDataFactory.CreateAnimal();

        AnimalRepositoryMock
            .Setup(r => r.GetByIdAsync(animal.Id, default))
            .ReturnsAsync(animal);

        // Act
        await _service.OrganizeFeedingAsync(animal.Id);

        // Assert
        FeedingScheduleRepositoryMock.Verify(
            r => r.AddAsync(It.Is<FeedingSchedule>(s => 
                    s.AnimalId == animal.Id && 
                    s.FoodType == animal.FavoriteFood), 
                default), 
            Times.Once);
    }

    [Fact]
    public async Task GetTodayFeedingsAsync_ShouldReturnSchedules()
    {
        // Arrange
        var animal = TestDataFactory.CreateAnimal();
        var schedule = TestDataFactory.CreateFeedingSchedule(animal);
        var schedules = new[] { schedule };

        FeedingScheduleRepositoryMock
            .Setup(r => r.GetScheduledForDateAsync(It.IsAny<DateTime>(), default))
            .ReturnsAsync(schedules);

        // Act
        var result = await _service.GetTodayFeedingsAsync();

        // Assert
        result.Should().BeEquivalentTo(schedules);
    }
}