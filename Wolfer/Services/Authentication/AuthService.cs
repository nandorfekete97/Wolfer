using Microsoft.AspNetCore.Identity;
using Wolfer.Data.Entities;
using Microsoft.Extensions.Configuration;

namespace Wolfer.Services.Authentication;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly string _adminCode;

    public AuthService(UserManager<IdentityUser> userManager, ITokenService tokenService, IConfiguration config)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _adminCode = config["AdminSettings:AdminCode"];
    }

    public async Task<AuthResult> RegisterAsync(string email, string username, string password, string role, string? adminCode = null)
    {
        if (role == "Admin" && adminCode != _adminCode)
        {
            return new AuthResult(null, false, email, username, "Invalid admin code.");
        }
        
        var user = new IdentityUser { UserName = username, Email = email };
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return FailedRegistration(result, email, username);
        }

        await _userManager.AddToRoleAsync(user, role);
        return new AuthResult(user.Id, true, email, username, "");
    }
    
    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        var managedUser = await _userManager.FindByEmailAsync(email);

        if (managedUser == null)
        {
            return InvalidEmail(email);
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, password);
        if (!isPasswordValid)
        {
            return InvalidPassword(email, managedUser.UserName);
        }

        var roles = await _userManager.GetRolesAsync(managedUser);
        
        if (roles.Count == 0)
        {
            await _userManager.AddToRoleAsync(managedUser, "User");
            roles = await _userManager.GetRolesAsync(managedUser);
        }
        
        var accessToken = _tokenService.CreateToken(managedUser, roles[0]);

        return new AuthResult(managedUser.Id, true, managedUser.Email, managedUser.UserName, accessToken);
    }

    private static AuthResult InvalidEmail(string email)
    {
        var result = new AuthResult("", false, email, "", "");
        result.ErrorMessages.Add("Bad credentials", "Invalid email");
        return result;
    }

    private static AuthResult InvalidPassword(string email, string userName)
    {
        var result = new AuthResult("", false, email, userName, "");
        result.ErrorMessages.Add("Bad credentials", "Invalid password");
        return result;
    }
    
    private static AuthResult FailedRegistration(IdentityResult result, string email, string username)
    {
        var authResult = new AuthResult("", false, email, username, "");

        foreach (var error in result.Errors)
        {
            authResult.ErrorMessages.Add(error.Code, error.Description);
        }

        return authResult;
    } 
}