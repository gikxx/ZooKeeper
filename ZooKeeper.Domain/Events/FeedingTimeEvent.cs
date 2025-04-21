using ZooKeeper.Domain.Enums;
using ZooKeeper.Domain.ValueObjects;

namespace ZooKeeper.Domain.Events;

public record FeedingTimeEvent
{
    public AnimalId AnimalId { get; }
    public DateTime ScheduledTime { get; }
    public FoodType FoodType { get; }
    public FeedingScheduleId ScheduleId { get; }

    public FeedingTimeEvent(
        AnimalId animalId,
        DateTime scheduledTime,
        FoodType foodType,
        FeedingScheduleId scheduleId)
    {
        AnimalId = animalId;
        ScheduledTime = scheduledTime;
        FoodType = foodType;
        ScheduleId = scheduleId;
    }
}