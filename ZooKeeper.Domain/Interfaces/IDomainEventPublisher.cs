namespace ZooKeeper.Domain.Interfaces;

public interface IDomainEventPublisher
{
    Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default);
    Task PublishAsync<TEvent>(IEnumerable<TEvent> domainEvents, CancellationToken cancellationToken = default);
}