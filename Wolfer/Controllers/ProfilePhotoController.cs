using Microsoft.AspNetCore.Mvc;
using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;
using Wolfer.Services;

namespace Wolfer.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfilePhotoController : ControllerBase
{
    private IProfilePhotoService _profilePhotoService;

    public ProfilePhotoController(IProfilePhotoService profilePhotoService)
    {
        _profilePhotoService = profilePhotoService;
    }

    [HttpGet("GetByUserId/{userId}")]
    public async Task<IActionResult> GetByUserId(string userId)
    {
        try
        {
            ProfilePhotoEntity profilePhoto = await _profilePhotoService.GetPhotoByUserId(userId);
            return File(profilePhoto.Photo, profilePhoto.ContentType);
        }
        catch (Exception e)
        {
            return NotFound(new { message = e.Message });
        }
    }

    [HttpPost("Upload")]
    public async Task<IActionResult> Upload([FromForm] ProfilePhotoUploadDTO dto)
    {
        try
        {
            await _profilePhotoService.AddProfilePhoto(dto);
            return Ok(new { message = "Photo uploaded successfully." });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}