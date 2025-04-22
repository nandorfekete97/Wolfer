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
    
    public async Task<List<UserTrainingEntity>> GetByUserId(int userId)
    {
        return await _dbContext.UserTrainings.Where(entity => entity.UserId == userId).ToListAsync();
    }

    public async Task<List<UserTrainingEntity>> GetByTrainingId(int trainingId)
    {
        return await _dbContext.UserTrainings.Where(entity => entity.TrainingId == trainingId).ToListAsync();
    }
}