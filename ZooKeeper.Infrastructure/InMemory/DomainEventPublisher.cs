using Microsoft.Extensions.Logging;
using ZooKeeper.Domain.Interfaces;
using ZooKeeper.Domain.Events;

namespace ZooKeeper.Infrastructure.EventBus;

public class DomainEventPublisher : IDomainEventPublisher
{
    private readonly ILogger<DomainEventPublisher> _logger;
    private readonly IServiceProvider _serviceProvider;

    public DomainEventPublisher(
        ILogger<DomainEventPublisher> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Publishing domain event: {EventType}", typeof(TEvent).Name);

            switch (domainEvent)
            {
                case AnimalMovedEvent movedEvent:
                    await HandleAnimalMoved(movedEvent);
                    break;
                case FeedingTimeEvent feedingEvent:
                    await HandleFeedingTime(feedingEvent);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing domain event");
            throw;
        }
    }

    public Task PublishAsync<TEvent>(IEnumerable<TEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        return Task.WhenAll(domainEvents.Select(e => PublishAsync(e, cancellationToken)));
    }

    private async Task HandleAnimalMoved(AnimalMovedEvent @event)
    {
        _logger.LogInformation(
            "Animal {AnimalId} moved from {FromEnclosure} to {ToEnclosure}",
            @event.AnimalId,
            @event.FromEnclosureId,
            @event.ToEnclosureId);
    }

    private async Task HandleFeedingTime(FeedingTimeEvent @event)
    {
        _logger.LogInformation(
            "Feeding time for animal {AnimalId} at {FeedingTime} with {FoodType}",
            @event.AnimalId,
            @event.ScheduledTime,
            @event.FoodType);
    }
}