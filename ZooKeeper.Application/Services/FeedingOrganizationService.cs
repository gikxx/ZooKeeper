using ZooKeeper.Domain.Entities;
using ZooKeeper.Domain.Events;
using ZooKeeper.Domain.Interfaces;
using ZooKeeper.Domain.ValueObjects;
using ZooKeeper.Domain.Enums;

namespace ZooKeeper.Application.Services;

public class FeedingOrganizationService
{
    private readonly IFeedingScheduleRepository _feedingScheduleRepository;
    private readonly IAnimalRepository _animalRepository;
    private readonly IDomainEventPublisher _eventPublisher;

    public FeedingOrganizationService(
        IFeedingScheduleRepository feedingScheduleRepository,
        IAnimalRepository animalRepository,
        IDomainEventPublisher eventPublisher)
    {
        _feedingScheduleRepository = feedingScheduleRepository;
        _animalRepository = animalRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task OrganizeFeedingAsync(AnimalId animalId, CancellationToken cancellationToken = default)
    {
        var animal = await _animalRepository.GetByIdAsync(animalId, cancellationToken)
                     ?? throw new InvalidOperationException("Животное не найдено");
        var foodType = animal.FavoriteFood;
        
        var schedule = new FeedingSchedule(
            FeedingScheduleId.Create(),
            animalId,
            DateTime.UtcNow.AddHours(1),
            foodType // или определять тип еды на основе типа животного
        );

        await _feedingScheduleRepository.AddAsync(schedule, cancellationToken);
        await _eventPublisher.PublishAsync(schedule.ScheduleFeeding(), cancellationToken);
    }

    public async Task<IEnumerable<FeedingSchedule>> GetTodayFeedingsAsync(CancellationToken cancellationToken = default)
    {
        return await _feedingScheduleRepository.GetScheduledForDateAsync(DateTime.UtcNow, cancellationToken);
    }
}