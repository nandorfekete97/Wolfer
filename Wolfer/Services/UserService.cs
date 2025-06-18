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
    private readonly UserManager<IdentityUser> _userManager;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger, UserManager<IdentityUser> userManager)
    {
        _userRepository = userRepository;
        _logger = logger;
        _userManager = userManager;
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

    public async Task UpdateUser(UserInfoUpdateDTO userInfoUpdateDto)
    {
        if (string.IsNullOrEmpty(userInfoUpdateDto.Email) || string.IsNullOrEmpty(userInfoUpdateDto.UserName))
            throw new ArgumentException("All properties must be filled out.");

        var user = await _userManager.FindByIdAsync(userInfoUpdateDto.Id);
        if (user == null)
            throw new Exception("User not found.");

        user.Email = userInfoUpdateDto.Email;
        user.UserName = userInfoUpdateDto.UserName;

        await _userRepository.UpdateUser(user);
    }

    // password change will be in different method for clean responsibilities
    // public async Task ChangePassword(ChangePasswordDTO dto)
    // {
    //     var user = await _userManager.FindByIdAsync(dto.UserId)
    //                ?? throw new Exception("User not found.");
    //
    //     if (dto.OldPassword == dto.NewPassword)
    //         throw new Exception("New password must be different from the old password.");
    //
    //     var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
    //     if (!result.Succeeded)
    //         throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
    // }

    public async Task DeleteUser(Guid userId)
    {
        await _userRepository.DeleteUserById(userId);
    }

    private IdentityUser ConvertDtoToEntity(UserInfoUpdateDTO userInfoUpdateDto)
    {
        IdentityUser userEntity = new IdentityUser
        {
            UserName = userInfoUpdateDto.UserName,
            Email = userInfoUpdateDto.Email,
            Id = userInfoUpdateDto.Id
        };
        return userEntity;
    }
}