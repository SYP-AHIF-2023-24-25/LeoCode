using Base.Tools;
using Core.Entities;

using Microsoft.EntityFrameworkCore;

using System.Diagnostics;

namespace Persistence;
public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<Assignments> Assignments { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Snippet> Snippets { get; set; }

    public DbSet<AssignmentUser> AssignmentUsers { get; set; }

    public DbSet<ArrayOfSnippets> ArrayOfSnippets { get; set; }
    




    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public ApplicationDbContext()
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            //We need this for migration
            var connectionString = ConfigurationHelper.GetConfiguration().Get("DefaultConnection", "ConnectionStrings");
            optionsBuilder.UseSqlServer(connectionString);
        }

        optionsBuilder.LogTo(message => Debug.WriteLine(message));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Exercise-Teacher Beziehung
        modelBuilder.Entity<Exercise>()
            .HasOne(e => e.Teacher)
            .WithMany() // Keine Rückbeziehung
            .HasForeignKey(e => e.TeacherId)
            .OnDelete(DeleteBehavior.NoAction); // Verhindert Cascade Delete

        // Assignment-Teacher Beziehung
        modelBuilder.Entity<Assignments>()
            .HasOne(a => a.Teacher)
            .WithMany() // Keine Rückbeziehung
            .HasForeignKey(a => a.TeacherId)
            .OnDelete(DeleteBehavior.NoAction); // Verhindert Cascade Delete

        // Assignment-Exercise Beziehung
        modelBuilder.Entity<Assignments>()
            .HasOne(a => a.Exercise)
            .WithMany()
            .HasForeignKey(a => a.ExerciseId)
            .OnDelete(DeleteBehavior.NoAction); // Verhindert Cascade Delete

        modelBuilder.Entity<AssignmentUser>()
            .HasKey(au => new { au.AssignmentId, au.UserId });

        modelBuilder.Entity<AssignmentUser>()
            .HasOne(au => au.Assignment)
            .WithMany(a => a.AssignmentUsers)
            .HasForeignKey(au => au.AssignmentId);

        modelBuilder.Entity<AssignmentUser>()
            .HasOne(au => au.User)
            .WithMany(u => u.AssignmentUsers)
            .HasForeignKey(au => au.UserId);
    }
}