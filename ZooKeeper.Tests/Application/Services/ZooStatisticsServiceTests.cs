using FluentAssertions;
using Moq;
using ZooKeeper.Application.Services;
using ZooKeeper.Domain.Enums;
using ZooKeeper.Domain.Interfaces;
using ZooKeeper.Tests.TestHelpers;

namespace ZooKeeper.Tests.Application.Services;

public class ZooStatisticsServiceTests
{
    private readonly Mock<IAnimalRepository> _animalRepositoryMock;
    private readonly Mock<IEnclosureRepository> _enclosureRepositoryMock;
    private readonly Mock<IFeedingScheduleRepository> _feedingScheduleRepositoryMock;
    private readonly ZooStatisticsService _service;

    public ZooStatisticsServiceTests()
    {
        _animalRepositoryMock = new Mock<IAnimalRepository>();
        _enclosureRepositoryMock = new Mock<IEnclosureRepository>();
        _feedingScheduleRepositoryMock = new Mock<IFeedingScheduleRepository>();

        _service = new ZooStatisticsService(
            _animalRepositoryMock.Object,
            _enclosureRepositoryMock.Object,
            _feedingScheduleRepositoryMock.Object);
    }

    [Fact]
    public async Task GetStatisticsAsync_ShouldReturnCorrectStats()
    {
        // Arrange
        var animals = new[]
        {
            TestDataFactory.CreateAnimal(),
            TestDataFactory.CreateAnimal(),
            TestDataFactory.CreateAnimal()
        };
        animals[2].UpdateStatus(AnimalStatus.Sick);

        var enclosure1 = TestDataFactory.CreateEnclosure(capacity: 2);
        var enclosure2 = TestDataFactory.CreateEnclosure(capacity: 2);

        enclosure1.AddAnimal(animals[0].Id);
        enclosure1.AddAnimal(animals[1].Id);

        var enclosures = new[] { enclosure1, enclosure2 };

        var schedules = new[]
        {
            TestDataFactory.CreateFeedingSchedule(animals[0], DateTime.UtcNow)
        };

        _animalRepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(animals);

        _enclosureRepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(enclosures);

        _feedingScheduleRepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(schedules);

        // Act
        var stats = await _service.GetStatisticsAsync();

        // Assert
        stats.Should().BeEquivalentTo(new
        {
            TotalAnimals = 3,
            HealthyAnimals = 2,
            SickAnimals = 1,
            TotalEnclosures = 2,
            AvailableEnclosures = 1,
            ActiveFeedings = 1
        });

        // Verify
        _animalRepositoryMock.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        _enclosureRepositoryMock.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        _feedingScheduleRepositoryMock.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}