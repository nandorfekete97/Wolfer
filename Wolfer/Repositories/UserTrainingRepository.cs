using Microsoft.EntityFrameworkCore;
using Wolfer.Data.Context;
using Wolfer.Data.Entities;

namespace Wolfer.Repositories;

public class UserTrainingRepository : IUserTrainingRepository
{
    private WolferContext _dbContext;

    public UserTrainingRepository(WolferContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<UserTrainingEntity>> GetByUserId(Guid userId)
    {
        return await _dbContext.UserTrainings.Where(entity => entity.UserId == userId).ToListAsync();
    }

    public async Task<List<UserTrainingEntity>> GetByTrainingId(int trainingId)
    {
        return await _dbContext.UserTrainings.Where(entity => entity.TrainingId == trainingId).ToListAsync();
    }

    public async Task<UserTrainingEntity> GetByUserIdAndTrainingId(Guid userId, int trainingId)
    {
        return await _dbContext.UserTrainings.FirstOrDefaultAsync(
            entity => entity.UserId == userId && entity.TrainingId == trainingId);
    }

    public async Task Create(UserTrainingEntity userTrainingEntity)
    {
        if (userTrainingEntity == null)
        {
            throw new ArgumentNullException(nameof(userTrainingEntity));
        }

        await _dbContext.UserTrainings.AddAsync(userTrainingEntity);
        await _dbContext.SaveChangesAsync();
    }
}