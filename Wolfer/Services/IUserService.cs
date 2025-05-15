using Microsoft.AspNetCore.Identity;
using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;

namespace Wolfer.Services;

public interface IUserService
{
    public Task<IdentityUser?> GetById(string userId);
    public Task<List<IdentityUser>> GetByUserIds(List<string> userIds);
    public Task UpdateUser(UserDTO userDto);
    public Task DeleteUser(string userId);
}