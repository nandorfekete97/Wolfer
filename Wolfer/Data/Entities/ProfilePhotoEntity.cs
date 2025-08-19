using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Wolfer.Data.Entities;

public class ProfilePhotoEntity
{
    [Key]
    public string UserId { get; set; }
    public byte[] Photo { get; set; }
    public string ContentType { get; set; }
    public IdentityUser User { get; set; }
}