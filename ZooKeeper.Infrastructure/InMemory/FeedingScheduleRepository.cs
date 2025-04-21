using ZooKeeper.Domain.Entities;
using ZooKeeper.Domain.Interfaces;
using ZooKeeper.Domain.ValueObjects;

namespace ZooKeeper.Infrastructure.InMemory;

public class FeedingScheduleRepository : IFeedingScheduleRepository
{
    private readonly Dictionary<FeedingScheduleId, FeedingSchedule> _schedules = new();

    public Task<FeedingSchedule?> GetByIdAsync(FeedingScheduleId id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_schedules.GetValueOrDefault(id));
    }

    public Task<IEnumerable<FeedingSchedule>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<FeedingSchedule>>(_schedules.Values);
    }

    public Task AddAsync(FeedingSchedule entity, CancellationToken cancellationToken = default)
    {
        _schedules[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(FeedingSchedule entity, CancellationToken cancellationToken = default)
    {
        _schedules[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(FeedingScheduleId id, CancellationToken cancellationToken = default)
    {
        _schedules.Remove(id);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<FeedingSchedule>> GetByAnimalIdAsync(AnimalId animalId, CancellationToken cancellationToken = default)
    {
        var schedules = _schedules.Values
            .Where(s => s.AnimalId == animalId);
        return Task.FromResult(schedules);
    }

    public Task<IEnumerable<FeedingSchedule>> GetScheduledForDateAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        var schedules = _schedules.Values
            .Where(s => s.FeedingTime.Date == date.Date);
        return Task.FromResult(schedules);
    }
}