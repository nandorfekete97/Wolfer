using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;
using Wolfer.Repositories;

namespace Wolfer.Services;

public class UserService : IUserService
{
    private IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IdentityUser?> GetById(Guid userId)
    {
        if (String.IsNullOrEmpty(userId.ToString()))
        {
            throw new ArgumentException("ID must be positive integer.");
        }

        return await _userRepository.GetUserById(userId);
    }

    public async Task<List<IdentityUser>> GetByUserIds(List<Guid> userIds)
    {
        List<Guid> filteredUserIds = userIds.Where(userId => !String.IsNullOrEmpty(userId.ToString())).ToList();

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