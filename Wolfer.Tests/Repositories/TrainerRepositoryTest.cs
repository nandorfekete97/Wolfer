using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Wolfer.Data.Context;
using Wolfer.Data.Entities;
using Wolfer.Repositories;

namespace Wolfer.Tests.Repositories;

[TestFixture]
[TestOf(typeof(TrainerRepository))]
public class TrainerRepositoryTest
{
    private WolferContext _dbContext;
    private TrainerRepository _repository;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<WolferContext>()
            .UseInMemoryDatabase(databaseName: "WolferTestDb")
            .Options;

        _dbContext = new WolferContext(options);

        _repository = new TrainerRepository(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task GetByIdFails_IfIdNonExistent()
    {
        int wrongId = -1;

        var result = await _repository.GetById(wrongId);
        
        Assert.That(result, Is.Null);
    }
    
    // test getbyid separately
    // test getbyusername separately

    [Test]
    public async Task CreateTrainer_AddsTrainerSuccessfully()
    {
        TrainerEntity trainer = new TrainerEntity()
        {
            FirstName = "Ákos",
            LastName = "Lengyel",
            Username = "wolf",
            Password = "cornisland25",
            Email = "akos@lengyel.com"
        };

        await _repository.CreateTrainer(trainer);
        
        var result = await _repository.GetByUserName(trainer.Username);

        CompareTwoTrainerEntities(result, trainer);
    }

    [Test]
    public async Task GetByUserName_ReturnsCorrectTrainer()
    {
        TrainerEntity trainer = new TrainerEntity
        {
            FirstName = "Sziszi",
            LastName = "Ravasz",
            Username = "sziszi",
            Password = "cornisland123",
            Email = "sziszi@ravasz.com"
        };

        await _dbContext.Trainers.AddAsync(trainer);
        await _dbContext.SaveChangesAsync();

        var result = await _repository.GetByUserName("sziszi");
        
        CompareTwoTrainerEntities(result, trainer);
    }

    [Test]
    public async Task UpdateTrainer_UpdatesTrainerSuccessfully()
    {
        TrainerEntity trainer = new TrainerEntity
        {
            FirstName = "Sziszi",
            LastName = "Ravasz",
            Username = "sziszi",
            Password = "cornisland123",
            Email = "sziszi@ravasz.com"
        };

        await _dbContext.Trainers.AddAsync(trainer);
        await _dbContext.SaveChangesAsync();

        TrainerEntity trainerToUpdate = await _repository.GetByUserName(trainer.Username);

        string newUserName = "sziszike";
        trainerToUpdate.Username = newUserName;
        // research into why test is also functional with comparing to original trainer
        // because by updating trainerToUpdate property, original trainer object is updated as well
        await _repository.UpdateTrainer(trainerToUpdate);
        
        var result = await _repository.GetById(trainerToUpdate.Id);
        CompareTwoTrainerEntities(result, trainerToUpdate);
    }

    private void CompareTwoTrainerEntities(TrainerEntity actualTrainer, TrainerEntity expected)
    {
        Assert.That(actualTrainer, Is.Not.Null);
        Assert.That(actualTrainer.Id, Is.Not.Null);
        Assert.That(actualTrainer.FirstName, Is.EqualTo(expected.FirstName));
        Assert.That(actualTrainer.LastName, Is.EqualTo(expected.LastName));
        Assert.That(actualTrainer.Username, Is.EqualTo(expected.Username));
        Assert.That(actualTrainer.Password, Is.EqualTo(expected.Password));
        Assert.That(actualTrainer.Email, Is.EqualTo(expected.Email));
    }
}