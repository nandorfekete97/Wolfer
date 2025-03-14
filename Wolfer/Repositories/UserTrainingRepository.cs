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
    
    public async Task<UserTrainingsEntity> GetByUserId(int userId)
    {
        return await _dbContext.UserTrainingsEnumerable.FirstOrDefaultAsync(entity => entity.UserId == userId);
    }

    public async Task<UserTrainingsEntity> GetByTrainingId(int trainingId)
    {
        return await _dbContext.UserTrainingsEnumerable.FirstOrDefaultAsync(entity => entity.TrainingId == trainingId);
    }
    
    
}