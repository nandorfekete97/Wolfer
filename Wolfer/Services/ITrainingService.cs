using Wolfer.Data;
using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;

namespace Wolfer.Services;

public interface ITrainingService
{
    public Task<TrainingEntity> GetById(int id);
    public Task<List<TrainingEntity>> GetTrainingsByDate(DateOnly date);
    public Task<List<TrainingEntity>> GetTrainingsByType(TrainingType type);
    public Task CreateTraining(TrainingDTO trainingDto);
    public Task CreateTrainings(List<TrainingDTO> trainerDtos);
    public Task UpdateTraining(TrainingDTO trainingDto);
    public Task DeleteTraining(int id);
    public Task DeleteTrainingsByDate(DateOnly date);
}