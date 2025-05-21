using Microsoft.AspNetCore.Identity;
using Wolfer.Data.Entities;

namespace Wolfer.Repositories;

public interface IUserRepository
{
    Task<IdentityUser?> GetUserById(string userId);
    Task<List<IdentityUser>> GetByIds(List<string> userIds);
    Task UpdateUser(IdentityUser user, string oldPassword, string newPassword);
    Task<bool> DeleteUserById(string userId);
}