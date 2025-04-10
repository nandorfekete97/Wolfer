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
    public async Task CreateTrainer_AddsTrainerSuccessfully()
    {
        TrainerEntity trainer = new TrainerEntity()
        {
            Id = 123,
            FirstName = "Ákos",
            LastName = "Lengyel",
            Username = "wolf",
            Password = "cornisland25",
            Email = "akos@lengyel.com"
        };

        await _repository.CreateTrainer(trainer);

        var result = await _repository.GetById(123);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.FirstName, Is.EqualTo("Ákos"));
    }

    [Test]
    public async Task GetByUserName_ReturnsCorrectTrainer()
    {
        TrainerEntity trainer = new TrainerEntity
        {
            Id = 2,
            FirstName = "Sziszi",
            LastName = "Ravasz",
            Username = "sziszi",
            Password = "cornisland123",
            Email = "sziszi@ravasz.com"
        };

        await _dbContext.Trainers.AddAsync(trainer);
        await _dbContext.SaveChangesAsync();

        var result = await _repository.GetByUserName("sziszi");
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.LastName, Is.EqualTo("Ravasz"));
    }
}