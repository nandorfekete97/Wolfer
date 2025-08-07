using System.ComponentModel.DataAnnotations;

namespace Wolfer.Contracts;

public record RegistrationRequest(
    [Required]string Email,
    [Required]string UserName,
    [Required]string Password,
    [Required]string Role,
    string? AdminCode
    );