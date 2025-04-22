using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;
using Wolfer.Repositories;

namespace Wolfer.Services;

public class TrainerService : ITrainerService
{
    private ITrainerRepository _trainerRepository;

    public TrainerService(ITrainerRepository trainerRepository)
    {
        _trainerRepository = trainerRepository;
    }

    public async Task<TrainerEntity> GetById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("ID must be positive integer.");
        }
        
        return await _trainerRepository.GetById(id);
    }

    public async Task<TrainerEntity> GetByFirstName(string firstName)
    {
        if (string.IsNullOrEmpty(firstName))
        {
            throw new ArgumentException("First name cannot by empty.");
        }

        return await _trainerRepository.GetByFirstName(firstName);
    }

    public async Task<TrainerEntity> GetByUserName(string userName)
    {
        if (string.IsNullOrEmpty(userName))
        {
            throw new ArgumentException("Username cannot by empty.");
        }

        return await _trainerRepository.GetByUserName(userName);
    }

    public async Task CreateTrainer(TrainerDTO trainerDto)
    {
        if (trainerDto.Id != 0)
        {
            throw new ArgumentException("Trainer ID must be null.");
        }

        if (trainerDto.FirstName == "" || trainerDto.LastName == "" || trainerDto.Email == "" ||
            trainerDto.Password == "")
        {
            throw new ArgumentException("All properties must be filled out.");
        }
        
        TrainerEntity newTrainerEntity = ConvertDtoToEntity(trainerDto);
        
        await _trainerRepository.CreateTrainer(newTrainerEntity);
    }

    public async Task UpdateTrainer(TrainerDTO trainerDto)
    {
        if (!await _trainerRepository.IsTrainerPresent(trainerDto.Id))
        {
            throw new ArgumentException("Invalid ID.");
        }
        
        if (trainerDto.FirstName == "" || trainerDto.LastName == "" || trainerDto.Email == "" ||
            trainerDto.Password == "")
        {
            throw new ArgumentException("All properties must be filled out.");
        }
        
        TrainerEntity newTrainerEntity = ConvertDtoToEntity(trainerDto);
        
        await _trainerRepository.UpdateTrainer(newTrainerEntity);
    }

    public async Task DeleteTrainer(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid ID.");
        }

        await _trainerRepository.DeleteTrainer(id);
    }

    private TrainerEntity ConvertDtoToEntity(TrainerDTO trainerDto)
    {
        TrainerEntity trainerEntity = new TrainerEntity
        {
            FirstName = trainerDto.FirstName,
            LastName = trainerDto.LastName,
            Username = trainerDto.Username,
            Email = trainerDto.Email,
            Password = trainerDto.Password,
            Id = trainerDto.Id
        };
        return trainerEntity;
    }
}