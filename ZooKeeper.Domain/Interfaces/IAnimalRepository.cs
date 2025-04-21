using ZooKeeper.Domain.Entities;
using ZooKeeper.Domain.Enums;
using ZooKeeper.Domain.ValueObjects;

namespace ZooKeeper.Domain.Interfaces;

public interface IAnimalRepository : IRepository<Animal, AnimalId>
{
    Task<IEnumerable<Animal>> GetByEnclosureIdAsync(EnclosureId enclosureId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Animal>> GetByStatusAsync(AnimalStatus status, CancellationToken cancellationToken = default);
}