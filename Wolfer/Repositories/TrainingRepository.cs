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
    
    public async Task<List<TrainingEntity>> GetByIds(List<int> trainingIds)
    {
        return await _dbContext.Trainings
            .Where(t => trainingIds.Contains(t.Id))
            .ToListAsync();
    }
    
    public async Task CreateTraining(TrainingEntity trainingEntity)
    {
        await _dbContext.Trainings.AddAsync(trainingEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task CreateTrainings(List<TrainingEntity> trainings)
    {
        await _dbContext.Trainings.AddRangeAsync(trainings);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateTraining(TrainingEntity trainingEntity)
    {
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

    public async Task<bool> DeleteTrainingsByDate(DateOnly date)
    {
        DateTime dayStart = date.ToDateTime(TimeOnly.MinValue);
        DateTime dayEnd = date.ToDateTime(TimeOnly.MaxValue);
        
        var trainingsToDelete =
            await _dbContext.Trainings.Where(training => training.Date >= dayStart && training.Date <= dayEnd).ToListAsync();

        if (trainingsToDelete.Any())
        {
            _dbContext.RemoveRange(trainingsToDelete);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }
}