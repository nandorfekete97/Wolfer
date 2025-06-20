using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wolfer.Data.Entities;

namespace Wolfer.Data.Context;

public class WolferContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public DbSet<UserTrainingEntity> UserTrainings { get; set; }
    public DbSet<TrainingEntity> Trainings { get; set; }
    public DbSet<TrainerEntity> Trainers { get; set; }
    public DbSet<PersonalRecordEntity> PersonalRecords { get; set; }
    
    public WolferContext(DbContextOptions<WolferContext> options) : base(options)
    {
    }
}