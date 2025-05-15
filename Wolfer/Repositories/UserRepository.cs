using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wolfer.Data.Context;
using Wolfer.Data.Entities;

namespace Wolfer.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserRepository(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<IdentityUser?> GetUserById(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }
    
    public async Task<List<IdentityUser>> GetByIds(List<string> userIds)
    {
        return await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
    }

    public async Task UpdateUser(IdentityUser user, string oldPassword, string newPassword)
    {
        await _userManager.UpdateAsync(user);
        
        if (!String.IsNullOrEmpty(oldPassword) && !String.IsNullOrEmpty(newPassword) && oldPassword != newPassword)
        {
            await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }
    }

    public async Task<bool> DeleteUserById(string userId)
    {
        var userToDelete = await GetUserById(userId);

        if (userToDelete is not null)
        {
            await _userManager.DeleteAsync(userToDelete);
            return true;
        }

        return false;
    }
}