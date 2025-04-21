using ZooKeeper.Domain.Enums;
using ZooKeeper.Domain.Events;
using ZooKeeper.Domain.ValueObjects;

namespace ZooKeeper.Domain.Entities;

public class Animal
{
    public AnimalId Id { get; private set; }
    public AnimalName Name { get; private set; }
    public string Species { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public Gender Gender { get; private set; }
    public FoodType FavoriteFood { get; private set; }
    public AnimalStatus Status { get; private set; }
    public EnclosureId? CurrentEnclosureId { get; private set; }

    private Animal() { }

    public Animal(
        AnimalId id,
        AnimalName name,
        string species,
        DateTime dateOfBirth,
        Gender gender,
        FoodType favoriteFood)
    {
        Id = id;
        Name = name;
        Species = species;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        FavoriteFood = favoriteFood;
    }

    public AnimalMovedEvent MoveToEnclosure(EnclosureId newEnclosureId)
    {
        var oldEnclosureId = CurrentEnclosureId;
        CurrentEnclosureId = newEnclosureId;

        return new AnimalMovedEvent(Id, oldEnclosureId, newEnclosureId);
    }
    
    public void Heal()
    {
        if (Status != AnimalStatus.Sick)
            throw new InvalidOperationException("Животное не нуждается в лечении");
        
        Status = AnimalStatus.Healthy;
    }
    
    public void UpdateStatus(AnimalStatus newStatus)
    {
        Status = newStatus;
    }

    public FeedingTimeEvent Feed()
    {
        if (Status == AnimalStatus.Sick)
            throw new InvalidOperationException("Нельзя кормить больное животное без назначения врача");

        if (Status == AnimalStatus.Treatment)
            throw new InvalidOperationException("Животное на лечении, требуется особый режим кормления");

        if (Status == AnimalStatus.Quarantine)
            throw new InvalidOperationException("Животное на карантине, требуется особый режим кормления");

        return new FeedingTimeEvent(
            Id, 
            DateTime.UtcNow, 
            FavoriteFood,
            FeedingScheduleId.Create()
        );
    }

    public int GetAge()
    {
        return (int)((DateTime.UtcNow - DateOfBirth).TotalDays / 365);
    }
}