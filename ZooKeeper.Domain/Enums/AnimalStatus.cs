using System.Text.Json.Serialization;

namespace ZooKeeper.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AnimalStatus
{
    Healthy,    // Здоров
    Sick,       // Болен
    Quarantine, // На карантине
    Treatment   // На лечении
}