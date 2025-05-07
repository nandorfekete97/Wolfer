namespace Wolfer.Services.Authentication;

public record AuthResult(
    string UserId,
    bool Success,
    string Email,
    string UserName,
    string Token)
{
    public readonly Dictionary<string, string> ErrorMessages = new();
}