using Wolfer.Data;
using Wolfer.Data.Entities;

namespace Wolfer.Repositories;

public interface ITrainingRepository
{
    Task<TrainingEntity> GetById(int id);
    Task<List<TrainingEntity>> GetTrainingsByDate(DateOnly date);
    Task<List<TrainingEntity>> GetTrainingsByType(TrainingType trainingType);
    Task<List<TrainingEntity>> GetByIds(List<int> trainingIds);
}