using System.ComponentModel.DataAnnotations;
using ZooKeeper.Domain.Enums;

namespace ZooKeeper.Presentation.Models.Animals;

public record CreateAnimalRequest(
    [Required(ErrorMessage = "Имя животного обязательно")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Длина имени должна быть от 2 до 100 символов")]
    string Name,
    
    [Required(ErrorMessage = "Вид животного обязателен")]
    string Species,
    
    [Required(ErrorMessage = "Дата рождения обязательна")]
    DateTime DateOfBirth,
    
    [Required(ErrorMessage = "Пол животного обязателен")]
    Gender Gender,
    
    [Required(ErrorMessage = "Любимая еда обязательна")]
    FoodType FavoriteFood);