using Microsoft.AspNetCore.Mvc;
using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;
using Wolfer.Services;

namespace Wolfer.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonalRecordController : ControllerBase
{
    private IPersonalRecordService _personalRecordService;

    public PersonalRecordController(IPersonalRecordService personalRecordService)
    {
        _personalRecordService = personalRecordService;
    }

    [HttpGet("GetByUserId/{userId}")]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        try
        {
            List<PersonalRecordEntity> personalRecordEntities = await _personalRecordService.GetByUserId(userId);
            return Ok(new { personalRecordEntities });
        }
        catch (Exception e)
        {
            return NotFound(new { message = e.Message });
        }
    }

    [HttpPost("AddPersonalRecord")]
    public async Task<IActionResult> AddPersonalRecord([FromBody] PersonalRecordDTO personalRecordDto)
    {
        try
        {
            await _personalRecordService.AddPersonalRecord(personalRecordDto);
            return Ok(new { message = "Personal record created." });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPut("UpdatePersonalRecord")]
    public async Task<IActionResult> UpdatePersonalRecord([FromBody] PersonalRecordDTO personalRecordDto)
    {
        try
        {
            await _personalRecordService.UpdatePersonalRecord(personalRecordDto);
            return Ok(new { message = "Personal record updated." });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}