using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;
using Wolfer.Services;

namespace Wolfer.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("GetUserById/{userId}"), Authorize(Roles="User")]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        try
        {
            IdentityUser userEntity = await _userService.GetById(userId);
            return Ok(new { userEntity });
        }
        catch (Exception e)
        {
            return NotFound(new { message = e.Message });
        }
    }
    
    [HttpPut("UpdateUser"), Authorize(Roles="User")]
    public async Task<IActionResult> UpdateUser([FromBody] UserInfoUpdateDTO userInfoUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Model binding failed", errors = ModelState });
        }
        try
        {
            await _userService.UpdateUser(userInfoUpdateDto);
            return Ok(new { message = "User updated."});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpDelete("DeleteUser/{id}"), Authorize(Roles="User")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        try
        {
            await _userService.DeleteUser(userId);
            return Ok(new { message = "User deleted."});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPut("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Model binding failed", errors = ModelState });
        }
        try
        {
            await _userService.ChangePassword(changePasswordDto);
            return Ok(new { message = "Password changed."});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}