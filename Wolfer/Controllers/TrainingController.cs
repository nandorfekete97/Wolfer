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

    [HttpGet("GetTrainingById/{id}")]
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
    
    [HttpGet("GetTrainingsForUser/{userId}")]
    public async Task<IActionResult> GetTrainingsForUser(string userId)
    {
        try
        {
            var trainings = await _userTrainingService.GetByUserId(userId);
            return Ok(trainings);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("GetTrainingsByDate/{date}")]
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
    
    [HttpGet("GetTrainingsByType/{type}")]
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

    [HttpPost("AddTraining")]
    public async Task<IActionResult> AddTraining([FromBody] TrainingDTO trainingDto)
    {
        try
        {
            await _trainingService.CreateTraining(trainingDto);
            Console.WriteLine("trainingDto.Date: " + trainingDto.Date);
            Console.WriteLine("trainingDto.TrainingType: " + trainingDto.TrainingType);
            return Ok(new { message = "Training created."});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPut("UpdateTraining")]
    public async Task<IActionResult> UpdateTraining(TrainingDTO trainingDto)
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

    [HttpDelete("DeleteTraining/{id}")]
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
}