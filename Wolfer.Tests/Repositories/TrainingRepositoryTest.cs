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
[TestOf(typeof(TrainingRepository))]
public class TrainingRepositoryTest
{
    private WolferContext _dbContext;
    private TrainingRepository _repository;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<WolferContext>()
            .UseInMemoryDatabase(databaseName: "WolferTestDb")
            .Options;

        _dbContext = new WolferContext(options);

        _repository = new TrainingRepository(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task GetById_ReturnsCorrectTraining()
    {
        TrainingType dummyType = TrainingType.CrossFit;
        DateTime dummyDate = DateTime.Today;
        
        TrainingEntity crossfit = new TrainingEntity
        {
            Date = dummyDate,
            TrainingType = dummyType
        };

        TrainingEntity weightLifting = new TrainingEntity
        {
            Date = dummyDate,
            TrainingType = dummyType
        };

        await _dbContext.Trainings.AddAsync(crossfit);
        await _dbContext.Trainings.AddAsync(weightLifting);
        await _dbContext.SaveChangesAsync();

        var result = await _repository.GetById(weightLifting.Id);

        CompareTwoTrainingEntities(result, weightLifting);
    }

    [Test]
    public async Task GetByIdFails_IfIdNonExistent()
    {
        int wrongId = -1;

        var result = await _repository.GetById(wrongId);
        
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateTraining_AddsTrainingSuccessfully()
    {
        TrainingType dummyType = TrainingType.CrossFit;
        DateTime dummyDate = DateTime.Today;
        
        TrainingEntity crossfit = new TrainingEntity
        {
            Date = dummyDate,
            TrainingType = dummyType
        }; 
        
        TrainingEntity weightLifting = new TrainingEntity
        {
            Date = dummyDate,
            TrainingType = dummyType
        };

        await _repository.CreateTraining(weightLifting);
        await _repository.CreateTraining(crossfit);

        var crossfitResult = await _repository.GetById(crossfit.Id);
        var weightLiftingResult = await _repository.GetById(weightLifting.Id);
        
        CompareTwoTrainingEntities(crossfitResult, crossfit);
        CompareTwoTrainingEntities(weightLiftingResult, weightLifting);
    }

    [Test]
    public async Task GetTrainingsByDate_ReturnsCorrectTrainings()
    {
        TrainingType dummyType = TrainingType.CrossFit;
        DateTime today = DateTime.Today;
        DateTime tomorrow = DateTime.Today.AddDays(1);
        
        TrainingEntity crossfit = new TrainingEntity
        {
            Date = today,
            TrainingType = dummyType
        }; 
        
        TrainingEntity legDay = new TrainingEntity
        {
            Date = tomorrow,
            TrainingType = dummyType
        }; 
        
        TrainingEntity weightLifting = new TrainingEntity
        {
            Date = today,
            TrainingType = dummyType
        };

        await _dbContext.Trainings.AddAsync(crossfit);
        await _dbContext.Trainings.AddAsync(legDay);
        await _dbContext.Trainings.AddAsync(weightLifting);
        await _dbContext.SaveChangesAsync();

        var result = await _repository.GetTrainingsByDate(DateOnly.FromDateTime(today));
        List<TrainingEntity> expected = new List<TrainingEntity>
        {
            crossfit, weightLifting
        };
        
        Assert.That(result, Is.EquivalentTo(expected));
    }
    
    [Test]
    public async Task GetTrainingsByType_ReturnsCorrectTrainings()
    {
        DateTime dummyDate = DateTime.Today;
        
        TrainingEntity functionalBodyBuilding1 = new TrainingEntity
        {
            Date = dummyDate,
            TrainingType = TrainingType.FunctionalBodyBuilding
        }; 
        
        TrainingEntity crossfit = new TrainingEntity
        {
            Date = dummyDate,
            TrainingType = TrainingType.CrossFit
        }; 
        
        TrainingEntity functionalBodyBuilding2 = new TrainingEntity
        {
            Date = dummyDate,
            TrainingType = TrainingType.FunctionalBodyBuilding
        };

        await _dbContext.Trainings.AddAsync(functionalBodyBuilding1);
        await _dbContext.Trainings.AddAsync(crossfit);
        await _dbContext.Trainings.AddAsync(functionalBodyBuilding2);
        await _dbContext.SaveChangesAsync();

        var result = await _repository.GetTrainingsByType(TrainingType.FunctionalBodyBuilding);
        List<TrainingEntity> expected = new List<TrainingEntity>
        {
            functionalBodyBuilding1, functionalBodyBuilding2
        };
        
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public async Task UpdateTraining_UpdatesTrainingSuccessfully()
    {
        TrainingEntity crossfit = new TrainingEntity
        {
            Date = new DateTime(2025, 4, 10),
            TrainingType = TrainingType.CrossFit
        };

        await _dbContext.Trainings.AddAsync(crossfit);
        await _dbContext.SaveChangesAsync();

        TrainingEntity trainingToUpdate = await _repository.GetById(crossfit.Id);

        DateTime newDate = crossfit.Date.AddDays(1);
        crossfit.Date = newDate;
        await _repository.UpdateTraining(trainingToUpdate);

        var result = await _repository.GetById(trainingToUpdate.Id);
        CompareTwoTrainingEntities(result, trainingToUpdate);
    }

    [Test]
    public async Task DeleteTraining_DeletesSuccessfully()
    {
        TrainingType dummyType = TrainingType.CrossFit;
        DateTime dummyDate = DateTime.Today;
        
        TrainingEntity crossfit = new TrainingEntity
        {
            Date = dummyDate,
            TrainingType = dummyType
        }; 
        
        TrainingEntity legDay = new TrainingEntity
        {
            Date = dummyDate,
            TrainingType = TrainingType.LegDay
        }; 
        
        TrainingEntity weightLifting = new TrainingEntity
        {
            Date = dummyDate,
            TrainingType = dummyType
        };

        await _dbContext.Trainings.AddAsync(crossfit);
        await _dbContext.Trainings.AddAsync(legDay);
        await _dbContext.Trainings.AddAsync(weightLifting);
        await _dbContext.SaveChangesAsync();

        var firstResult = await _repository.GetTrainingsByDate(DateOnly.FromDateTime(dummyDate));

        List<TrainingEntity> expected = new List<TrainingEntity>
        {
            crossfit, legDay, weightLifting
        };
        
        Assert.That(firstResult, Is.EquivalentTo(expected));

        TrainingEntity legDayAquired = await _repository.GetById(legDay.Id);
        
        await _repository.DeleteTraining(legDayAquired.Id);

        var afterDelete = await _repository.GetTrainingsByDate(DateOnly.FromDateTime(dummyDate));

        List<TrainingEntity> expectedAfterDelete = new List<TrainingEntity>
        {
            crossfit, weightLifting
        };
        Assert.That(afterDelete, Is.EquivalentTo(expectedAfterDelete));
    }
    
    private void CompareTwoTrainingEntities(TrainingEntity actualTraining, TrainingEntity expected)
    {
        Assert.That(actualTraining, Is.Not.Null);
        Assert.That(actualTraining.Id, Is.Not.Null);
        Assert.That(actualTraining.Date, Is.EqualTo(expected.Date));
        Assert.That(actualTraining.TrainingType, Is.EqualTo(expected.TrainingType));
    }
}