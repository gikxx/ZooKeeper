namespace ZooKeeper.Domain.ValueObjects;

public record EnclosureCapacity
{
    public int Value { get; }

    public EnclosureCapacity(int value)
    {
        if (value <= 0)
            throw new ArgumentException("Вместимость вольера должна быть положительным числом");
            
        if (value > 100)
            throw new ArgumentException("Вместимость вольера не может превышать 100");
            
        Value = value;
    }
}