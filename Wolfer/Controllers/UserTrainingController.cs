using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wolfer.Data.Entities;
using Wolfer.Services;

namespace Wolfer.Controllers;

[ApiController]
[Route(("[controller]"))]
public class UserTrainingController : ControllerBase
{
    private IUserTrainingService _userTrainingService;

    public UserTrainingController(IUserTrainingService userTrainingService)
    {
        _userTrainingService = userTrainingService;
    }

    [HttpGet("GetUpcomingTrainingsByUserId/{userId}")]
    public async Task<IActionResult> GetUpcomingTrainingsByUserId(string userId)
    {
        try
        {
            List<TrainingEntity> trainingEntities = await _userTrainingService.GetByUserId(userId);
            return Ok(new { trainingEntities });
        }
        catch (Exception e)
        {
            return NotFound(new { message = e.Message });
        }
    }

    [HttpGet("GetUsersByTrainingId/{trainingId}")]
    public async Task<IActionResult> GetUsersByTrainingId(int trainingId)
    {
        try
        {
            List<IdentityUser> userEntities = await _userTrainingService.GetByTrainingId(trainingId);
            return Ok(new { userEntities });
        }
        catch (Exception e)
        {
            return NotFound(new { message = e.Message });
        }
    }
    
    [HttpGet("GetPastTrainingsForUser/{userId}")]
    public async Task<IActionResult> GetPastTrainingsForUser(string userId)
    {
        try
        {
            var trainings = await _userTrainingService.GetPastTrainingsByUserId(userId);
            return Ok(trainings);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("SignUserUpForTraining/users/{userId}/trainings/{trainingId}")]
    public async Task<IActionResult> SignUserUpForTraining(Guid userId, int trainingId)
    {
        try
        {
            await _userTrainingService.SignUpUserToTraining(userId, trainingId);
            return Ok(new { message = "UserTraining created" });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpDelete("SignUserOffFromTraining/users/{userId}/trainings/{trainingId}")]
    public async Task<IActionResult> SignUserOffFromTraining(Guid userId, int trainingId)
    {
        try
        {
            await _userTrainingService.SignOffUserFromTraining(userId, trainingId);
            return Ok(new { message = "UserTraining deleted" });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}