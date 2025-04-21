using ZooKeeper.Domain.ValueObjects;

namespace ZooKeeper.Domain.Events;

public record AnimalMovedEvent
{
    public AnimalId AnimalId { get; }
    public EnclosureId? FromEnclosureId { get; }
    public EnclosureId ToEnclosureId { get; }
    public DateTime MovedAt { get; }

    public AnimalMovedEvent(
        AnimalId animalId,
        EnclosureId? fromEnclosureId,
        EnclosureId toEnclosureId)
    {
        AnimalId = animalId;
        FromEnclosureId = fromEnclosureId;
        ToEnclosureId = toEnclosureId;
        MovedAt = DateTime.UtcNow;
    }
}