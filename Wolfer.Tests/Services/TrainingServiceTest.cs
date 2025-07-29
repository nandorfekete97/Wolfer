using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Moq.Protected;
using Wolfer.Data;
using Wolfer.Data.DTOs;
using Wolfer.Data.Entities;
using Wolfer.Repositories;
using Wolfer.Services;

namespace Wolfer.Tests.Services;

[TestFixture]
[TestOf(typeof(TrainingService))]
public class TrainingServiceTest
{
    private Mock<ITrainingRepository> _trainingRepositoryMock;
    private Mock<IUserTrainingRepository> _userTrainingRepositoryMock;
    private TrainingService _trainingService;

    [SetUp]
    public void SetUp()
    {
        _trainingRepositoryMock = new Mock<ITrainingRepository>();
        _userTrainingRepositoryMock = new Mock<IUserTrainingRepository>();
        _trainingService = new TrainingService(_trainingRepositoryMock.Object, _userTrainingRepositoryMock.Object);
    }
    
    [Test]
    public async Task GetById_ValidId_ReturnsTraining()
    {
        int trainingId = 1;
        var expectedTraining = new TrainingEntity { Id = trainingId };

        _trainingRepositoryMock.Setup(repository => repository.GetById(trainingId)).ReturnsAsync(expectedTraining);

        var result = await _trainingService.GetById(trainingId);
        
        Assert.That(result, Is.EqualTo(expectedTraining));
        _trainingRepositoryMock.Verify(repository => repository.GetById(trainingId), Times.Once);
    }

    [Test]
    public async Task GetById_InvalidId_ThrowsException()
    {
        int trainingId = 0;

        var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _trainingService.GetById(trainingId));
        
