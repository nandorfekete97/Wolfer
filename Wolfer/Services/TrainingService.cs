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

    public Task CreateTraining(TrainingDTO trainingDto)
    {
        throw new NotImplementedException();

        // if (trainingDto.Id != 0)
        // {
        //     throw new ArgumentException("Trainer ID must be null.");
        // }
        //
        // // training type is an enum, need to check if it's not defined here
        // if (trainingDto.TrainingType == null || trainingDto.Date == null)
        // {
        //     throw new ArgumentException("All properties must be filled out.");
        // }
        //
        // TrainingEntity newTrainingEntity = ConvertDtoToEntity(trainingDto);
        //
    }

    public Task UpdateTraining(TrainingDTO trainingDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTraining(TrainingDTO trainingDto)
    {
        throw new NotImplementedException();
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