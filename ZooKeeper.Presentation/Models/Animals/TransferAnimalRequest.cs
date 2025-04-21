using System.ComponentModel.DataAnnotations;

namespace ZooKeeper.Presentation.Models.Animals;

public record TransferAnimalRequest(
    [Required(ErrorMessage = "Идентификатор вольера обязателен")]
    Guid EnclosureId);