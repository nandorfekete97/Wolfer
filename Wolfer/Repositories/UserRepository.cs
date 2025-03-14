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
        var userToDelete = _dbContext.Users.FirstOrDefaultAsync(userEntity => userEntity.Id == userId);

        if (userToDelete is not null)
        {
            _dbContext.Remove(userToDelete);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }
}