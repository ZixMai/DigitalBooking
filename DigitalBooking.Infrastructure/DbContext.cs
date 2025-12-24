using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace DigitalBooking.Infrastructure;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext 
{
    public DbSet<User> Users { get; set; }
        
    public DbContext(DbContextOptions<DbContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(b =>
        {
            b.ToTable("users");

            b.HasKey(x => x.Id);

            b.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(256);

            b.Property(x => x.PasswordHash)
                .IsRequired()
                .HasMaxLength(512);

            b.Property(x => x.CreatedAt)
                .IsRequired();

            b.HasIndex(x => x.Username)
                .IsUnique();
        });
    }
}