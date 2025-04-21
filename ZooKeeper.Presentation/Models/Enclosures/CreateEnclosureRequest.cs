using System.ComponentModel.DataAnnotations;
using ZooKeeper.Domain.Enums;

namespace ZooKeeper.Presentation.Models.Enclosures;

public record CreateEnclosureRequest(
    [Required(ErrorMessage = "Тип вольера обязателен")]
    EnclosureType Type,
    
    [Required(ErrorMessage = "Максимальная вместимость обязательна")]
    [Range(1, 100, ErrorMessage = "Вместимость должна быть от 1 до 100")] int MaxCapacity);