using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;
using Wolfer.Repositories;

namespace Wolfer.Services;

public class UserService : IUserService
{
    private IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<IdentityUser?> GetById(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Invalid ID.");
        }

        return await _userRepository.GetUserById(userId);
    }

    public async Task<List<IdentityUser>> GetByUserIds(List<Guid> userIds)
    {
        var invalidIds = userIds.Where(id => id == Guid.Empty).ToList();
        
        if (invalidIds.Any())
        {
            _logger.LogWarning("Invalid IDs: {invalidIds}", string.Join(", ", invalidIds));
        }
        
        List<Guid> filteredUserIds = userIds.Where(userId => userId != Guid.Empty).ToList();
        
        return await _userRepository.GetByIds(filteredUserIds);
    }

    public async Task UpdateUser(UserDTO userDto)
    {
        if (String.IsNullOrEmpty(userDto.Email) || String.IsNullOrEmpty(userDto.UserName))
        {
            throw new ArgumentException("All properties must be filled out.");
        }
        
        IdentityUser newUserEntity = ConvertDtoToEntity(userDto);

        await _userRepository.UpdateUser(newUserEntity, userDto.OldPassword, userDto.NewPassword);
    }

    public async Task DeleteUser(Guid userId)
    {
        await _userRepository.DeleteUserById(userId);
    }

    private IdentityUser ConvertDtoToEntity(UserDTO userDto)
    {
        IdentityUser userEntity = new IdentityUser
        {
            UserName = userDto.UserName,
            Email = userDto.Email,
            Id = userDto.Id
        };
        return userEntity;
    }
}