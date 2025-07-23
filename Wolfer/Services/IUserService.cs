using Microsoft.AspNetCore.Identity;
using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;

namespace Wolfer.Services;

public interface IUserService
{
    public Task<IdentityUser?> GetById(Guid userId);
    public Task<List<IdentityUser>> GetByUserIds(List<Guid> userIds);
    public Task UpdateUser(UserInfoUpdateDTO userInfoUpdateDto);
    public Task ChangePassword(ChangePasswordDTO dto);
    public Task DeleteUser(Guid userId);
}