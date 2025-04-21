using ZooKeeper.Domain.Enums;
using ZooKeeper.Domain.Events;
using ZooKeeper.Domain.ValueObjects;

namespace ZooKeeper.Domain.Entities;

public class FeedingSchedule
{
    public FeedingScheduleId Id { get; private set; }
    public AnimalId AnimalId { get; private set; }
    public DateTime FeedingTime { get; private set; }
    public FoodType FoodType { get; private set; }
    public bool IsCompleted { get; private set; }

    private FeedingSchedule() { }

    public FeedingSchedule(
        FeedingScheduleId id,
        AnimalId animalId,
        DateTime feedingTime,
        FoodType foodType)
    {
        Id = id;
        AnimalId = animalId;
        FeedingTime = feedingTime;
        FoodType = foodType;
        IsCompleted = false;
    }

    public FeedingTimeEvent ScheduleFeeding()
    {
        if (IsCompleted)
            throw new InvalidOperationException("Нельзя запланировать уже выполненное кормление");

        return new FeedingTimeEvent(AnimalId, FeedingTime, FoodType, Id);
    }

    public void MarkAsCompleted()
    {
        if (DateTime.UtcNow < FeedingTime)
            throw new InvalidOperationException("Нельзя отметить кормление как выполненное до назначенного времени");

        IsCompleted = true;
    }

    public FeedingTimeEvent UpdateSchedule(DateTime newFeedingTime)
    {
        if (IsCompleted)
            throw new InvalidOperationException("Нельзя изменить время уже выполненного кормления");

        FeedingTime = newFeedingTime;
        return new FeedingTimeEvent(AnimalId, FeedingTime, FoodType, Id);
    }
}