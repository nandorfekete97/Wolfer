using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Wolfer.Data.DTOs;
using Wolfer.Repositories;
using Wolfer.Services;
using ILogger = NUnit.Framework.Internal.ILogger;

namespace Wolfer.Tests.Services;

[TestFixture]
[TestOf(typeof(UserService))]
public class UserServiceTest
{
    private Mock<IUserRepository> _userRepositoryMock;
    private UserService _userService;
    private Mock<ILogger<UserService>> _loggerMock;

    [SetUp]
    public void SetUp()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _loggerMock = new Mock<ILogger<UserService>>();
        _userService = new UserService(_userRepositoryMock.Object, _loggerMock.Object);
    }
    
    [Test]
    public async Task GetById_ValidId_ReturnsUser()
    {
        Guid userId = Guid.NewGuid();
        var expectedUser = new IdentityUser { Id = userId.ToString() };

        _userRepositoryMock.Setup(repository => repository.GetUserById(userId)).ReturnsAsync(expectedUser);

        var result = await _userService.GetById(userId);
        
        Assert.That(result, Is.EqualTo(expectedUser));
        _userRepositoryMock.Verify(repository => repository.GetUserById(userId), Times.Once);
    }
    
    [Test]
    public async Task GetById_InvalidId_ThrowsException()
    {
        Guid invalidUserId = Guid.Empty;
        
        var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _userService.GetById(invalidUserId));
        
        Assert.That(exception.Message, Is.EqualTo("Invalid ID."));
    }

    [Test]
    public async Task GetByUserIds_ValidIds_ReturnsUsers()
    {
        Guid userId1 = Guid.NewGuid();
        Guid userId2 = Guid.NewGuid();

        IdentityUser user1 = new IdentityUser { Id = userId1.ToString() };
        IdentityUser user2 = new IdentityUser { Id = userId2.ToString() };

        var expectedUsers = new List<IdentityUser> { user1, user2 };
        List<Guid> userIds = new List<Guid> { userId1, userId2 };

        _userRepositoryMock.Setup(repository => repository.GetByIds(userIds)).ReturnsAsync(expectedUsers);
        
        var result = await _userService.GetByUserIds(userIds);
        
        Assert.That(result, Is.EquivalentTo(expectedUsers));
        _userRepositoryMock.Verify(repository => repository.GetByIds(userIds), Times.Once);
    }

    [Test]
    public async Task GetByUserIds_InvalidIdOrIds_ThrowsException()
    {
        Guid userId1 = Guid.NewGuid();
        Guid userId2 = Guid.Empty;
        Guid userId3 = Guid.NewGuid();

        List<Guid> userIds = new List<Guid> { userId1, userId2, userId3 };
        List<Guid> validUserIds = new List<Guid> { userId1, userId3 };

        IdentityUser user1 = new IdentityUser { Id = userId1.ToString() };
        IdentityUser user2 = new IdentityUser { Id = userId3.ToString() };
        List<IdentityUser> validUsers = new List<IdentityUser> { user1, user2};

        var loggerMock = new Mock<ILogger<UserService>>();
        
        _userRepositoryMock.Setup(repository => repository.GetByIds(validUserIds))
            .ReturnsAsync(validUsers);

        var userService = new UserService(_userRepositoryMock.Object, loggerMock.Object);
        
        var result = await userService.GetByUserIds(userIds);
        
        Assert.That(result, Is.EquivalentTo(validUsers));
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(userId2.ToString())),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Test]
    public async Task UpdateUser_SuccessfullyUpdatesUser()
    {
        Guid userId = Guid.NewGuid();

        UserDTO userDto = new UserDTO
        {
            Id = userId.ToString(),
            UserName = "nanuelfekete97",
            OldPassword = "Sepa",
            NewPassword = "Sári",
            Email = "nandor.fekete@izeja.com"
        };

        var expectedUser = new IdentityUser
        {
            Email = userDto.Email,
            UserName = userDto.UserName
        };

        await _userService.UpdateUser(userDto);
        
        _userRepositoryMock.Verify(repository => repository.UpdateUser(It.Is<IdentityUser>(user => user.Email == expectedUser.Email && user.UserName == expectedUser.UserName), userDto.OldPassword, userDto.NewPassword), Times.Once);
    }
}