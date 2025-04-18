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
    
    [HttpGet("GetUserById/{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            UserEntity userEntity = await _userService.GetById(id);
            return Ok(new { userEntity });
        }
        catch (Exception e)
        {
            return NotFound(new { message = e.Message });
        }
    }

    [HttpGet("GetUserByUserName/{userName}")]
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

    [HttpGet("GetUserByFirstName/{firstName}")]
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

    [HttpPost("AddUser")]
    public async Task<IActionResult> AddUser(UserDTO userDto)
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

    [HttpPut("UpdateUser")]
    public async Task<IActionResult> UpdateUser(UserDTO userDto)
    {
        try
        {
            await _userService.UpdateUser(userDto);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpDelete("DeleteUser/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            await _userService.DeleteUser(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}