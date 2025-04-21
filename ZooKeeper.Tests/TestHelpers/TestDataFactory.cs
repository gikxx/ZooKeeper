using ZooKeeper.Domain.Entities;
using ZooKeeper.Domain.Enums;
using ZooKeeper.Domain.ValueObjects;

namespace ZooKeeper.Tests.TestHelpers;

public static class TestDataFactory
{
    public static Animal CreateAnimal(
        string species = "TestSpecies",
        AnimalStatus status = AnimalStatus.Healthy,
        Gender gender = Gender.Male,
        DateTime? dateOfBirth = null)
    {
        return new Animal(
            AnimalId.Create(),
            new AnimalName("TestAnimal"),
            species,
            dateOfBirth ?? DateTime.UtcNow.AddYears(-1),
            gender,
            FoodType.Meat
        );
    }

    public static Enclosure CreateEnclosure(
        EnclosureType type = EnclosureType.Predator,
        int capacity = 5)
    {
        return new Enclosure(
            EnclosureId.Create(),
            type,
            new EnclosureCapacity(capacity)
        );
    }

    public static FeedingSchedule CreateFeedingSchedule(
        Animal animal,
        DateTime? feedingTime = null)
    {
        return new FeedingSchedule(
            FeedingScheduleId.Create(),
            animal.Id,
            feedingTime ?? DateTime.UtcNow.AddHours(1),
            FoodType.Meat
        );
    }
}