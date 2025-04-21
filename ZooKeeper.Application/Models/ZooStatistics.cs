namespace ZooKeeper.Application.Models;

public record ZooStatistics(
    int TotalAnimals,
    int HealthyAnimals,
    int SickAnimals,
    int TotalEnclosures,
    int AvailableEnclosures,
    int ActiveFeedings);