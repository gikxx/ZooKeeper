namespace ZooKeeper.Domain.ValueObjects;

public record AnimalName
{
    public string Value { get; }

    public AnimalName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Имя животного не может быть пустым");

        if (value.Length > 50)
            throw new ArgumentException("Имя животного не может быть длиннее 50 символов");

        Value = value;
    }
}