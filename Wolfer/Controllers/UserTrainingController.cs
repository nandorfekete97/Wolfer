using Microsoft.AspNetCore.Http.HttpResults;
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

    [HttpGet("GetTrainingsByUserId/{userId}")]
    public async Task<IActionResult> GetTrainingsByUserId(int userId)
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
            List<UserEntity> userEntities = await _userTrainingService.GetByTrainingId(trainingId);
            return Ok(new { userEntities });
        }
        catch (Exception e)
        {
            return NotFound(new { message = e.Message });
        }
    }
}