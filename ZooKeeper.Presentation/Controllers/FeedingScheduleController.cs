using Microsoft.AspNetCore.Mvc;
using ZooKeeper.Application.Services;
using ZooKeeper.Domain.Entities;
using ZooKeeper.Domain.Interfaces;
using ZooKeeper.Domain.ValueObjects;

namespace ZooKeeper.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeedingScheduleController : ControllerBase
{
    private readonly FeedingOrganizationService _feedingService;
    private readonly IFeedingScheduleRepository _scheduleRepository;

    public FeedingScheduleController(
        FeedingOrganizationService feedingService,
        IFeedingScheduleRepository scheduleRepository)
    {
        _feedingService = feedingService;
        _scheduleRepository = scheduleRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FeedingSchedule>>> GetToday()
    {
        var schedule = await _feedingService.GetTodayFeedingsAsync();
        return Ok(schedule);
    }

    [HttpPost("animals/{animalId}")]
    public async Task<IActionResult> OrganizeFeeding(Guid animalId)
    {
        await _feedingService.OrganizeFeedingAsync(new AnimalId(animalId));
        return Ok();
    }
}