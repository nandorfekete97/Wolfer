using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using NUnit.Framework;
using Wolfer.Data.Context;
using Wolfer.Data.Entities;
using Wolfer.Repositories;

namespace Wolfer.Tests.Repositories;

[TestFixture]
[TestOf(typeof(UserRepository))]
public class UserRepositoryTest
{
    private WolferContext _dbContext;
    private UserRepository _repository;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<WolferContext>().UseInMemoryDatabase(databaseName: "WolferTestDb")
            .Options;

        
        _dbContext = new WolferContext(options);

        _repository = new UserRepository(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
    
    [Test]
    public async Task GetById_ReturnsUserSuccessfully()
    {
        UserEntity user = new UserEntity
        {
            Email = "nandor@fekete.com",
            FirstName = "Nándor",
            LastName = "Fekete",
            Password = "spurs97",
            Username = "nanu97"
        };

        _dbContext.Users.AddAsync(user);
        _dbContext.SaveChangesAsync();

        var result = await _repository.GetUserById(user.Id);

        CompareTwoUserEntities(result, user);
    }
    
    [Test]
    public async Task GetByIdFails_IfIdDoesntExist()
    {
        int wrongId = -1;

        var result = await _repository.GetUserById(wrongId);
        
        Assert.That(result, Is.Null);
    }
    
    [Test]
    public async Task GetUserByFirstName_ReturnsCorrectUser()
    {
         UserEntity nanu = new UserEntity
         {
             Email = "nandor@fekete.com",
             FirstName = "Nándor",
             LastName = "Fekete",
             Password = "spurs97",
             Username = "nanu97"
         };
         
         UserEntity roli = new UserEntity
        {
            Email = "roli@hury.com",
            FirstName = "Roland",
            LastName = "Hury",
            Password = "roli97",
            Username = "roland97"
        };
         
         _dbContext.Users.AddAsync(nanu);
         _dbContext.Users.AddAsync(roli);
         _dbContext.SaveChangesAsync();   
         
         var result = await _repository.GetUserByFirstName("Roland");
         
         CompareTwoUserEntities(result, roli);
    }
    
    [Test]
    public async Task GetUserByFirstName_FailsIfNameIsNotFound()
    {
        UserEntity nanu = new UserEntity
        {
            Email = "nandor@fekete.com",
            FirstName = "Nándor",
            LastName = "Fekete",
            Password = "spurs97",
            Username = "nanu97"
        };
         
        UserEntity roli = new UserEntity
        {
            Email = "roli@hury.com",
            FirstName = "Roland",
            LastName = "Hury",
            Password = "roli97",
            Username = "roland97"
        };
         
        _dbContext.Users.AddAsync(nanu);
        _dbContext.Users.AddAsync(roli);
        _dbContext.SaveChangesAsync();   
         
        var result = await _repository.GetUserByFirstName("Ábel");
         
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetAllUsers_ReturnsAllUsers()
    {
        UserEntity nanu = new UserEntity
        {
            Email = "nandor@fekete.com",
            FirstName = "Nándor",
            LastName = "Fekete",
            Password = "spurs97",
            Username = "nanu97"
        };
         
        UserEntity roli = new UserEntity
        {
            Email = "roli@hury.com",
            FirstName = "Roland",
            LastName = "Hury",
            Password = "roli97",
            Username = "roland97"
        };

        List<UserEntity> users = new List<UserEntity>{nanu, roli};
         
        _dbContext.Users.AddAsync(nanu);
        _dbContext.Users.AddAsync(roli);
        _dbContext.SaveChangesAsync();   
         
        var result = await _repository.GetAllUsers();
         
        Assert.That(result, Is.EquivalentTo(users));
    }

    [Test]
    public async Task CreateUser_SuccessfullyCreatesTrainer()
    {
        UserEntity nanu = new UserEntity
        {
            Email = "nandor@fekete.com",
            FirstName = "Nándor",
            LastName = "Fekete",
            Password = "spurs97",
            Username = "nanu97"
        };
         
        UserEntity roli = new UserEntity
        {
            Email = "roli@hury.com",
            FirstName = "Roland",
            LastName = "Hury",
            Password = "roli97",
            Username = "roland97"
        };

        await _repository.CreateUser(nanu);
        await _repository.CreateUser(roli);

        var nanuresult = await _repository.GetUserById(nanu.Id);
        var roliresult = await _repository.GetUserById(roli.Id);
        
        CompareTwoUserEntities(nanuresult, nanu);
        CompareTwoUserEntities(roliresult, roli);
    }

    [Test]
    public async Task UpdateUser_SuccessfullyUpdates()
    {
        UserEntity nanu = new UserEntity
        {
            Email = "nandor@fekete.com",
            FirstName = "Nándor",
            LastName = "Fekete",
            Password = "spurs97",
            Username = "nanu97"
        };

        await _dbContext.Users.AddAsync(nanu);
        await _dbContext.SaveChangesAsync();
        
        UserEntity userToUpdate = await _repository.GetUserById(nanu.Id);
        
        CompareTwoUserEntities(userToUpdate, nanu);
        userToUpdate.Username = "nanuel97";
        await _repository.UpdateUser(userToUpdate);

        var result = await _repository.GetUserById(userToUpdate.Id);
        CompareTwoUserEntities(result, userToUpdate);
    }

    [Test]
    public async Task DeleteUser_DeletesSuccessfully()
    {
        UserEntity nanu = new UserEntity
        {
            Email = "nandor@fekete.com",
            FirstName = "Nándor",
            LastName = "Fekete",
            Password = "spurs97",
            Username = "nanu97"
        };
         
        UserEntity roli = new UserEntity
        {
            Email = "roli@hury.com",
            FirstName = "Roland",
            LastName = "Hury",
            Password = "roli97",
            Username = "roland97"
        };

        List<UserEntity> users = new List<UserEntity>{nanu, roli};
         
        _dbContext.Users.AddAsync(nanu);
        _dbContext.Users.AddAsync(roli);
        _dbContext.SaveChangesAsync();

        var usersFetched = await _repository.GetAllUsers();
        
        Assert.That(usersFetched, Is.EquivalentTo(users));

        await _repository.DeleteUserById(nanu.Id);
        usersFetched = await _repository.GetAllUsers();
        var shouldBeRoli = usersFetched[0];
        
        Assert.That(usersFetched.Count, Is.EqualTo(users.Count - 1));
        CompareTwoUserEntities(shouldBeRoli, roli);
    }

    private void CompareTwoUserEntities(UserEntity actualUser, UserEntity expectedUser)
    {
        Assert.That(actualUser, Is.Not.Null);
        Assert.That(actualUser.Id, Is.Not.Null);
        Assert.That(actualUser.FirstName, Is.EqualTo(expectedUser.FirstName));
        Assert.That(actualUser.LastName, Is.EqualTo(expectedUser.LastName));
        Assert.That(actualUser.Username, Is.EqualTo(expectedUser.Username));
        Assert.That(actualUser.Password, Is.EqualTo(expectedUser.Password));
        Assert.That(actualUser.Email, Is.EqualTo(expectedUser.Email));
    }
}