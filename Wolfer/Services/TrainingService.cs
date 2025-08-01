using Wolfer.Data;
using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;
using Wolfer.Repositories;

namespace Wolfer.Services;

public class TrainingService : ITrainingService
{
    private ITrainingRepository _trainingRepository;
    private IUserTrainingRepository _userTrainingRepository;

    public TrainingService(ITrainingRepository trainingRepository, IUserTrainingRepository userTrainingRepository)
    {
        _trainingRepository = trainingRepository;
        _userTrainingRepository = userTrainingRepository;
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

        if (trainingDto.Date == default)
        {
            throw new ArgumentException("Date must be set.");
        }

        if (!Enum.IsDefined(typeof(TrainingType), trainingDto.TrainingType))
        {
            throw new ArgumentException("Invalid training type.");
        }
            
        if (await IsTrainingTimeOccupied(trainingDto.Date, trainingDto.Id))
        {
            throw new ArgumentException("Training date is occupied.");
        }
        
        TrainingEntity newTrainingEntity = ConvertDtoToEntity(trainingDto);

        await _trainingRepository.CreateTraining(newTrainingEntity);
    }

    public async Task CreateTrainings(List<TrainingDTO> trainerDtos)
    {
        if (trainerDtos == null || trainerDtos.Count == 0)
        {
            throw new ArgumentException("No trainings provided.");
        }

        var trainingEntities = new List<TrainingEntity>();

        foreach (var dto in trainerDtos)
        {
            if (dto.Id != 0)
            {
                throw new ArgumentException("Training ID must be 0.");
            }
            if (dto.Date == default)
            {
                throw new ArgumentException("Date must be set.");
            }
            if (!Enum.IsDefined(typeof(TrainingType), dto.TrainingType))
            {
                throw new ArgumentException("Invalid training type.");
            }
            if (await IsTrainingTimeOccupied(dto.Date, dto.Id))
            {
                throw new ArgumentException($"Training time {dto.Date} is occupied.");
            }
            
            trainingEntities.Add(ConvertDtoToEntity(dto));
        }

        await _trainingRepository.CreateTrainings(trainingEntities);
    }

    public async Task UpdateTraining(TrainingDTO trainingDto)
    {
        TrainingEntity trainingToUpdate = await GetById(trainingDto.Id);

        if (trainingToUpdate == null)
        {
            throw new ArgumentException("Invalid ID.");
        }

        if (!Enum.IsDefined(typeof(TrainingType), trainingDto.TrainingType) || trainingDto.Date == default)
        {
            throw new ArgumentException("All properties must be filled out.");
        }
        
        if (await IsTrainingTimeOccupied(trainingDto.Date, trainingDto.Id))
        {
            throw new ArgumentException("Training date is occupied.");
        }

        trainingToUpdate.Date = trainingDto.Date;
        trainingToUpdate.TrainingType = trainingDto.TrainingType;

        await _trainingRepository.UpdateTraining(trainingToUpdate);
    }

    public async Task DeleteTraining(int trainingId)
    {
        if (trainingId <= 0)
        {
            throw new ArgumentException("Invalid ID.");
        }

        await _trainingRepository.DeleteById(trainingId);
        await _userTrainingRepository.DeleteByTrainingId(trainingId);
    }

    public async Task DeleteTrainingsByDate(DateOnly date)
    {
        if (date == default)
        {
            throw new ArgumentException("Date must be provided.");
        }

        await _trainingRepository.DeleteTrainingsByDate(date);
    }

    private TrainingEntity ConvertDtoToEntity(TrainingDTO trainingDto)
    {
        TrainingEntity trainingEntity = new TrainingEntity
        {
            Id = trainingDto.Id,
            Date = trainingDto.Date,
            TrainingType = trainingDto.TrainingType
        };
        return trainingEntity;
    }

    protected virtual async Task<bool> IsTrainingTimeOccupied(DateTime trainingDate, int excludedId)
    {
        List<TrainingEntity> trainingEntities = await _trainingRepository.GetTrainingsByDate(DateOnly.FromDateTime(trainingDate));

        int newTrainingHour = trainingDate.Hour;
        int newTrainingMinute = trainingDate.Minute;

        return trainingEntities.Where(entity => entity.Id != excludedId).Any(entity => IsTimeDifferenceSmallerThanOneHour(newTrainingHour, newTrainingMinute, entity.Date.Hour, entity.Date.Minute));
    }

    private bool IsTimeDifferenceSmallerThanOneHour(int newTrainingHour, int newTrainingMinute, int existingTrainingHour, int existingTrainingMinute)
    {
        if (existingTrainingHour - newTrainingHour == 0)
        {
            return true;
        }
        if (existingTrainingHour - newTrainingHour == -1)
        {
            return newTrainingMinute < existingTrainingMinute;
        }
        if (existingTrainingHour - newTrainingHour == 1)
        {
            return newTrainingMinute > existingTrainingMinute;
        }
        return false;
    }
}