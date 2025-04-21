using FluentAssertions;
using ZooKeeper.Domain.ValueObjects;
using ZooKeeper.Tests.TestHelpers;

namespace ZooKeeper.Tests.Domain;

public class ValueObjectsTests : TestBase
{
    [Fact]
    public void AnimalId_Create_ShouldGenerateNewId()
    {
        // Act
        var id = AnimalId.Create();

        // Assert
        id.Value.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void AnimalId_WithEmptyGuid_ShouldThrow()
    {
        // Act & Assert
        var action = () => new AnimalId(Guid.Empty);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Идентификатор животного не может быть пустым");
    }

    [Fact]
    public void AnimalId_Equality_ShouldWork()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var id1 = new AnimalId(guid);
        var id2 = new AnimalId(guid);
        var id3 = AnimalId.Create();

        // Assert
        id1.Should().Be(id2);
        id1.Should().NotBe(id3);
        (id1 == id2).Should().BeTrue();
        (id1 != id3).Should().BeTrue();
    }

    [Fact]
    public void EnclosureId_Create_ShouldGenerateNewId()
    {
        // Act
        var id = EnclosureId.Create();

        // Assert
        id.Value.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void FeedingScheduleId_Create_ShouldGenerateNewId()
    {
        // Act
        var id = FeedingScheduleId.Create();

        // Assert
        id.Value.Should().NotBe(Guid.Empty);
    }
}