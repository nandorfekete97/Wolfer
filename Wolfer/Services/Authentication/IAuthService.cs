namespace Wolfer.Services.Authentication;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(string email, string username, string password, string role, string? adminCode = null);
    Task<AuthResult> LoginAsync(string email, string password);
}