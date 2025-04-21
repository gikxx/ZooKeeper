using Microsoft.AspNetCore.Mvc;
using ZooKeeper.Domain.Entities;
using ZooKeeper.Domain.Interfaces;
using ZooKeeper.Domain.ValueObjects;
using ZooKeeper.Presentation.Models.Enclosures;

namespace ZooKeeper.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnclosuresController : ControllerBase
{
    private readonly IEnclosureRepository _enclosureRepository;

    public EnclosuresController(IEnclosureRepository enclosureRepository)
    {
        _enclosureRepository = enclosureRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Enclosure>>> GetAll()
    {
        var enclosures = await _enclosureRepository.GetAllAsync();
        return Ok(enclosures);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Enclosure>> GetById(Guid id)
    {
        var enclosureId = new EnclosureId(id);
        var enclosure = await _enclosureRepository.GetByIdAsync(enclosureId);
        return enclosure is null ? NotFound() : Ok(enclosure);
    }

    [HttpPost]
    public async Task<ActionResult<Enclosure>> Create([FromBody] CreateEnclosureRequest request)
    {
        var enclosure = new Enclosure(
            new EnclosureId(Guid.NewGuid()),
            request.Type,
            new EnclosureCapacity(request.MaxCapacity));

        await _enclosureRepository.AddAsync(enclosure);
        return CreatedAtAction(nameof(GetById), new { id = enclosure.Id.Value }, enclosure);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var enclosureId = new EnclosureId(id);
        await _enclosureRepository.DeleteAsync(enclosureId);
        return NoContent();
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<Enclosure>>> GetAvailable()
    {
        var enclosures = await _enclosureRepository.GetAvailableAsync();
        return Ok(enclosures);
    }
}