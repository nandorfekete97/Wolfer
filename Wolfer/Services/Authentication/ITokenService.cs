using Microsoft.AspNetCore.Identity;

namespace Wolfer.Services.Authentication;

public interface ITokenService
{
    public string CreateToken(IdentityUser user, string role);
}