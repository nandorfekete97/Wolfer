using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using Wolfer.Data;
using Wolfer.Data.Entities;
using Wolfer.Repositories;
using Wolfer.Services;

namespace Wolfer.Tests.Services;

[TestFixture]
[TestOf(typeof(UserTrainingService))]
public class UserTrainingServiceTest
{
    private Mock<IUserTrainingRepository> _userTrainingRepoMock;
    private Mock<IUserRepository> _userRepoMock;
    private Mock<ITrainingRepository> _trainingRepoMock;
    private IUserTrainingService _userTrainingService;

    [SetUp]
    public void SetUp()
    {
        _userTrainingRepoMock = new Mock<IUserTrainingRepository>();
        _userRepoMock = new Mock<IUserRepository>();
        _trainingRepoMock = new Mock<ITrainingRepository>();
        _userTrainingService = new UserTrainingService(_userTrainingRepoMock.Object, _userRepoMock.Object, _trainingRepoMock.Object);
    }
    
    [Test]
    public async Task GetFutureTrainingsByUserId_SuccessfullyReturnsTrainings()
    {
        Guid userId = Guid.NewGuid();
        IdentityUser user = new IdentityUser { Id = userId.ToString() };

        _userRepoMock.Setup(repository => repository.GetUserById(userId)).ReturnsAsync(user);

        var now = new DateTime(2025, 6, 21, 12, 0, 0);

        TrainingEntity training1 = new TrainingEntity
            { Id = 1, TrainingType = TrainingType.WeightLifting, Date = DateTime.Now.AddDays(1) };
        TrainingEntity training2 = new TrainingEntity
            { Id = 2, TrainingType = TrainingType.FunctionalBodyBuilding, Date = DateTime.Now.AddDays(2) };
        TrainingEntity pastTraining = new TrainingEntity
            { Id = 3, TrainingType = TrainingType.CrossFit, Date = now.AddDays(-1) };

        UserTrainingEntity userTrainingEntity1 = new UserTrainingEntity { UserId = userId, TrainingId = training1.Id };
        UserTrainingEntity userTrainingEntity2 = new UserTrainingEntity { UserId = userId, TrainingId = training2.Id };
        UserTrainingEntity userTrainingEntity3 = new UserTrainingEntity
            { UserId = userId, TrainingId = pastTraining.Id };
        
        List<UserTrainingEntity> userTrainingEntities = new List<UserTrainingEntity>
            { userTrainingEntity1, userTrainingEntity2, userTrainingEntity3 };

        _userTrainingRepoMock.Setup(repository => repository.GetByUserId(userId)).ReturnsAsync(userTrainingEntities);

        List<int> trainingIds = new List<int> { userTrainingEntity1.TrainingId, userTrainingEntity2.TrainingId, userTrainingEntity3.TrainingId };

        List<TrainingEntity> trainingEntities = new List<TrainingEntity> { training1, training2, pastTraining };

        _trainingRepoMock.Setup(repository => repository.GetByIds(trainingIds)).ReturnsAsync(trainingEntities);

        List<TrainingEntity> expectedTrainings = new List<TrainingEntity> { training1, training2 };
        
        var result = await _userTrainingService.GetFutureTrainingsByUserId(userId);
        
        Assert.That(result, Is.EquivalentTo(expectedTrainings));
        _userRepoMock.Verify(repository => repository.GetUserById(It.IsAny<Guid>()), Times.Once);
        _userTrainingRepoMock.Verify(repository => repository.GetByUserId(It.IsAny<Guid>()), Times.Once);
        _trainingRepoMock.Verify(repository => repository.GetByIds(It.IsAny<List<int>>()), Times.Once);
    }

