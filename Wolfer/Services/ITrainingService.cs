using Wolfer.Data;
using Wolfer.Data.Entities;

namespace Wolfer.Services;

public interface ITrainingService
{
    public Task<TrainingEntity> GetById(int id);
    public Task<List<TrainingEntity>> GetTrainingsByDate(DateOnly date);
    public Task<List<TrainingEntity>> GetTrainingsByType(TrainingType type);
}