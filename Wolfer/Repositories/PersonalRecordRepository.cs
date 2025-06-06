using Microsoft.EntityFrameworkCore;
using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;
using Wolfer.Data.Context;

namespace Wolfer.Repositories;

public class PersonalRecordRepository : IPersonalRecordRepository
{
    private WolferContext _dbContext;

    public PersonalRecordRepository(WolferContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PersonalRecordEntity> GetById(int personalRecordId)
    {
        return await _dbContext.PersonalRecords.FirstOrDefaultAsync(entity => entity.Id == personalRecordId);
    }

    public async Task<List<PersonalRecordEntity>> GetByUserId(Guid userId)
    {
        return await _dbContext.PersonalRecords.Where(entity => entity.UserId == userId).ToListAsync();
    }

    public async Task AddPersonalRecord(PersonalRecordEntity personalRecordEntity)
    {
        await _dbContext.PersonalRecords.AddAsync(personalRecordEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdatePersonalRecord(PersonalRecordEntity personalRecordEntity)
    {
        await _dbContext.SaveChangesAsync();
    }
}