    [Test]
    public async Task GetFutureTrainingsByUserId_UserIsNull_ThrowsException()
    {
        Guid userId = Guid.NewGuid();
        
        _userRepoMock.Setup(repository => repository.GetUserById(userId))
            .ReturnsAsync((IdentityUser)null);

        var exception =
            Assert.ThrowsAsync<ArgumentException>(async () => await _userTrainingService.GetFutureTrainingsByUserId(userId));
        
        Assert.That(exception.Message, Is.EqualTo("Invalid ID."));
        _userRepoMock.Verify(repository => repository.GetUserById(It.IsAny<Guid>()), Times.Once);
        _userTrainingRepoMock.Verify(repository => repository.GetByUserId(It.IsAny<Guid>()), Times.Never);
        _trainingRepoMock.Verify(repository => repository.GetByIds(It.IsAny<List<int>>()), Times.Never);
    }

    [Test]
    public async Task GetPastTrainingsByUserId_SuccessfullyReturnsTrainings()
    {
        Guid userId = Guid.NewGuid();
        IdentityUser user = new IdentityUser { Id = userId.ToString() };

        _userRepoMock.Setup(repository => repository.GetUserById(userId)).ReturnsAsync(user);
        
        var now = new DateTime(2025, 6, 21, 12, 0, 0);

        TrainingEntity pastTraining1 = new TrainingEntity
            { Id = 1, TrainingType = TrainingType.WeightLifting, Date = DateTime.Now.AddDays(-1) };
        TrainingEntity futureTraining = new TrainingEntity
            { Id = 2, TrainingType = TrainingType.FunctionalBodyBuilding, Date = DateTime.Now.AddDays(2) };
        TrainingEntity pastTraining2 = new TrainingEntity
            { Id = 3, TrainingType = TrainingType.CrossFit, Date = now.AddDays(-2) };
        
        UserTrainingEntity userTrainingEntity1 = new UserTrainingEntity { UserId = userId, TrainingId = pastTraining1.Id };
        UserTrainingEntity userTrainingEntity2 = new UserTrainingEntity { UserId = userId, TrainingId = futureTraining.Id };
        UserTrainingEntity userTrainingEntity3 = new UserTrainingEntity
            { UserId = userId, TrainingId = pastTraining2.Id };
        
        List<UserTrainingEntity> userTrainingEntities = new List<UserTrainingEntity>
            { userTrainingEntity1, userTrainingEntity2, userTrainingEntity3 };

        _userTrainingRepoMock.Setup(repository => repository.GetByUserId(userId)).ReturnsAsync(userTrainingEntities);
        
        List<int> trainingIds = new List<int> { userTrainingEntity1.TrainingId, userTrainingEntity2.TrainingId, userTrainingEntity3.TrainingId };
        
        List<TrainingEntity> allTrainingsByUser = new List<TrainingEntity> { pastTraining1, pastTraining2, futureTraining };

        _trainingRepoMock.Setup(repository => repository.GetByIds(trainingIds)).ReturnsAsync(allTrainingsByUser);

        List<TrainingEntity> expectedTrainings = new List<TrainingEntity> { pastTraining1, pastTraining2 };

        var result = await _userTrainingService.GetPastTrainingsByUserId(userId);
        
        Assert.That(result, Is.EquivalentTo(expectedTrainings));
        _userRepoMock.Verify(repository => repository.GetUserById(It.IsAny<Guid>()), Times.Once);
        _userTrainingRepoMock.Verify(repository => repository.GetByUserId(It.IsAny<Guid>()), Times.Once);
        _trainingRepoMock.Verify(repository => repository.GetByIds(It.IsAny<List<int>>()), Times.Once);
    }
    
    [Test]
    public async Task GetPastTrainingsByUserId_UserIsNull_ThrowsException()
    {
        Guid userId = Guid.NewGuid();
        
        _userRepoMock.Setup(repository => repository.GetUserById(userId))
            .ReturnsAsync((IdentityUser)null);

        var exception =
            Assert.ThrowsAsync<ArgumentException>(async () => await _userTrainingService.GetPastTrainingsByUserId(userId));
        
        Assert.That(exception.Message, Is.EqualTo("Invalid ID."));
        _userRepoMock.Verify(repository => repository.GetUserById(It.IsAny<Guid>()), Times.Once);
        _userTrainingRepoMock.Verify(repository => repository.GetByUserId(It.IsAny<Guid>()), Times.Never);
        _trainingRepoMock.Verify(repository => repository.GetByIds(It.IsAny<List<int>>()), Times.Never);
    }

