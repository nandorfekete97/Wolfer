using Wolfer.Data.Entities;

namespace Wolfer.Repositories;

public interface ITrainerRepository
{
    Task<TrainerEntity> GetById(int id);
    Task<TrainerEntity> GetByFirstName(string firstName);
    Task<TrainerEntity> GetByUserName(string userName);
    Task CreateTrainer(TrainerEntity trainerEntity);
    Task UpdateTrainer(TrainerEntity trainerEntity);
    Task<bool> DeleteTrainer(int trainerId);
    Task<bool> IsTrainerPresent(int id);
}