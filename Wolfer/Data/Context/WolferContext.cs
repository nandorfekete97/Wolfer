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
    public DbSet<ProfilePhotoEntity> ProfilePhotos { get; set; }
    
    public WolferContext(DbContextOptions<WolferContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ProfilePhotoEntity>(entity =>
        {
            entity.HasKey(p => p.UserId);
            entity.Property(p => p.ContentType).IsRequired();
            entity.Property(p => p.Photo).IsRequired();
            entity.HasOne<IdentityUser>(p => p.User)
                .WithOne()
                .HasForeignKey<ProfilePhotoEntity>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}