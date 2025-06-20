using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using Wolfer.Repositories;

namespace Wolfer.Tests.Repositories;

[TestFixture]
[TestOf(typeof(UserRepository))]
public class UserRepositoryTest
{
    private Mock<UserManager<IdentityUser>> _userManagerMock;
    private UserRepository _repository;

    [SetUp]
    public void SetUp()
    {
        var store = new Mock<IUserStore<IdentityUser>>();
        _userManagerMock = new Mock<UserManager<IdentityUser>>(
            store.Object, null, null, null, null, null, null, null, null
        );
        _repository = new UserRepository(_userManagerMock.Object);
    }

    [Test]
    public async Task GetUserById_ReturnsUserSuccessfully()
    {
        var userId = Guid.NewGuid();
    
        var user = new IdentityUser { Id = userId.ToString(), Email = "nandor@fekete.com" };
        
        _userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(user);
        
        var result = await _repository.GetUserById(userId);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Email, Is.EqualTo("nandor@fekete.com"));
    }

    [Test]
    public async Task GetUserById_ReturnsNull_IfUserNotFound()
    {
        var userId = Guid.NewGuid();

        _userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString()))
            .ReturnsAsync((IdentityUser?)null);

        var result = await _repository.GetUserById(userId);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetByIds_ReturnsMatchingUsers()
    {
        var user1 = new IdentityUser { Id = Guid.NewGuid().ToString(), Email = "user1@test.com" };
        var user2 = new IdentityUser { Id = Guid.NewGuid().ToString(), Email = "user2@test.com" };
        var users = new List<IdentityUser> { user1, user2 };

        var userStore = users.AsQueryable().BuildMockDbSet();

        _userManagerMock.Setup(m => m.Users).Returns(userStore.Object);

        var result = await _repository.GetByIds(new List<Guid> { Guid.Parse(user1.Id), Guid.Parse(user2.Id) });

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Select(u => u.Email), Is.EquivalentTo(new[] { "user1@test.com", "user2@test.com" }));
    }
    
    [Test]
    public async Task DeleteUserById_DeletesIfUserExists()
    {
        var userId = Guid.NewGuid();
        var user = new IdentityUser { Id = userId.ToString() };

        _userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

        var result = await _repository.DeleteUserById(userId);

        Assert.That(result, Is.True);
        _userManagerMock.Verify(m => m.DeleteAsync(user), Times.Once);
    }

    [Test]
    public async Task DeleteUserById_ReturnsFalse_IfUserNotFound()
    {
        var userId = Guid.NewGuid();

        _userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync((IdentityUser?)null);

        var result = await _repository.DeleteUserById(userId);

        Assert.That(result, Is.False);
        _userManagerMock.Verify(m => m.DeleteAsync(It.IsAny<IdentityUser>()), Times.Never);
    }
}
