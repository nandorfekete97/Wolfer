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
        TrainingType crossFitType = TrainingType.CrossFit;
        DateTime today = DateTime.Today;
        
        TrainingEntity crossfit = new TrainingEntity
        {
            Date = today,
            TrainingType = crossFitType
        };

        TrainingEntity weightLifting = new TrainingEntity
        {
            Date = today,
            TrainingType = crossFitType
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
        TrainingType crossFitType = TrainingType.CrossFit;
        DateTime today = DateTime.Today;
        
        TrainingEntity crossfitTraining = new TrainingEntity
        {
            Date = today,
            TrainingType = crossFitType
        }; 
        
        TrainingEntity weightLiftingTraining = new TrainingEntity
        {
            Date = today,
            TrainingType = crossFitType
        };

        await _repository.CreateTraining(weightLiftingTraining);
        await _repository.CreateTraining(crossfitTraining);

        var crossfitResult = await _repository.GetById(crossfitTraining.Id);
        var weightLiftingResult = await _repository.GetById(weightLiftingTraining.Id);
        
        CompareTwoTrainingEntities(crossfitResult, crossfitTraining);
        CompareTwoTrainingEntities(weightLiftingResult, weightLiftingTraining);
    }

    [Test]
    public async Task GetTrainingsByDate_ReturnsCorrectTrainings()
    {
        TrainingType crossFitType = TrainingType.CrossFit;
        DateTime today = DateTime.Today;
        DateTime tomorrow = DateTime.Today.AddDays(1);
        
        TrainingEntity crossfitTraining = new TrainingEntity
        {
            Date = today,
            TrainingType = crossFitType
        }; 
        
        TrainingEntity legDayTraining = new TrainingEntity
        {
            Date = tomorrow,
            TrainingType = crossFitType
        }; 
        
        TrainingEntity weightLiftingTraining = new TrainingEntity
        {
            Date = today,
            TrainingType = crossFitType
        };

        await _dbContext.Trainings.AddAsync(crossfitTraining);
        await _dbContext.Trainings.AddAsync(legDayTraining);
        await _dbContext.Trainings.AddAsync(weightLiftingTraining);
        await _dbContext.SaveChangesAsync();

        var result = await _repository.GetTrainingsByDate(DateOnly.FromDateTime(today));
        List<TrainingEntity> expected = new List<TrainingEntity>
        {
            crossfitTraining, weightLiftingTraining
        };
        
        Assert.That(result, Is.EquivalentTo(expected));
    }
    
    [Test]
    public async Task GetTrainingsByType_ReturnsCorrectTrainings()
    {
        DateTime today = DateTime.Today;
        
        TrainingEntity functionalTraining1 = new TrainingEntity
        {
            Date = today,
            TrainingType = TrainingType.FunctionalBodyBuilding
        }; 
        
        TrainingEntity crossfitTraining = new TrainingEntity
        {
            Date = today,
            TrainingType = TrainingType.CrossFit
        }; 
        
        TrainingEntity functionalTraining2 = new TrainingEntity
        {
            Date = today,
            TrainingType = TrainingType.FunctionalBodyBuilding
        };

        await _dbContext.Trainings.AddAsync(functionalTraining1);
        await _dbContext.Trainings.AddAsync(crossfitTraining);
        await _dbContext.Trainings.AddAsync(functionalTraining2);
        await _dbContext.SaveChangesAsync();

        var result = await _repository.GetTrainingsByType(TrainingType.FunctionalBodyBuilding);
        List<TrainingEntity> expected = new List<TrainingEntity>
        {
            functionalTraining1, functionalTraining2
        };
        
        Assert.That(result, Is.EquivalentTo(expected));
    }

    [Test]
    public async Task UpdateTraining_UpdatesTrainingSuccessfully()
    {
        TrainingEntity crossfitTraining = new TrainingEntity
        {
            Date = new DateTime(2025, 4, 10),
            TrainingType = TrainingType.CrossFit
        };

        await _dbContext.Trainings.AddAsync(crossfitTraining);
        await _dbContext.SaveChangesAsync();

        TrainingEntity trainingToUpdate = await _repository.GetById(crossfitTraining.Id);

        DateTime newDate = crossfitTraining.Date.AddDays(1);
        crossfitTraining.Date = newDate;
        await _repository.UpdateTraining(trainingToUpdate);

        var result = await _repository.GetById(trainingToUpdate.Id);
        CompareTwoTrainingEntities(result, trainingToUpdate);
    }

    [Test]
    public async Task DeleteTraining_DeletesSuccessfully()
    {
        TrainingType crossFitType = TrainingType.CrossFit;
        DateTime today = DateTime.Today;
        
        TrainingEntity crossfitTraining = new TrainingEntity
        {
            Date = today,
            TrainingType = crossFitType
        }; 
        
        TrainingEntity legDayTraining = new TrainingEntity
        {
            Date = today,
            TrainingType = TrainingType.LegDay
        }; 
        
        TrainingEntity weightLiftingTraining = new TrainingEntity
        {
            Date = today,
            TrainingType = crossFitType
        };

        await _dbContext.Trainings.AddAsync(crossfitTraining);
        await _dbContext.Trainings.AddAsync(legDayTraining);
        await _dbContext.Trainings.AddAsync(weightLiftingTraining);
        await _dbContext.SaveChangesAsync();

        var firstResult = await _repository.GetTrainingsByDate(DateOnly.FromDateTime(today));

        List<TrainingEntity> expected = new List<TrainingEntity>
        {
            crossfitTraining, legDayTraining, weightLiftingTraining
        };
        
        Assert.That(firstResult, Is.EquivalentTo(expected));

        TrainingEntity legDayAquired = await _repository.GetById(legDayTraining.Id);
        
        await _repository.DeleteTraining(legDayAquired.Id);

        var afterDelete = await _repository.GetTrainingsByDate(DateOnly.FromDateTime(today));

        List<TrainingEntity> expectedAfterDelete = new List<TrainingEntity>
        {
            crossfitTraining, weightLiftingTraining
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