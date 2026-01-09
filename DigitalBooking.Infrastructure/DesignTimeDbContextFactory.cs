using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DigitalBooking.Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DbContext>
{
    public DbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=dummy;Username=dummy;Password=dummy");
        return new DbContext(optionsBuilder.Options);
    }
}