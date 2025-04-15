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

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();

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
         UserEntity user1 = new UserEntity
         {
             Email = "nandor@fekete.com",
             FirstName = "Nándor",
             LastName = "Fekete",
             Password = "spurs97",
             Username = "nanu97"
         };
         
         UserEntity user2 = new UserEntity
        {
            Email = "roli@hury.com",
            FirstName = "Roland",
            LastName = "Hury",
            Password = "roli97",
            Username = "roland97"
        };
         
        await _dbContext.Users.AddAsync(user1);
        await _dbContext.Users.AddAsync(user2);
        await _dbContext.SaveChangesAsync();   
         
        var result = await _repository.GetUserByFirstName(user2.FirstName);
         
        CompareTwoUserEntities(result, user2);
    }
    
    [Test]
    public async Task GetUserByFirstName_FailsIfNameIsNotFound()
    {
        UserEntity user1 = new UserEntity
        {
            Email = "nandor@fekete.com",
            FirstName = "Nándor",
            LastName = "Fekete",
            Password = "spurs97",
            Username = "nanu97"
        };
         
        UserEntity user2 = new UserEntity
        {
            Email = "roli@hury.com",
            FirstName = "Roland",
            LastName = "Hury",
            Password = "roli97",
            Username = "roland97"
        };
         
        await _dbContext.Users.AddAsync(user1);
        await _dbContext.Users.AddAsync(user2);
        await _dbContext.SaveChangesAsync();   
         
        var result = await _repository.GetUserByFirstName("Ábel");
         
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetAllUsers_ReturnsAllUsers()
    {
        UserEntity user1 = new UserEntity
        {
            Email = "nandor@fekete.com",
            FirstName = "Nándor",
            LastName = "Fekete",
            Password = "spurs97",
            Username = "nanu97"
        };
         
        UserEntity user2 = new UserEntity
        {
            Email = "roli@hury.com",
            FirstName = "Roland",
            LastName = "Hury",
            Password = "roli97",
            Username = "roland97"
        };

        List<UserEntity> users = new List<UserEntity>{user1, user2};
         
        await _dbContext.Users.AddAsync(user1);
        await _dbContext.Users.AddAsync(user2);
        await _dbContext.SaveChangesAsync();   
         
        var result = await _repository.GetAllUsers();
         
        Assert.That(result, Is.EquivalentTo(users));
    }

    [Test]
    public async Task CreateUser_SuccessfullyCreatesTrainer()
    {
        UserEntity user = new UserEntity
        {
            Email = "nandor@fekete.com",
            FirstName = "Nándor",
            LastName = "Fekete",
            Password = "spurs97",
            Username = "nanu97"
        };

        await _repository.CreateUser(user);

        var result = await _repository.GetUserById(user.Id);
        
        CompareTwoUserEntities(result, user);
    }

    [Test]
    public async Task UpdateUser_SuccessfullyUpdates()
    {
        UserEntity user = new UserEntity
        {
            Email = "nandor@fekete.com",
            FirstName = "Nándor",
            LastName = "Fekete",
            Password = "spurs97",
            Username = "nanu97"
        };

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        
        UserEntity userToUpdate = await _repository.GetUserById(user.Id);
        
        CompareTwoUserEntities(userToUpdate, user);
        userToUpdate.Username = "nanuel97";
        await _repository.UpdateUser(userToUpdate);

        var result = await _repository.GetUserById(userToUpdate.Id);
        CompareTwoUserEntities(result, userToUpdate);
    }

    [Test]
    public async Task DeleteUser_DeletesSuccessfully()
    {
        UserEntity user1 = new UserEntity
        {
            Email = "nandor@fekete.com",
            FirstName = "Nándor",
            LastName = "Fekete",
            Password = "spurs97",
            Username = "nanu97"
        };
         
        UserEntity user2 = new UserEntity
        {
            Email = "roli@hury.com",
            FirstName = "Roland",
            LastName = "Hury",
            Password = "roli97",
            Username = "roland97"
        };

        List<UserEntity> users = new List<UserEntity>{user1, user2};
         
        await _dbContext.Users.AddAsync(user1);
        await _dbContext.Users.AddAsync(user2);
        await _dbContext.SaveChangesAsync();

        var usersFetched = await _repository.GetAllUsers();
        
        Assert.That(usersFetched, Is.EquivalentTo(users));

        await _repository.DeleteUserById(user1.Id);
        
        var result = await _repository.GetAllUsers();
        
        Assert.That(result.Count, Is.EqualTo(1));
        CompareTwoUserEntities(result[0], user2);
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