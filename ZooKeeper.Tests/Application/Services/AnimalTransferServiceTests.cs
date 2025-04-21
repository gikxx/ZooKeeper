using FluentAssertions;
using Moq;
using ZooKeeper.Application.Services;
using ZooKeeper.Domain.Entities;
using ZooKeeper.Domain.Events;
using ZooKeeper.Domain.ValueObjects;
using ZooKeeper.Tests.TestHelpers;

namespace ZooKeeper.Tests.Application.Services;

public class AnimalTransferServiceTests : TestBase
{
    private readonly AnimalTransferService _service;

    public AnimalTransferServiceTests()
        : base()
    {
        _service = new AnimalTransferService(
            AnimalRepositoryMock.Object,
            EnclosureRepositoryMock.Object,
            EventPublisherMock.Object);
    }

    [Fact]
    public async Task TransferAnimalAsync_ValidTransfer_ShouldSucceed()
    {
        // Arrange
        var animal = TestDataFactory.CreateAnimal();
        var targetEnclosure = TestDataFactory.CreateEnclosure();

        AnimalRepositoryMock
            .Setup(r => r.GetByIdAsync(animal.Id, default))
            .ReturnsAsync(animal);

        EnclosureRepositoryMock
            .Setup(r => r.GetByIdAsync(targetEnclosure.Id, default))
            .ReturnsAsync(targetEnclosure);

        // Act
        await _service.TransferAnimalAsync(animal.Id, targetEnclosure.Id);

        // Assert
        EnclosureRepositoryMock.Verify(r => r.UpdateAsync(targetEnclosure, default), Times.Once);
        AnimalRepositoryMock.Verify(r => r.UpdateAsync(animal, default), Times.Once);
        EventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<AnimalMovedEvent>(), default), Times.Once);
    }

    [Fact]
    public async Task TransferAnimalAsync_AnimalNotFound_ShouldThrow()
    {
        // Arrange
        var animalId = AnimalId.Create();
        var enclosureId = EnclosureId.Create();

        AnimalRepositoryMock
            .Setup(r => r.GetByIdAsync(animalId, default))
            .ReturnsAsync((Animal?)null);

        // Act & Assert
        var action = () => _service.TransferAnimalAsync(animalId, enclosureId);
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Животное не найдено");
    }

    [Fact]
    public async Task TransferAnimalAsync_EnclosureNotFound_ShouldThrow()
    {
        // Arrange
        var animal = TestDataFactory.CreateAnimal();
        var enclosureId = EnclosureId.Create();

        AnimalRepositoryMock
            .Setup(r => r.GetByIdAsync(animal.Id, default))
            .ReturnsAsync(animal);

        EnclosureRepositoryMock
            .Setup(r => r.GetByIdAsync(enclosureId, default))
            .ReturnsAsync((Enclosure?)null);

        // Act & Assert
        var action = () => _service.TransferAnimalAsync(animal.Id, enclosureId);
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Целевой вольер не найден");
    }
}

