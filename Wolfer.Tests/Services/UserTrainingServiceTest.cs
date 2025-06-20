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
    public async Task GetByUserId_SuccessfullyReturnsTrainings()
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

        List<TrainingEntity> trainingEntities = new List<TrainingEntity> { training1, training2 };

        _trainingRepoMock.Setup(repository => repository.GetByIds(trainingIds)).ReturnsAsync(trainingEntities);

        var result = await _userTrainingService.GetByUserId(userId);
        
        Assert.That(result, Is.EquivalentTo(trainingEntities));
    }
}