namespace ZooKeeper.Domain.ValueObjects;

public record EnclosureId
{
    public Guid Value { get; }

    public EnclosureId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Идентификатор вольера не может быть пустым");
            
        Value = value;
    }

    public static EnclosureId Create() => new(Guid.NewGuid());
}