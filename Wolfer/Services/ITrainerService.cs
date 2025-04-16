using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;

namespace Wolfer.Services;

public interface ITrainerService
{
    public Task<TrainerEntity> GetById(int id);
    public Task<TrainerEntity> GetByFirstName(string firstName);
    public Task<TrainerEntity> GetByUserName(string userName);
    public Task CreateTrainer(TrainerDTO trainerDto);
    public Task UpdateTrainer(TrainerDTO trainerDto);
    public Task DeleteTrainer(int id);
}