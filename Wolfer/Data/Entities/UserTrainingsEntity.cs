using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Wolfer.Data.Entities;

[PrimaryKey(nameof(UserId), nameof(TrainingId))]
public class UserTrainingsEntity
{
    public int UserId;
    public int TrainingId;
}