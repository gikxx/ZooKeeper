using ZooKeeper.Domain.Entities;
using ZooKeeper.Domain.Enums;
using ZooKeeper.Domain.Interfaces;
using ZooKeeper.Domain.ValueObjects;

namespace ZooKeeper.Infrastructure.InMemory;

public class AnimalRepository : IAnimalRepository
{
    private readonly Dictionary<AnimalId, Animal> _animals = new();

    public Task<Animal?> GetByIdAsync(AnimalId id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_animals.GetValueOrDefault(id));
    }

    public Task<IEnumerable<Animal>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<Animal>>(_animals.Values);
    }

    public Task AddAsync(Animal entity, CancellationToken cancellationToken = default)
    {
        _animals[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Animal entity, CancellationToken cancellationToken = default)
    {
        _animals[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(AnimalId id, CancellationToken cancellationToken = default)
    {
        _animals.Remove(id);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Animal>> GetByEnclosureIdAsync(EnclosureId enclosureId, CancellationToken cancellationToken = default)
    {
        var animals = _animals.Values
            .Where(a => a.CurrentEnclosureId == enclosureId);
        return Task.FromResult(animals);
    }

    public Task<IEnumerable<Animal>> GetByStatusAsync(AnimalStatus status, CancellationToken cancellationToken = default)
    {
        var animals = _animals.Values
            .Where(a => a.Status == status);
        return Task.FromResult(animals);
    }
}