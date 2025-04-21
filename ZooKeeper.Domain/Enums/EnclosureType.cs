using System.Text.Json.Serialization;

namespace ZooKeeper.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EnclosureType
{
    Predator,   // Для хищников
    Herbivore,  // Для травоядных
    Aviary,     // Птичий вольер
    Aquarium,   // Аквариум
    Terrarium,  // Террариум
    Insectarium // Инсектарий
}