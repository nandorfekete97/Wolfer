using Microsoft.AspNetCore.Identity;
using Wolfer.Data.Entities;

namespace Wolfer.Repositories;

public interface IUserRepository
{
    Task<IdentityUser?> GetUserById(Guid userId);
    Task<List<IdentityUser>> GetByIds(List<Guid> userIds);
    Task UpdateUser(IdentityUser user);
    Task<bool> DeleteUserById(Guid userId);
}