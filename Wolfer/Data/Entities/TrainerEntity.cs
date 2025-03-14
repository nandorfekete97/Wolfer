using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Wolfer.Data.Entities;

[PrimaryKey(nameof(Id))]
public class TrainerEntity
{
    public int Id;
    public string FirstName;
    public string LastName;
    public string Username;
    public string Password;
    public string Email;
}