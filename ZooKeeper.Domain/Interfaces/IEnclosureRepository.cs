using ZooKeeper.Domain.Entities;
using ZooKeeper.Domain.Enums;
using ZooKeeper.Domain.ValueObjects;

namespace ZooKeeper.Domain.Interfaces;

public interface IEnclosureRepository : IRepository<Enclosure, EnclosureId>
{
    Task<IEnumerable<Enclosure>> GetByTypeAsync(EnclosureType type, CancellationToken cancellationToken = default);
    Task<IEnumerable<Enclosure>> GetAvailableAsync(CancellationToken cancellationToken = default);
}