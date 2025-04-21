using System.Text.Json.Serialization;

namespace ZooKeeper.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FoodType
{
    Meat,           // Мясо
    Fish,           // Рыба
    Vegetables,     // Овощи
    Fruits,         // Фрукты
    Insects,        // Насекомые
    SpecialFood     // Специальный корм
}