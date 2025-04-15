using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Wolfer.Data.Entities;

[PrimaryKey(nameof(Id))]
public class TrainingEntity
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public TrainingType TrainingType { get; set; }
}