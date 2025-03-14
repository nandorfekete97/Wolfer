using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Wolfer.Data.Entities;

[PrimaryKey(nameof(Id))]
public class TrainingEntity
{
    public int Id;
    public DateTime Date;
    public TrainingType TrainingType;
}