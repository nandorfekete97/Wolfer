using Wolfer.Data;
using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;

namespace Wolfer.Repositories;

public interface ITrainingRepository
{
    Task<TrainingEntity> GetById(int id);
    Task<List<TrainingEntity>> GetTrainingsByDate(DateOnly date);
    Task<List<TrainingEntity>> GetTrainingsByType(TrainingType trainingType);
    Task CreateTraining(TrainingEntity training);
    Task CreateTrainings(List<TrainingEntity> trainings);
    Task UpdateTraining(TrainingEntity training);
    Task<bool> DeleteById(int id);
    Task<bool> DeleteTrainingsByDate(DateOnly date);
    Task<List<TrainingEntity>> GetByIds(List<int> trainingIds);
}