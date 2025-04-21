namespace ZooKeeper.Domain.ValueObjects;

public record FeedingScheduleId
{
    public Guid Value { get; }

    public FeedingScheduleId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Идентификатор расписания не может быть пустым");
            
        Value = value;
    }

    public static FeedingScheduleId Create() => new(Guid.NewGuid());
}