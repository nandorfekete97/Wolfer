using Microsoft.AspNetCore.Identity;
using Wolfer.Data.Entities;

namespace Wolfer.Repositories;

public interface IUserRepository
{
    Task<IdentityUser?> GetUserById(Guid userId);
    Task<List<IdentityUser>> GetByIds(List<Guid> userIds);
    Task UpdateUser(IdentityUser user, string oldPassword, string newPassword);
    Task<bool> DeleteUserById(Guid userId);
}