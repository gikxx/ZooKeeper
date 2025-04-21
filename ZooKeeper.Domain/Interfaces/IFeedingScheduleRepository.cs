using ZooKeeper.Domain.Entities;
using ZooKeeper.Domain.ValueObjects;

namespace ZooKeeper.Domain.Interfaces;

public interface IFeedingScheduleRepository : IRepository<FeedingSchedule, FeedingScheduleId>
{
    Task<IEnumerable<FeedingSchedule>> GetByAnimalIdAsync(AnimalId animalId, CancellationToken cancellationToken = default);
    Task<IEnumerable<FeedingSchedule>> GetScheduledForDateAsync(DateTime date, CancellationToken cancellationToken = default);
}