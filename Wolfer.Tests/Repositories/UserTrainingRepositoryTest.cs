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
[TestOf(typeof(UserTrainingRepository))]
public class UserTrainingRepositoryTest
{
    private WolferContext _dbContext;
    private UserTrainingRepository _repository;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<WolferContext>().UseInMemoryDatabase(databaseName: "WolferTestDb")
            .Options;

        _dbContext = new WolferContext(options);

        _repository = new UserTrainingRepository(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
    
    [Test]
    public async Task GetByUserId_ReturnsCorrectUserTrainings()
    {
        // create user and trainings
        UserEntity user = new UserEntity
        {
            Email = "nandor@fekete.com",
            FirstName = "Nándor",
            LastName = "Fekete",
            Username = "nandorfekete97",
            Password = "abc123"
        };

        TrainingEntity weighLiftingTraining = new TrainingEntity
        {
            TrainingType = TrainingType.WeightLifting,
            Date = DateTime.Today
        };

        TrainingEntity legDayTraining = new TrainingEntity
        {
            TrainingType = TrainingType.LegDay,
            Date = DateTime.Today
        };

        TrainingEntity crossFitTraining = new TrainingEntity
        {
            TrainingType = TrainingType.CrossFit,
            Date = DateTime.Today
        };

        // add them to db
        await _dbContext.Users.AddAsync(user);
        await _dbContext.Trainings.AddAsync(weighLiftingTraining);
        await _dbContext.Trainings.AddAsync(legDayTraining);
        await _dbContext.Trainings.AddAsync(crossFitTraining);
        
        await _dbContext.SaveChangesAsync();
        
        // create usertrainings with existing data
        UserTrainingEntity userTraining1 = new UserTrainingEntity
        {
            TrainingId = weighLiftingTraining.Id,
            UserId = user.Id
        };

        int differentId = 99;

        UserTrainingEntity userTraining2 = new UserTrainingEntity
        {
            TrainingId = crossFitTraining.Id,
            UserId = differentId
        };

        UserTrainingEntity userTraining3 = new UserTrainingEntity
        {
            TrainingId = legDayTraining.Id,
            UserId = user.Id
        };

        await _dbContext.UserTrainings.AddAsync(userTraining1);
        await _dbContext.UserTrainings.AddAsync(userTraining2);
        await _dbContext.UserTrainings.AddAsync(userTraining3);
        await _dbContext.SaveChangesAsync();

        List<UserTrainingEntity> expected = new List<UserTrainingEntity>
        {
            userTraining1, userTraining3
        };
        
        // assert
        var result = await _repository.GetByUserId(user.Id);

        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public async Task GetByUserId_FailsWithWrongId()
    {
        int wrongId = -1;

        var result = await _repository.GetByUserId(wrongId);
        
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetByTrainingId_ReturnsCorrectUserTrainings()
    {
        UserEntity user = new UserEntity
        {
            Email = "nandor@fekete.com",
            FirstName = "Nándor",
            LastName = "Fekete",
            Username = "nandorfekete97",
            Password = "abc123"
        };

        UserEntity user2 = new UserEntity
        {
            Email = "abel@kosar.com",
            FirstName = "Ábel",
            LastName = "Kosár",
            Username = "abelkosar96",
            Password = "abc123"
        };
        
        UserEntity user3 = new UserEntity
        {
            Email = "rolandhury@kosar.com",
            FirstName = "Roland",
            LastName = "Hury",
            Username = "roli24",
            Password = "abc123"
        };

        TrainingEntity weighLiftingTraining = new TrainingEntity
        {
            TrainingType = TrainingType.WeightLifting,
            Date = DateTime.Today
        };

        TrainingEntity legDayTraining = new TrainingEntity
        {
            TrainingType = TrainingType.LegDay,
            Date = DateTime.Today
        };

        await _dbContext.Users.AddAsync(user);
        await _dbContext.Trainings.AddAsync(weighLiftingTraining);
        await _dbContext.Trainings.AddAsync(legDayTraining);
        
        await _dbContext.SaveChangesAsync();
        
        // create different usertraining entities
        // training id should be same for at least 2 entities (expected result)
        // all usertraining entities added to db
        UserTrainingEntity userTraining1 = new UserTrainingEntity
        {
            TrainingId = weighLiftingTraining.Id,
            UserId = user.Id
        };

        UserTrainingEntity userTraining2 = new UserTrainingEntity
        {
            TrainingId = legDayTraining.Id,
            UserId = user2.Id
        };

        UserTrainingEntity userTraining3 = new UserTrainingEntity
        {
            TrainingId = weighLiftingTraining.Id,
            UserId = user3.Id
        };

        await _dbContext.UserTrainings.AddAsync(userTraining1);
        await _dbContext.UserTrainings.AddAsync(userTraining2);
        await _dbContext.UserTrainings.AddAsync(userTraining3);
        await _dbContext.SaveChangesAsync();

        List<UserTrainingEntity> expected = new List<UserTrainingEntity>
        {
            userTraining1, userTraining3
        };
        
        // assert
        var result = await _repository.GetByTrainingId(weighLiftingTraining.Id);

        Assert.That(result, Is.EquivalentTo(expected));
    }
    
    [Test]
    public async Task GetByTrainingId_FailsWithWrongId()
    {
        int wrongId = -1;

        var result = await _repository.GetByTrainingId(wrongId);
        
        Assert.That(result, Is.Empty);
    }

    private void CompareTwoUserTrainings(UserTrainingEntity actualUserTraining, UserTrainingEntity expected)
    {
        Assert.That(actualUserTraining, Is.Not.Null);
        Assert.That(actualUserTraining.TrainingId, Is.Not.Null);
        Assert.That(actualUserTraining.UserId, Is.Not.Null);
        Assert.That(actualUserTraining.UserId, Is.EqualTo(expected.UserId));
        Assert.That(actualUserTraining.TrainingId, Is.EqualTo(expected.TrainingId));
    }
}