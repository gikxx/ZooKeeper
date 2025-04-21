using Microsoft.AspNetCore.Mvc;
using ZooKeeper.Application.Services;
using ZooKeeper.Domain.Entities;
using ZooKeeper.Domain.Interfaces;
using ZooKeeper.Domain.ValueObjects;
using ZooKeeper.Presentation.Models.Animals;

namespace ZooKeeper.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly AnimalTransferService _transferService;
    private readonly IAnimalRepository _animalRepository;

    public AnimalsController(
        AnimalTransferService transferService,
        IAnimalRepository animalRepository)
    {
        _transferService = transferService;
        _animalRepository = animalRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Animal>>> GetAll()
    {
        var animals = await _animalRepository.GetAllAsync();
        return Ok(animals);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Animal>> GetById(Guid id)
    {
        var animalId = new AnimalId(id);
        var animal = await _animalRepository.GetByIdAsync(animalId);
        return animal is null ? NotFound() : Ok(animal);
    }

    [HttpPost]
    public async Task<ActionResult<Animal>> Create([FromBody] CreateAnimalRequest request)
    {
        var animal = new Animal(
            new AnimalId(Guid.NewGuid()),
            new AnimalName(request.Name),
            request.Species,
            request.DateOfBirth,
            request.Gender,
            request.FavoriteFood);

        await _animalRepository.AddAsync(animal);
        return CreatedAtAction(nameof(GetById), new { id = animal.Id.Value }, animal);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _animalRepository.DeleteAsync(new AnimalId(id));
        return NoContent();
    }

    [HttpPost("{id}/transfer")]
    public async Task<IActionResult> TransferToEnclosure(Guid id, [FromBody] TransferAnimalRequest request)
    {
        try
        {
            await _transferService.TransferAnimalAsync(
                new AnimalId(id),
                new EnclosureId(request.EnclosureId));
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}