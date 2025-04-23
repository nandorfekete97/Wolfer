namespace Wolfer.Contracts;

public record AuthRequest(
    string Email,
    string Password);