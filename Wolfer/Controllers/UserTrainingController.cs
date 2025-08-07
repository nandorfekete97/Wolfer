using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("GetUpcomingTrainingsByUserId/{userId}"), Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetUpcomingTrainingsByUserId(Guid userId)
    {
        try
        {
            List<TrainingEntity> trainingEntities = await _userTrainingService.GetFutureTrainingsByUserId(userId);
            return Ok(new { trainingEntities });
        }
        catch (Exception e)
        {
            return NotFound(new { message = e.Message });
        }
    }
    
    [HttpGet("GetPastTrainingsForUser/{userId}"), Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetPastTrainingsForUser(Guid userId)
    {
        try
        {
            List<TrainingEntity> trainingEntities = await _userTrainingService.GetPastTrainingsByUserId(userId);
            return Ok(new { trainingEntities });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("GetUsersByTrainingId/{trainingId}"), Authorize(Roles = "Admin,User")]
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

    [HttpPost("SignUserUpForTraining/users/{userId}/trainings/{trainingId}"), Authorize(Roles = "Admin,User")]
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

    [HttpDelete("SignUserOffFromTraining/users/{userId}/trainings/{trainingId}"), Authorize(Roles = "Admin,User")]
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