using ZooKeeper.Domain.Enums;
using ZooKeeper.Domain.ValueObjects;

namespace ZooKeeper.Domain.Entities;

public class Enclosure
{
    private readonly HashSet<AnimalId> _animals = new();

    public EnclosureId Id { get; private set; }
    public EnclosureType Type { get; private set; }
    public EnclosureCapacity Capacity { get; private set; }
    public IReadOnlyCollection<AnimalId> Animals => _animals;
    private bool _isClean = true;
    private DateTime _lastCleaningTime = DateTime.MinValue;

    private Enclosure() { }

    public Enclosure(
        EnclosureId id,
        EnclosureType type,
        EnclosureCapacity capacity)
    {
        Id = id;
        Type = type;
        Capacity = capacity;
    }

    public bool CanAddAnimal() => _animals.Count < Capacity.Value;

    public void AddAnimal(AnimalId animalId)
    {
        if (!CanAddAnimal())
            throw new InvalidOperationException("Вольер переполнен");

        _animals.Add(animalId);
    }

    public void RemoveAnimal(AnimalId animalId)
    {
        if (!_animals.Contains(animalId))
            throw new InvalidOperationException("Животное не найдено в вольере");

        _animals.Remove(animalId);
    }


    public void Clean()
    {
        if (_animals.Count == 0)
            throw new InvalidOperationException("Нет необходимости убирать пустой вольер");

        if (_isClean && DateTime.UtcNow - _lastCleaningTime < TimeSpan.FromHours(12))
            throw new InvalidOperationException("Вольер уже чистый и не требует уборки");

        // Выполняем уборку
        _isClean = true;
        _lastCleaningTime = DateTime.UtcNow;
    }

    public bool NeedsCleaning()
    {
        return !_isClean || DateTime.UtcNow - _lastCleaningTime >= TimeSpan.FromHours(12);
    }

    public void MarkDirty()
    {
        _isClean = false;
    }
}