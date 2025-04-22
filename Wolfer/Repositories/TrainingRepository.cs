using Microsoft.EntityFrameworkCore;
using Wolfer.Data;
using Wolfer.Data.Context;
using Wolfer.Data.DTOs;
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
        return await _dbContext.Trainings
            .Where(entity => DateOnly.FromDateTime(entity.Date.Date) == date)
            .ToListAsync();
    }

    public async Task<List<TrainingEntity>> GetTrainingsByType(TrainingType trainingType)
    {
        return await _dbContext.Trainings.Where(entity => entity.TrainingType == trainingType).ToListAsync();
    }
    
    public async Task CreateTraining(TrainingEntity trainingEntity)
    {
        await _dbContext.Trainings.AddAsync(trainingEntity);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task UpdateTraining(TrainingEntity trainingEntity)
    {
        _dbContext.Update(trainingEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteById(int trainingId)
    {
        var trainingToDelete = await _dbContext.Trainings.FirstOrDefaultAsync(entity => entity.Id == trainingId);

        if (trainingToDelete is not null)
        {
            _dbContext.Remove(trainingToDelete);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }
}