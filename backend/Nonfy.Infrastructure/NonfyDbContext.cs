using Microsoft.EntityFrameworkCore;
using Nonfy.Domain.Entities;

namespace Nonfy.Infrastructure;

public class NonfyDbContext : DbContext
{
    public NonfyDbContext(DbContextOptions<NonfyDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.BusinessName).IsRequired().HasMaxLength(255);
            entity.Property(u => u.Slug).IsRequired().HasMaxLength(100);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.CreatedAt).IsRequired();
        });
    }
}
