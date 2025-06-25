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
        List<UserTrainingEntity> userTrainingEntities = await _dbContext.UserTrainings.ToListAsync();

        List<UserTrainingEntity> result = new List<UserTrainingEntity>();
        
        for (int i = 0; i < userTrainingEntities.Count; i++)
        {
            if (userTrainingEntities[i].UserId == userId)
            {
                result.Add(userTrainingEntities[i]);
            }
        }

        return result;
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

    public async Task<bool> Delete(Guid userId, int trainingId)
    {
        var userTrainingToDelete =
            await _dbContext.UserTrainings.FirstOrDefaultAsync(entity =>
                entity.UserId == userId && entity.TrainingId == trainingId);

        if (userTrainingToDelete is not null)
        {
            _dbContext.Remove(userTrainingToDelete);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }
}