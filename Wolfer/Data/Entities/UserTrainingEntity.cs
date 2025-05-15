using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Wolfer.Data.Entities;

[PrimaryKey(nameof(UserId), nameof(TrainingId))]
public class UserTrainingEntity
{
    public string UserId { get; set; }
    public int TrainingId { get; set; }
}