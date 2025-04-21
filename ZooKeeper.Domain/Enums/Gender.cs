using System.Text.Json.Serialization;

namespace ZooKeeper.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Gender
{
    Male,   // Мужской
    Female  // Женский
}