using Wolfer.Data;
using Wolfer.Data.DTOs;
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

    public async Task CreateTraining(TrainingDTO trainingDto)
    {
        if (trainingDto.Id != 0)
        {
            throw new ArgumentException("Trainer ID must be null.");
        }
        
        if (trainingDto.TrainingType == null || trainingDto.Date == null)
        {
            throw new ArgumentException("All properties must be filled out.");
        }
        
        TrainingEntity newTrainingEntity = ConvertDtoToEntity(trainingDto);

        await _trainingRepository.CreateTraining(newTrainingEntity);
    }

    public async Task UpdateTraining(TrainingDTO trainingDto)
    {
        TrainingEntity trainingToUpdate = await GetById(trainingDto.Id);

        if (trainingToUpdate == null)
        {
            throw new ArgumentException("Invalid ID.");
        }

        if (!Enum.IsDefined(typeof(TrainingType), trainingDto.TrainingType) || trainingDto.Date == DateTime.MinValue)
        {
            throw new ArgumentException("All properties must be filled out.");
        }

        TrainingEntity newTrainingEntity = ConvertDtoToEntity(trainingDto);

        await _trainingRepository.UpdateTraining(newTrainingEntity);
    }

    public async Task DeleteTraining(int trainingId)
    {
        if (trainingId <= 0)
        {
            throw new ArgumentException("Invalid ID.");
        }

        await _trainingRepository.DeleteById(trainingId);
    }

    private TrainingEntity ConvertDtoToEntity(TrainingDTO trainingDto)
    {
        TrainingEntity trainingEntity = new TrainingEntity
        {
            Date = trainingDto.Date,
            TrainingType = trainingDto.TrainingType
        };
        return trainingEntity;
    }
}