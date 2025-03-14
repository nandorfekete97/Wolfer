using Microsoft.EntityFrameworkCore;
using Wolfer.Data;
using Wolfer.Data.Context;
using Wolfer.Data.Entities;

namespace Wolfer.Repositories;

public class TrainingRepository : ITrainingRepository
{
    private WolferContext _dbContext;

    public TrainingRepository(WolferContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<TrainingEntity> GetById(int id)
    {
        return await _dbContext.Trainings.FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public async Task<List<TrainingEntity>> GetTrainingsByDate(DateOnly date)
    {

        return await _dbContext.Trainings.Where(entity => date.CompareTo(entity.Date.Date) == 0).ToListAsync();
    }

    public async Task<List<TrainingEntity>> GetTrainingsByType(TrainingType trainingType)
    {
        return await _dbContext.Trainings.Where(entity => entity.TrainingType == trainingType).ToListAsync();
    }
}