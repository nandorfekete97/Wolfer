using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
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
    private UserTrainingRepository _userTrainingRepository;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<WolferContext>().UseInMemoryDatabase(databaseName: "WolferTestDb")
            .Options;

        _dbContext = new WolferContext(options);

        _userTrainingRepository = new UserTrainingRepository(_dbContext);
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
        var result = await _userTrainingRepository.GetByUserId(userId);
    
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public async Task GetByUserId_FailsWithWrongId()
    {
        Guid wrongId = Guid.NewGuid();
    
        var result = await _userTrainingRepository.GetByUserId(wrongId);
        
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
        var result = await _userTrainingRepository.GetByTrainingId(weighLiftingTraining.Id);

        Assert.That(result, Is.EquivalentTo(expected));
        Assert.That(result.Select(userTraining => userTraining.UserId), Is.EquivalentTo(new[] { user1Id, user3Id }));
    }
    
    [Test]
    public async Task GetByTrainingId_FailsWithWrongId()
    {
        int wrongId = -1;
    
        var result = await _userTrainingRepository.GetByTrainingId(wrongId);
        
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task Create_AddsUserTrainingToDatabase()
    {
        Guid userId = Guid.NewGuid();

        IdentityUser user = new IdentityUser
        {
            Id = userId.ToString(), Email = "test@email.com"
        };

        TrainingEntity training = new TrainingEntity
        {
            Id = 1, TrainingType = TrainingType.FunctionalBodyBuilding, Date = DateTime.Now
        };

        await _dbContext.Users.AddAsync(user);
        await _dbContext.Trainings.AddAsync(training);
        await _dbContext.SaveChangesAsync();

        UserTrainingEntity userTrainingEntity = new UserTrainingEntity
        {
            UserId = userId, TrainingId = training.Id
        };

        await _userTrainingRepository.Create(userTrainingEntity);

        var userTrainingsInDb = await _dbContext.UserTrainings.ToListAsync();
        
        Assert.That(userTrainingsInDb.Count, Is.EqualTo(1));
        Assert.That(userTrainingsInDb[0].UserId, Is.EqualTo(userTrainingEntity.UserId));
        Assert.That(userTrainingsInDb[0].TrainingId, Is.EqualTo(userTrainingEntity.TrainingId));
    }

    [Test]
    public async Task Create_NullUserTrainingEntity_ThrowsException()
    {
        var exception =
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _userTrainingRepository.Create(null));
        
        Assert.That(exception.ParamName, Is.EqualTo("userTrainingEntity"));
    }

    [Test]
    public async Task Delete_SuccessfullyDeletesUserTrainingFromDb()
    {
        Guid userId = Guid.NewGuid();
        int trainingId = 1;

        UserTrainingEntity userTrainingEntity = new UserTrainingEntity { UserId = userId, TrainingId = trainingId };

        await _dbContext.UserTrainings.AddAsync(userTrainingEntity);
        await _dbContext.SaveChangesAsync();
        
        var userTrainingsInDb = await _dbContext.UserTrainings.ToListAsync();
        
        Assert.That(userTrainingsInDb.Count, Is.EqualTo(1));
        Assert.That(userTrainingsInDb[0].UserId, Is.EqualTo(userId));
        Assert.That(userTrainingsInDb[0].TrainingId, Is.EqualTo(trainingId));

        var result = await _userTrainingRepository.Delete(userId, trainingId);
        
        var userTrainingsInDbAfterDelete = await _dbContext.UserTrainings.ToListAsync();
        
        Assert.That(result, Is.True);
        Assert.That(userTrainingsInDbAfterDelete.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task Delete_NullUserTraining_ReturnsFalse()
    {
        Guid nonExistingUserId = Guid.NewGuid();
        int nonExistingTrainingId = 99;
        
        var result = await _userTrainingRepository.Delete(nonExistingUserId, nonExistingTrainingId);
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task DeleteTrainingById_SuccessfullyDeletesUserTrainings()
    {
        DateTime today = DateTime.Today;
        int trainingId = 5;
        int trainingId2 = 6;
        Guid userId1 = Guid.NewGuid();
        Guid userId2 = Guid.NewGuid();
        
        TrainingEntity training = new TrainingEntity { Id = trainingId, Date = today, TrainingType = TrainingType.LegDay };
        TrainingEntity training2 = new TrainingEntity { Id = trainingId2, Date = today, TrainingType = TrainingType.CrossFit };
        
        UserTrainingEntity userTraining1 = new UserTrainingEntity { UserId = userId1, TrainingId = trainingId };
        UserTrainingEntity userTraining2 = new UserTrainingEntity { UserId = userId1, TrainingId = trainingId2 };
        UserTrainingEntity userTraining3 = new UserTrainingEntity { UserId = userId2, TrainingId = trainingId };

        await _dbContext.Trainings.AddRangeAsync(training, training2);
        await _dbContext.UserTrainings.AddRangeAsync(userTraining1, userTraining2, userTraining3);
        await _dbContext.SaveChangesAsync();

        var userTrainingsBefore = await _dbContext.UserTrainings.ToListAsync();
        Assert.That(userTrainingsBefore.Count, Is.EqualTo(3));

        await _userTrainingRepository.DeleteByTrainingId(trainingId);
        
        var userTrainingsAfterDelete = await _dbContext.UserTrainings.ToListAsync();
        Assert.That(userTrainingsAfterDelete.Count, Is.EqualTo(1));
        Assert.That(userTrainingsAfterDelete[0].TrainingId, Is.EqualTo(trainingId2));
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