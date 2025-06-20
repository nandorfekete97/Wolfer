using System.Collections.Generic;

namespace Wolfer.Tests;
using Microsoft.AspNetCore.Identity;
using Moq;

public static class MockUserManagerHelper
{
    public static Mock<UserManager<TUser>> CreateMockUserManager<TUser>() where TUser : class
    {
        var store = new Mock<IUserStore<TUser>>();

        return new Mock<UserManager<TUser>>(
            store.Object,
            null, // IOptions<IdentityOptions>
            null, // IPasswordHasher<TUser>
            new List<IUserValidator<TUser>>(),
            new List<IPasswordValidator<TUser>>(),
            null, // ILookupNormalizer
            null, // IdentityErrorDescriber
            null, // IServiceProvider
            null  // ILogger<UserManager<TUser>>
        );
    }
}
