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
    
    [HttpGet("GetTrainerById/{id}")]
    public async Task<IActionResult> GetTrainerById(int id)
    {
        try
        {
            TrainerEntity trainerEntity = await _trainerService.GetById(id);
            return Ok(new { trainerEntity });
        }
        catch (Exception e)
        {
            return NotFound(new { message = e.Message });
        }
    }

    [HttpGet("GetTrainerByUserName/{userName}")]
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

    [HttpGet("GetTrainerByFirstName/{firstName}")]
    public async Task<IActionResult> GetTrainerByFirstName(string firstName)
    {
        try
        {
            TrainerEntity trainerEntity = await _trainerService.GetByFirstName(firstName);
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

    [HttpPut("UpdateTrainer")]
    public async Task<IActionResult> UpdateTrainer(TrainerDTO trainerDto)
    {
        try
        {
            await _trainerService.UpdateTrainer(trainerDto);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpDelete("DeleteTrainer/{id}")]
    public async Task<IActionResult> DeleteTrainer(int id)
    {
        try
        {
            await _trainerService.DeleteTrainer(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}