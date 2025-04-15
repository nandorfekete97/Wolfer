using Microsoft.EntityFrameworkCore;
using Wolfer.Data.Context;
using Wolfer.Data.Entities;

namespace Wolfer.Repositories;

public class TrainerRepository : ITrainerRepository
{
    private WolferContext _dbContext;

    public TrainerRepository(WolferContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<TrainerEntity> GetById(int id)
    {
        return await _dbContext.Trainers.FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public async Task<TrainerEntity> GetByFirstName(string firstName)
    {
        return await _dbContext.Trainers.FirstOrDefaultAsync(entity => entity.FirstName == firstName);
    }

    public async Task<TrainerEntity> GetByUserName(string userName)
    {
        return await _dbContext.Trainers.FirstOrDefaultAsync(entity => entity.Username == userName);
    }

    public async Task CreateTrainer(TrainerEntity trainerEntity)
    {
        await _dbContext.Trainers.AddAsync(trainerEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateTrainer(TrainerEntity trainerEntity)
    {
        _dbContext.Update(trainerEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteTrainer(int trainerId)
    {
        var trainerToDelete = await _dbContext.Trainers.FirstOrDefaultAsync(entity => entity.Id == trainerId);

        if (trainerToDelete is not null)
        {
            _dbContext.Remove(trainerToDelete);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }
}