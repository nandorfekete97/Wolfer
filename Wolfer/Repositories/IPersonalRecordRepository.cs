using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;

namespace Wolfer.Repositories;

public interface IPersonalRecordRepository
{
    Task<PersonalRecordEntity> GetById(int personalRecordId);
    Task<List<PersonalRecordEntity>> GetByUserId(Guid userId);
    Task AddPersonalRecord(PersonalRecordEntity personalRecordEntity);
    Task UpdatePersonalRecord(PersonalRecordEntity personalRecordEntity);
}