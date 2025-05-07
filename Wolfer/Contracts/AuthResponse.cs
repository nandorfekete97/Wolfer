namespace Wolfer.Contracts;

public record AuthResponse(
    string UserId,
    string Email,
    string UserName,
    string Token);