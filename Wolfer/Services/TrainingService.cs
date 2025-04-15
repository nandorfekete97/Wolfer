using Wolfer.Data;
using Wolfer.Data.Entities;
using Wolfer.Repositories;

namespace Wolfer.Services;

public class TrainingService : ITrainingService
{
    private ITrainingRepository _trainingRepository;

    public TrainingService(ITrainingRepository trainingRepository)
    {
        _trainingRepository = trainingRepository;
    }

    public async Task<TrainingEntity> GetById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("ID must be positive integer.");
        }

        return await _trainingRepository.GetById(id);
    }

    public async Task<List<TrainingEntity>> GetTrainingsByDate(DateOnly date)
    {
        if (date == default)
        {
            throw new ArgumentException("Invalid date.");
        }

        return await _trainingRepository.GetTrainingsByDate(date);
    }

    public async Task<List<TrainingEntity>> GetTrainingsByType(TrainingType type)
    {
        if (!Enum.IsDefined(typeof(TrainingType), type))
        {
            throw new ArgumentException("Invalid training type.");
        }

        return await _trainingRepository.GetTrainingsByType(type);
    }
}