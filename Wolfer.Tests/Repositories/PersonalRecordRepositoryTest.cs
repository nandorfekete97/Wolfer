using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Wolfer.Data;
using Wolfer.Data.Context;
using Wolfer.Data.Entities;
using Wolfer.Repositories;

namespace Wolfer.Tests.Repositories;

[TestFixture]
[TestOf(typeof(PersonalRecordRepository))]
public class PersonalRecordRepositoryTest
{
    private WolferContext _dbContext;
    private PersonalRecordRepository _personalRecordRepository;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<WolferContext>().UseInMemoryDatabase(databaseName: "WolferTestDb")
            .Options;

        _dbContext = new WolferContext(options);
        _personalRecordRepository = new PersonalRecordRepository(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task GetById_SuccessfullyReturnsPersonalRecord()
    {
        Guid userId = Guid.NewGuid();

        PersonalRecordEntity personalRecordEntity1 = new PersonalRecordEntity
            { UserId = userId, ExerciseType = ExerciseType.Deadlift, Id = 1, Weight = 160 };
        PersonalRecordEntity personalRecordEntity2 = new PersonalRecordEntity
            { UserId = userId, ExerciseType = ExerciseType.Squat, Id = 2, Weight = 120 };

        await _dbContext.PersonalRecords.AddAsync(personalRecordEntity1);
        await _dbContext.PersonalRecords.AddAsync(personalRecordEntity2);
        await _dbContext.SaveChangesAsync();

        var result = await _personalRecordRepository.GetById(personalRecordEntity2.Id);
        
        Assert.That(result.Id, Is.EqualTo(personalRecordEntity2.Id));
        Assert.That(result.UserId, Is.EqualTo(personalRecordEntity2.UserId));
        Assert.That(result.ExerciseType, Is.EqualTo(personalRecordEntity2.ExerciseType));
        Assert.That(result.Weight, Is.EqualTo(personalRecordEntity2.Weight));
    }
    
    [Test]
    public async Task GetByUserId_SuccessfullyReturnsPersonalRecord()
    {
        Guid userId = Guid.NewGuid();
        Guid userId2 = Guid.NewGuid();
        
        PersonalRecordEntity personalRecordEntity1 = new PersonalRecordEntity
            { UserId = userId, ExerciseType = ExerciseType.Deadlift, Id = 1, Weight = 160 };
        PersonalRecordEntity personalRecordEntity2 = new PersonalRecordEntity
            { UserId = userId2, ExerciseType = ExerciseType.Squat, Id = 2, Weight = 120 };
        PersonalRecordEntity personalRecordEntity3 = new PersonalRecordEntity
            { UserId = userId, ExerciseType = ExerciseType.Clean, Id = 3, Weight = 90 };
        PersonalRecordEntity personalRecordEntity4 = new PersonalRecordEntity
            { UserId = userId2, ExerciseType = ExerciseType.PullUp, Id = 4, Weight = 25 };

        await _dbContext.PersonalRecords.AddAsync(personalRecordEntity1);
        await _dbContext.PersonalRecords.AddAsync(personalRecordEntity2);
        await _dbContext.PersonalRecords.AddAsync(personalRecordEntity3);
        await _dbContext.PersonalRecords.AddAsync(personalRecordEntity4);
        await _dbContext.SaveChangesAsync();

        var result = await _personalRecordRepository.GetByUserId(userId2);

        List<PersonalRecordEntity> expected = new List<PersonalRecordEntity>
            { personalRecordEntity2, personalRecordEntity4 };
        
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public async Task AddPersonalRecord_Successfully_AddsPersonalRecordToDb()
    {
        Guid userId = Guid.NewGuid();
        PersonalRecordEntity personalRecordEntity = new PersonalRecordEntity
            { Id = 1, ExerciseType = ExerciseType.BenchPress, Weight = 95, UserId = userId };

        List<PersonalRecordEntity> expectedEmptyList = await _dbContext.PersonalRecords.ToListAsync();
        
        Assert.That(expectedEmptyList.Count, Is.EqualTo(0));

        await _personalRecordRepository.AddPersonalRecord(personalRecordEntity);

        var personalRecords = await _dbContext.PersonalRecords.ToListAsync();
        
        Assert.That(personalRecords[0].UserId, Is.EqualTo(userId));
        Assert.That(personalRecords[0].ExerciseType, Is.EqualTo(personalRecordEntity.ExerciseType));
        Assert.That(personalRecords[0].Weight, Is.EqualTo(personalRecordEntity.Weight));
        Assert.That(personalRecords[0].Id, Is.EqualTo(personalRecordEntity.Id));
    }

    [Test]
    public async Task UpdatePersonalRecord_SuccessfullyUpdatesPersonalRecord()
    {
        Guid userId = Guid.NewGuid();
        PersonalRecordEntity personalRecordEntity = new PersonalRecordEntity
            { Id = 1, ExerciseType = ExerciseType.BenchPress, Weight = 95, UserId = userId };

        await _dbContext.PersonalRecords.AddAsync(personalRecordEntity);
        await _dbContext.SaveChangesAsync();

        var originalPersonalRecord = await _personalRecordRepository.GetById(personalRecordEntity.Id);
        Assert.That(originalPersonalRecord.Weight, Is.EqualTo(95));

        int newWeight = 100;
        originalPersonalRecord.Weight = newWeight;

        await _personalRecordRepository.UpdatePersonalRecord(originalPersonalRecord);

        var newPersonalRecord = await _personalRecordRepository.GetById(personalRecordEntity.Id);
        Assert.That(newPersonalRecord.Weight, Is.EqualTo(100));
    }
}