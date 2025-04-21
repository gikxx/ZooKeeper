using Moq;
using ZooKeeper.Domain.Interfaces;

namespace ZooKeeper.Tests.TestHelpers;

public abstract class TestBase
{
    protected readonly Mock<IAnimalRepository> AnimalRepositoryMock;
    protected readonly Mock<IEnclosureRepository> EnclosureRepositoryMock;
    protected readonly Mock<IFeedingScheduleRepository> FeedingScheduleRepositoryMock;
    protected readonly Mock<IDomainEventPublisher> EventPublisherMock;

    protected TestBase()
    {
        AnimalRepositoryMock = new Mock<IAnimalRepository>();
        EnclosureRepositoryMock = new Mock<IEnclosureRepository>();
        FeedingScheduleRepositoryMock = new Mock<IFeedingScheduleRepository>();
        EventPublisherMock = new Mock<IDomainEventPublisher>();
    }
}