using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Wolfer.Data;
using Wolfer.Data.Context;
using Wolfer.Data.Entities;
using Wolfer.Repositories;
using Guid = System.Guid;

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
        Guid userId = Guid.NewGuid();
        
        // create user and trainings
        IdentityUser user = new IdentityUser
        {
            Id = userId.ToString(),
            Email = "nandor@fekete.com"
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
            UserId = userId
        };
    
        Guid differentId = Guid.NewGuid();
    
        UserTrainingEntity userTraining2 = new UserTrainingEntity
        {
            TrainingId = crossFitTraining.Id,
            UserId = differentId
        };
    
        UserTrainingEntity userTraining3 = new UserTrainingEntity
        {
            TrainingId = legDayTraining.Id,
            UserId = userId
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
        var result = await _repository.GetByUserId(userId);
    
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public async Task GetByUserId_FailsWithWrongId()
    {
        Guid wrongId = Guid.NewGuid();
    
        var result = await _repository.GetByUserId(wrongId);
        
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetByTrainingId_ReturnsCorrectUserTrainings()
    {
        Guid user1Id = Guid.NewGuid();
        Guid user2Id = Guid.NewGuid();
        Guid user3Id = Guid.NewGuid();
        
        IdentityUser user1 = new IdentityUser
        {
            Id = user1Id.ToString(),
            Email = "nandor@fekete.com"
        };

        IdentityUser user2 = new IdentityUser
        {
            Id = user2Id.ToString(),
            Email = "abel@kosar.com",
        };
        
        IdentityUser user3 = new IdentityUser
        {
            Id = user3Id.ToString(),
            Email = "roland@hury.com",
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

        await _dbContext.Users.AddAsync(user1);
        await _dbContext.Trainings.AddAsync(weighLiftingTraining);
        await _dbContext.Trainings.AddAsync(legDayTraining);
        
        await _dbContext.SaveChangesAsync();
        
        // create different usertraining entities
        // training id should be same for at least 2 entities (expected result)
        // all usertraining entities added to db
        UserTrainingEntity userTraining1 = new UserTrainingEntity
        {
            TrainingId = weighLiftingTraining.Id,
            UserId = user1Id
        };

        UserTrainingEntity userTraining2 = new UserTrainingEntity
        {
            TrainingId = legDayTraining.Id,
            UserId = user2Id
        };

        UserTrainingEntity userTraining3 = new UserTrainingEntity
        {
            TrainingId = weighLiftingTraining.Id,
            UserId = user3Id
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
        Assert.That(result.Select(userTraining => userTraining.UserId), Is.EquivalentTo(new[] { user1Id, user3Id }));
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