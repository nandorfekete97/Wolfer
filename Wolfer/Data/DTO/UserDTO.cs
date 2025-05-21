namespace Wolfer.Data.DTOs;

public class UserDTO
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string Email { get; set; }
}