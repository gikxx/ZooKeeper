namespace ZooKeeper.Domain.ValueObjects;

public record AnimalId
{
    public Guid Value { get; }

    public AnimalId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Идентификатор животного не может быть пустым");
            
        Value = value;
    }

    public static AnimalId Create() => new(Guid.NewGuid());
}