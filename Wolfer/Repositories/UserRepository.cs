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
    
    public async Task<IdentityUser?> GetUserById(Guid userId)
    {
        return await _userManager.FindByIdAsync(userId.ToString());
    }
    
    public async Task<List<IdentityUser>> GetByIds(List<Guid> userIds)
    {
        List<string> userIdsConverted = userIds.Select(id => id.ToString()).ToList();
        return await _userManager.Users.Where(u => userIdsConverted.Contains(u.Id)).ToListAsync();
    }

    public async Task UpdateUser(IdentityUser user)
    {
        var result = await _userManager.UpdateAsync(user);
    
        if (!result.Succeeded)
            throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
    }
    
    public async Task<bool> DeleteUserById(Guid userId)
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