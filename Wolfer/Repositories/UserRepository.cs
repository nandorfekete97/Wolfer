using Microsoft.EntityFrameworkCore;
using Wolfer.Data.Context;
using Wolfer.Data.Entities;

namespace Wolfer.Repositories;

public class UserRepository : IUserRepository
{
    private WolferContext _dbContext;

    public UserRepository(WolferContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<UserEntity?> GetUserById(int userId)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(userEntity => userEntity.Id == userId);
    }

    public async Task<UserEntity?> GetUserByFirstName(string firstName)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(userEntity => userEntity.FirstName == firstName);
    }

    public async Task<UserEntity?> GetUserByUserName(string userName)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(userEntity => userEntity.Username == userName);
    }

    public async Task<List<UserEntity>> GetAllUsers()
    {
        return await _dbContext.Users.ToListAsync();
    }
    
    public async Task<List<UserEntity>> GetByIds(List<int> userIds)
    {
        return await _dbContext.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();
    }

    public async Task CreateUser(UserEntity user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateUser(UserEntity user)
    {
        _dbContext.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteUserById(int userId)
    {
        var userToDelete = await _dbContext.Users.FirstOrDefaultAsync(userEntity => userEntity.Id == userId);

        if (userToDelete is not null)
        {
            _dbContext.Remove(userToDelete);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> IsUserPresent(int userId)
    {
        return _dbContext.Users.Any(entity => entity.Id == userId);
    }
}