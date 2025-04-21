using ZooKeeper.Application.Models;
using ZooKeeper.Domain.Entities;
using ZooKeeper.Domain.Enums;
using ZooKeeper.Domain.Interfaces;
using ZooKeeper.Domain.ValueObjects;

namespace ZooKeeper.Application.Services;

public class ZooStatisticsService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IEnclosureRepository _enclosureRepository;
    private readonly IFeedingScheduleRepository _feedingScheduleRepository;

    public ZooStatisticsService(
        IAnimalRepository animalRepository,
        IEnclosureRepository enclosureRepository,
        IFeedingScheduleRepository feedingScheduleRepository)
    {
        _animalRepository = animalRepository;
        _enclosureRepository = enclosureRepository;
        _feedingScheduleRepository = feedingScheduleRepository;
    }

    public async Task<ZooStatistics> GetStatisticsAsync(CancellationToken cancellationToken = default)
    {
        var animals = await _animalRepository.GetAllAsync(cancellationToken);
        var enclosures = await _enclosureRepository.GetAllAsync(cancellationToken);
        var activeFeedings = await _feedingScheduleRepository.GetAllAsync(cancellationToken);

        return new ZooStatistics(
            TotalAnimals: animals.Count(),
            HealthyAnimals: animals.Count(a => a.Status == AnimalStatus.Healthy),
            SickAnimals: animals.Count(a => a.Status == AnimalStatus.Sick),
            TotalEnclosures: enclosures.Count(),
            AvailableEnclosures: enclosures.Count(e => e.CanAddAnimal()),
            ActiveFeedings: activeFeedings.Count()
        );
    }
}