using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Wolfer.Data;
using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;
using Wolfer.Repositories;
using Wolfer.Services;

namespace Wolfer.Tests.Services;

[TestFixture]
[TestOf(typeof(PersonalRecordService))]
public class PersonalRecordServiceTest
{
    private Mock<IPersonalRecordRepository> _personalRecordRepoMock;
    private IPersonalRecordService _personalRecordService;

    [SetUp]
    public void SetUp()
    {
        _personalRecordRepoMock = new Mock<IPersonalRecordRepository>();
        _personalRecordService = new PersonalRecordService(_personalRecordRepoMock.Object);
    }

    [Test]
    public async Task GetByUserId_SuccessfullyReturnsPersonalRecord()
    {
        Guid userId = Guid.NewGuid();

        PersonalRecordEntity personalRecord1 = new PersonalRecordEntity
            { ExerciseType = ExerciseType.Deadlift, Id = 1, Weight = 160, UserId = userId };
        PersonalRecordEntity personalRecord2 = new PersonalRecordEntity
            { ExerciseType = ExerciseType.BenchPress, Id = 2, Weight = 100, UserId = userId };
        List<PersonalRecordEntity> personalRecords = new List<PersonalRecordEntity>
            { personalRecord1, personalRecord2 };
        
        _personalRecordRepoMock.Setup(repository => repository.GetByUserId(userId)).ReturnsAsync(personalRecords);

        var result = await _personalRecordService.GetByUserId(userId);
        
        Assert.That(result, Is.EquivalentTo(personalRecords));
    }

    [Test]
    public async Task GetByUserId_InvalidUserId_ThrowsException()
    {
        Guid invalidUserId = Guid.Empty;

        var exception =
            Assert.ThrowsAsync<ArgumentException>(async () => await _personalRecordService.GetByUserId(invalidUserId));
        
        Assert.That(exception.Message, Is.EqualTo("Invalid user ID."));
        _personalRecordRepoMock.Verify(repository => repository.GetByUserId(It.IsAny<Guid>()), Times.Never);
    }

    [Test]
    public async Task AddPersonalRecord_SuccessfullyAddsPersonalRecord()
    {
        Guid userId = Guid.NewGuid();
        
        PersonalRecordDTO personalRecordDto = new PersonalRecordDTO
            { Id = 0, Weight = 165, ExerciseType = ExerciseType.Deadlift, UserId = userId };

        var expectedEntity = new PersonalRecordEntity
        {
            Weight = personalRecordDto.Weight,
            ExerciseType = personalRecordDto.ExerciseType,
            UserId = personalRecordDto.UserId
        };

        await _personalRecordService.AddPersonalRecord(personalRecordDto);
        
        _personalRecordRepoMock.Verify(repository => repository.AddPersonalRecord(It.Is<PersonalRecordEntity>(entity => entity.Weight == expectedEntity.Weight && entity.ExerciseType == expectedEntity.ExerciseType && entity.UserId == expectedEntity.UserId)), Times.Once);
    }

    [Test]
    public async Task AddPersonalRecord_NonZeroId_ThrowsException()
    {
        int personalRecordDtoId = 1;
        Guid validUserId = Guid.NewGuid();

        PersonalRecordDTO personalRecordDto = new PersonalRecordDTO
            { Id = personalRecordDtoId, ExerciseType = ExerciseType.Squat, Weight = 120, UserId = validUserId };

        var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            await _personalRecordService.AddPersonalRecord(personalRecordDto));
        
        Assert.That(exception.Message, Is.EqualTo("ID must be null."));
        _personalRecordRepoMock.Verify(repository => repository.AddPersonalRecord(It.IsAny<PersonalRecordEntity>()), Times.Never);
    }

    [Test]
    public async Task UpdatePersonalRecord_SuccessfullyUpdatesPersonalRecord()
    {
        Guid userId = Guid.NewGuid();
        int personalRecordId = 1;
        
        PersonalRecordDTO personalRecordDto = new PersonalRecordDTO
            { Id = personalRecordId, ExerciseType = ExerciseType.PullUp, Weight = 25, UserId = userId };

        PersonalRecordEntity existingEntity = new PersonalRecordEntity
            { Id = personalRecordId, ExerciseType = ExerciseType.PullUp, Weight = 20, UserId = userId };

        _personalRecordRepoMock.Setup(repository => repository.GetById(personalRecordId))
            .ReturnsAsync(existingEntity);
        
        await _personalRecordService.UpdatePersonalRecord(personalRecordDto);
        
        _personalRecordRepoMock.Verify(repository => repository.UpdatePersonalRecord(It.Is<PersonalRecordEntity>(entity => entity.Id == personalRecordId && entity.UserId == userId && entity.ExerciseType == personalRecordDto.ExerciseType && entity.Weight == personalRecordDto.Weight)), Times.Once);
    }
}