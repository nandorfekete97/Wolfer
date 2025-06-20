using Wolfer.Data;
using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;
using Wolfer.Repositories;

namespace Wolfer.Services;

public class PersonalRecordService : IPersonalRecordService
{
    private IPersonalRecordRepository _personalRecordRepository;

    public PersonalRecordService(IPersonalRecordRepository personalRecordRepository)
    {
        _personalRecordRepository = personalRecordRepository;
    }

    public async Task<List<PersonalRecordEntity>> GetByUserId(Guid userId)
    {
        return await _personalRecordRepository.GetByUserId(userId);
    }

    public async Task AddPersonalRecord(PersonalRecordDTO personalRecordDto)
    {
        if (personalRecordDto.Id != 0)
        {
            throw new ArgumentException("ID must be null.");
        }

        AssertIfDTOPropertiesAreDefined(personalRecordDto);

        await _personalRecordRepository.AddPersonalRecord(ConvertDTOToEntity(personalRecordDto));
    }

    public async Task UpdatePersonalRecord(PersonalRecordDTO personalRecordDto)
    {
        PersonalRecordEntity personalRecordToUpdate = await _personalRecordRepository.GetById(personalRecordDto.Id);

        if (personalRecordToUpdate == null)
        {
            throw new ArgumentException("Personal Record was not found.");
        }
        
        AssertIfDTOPropertiesAreDefined(personalRecordDto);

        personalRecordToUpdate.ExerciseType = personalRecordDto.ExerciseType;
        personalRecordToUpdate.Weight = personalRecordDto.Weight;

        await _personalRecordRepository.UpdatePersonalRecord(personalRecordToUpdate);
    }

    private PersonalRecordEntity ConvertDTOToEntity(PersonalRecordDTO personalRecordDto)
    {
        return new PersonalRecordEntity
        {
            Id = personalRecordDto.Id,
            ExerciseType = personalRecordDto.ExerciseType,
            Weight = personalRecordDto.Weight,
            UserId = personalRecordDto.UserId
        };
    }

    private void AssertIfDTOPropertiesAreDefined(PersonalRecordDTO personalRecordDto)
    {
        if (!Enum.IsDefined(typeof(ExerciseType), personalRecordDto.ExerciseType) ||
            personalRecordDto.UserId == Guid.Empty || personalRecordDto.Weight <= 0)
        {
            throw new ArgumentException("User ID, Exercise type and exercise weight must be defined.");
        }
    }
}