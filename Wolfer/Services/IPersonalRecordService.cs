using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;

namespace Wolfer.Services;

public interface IPersonalRecordService
{
    Task<List<PersonalRecordEntity>> GetByUserId(Guid userId);
    Task AddPersonalRecord(PersonalRecordDTO personalRecordDto);
    Task UpdatePersonalRecord(PersonalRecordDTO personalRecordDto);
}