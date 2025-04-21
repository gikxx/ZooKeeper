using ZooKeeper.Domain.Enums;
using ZooKeeper.Domain.Events;
using ZooKeeper.Domain.Interfaces;
using ZooKeeper.Domain.ValueObjects;

namespace ZooKeeper.Application.Services;

public class AnimalTransferService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IEnclosureRepository _enclosureRepository;
    private readonly IDomainEventPublisher _eventPublisher;

    public AnimalTransferService(
        IAnimalRepository animalRepository,
        IEnclosureRepository enclosureRepository,
        IDomainEventPublisher eventPublisher)
    {
        _animalRepository = animalRepository;
        _enclosureRepository = enclosureRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task TransferAnimalAsync(AnimalId animalId, EnclosureId targetEnclosureId, CancellationToken cancellationToken = default)
    {
        var animal = await _animalRepository.GetByIdAsync(animalId, cancellationToken)
                     ?? throw new InvalidOperationException("Животное не найдено");

        var targetEnclosure = await _enclosureRepository.GetByIdAsync(targetEnclosureId, cancellationToken)
                              ?? throw new InvalidOperationException("Целевой вольер не найден");
        
        if (targetEnclosure.Type == EnclosureType.Herbivore && animal.Species == "Predator")
            throw new InvalidOperationException("Хищник не может быть помещён в вольер для травоядных");

        if (!targetEnclosure.CanAddAnimal())
            throw new InvalidOperationException("Целевой вольер переполнен");

        var sourceEnclosureId = animal.CurrentEnclosureId;

        if (sourceEnclosureId != null)
        {
            var sourceEnclosure = await _enclosureRepository.GetByIdAsync(sourceEnclosureId, cancellationToken);
            sourceEnclosure?.RemoveAnimal(animalId);
            await _enclosureRepository.UpdateAsync(sourceEnclosure!, cancellationToken);
        }

        targetEnclosure.AddAnimal(animalId);
        var movedEvent = animal.MoveToEnclosure(targetEnclosureId);

        await _enclosureRepository.UpdateAsync(targetEnclosure, cancellationToken);
        await _animalRepository.UpdateAsync(animal, cancellationToken);
        await _eventPublisher.PublishAsync(movedEvent, cancellationToken);
    }
}