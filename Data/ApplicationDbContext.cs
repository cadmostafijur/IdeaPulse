using Microsoft.EntityFrameworkCore;
using IdeaPulse.Models;

namespace IdeaPulse.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<IdeaAnalysis> IdeaAnalyses { get; set; }
    public DbSet<AIRequestLog> AIRequestLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // IdeaAnalysis configuration
        modelBuilder.Entity<IdeaAnalysis>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                  .WithMany(u => u.IdeaAnalyses)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // AIRequestLog configuration
        modelBuilder.Entity<AIRequestLog>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
    }
}

