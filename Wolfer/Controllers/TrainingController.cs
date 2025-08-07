using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wolfer.Data;
using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;
using Wolfer.Services;

namespace Wolfer.Controllers;

[ApiController]
[Route("[controller]")]
public class TrainingController : ControllerBase
{
    private ITrainingService _trainingService;
    private IUserTrainingService _userTrainingService;

    public TrainingController(ITrainingService trainingService, IUserTrainingService userTrainingService)
    {
        _trainingService = trainingService;
        _userTrainingService = userTrainingService;
    }

    [HttpGet("GetTrainingById/{id}"), Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetTrainingById(int id)
    {
        try
        {
            TrainingEntity trainingEntity = await _trainingService.GetById(id);
            return Ok(new { trainingEntity });
        }
        catch (Exception e)
        {
            return NotFound(new { message = e.Message });
        }
    }
    
    [HttpGet("GetTrainingsForUser/{userId}"), Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetTrainingsForUser(Guid userId)
    {
        try
        {
            var trainings = await _userTrainingService.GetFutureTrainingsByUserId(userId);
            return Ok(trainings);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("GetTrainingsByDate/{date}"), Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetTrainingsByDate(DateOnly date)
    {
        try
        {
            List<TrainingEntity> trainingEntities = await _trainingService.GetTrainingsByDate(date);
            return Ok(new { trainingEntities });
        }
        catch (Exception e)
        {
            return NotFound(new { message = e.Message });
        }
    }
    
    [HttpGet("GetTrainingsByType/{type}"), Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetTrainingsByType(TrainingType trainingType)
    {
        try
        {
            List<TrainingEntity> trainingEntities = await _trainingService.GetTrainingsByType(trainingType);
            return Ok(new { trainingEntities });
        }
        catch (Exception e)
        {
            return NotFound(new { message = e.Message });
        }
    }

    [HttpPost("AddTraining"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddTraining([FromBody] TrainingDTO trainingDto)
    {
        try
        {
            await _trainingService.CreateTraining(trainingDto);
            return Ok(new { message = "Training created."});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPost("AddTrainings"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddTrainings([FromBody] List<TrainingDTO> trainerDtos)
    {
        try
        {
            await _trainingService.CreateTrainings(trainerDtos);
            return Ok(new { message = "Trainings created." });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPut("UpdateTraining"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateTraining([FromBody] TrainingDTO trainingDto)
    {
        try
        {
            await _trainingService.UpdateTraining(trainingDto);
            return Ok(new { message = "Training updated."});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpDelete("DeleteTraining/{id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTraining(int id)
    {
        try
        {
            await _trainingService.DeleteTraining(id);
            return Ok(new { message = "Training deleted."});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpDelete("DeleteTrainingsByDate/{date}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTrainingsByDate(DateOnly date)
    {
        try
        {
            await _trainingService.DeleteTrainingsByDate(date);
            return Ok(new { message = "Trainings deleted."});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}