    [Test]
    public async Task GetByTrainingId_SuccessfullyReturnsUsers()
    {
        TrainingEntity training = new TrainingEntity { Id = 1 };

        _trainingRepoMock.Setup(repository => repository.GetById(training.Id)).ReturnsAsync(training);

        Guid user1Id = Guid.NewGuid();
        Guid user2Id = Guid.NewGuid();
        IdentityUser user1 = new IdentityUser { Id = user1Id.ToString() };
        IdentityUser user2 = new IdentityUser { Id = user2Id.ToString() };

        int differentTrainingId = 99;
        
        UserTrainingEntity userTraining1 = new UserTrainingEntity { TrainingId = training.Id, UserId = user1Id };
        UserTrainingEntity userTraining2 = new UserTrainingEntity { TrainingId = training.Id, UserId = user2Id };

        List<UserTrainingEntity> expectedUserTrainingEntities =
            new List<UserTrainingEntity> { userTraining1, userTraining2 };
        
        _userTrainingRepoMock.Setup(repository => repository.GetByTrainingId(training.Id))
            .ReturnsAsync(expectedUserTrainingEntities);

        List<IdentityUser> expectedUsers = new List<IdentityUser> { user1, user2 };

        List<Guid> userIds = new List<Guid> { user1Id, user2Id };

        _userRepoMock.Setup(repository => repository.GetByIds(userIds)).ReturnsAsync(expectedUsers);
        
        var result = await _userTrainingService.GetByTrainingId(training.Id);
        
        Assert.That(result, Is.EquivalentTo(expectedUsers));
        _trainingRepoMock.Verify(repository => repository.GetById(It.IsAny<int>()), Times.Once());
        _userTrainingRepoMock.Verify(repository => repository.GetByTrainingId(It.IsAny<int>()), Times.Once());
        _userRepoMock.Verify(repository => repository.GetByIds(It.IsAny<List<Guid>>()), Times.Once);
    }

    [Test]
    public async Task GetByTrainingId_InvalidId_ThrowsException()
    {
        int trainingId = 0;

        var exception =
            Assert.ThrowsAsync<ArgumentException>(async () => await _userTrainingService.GetByTrainingId(trainingId));
        
        Assert.That(exception.Message, Is.EqualTo("ID must be positive integer."));
    }

    [Test]
    public async Task GetByTrainingId_NullTraining_ThrowsException()
    {
        int trainingId = 1;

        _trainingRepoMock.Setup(repository => repository.GetById(trainingId)).ReturnsAsync((TrainingEntity)null);

        var exception =
            Assert.ThrowsAsync<ArgumentException>(async () => await _userTrainingService.GetByTrainingId(trainingId));
        
        Assert.That(exception.Message, Is.EqualTo("Invalid training ID."));
    }

    [Test]
    public async Task SignUpUserToTraining_SuccessfullyCreatesUserTraining()
    {
        Guid userId = Guid.NewGuid();
        IdentityUser user = new IdentityUser { Id = userId.ToString() };

        int trainingId = 1;
        TrainingEntity training = new TrainingEntity
        {
            Id = trainingId
        };

        _userRepoMock.Setup(repository => repository.GetUserById(userId)).ReturnsAsync(user);
        _trainingRepoMock.Setup(repository => repository.GetById(trainingId)).ReturnsAsync(training);
        _userTrainingRepoMock.Setup(repository => repository.GetByUserIdAndTrainingId(userId, trainingId))
            .ReturnsAsync((UserTrainingEntity)null);

        await _userTrainingService.SignUpUserToTraining(userId, trainingId);
        
        _userRepoMock.Verify(repository => repository.GetUserById(userId), Times.Once);
        _trainingRepoMock.Verify(repository => repository.GetById(trainingId), Times.Once);
        _userTrainingRepoMock.Verify(repository => repository.GetByUserIdAndTrainingId(userId, trainingId), Times.Once);
        
        _userTrainingRepoMock.Verify(repository => repository.Create(It.Is<UserTrainingEntity>(entity => entity.UserId == userId && entity.TrainingId == trainingId)));
    }
}