        Assert.That(exception.Message, Is.EqualTo("ID must be positive integer."));
    }

    [Test]
    public async Task GetById_NonExistentId_ReturnsNull()
    {
        int nonExistingTrainingId = 1;
        
        _trainingRepositoryMock.Setup(repository => repository.GetById(nonExistingTrainingId)).ReturnsAsync((TrainingEntity)null);

        var result = await _trainingService.GetById(nonExistingTrainingId);
        
        _trainingRepositoryMock.Verify(repository => repository.GetById(nonExistingTrainingId), Times.Once);
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetTrainingsByDate_ValidDate_ReturnsTrainings()
    {
        DateOnly wantedDate = new DateOnly(2025, 6, 5);

        TrainingEntity expectedTraining1 = new TrainingEntity { Date = wantedDate.ToDateTime(TimeOnly.MinValue) };
        TrainingEntity expectedTraining2 = new TrainingEntity { Date = wantedDate.ToDateTime(TimeOnly.MaxValue) };

        List<TrainingEntity> expectedTrainings = new List<TrainingEntity> { expectedTraining1, expectedTraining2 };

        _trainingRepositoryMock.Setup(repository => repository.GetTrainingsByDate(wantedDate))
            .ReturnsAsync(expectedTrainings);

        var result = await _trainingService.GetTrainingsByDate(wantedDate);
        
        Assert.That(result, Is.EquivalentTo(expectedTrainings));
        _trainingRepositoryMock.Verify(r => r.GetTrainingsByDate(wantedDate), Times.Once);
    }

    [Test]
    public async Task GetTrainingsByDate_InvalidDate_ThrowsException()
    {
        DateOnly invalidDate = default;

        var exception =
            Assert.ThrowsAsync<ArgumentException>(async () => await _trainingService.GetTrainingsByDate(invalidDate));
        
        Assert.That(exception.Message, Is.EqualTo("Invalid date."));
    }
    
    [Test]
    public async Task GetTrainingsByType_ValidType_ReturnsTrainings()
    {
        TrainingType validType = TrainingType.WeightLifting;

        TrainingEntity training1 = new TrainingEntity { TrainingType = validType };
        TrainingEntity training3 = new TrainingEntity { TrainingType = validType };

        List<TrainingEntity> expectedTrainings = new List<TrainingEntity> { training1, training3 };

        _trainingRepositoryMock.Setup(repository => repository.GetTrainingsByType(validType))
            .ReturnsAsync(expectedTrainings);

        var result = await _trainingService.GetTrainingsByType(validType);
        
        Assert.That(result, Is.EquivalentTo(expectedTrainings));
        _trainingRepositoryMock.Verify(repository => repository.GetTrainingsByType(validType), Times.Once);
    }

    [Test]
    public async Task GetTrainingsByType_InvalidType_ThrowsException()
    {
        var invalidType = (TrainingType)999;

        var exception =
            Assert.ThrowsAsync<ArgumentException>(async () => await _trainingService.GetTrainingsByType(invalidType));
        
        Assert.That(exception.Message, Is.EqualTo("Invalid training type."));
    }
    
    [Test]
    public async Task CreateTraining_SuccessfullyCreatesTraining()
    {
        TrainingDTO trainingDto = new TrainingDTO
        {
            Date = new DateTime(2025, 06, 05, 14, 45, 00),
            TrainingType = TrainingType.WeightLifting
        };

        _trainingRepositoryMock.Setup(repository => repository.CreateTraining(It.IsAny<TrainingEntity>()))
            .Returns(Task.CompletedTask);

        var trainingServiceMock = new Mock<TrainingService>(_trainingRepositoryMock.Object) { CallBase = true };

        trainingServiceMock.Protected()
            .Setup<Task<bool>>("IsTrainingTimeOccupied", ItExpr.IsAny<DateTime>(), ItExpr.IsAny<int>()).ReturnsAsync(false);

        await trainingServiceMock.Object.CreateTraining(trainingDto);
        
        _trainingRepositoryMock.Verify(repository => repository.CreateTraining(It.Is<TrainingEntity>(entity => entity.Date == trainingDto.Date && entity.TrainingType == trainingDto.TrainingType)), Times.Once);
    }

    [Test]
    public async Task CreateTraining_IdNotZero_ThrowsException()
    {
        TrainingDTO trainingDto = new TrainingDTO
        {
            Id = 1
        };
        
        var exception =
            Assert.ThrowsAsync<ArgumentException>(async () => await _trainingService.CreateTraining(trainingDto));
        
        Assert.That(exception.Message, Is.EqualTo("Trainer ID must be null."));
    }

    [Test]
    public async Task CreateTraining_DefaultDate_ThrowsException()
    {
        TrainingDTO trainingDto = new TrainingDTO
        {
            Date = default
        };
        
        var exception =
            Assert.ThrowsAsync<ArgumentException>(async () => await _trainingService.CreateTraining(trainingDto));
        
        Assert.That(exception.Message, Is.EqualTo("Date must be set."));
    }

    [Test]
    public async Task CreateTraining_UndefinedTrainingType_ThrowsException()
    {
        var invalidTrainingType = (TrainingType)999;
        DateTime validDate = DateTime.Today;

        TrainingDTO trainingDto = new TrainingDTO { TrainingType = invalidTrainingType, Date = validDate};
        
        var exception =
            Assert.ThrowsAsync<ArgumentException>(async () => await _trainingService.CreateTraining(trainingDto));
        
        Assert.That(exception.Message, Is.EqualTo("Invalid training type."));
    }
    
    [Test]
    public async Task CreateTraining_TrainingIsOccupied_ThrowsException()
    {
        TrainingDTO trainingDto = new TrainingDTO
        {
            Date = new DateTime(2025, 06, 05, 14, 45, 00),
            TrainingType = TrainingType.WeightLifting
        };

        var trainingServiceMock = new Mock<TrainingService>(_trainingRepositoryMock.Object) { CallBase = true };

        trainingServiceMock.Protected()
            .Setup<Task<bool>>("IsTrainingTimeOccupied", trainingDto.Date, trainingDto.Id).ReturnsAsync(true);

        var exception =
            Assert.ThrowsAsync<ArgumentException>(async () =>
                await trainingServiceMock.Object.CreateTraining(trainingDto));
        
        Assert.That(exception.Message, Is.EqualTo("Training date is occupied."));
    }
    
    [Test]
    public async Task UpdateTraining_SuccessfullyUpdatesTraining()
    {
        TrainingDTO trainingDto = new TrainingDTO
        {
            Id = 1,
            Date = new DateTime(2025, 6, 21, 14, 45, 00),
            TrainingType = TrainingType.WeightLifting
        };

        var existingEntity = new TrainingEntity
        {
            Id = 1,
            Date = new DateTime(2025, 6,19, 14, 45, 00), TrainingType = TrainingType.WeightLifting
        };

        _trainingRepositoryMock.Setup(repository => repository.GetById(trainingDto.Id)).ReturnsAsync(existingEntity);

        _trainingRepositoryMock.Setup(repository => repository.UpdateTraining(It.IsAny<TrainingEntity>()))
            .Returns(Task.CompletedTask);

        var trainingServiceMock = new Mock<TrainingService>(_trainingRepositoryMock.Object);

        trainingServiceMock.Protected()
            .Setup<Task<bool>>("IsTrainingTimeOccupied", ItExpr.IsAny<DateTime>(), ItExpr.IsAny<int>())
            .ReturnsAsync(false);

        await trainingServiceMock.Object.UpdateTraining(trainingDto);
        
        _trainingRepositoryMock.Verify(repository => repository.GetById(It.IsAny<int>()), Times.Once);
        _trainingRepositoryMock.Verify(repository => repository.UpdateTraining(It.Is<TrainingEntity>(t => t.Id == trainingDto.Id && t.Date == trainingDto.Date && t.TrainingType == trainingDto.TrainingType)), Times.Once);
        
        trainingServiceMock.Protected().Verify("IsTrainingTimeOccupied", Times.Once(), ItExpr.Is<DateTime>(date => date == trainingDto.Date), ItExpr.Is<int>(id => id == trainingDto.Id));
    }

    [Test]
    public async Task UpdateTraining_NonExistentId_ThrowsException()
    {
        int nonExistentId = 666;
        
        TrainingDTO trainingDto = new TrainingDTO
        {
            Id = nonExistentId,
            Date = DateTime.Today,
            TrainingType = TrainingType.WeightLifting
        };
        
        var trainingServiceMock = new Mock<TrainingService>(_trainingRepositoryMock.Object) { CallBase = true };

        _trainingRepositoryMock.Setup(repository => repository.GetById(nonExistentId))
            .ReturnsAsync((TrainingEntity)null);

        var exception =
            Assert.ThrowsAsync<ArgumentException>(async () =>
                await trainingServiceMock.Object.UpdateTraining(trainingDto));
        
        Assert.That(exception.Message, Is.EqualTo("Invalid ID."));
    }

    [Test]
    public async Task DeleteTraining_SuccessfullyDeletesTraining()
    {
        int trainingId = 1;

        await _trainingService.DeleteTraining(trainingId);
        
        _trainingRepositoryMock.Verify(repository => repository.DeleteById(trainingId), Times.Once);
        _userTrainingRepositoryMock.Verify(repository => repository.DeleteByTrainingId(trainingId), Times.Once);
    }

    [Test]
    public async Task DeleteTraining_InvalidId_ThrowsException()
    {
        int invalidTrainingId = 0;

        var exception =
            Assert.ThrowsAsync<ArgumentException>(async () => await _trainingService.DeleteTraining(invalidTrainingId));
        
        Assert.That(exception.Message, Is.EqualTo("Invalid ID."));
        _trainingRepositoryMock.Verify(repository => repository.DeleteById(invalidTrainingId), Times.Never);
    }
}