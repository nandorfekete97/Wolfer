namespace Wolfer.Data.DTOs;

public class ProfilePhotoUploadDTO
{
    public string UserId { get; set; }
    public IFormFile Photo { get; set; }
    public string ContentType { get; set; }
}