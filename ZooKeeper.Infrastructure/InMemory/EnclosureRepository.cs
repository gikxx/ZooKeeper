using ZooKeeper.Domain.Entities;
using ZooKeeper.Domain.Enums;
using ZooKeeper.Domain.Interfaces;
using ZooKeeper.Domain.ValueObjects;

namespace ZooKeeper.Infrastructure.InMemory;

public class EnclosureRepository : IEnclosureRepository
{
    private readonly Dictionary<EnclosureId, Enclosure> _enclosures = new();

    public Task<Enclosure?> GetByIdAsync(EnclosureId id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_enclosures.GetValueOrDefault(id));
    }

    public Task<IEnumerable<Enclosure>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<Enclosure>>(_enclosures.Values);
    }

    public Task AddAsync(Enclosure entity, CancellationToken cancellationToken = default)
    {
        _enclosures[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Enclosure entity, CancellationToken cancellationToken = default)
    {
        _enclosures[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(EnclosureId id, CancellationToken cancellationToken = default)
    {
        _enclosures.Remove(id);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Enclosure>> GetByTypeAsync(EnclosureType type, CancellationToken cancellationToken = default)
    {
        var enclosures = _enclosures.Values
            .Where(e => e.Type == type);
        return Task.FromResult(enclosures);
    }

    public Task<IEnumerable<Enclosure>> GetAvailableAsync(CancellationToken cancellationToken = default)
    {
        var available = _enclosures.Values
            .Where(e => e.CanAddAnimal());
        return Task.FromResult(available);
    }
}