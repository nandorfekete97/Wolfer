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
    
    [HttpGet("GetUserById/{id}"), Authorize(Roles="User")]
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
    public async Task<IActionResult> UpdateUser(UserDTO userDto)
    {
        try
        {
            await _userService.UpdateUser(userDto);
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
}