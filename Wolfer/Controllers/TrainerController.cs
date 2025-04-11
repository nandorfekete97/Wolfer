using Microsoft.AspNetCore.Mvc;
using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;
using Wolfer.Services;

namespace Wolfer.Controllers;

[ApiController]
[Route("[controller]")]
public class TrainerController : ControllerBase
{
    private ITrainerService _trainerService;

    public TrainerController(ITrainerService trainerService)
    {
        _trainerService = trainerService;
    }

    [HttpGet("GetTrainer")]
    public async Task<IActionResult> GetTrainerByUserName(string userName)
    {
        try
        {
            TrainerEntity trainerEntity = await _trainerService.GetByUserName(userName);
            return Ok(new { trainerEntity });
        }
        catch (Exception e)
        {
            return NotFound(new { message = e.Message });
        }
    }

    [HttpPost("AddTrainer")]
    public async Task<IActionResult> AddTrainer(TrainerDTO trainerDto)
    {
        try
        {
            await _trainerService.CreateTrainer(trainerDto);
            return Ok(new { message = "Trainer created."});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}