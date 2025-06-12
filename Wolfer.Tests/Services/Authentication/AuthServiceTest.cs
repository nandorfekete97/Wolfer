using NUnit.Framework;
using Wolfer.Services.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Wolfer.Data.Entities;

namespace Wolfer.Tests.Services.Authentication;

[TestFixture]
[TestOf(typeof(AuthService))]
public class AuthServiceTest
{
    private Mock<UserManager<IdentityUser>> _userManagerMock;
    private Mock<ITokenService> _tokenServiceMock;
    private AuthService _authService;

    [SetUp]
    public void SetUp()
    {
        var store = new Mock<IUserStore<IdentityUser>>();
        _userManagerMock =
            new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);

        _tokenServiceMock = new Mock<ITokenService>();

        _authService = new AuthService(_userManagerMock.Object, _tokenServiceMock.Object);
    }

    [Test]
    public async Task RegisterAsync_SuccessfulRegistration_ReturnsSuccessResult()
    {
        var email = "test@example.com";
        var username = "testuser";
        var password = "Test123!";
        var role = "User";

        _userManagerMock.Setup(manager => manager.CreateAsync(It.IsAny<IdentityUser>(), password))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(manager => manager.AddToRoleAsync(It.IsAny<IdentityUser>(), role))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _authService.RegisterAsync(email, username, password, role);
        
        Assert.That(result.Success, Is.True);
        Assert.That(result.Email, Is.EqualTo(email));
        Assert.That(result.UserName, Is.EqualTo(username));
    }

    [Test]
    public async Task RegisterAsync_FailedRegistration_ReturnsFailureResult()
    {
        var email = "test@example.com";
        var username = "testuser";
        var password = "Test123!";
        var role = "User";

        var identityErrors = new[]
            { new IdentityError { Code = "DuplicateUser", Description = "User already exists." } };
        _userManagerMock.Setup(manager => manager.CreateAsync(It.IsAny<IdentityUser>(), password))
            .ReturnsAsync(IdentityResult.Failed(identityErrors));

        var result = await _authService.RegisterAsync(email, username, password, role);

        Assert.That(result.Success, Is.False);
        Assert.That(result.ErrorMessages.ContainsKey("DuplicateUser"), Is.True);
    }

    [Test]
    public async Task LoginAsync_ValidCredentials_ReturnsSuccessResult()
    {
        var email = "test@example.com";
        var password = "Test123!";
        var id = "123";
        var user = new IdentityUser { Email = email, UserName = "testuser", Id = id };
        var role = "Admin";
        var token = "jwt-token";

        _userManagerMock.Setup(manager => manager.FindByEmailAsync(email)).ReturnsAsync(user);
        _userManagerMock.Setup(manager => manager.CheckPasswordAsync(user, password)).ReturnsAsync(true);
        _userManagerMock.Setup(manager => manager.GetRolesAsync(user)).ReturnsAsync(new List<string> { role });
        _tokenServiceMock.Setup(service => service.CreateToken(user, role)).Returns(token);

        var result = await _authService.LoginAsync(email, password);
        
        Assert.That(result.Success, Is.True);
        Assert.That(result.Token, Is.EqualTo(token));
        Assert.That(result.UserId, Is.EqualTo(id));
    }

    [Test]
    public async Task LoginAsync_InvalidEmail_ReturnsFailureResult()
    {
        var email = "wrong@example.com";
        var password = "Test123!";

        _userManagerMock.Setup(manager => manager.FindByEmailAsync(email)).ReturnsAsync((IdentityUser)null);

        var result = await _authService.LoginAsync(email, password);
        
        Assert.That(result.Success, Is.False);
        Assert.That(result.ErrorMessages.ContainsKey("Bad credentials"), Is.True);
    }

    [Test]
    public async Task LoginAsync_InvalidPassword_ReturnsFailureResult()
    {
        var email = "test@example.com";
        var username = "testuser";
        var password = "wrongpass";
        var user = new IdentityUser { Email = email, UserName = username };

        _userManagerMock.Setup(manager => manager.FindByEmailAsync(email)).ReturnsAsync(user);
        _userManagerMock.Setup(manager => manager.CheckPasswordAsync(user, password)).ReturnsAsync(false);

        var result = await _authService.LoginAsync(email, password);
        
        Assert.That(result.Success, Is.False);
        Assert.That(result.ErrorMessages.ContainsKey("Bad credentials"), Is.True);
    }
}