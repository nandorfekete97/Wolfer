using Microsoft.EntityFrameworkCore;
using Wolfer.Data.Entities;

namespace Wolfer.Data.Context;

public class WolferContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserTrainingsEntity> UserTrainingsEnumerable { get; set; }
    public DbSet<TrainingEntity> Trainings { get; set; }
    public DbSet<TrainerEntity> Trainers { get; set; }
    
    public WolferContext(DbContextOptions<WolferContext> options) : base(options)
    {
    }


}