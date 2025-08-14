namespace Wolfer.Data.DTOs;

public class ProfilePhotoDTO
{
    public string UserId { get; set; }
    public byte[] Photo { get; set; }
    public string ContentType { get; set; }
}