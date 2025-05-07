using Microsoft.AspNetCore.Authorization;
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
    public async Task<IActionResult> GetUserById(Guid id)
    {
        try
        {
            UserEntity userEntity = await _userService.GetById(1);
            return Ok(new { userEntity });
        }
        catch (Exception e)
        {
            return NotFound(new { message = e.Message });
        }
    }

    [HttpGet("GetUserByUserName/{userName}"), Authorize(Roles="User")]
    public async Task<IActionResult> GetUserByUserName(string userName)
    {
        try
        {
            UserEntity userEntity = await _userService.GetByUserName(userName);
            return Ok(new { userEntity });
        }
        catch (Exception e)
        {
            return NotFound(new { message = e.Message });
        }
    }

    [HttpGet("GetUserByFirstName/{firstName}"), Authorize(Roles="User")]
    public async Task<IActionResult> GetUserByFirstName(string firstName)
    {
        try
        {
            UserEntity userEntity = await _userService.GetByFirstName(firstName);
            return Ok(new { userEntity });
        }
        catch (Exception e)
        {
            return NotFound(new { message = e.Message });
        }
    }

    [HttpPost("AddUser"), Authorize(Roles="User")]
    public async Task<IActionResult> AddUser([FromBody] UserDTO userDto)
    {
        try
        {
            await _userService.CreateUser(userDto);
            return Ok(new { message = "User created."});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
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
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            await _userService.DeleteUser(1);
            return Ok(new { message = "User deleted."});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}