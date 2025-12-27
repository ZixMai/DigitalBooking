using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace DigitalBooking.Infrastructure.Repositories;

public class StudentRepository(DbContext dbContext) : IStudentRepository
{
    public async Task<List<StudentProfile>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.StudentProfiles.ToListAsync(cancellationToken);
    }

    public async Task<StudentProfile?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.StudentProfiles
            .Include(e => e.User)
                .ThenInclude(user => user.Department)
            .Include(e => e.Group)
            .FirstOrDefaultAsync(e => e.UserId == id, cancellationToken);
    }

    public async Task CreateAsync(User user, StudentProfile profile, CancellationToken ct)
    {
        await using var tx = await dbContext.Database.BeginTransactionAsync(ct);

        var existing = await dbContext.Users.FirstAsync(u => u.Id == user.Id, ct);
        dbContext.Entry(existing).CurrentValues.SetValues(user);

        await dbContext.StudentProfiles.AddAsync(profile, ct);

        await dbContext.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
    }

    public async Task UpdateAsync(User user, StudentProfile entity, CancellationToken ct)
    {
        await using var tx = await dbContext.Database.BeginTransactionAsync(ct);
        
        var existingUser = await dbContext.Users
            .FirstAsync(u => u.Id == user.Id, ct);
        dbContext.Entry(existingUser).CurrentValues.SetValues(user);
        
        var existing = await dbContext.StudentProfiles
            .FirstAsync(studentProfile => studentProfile.Id == entity.Id, ct);
        dbContext.Entry(existing).CurrentValues.SetValues(entity);

        await dbContext.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        await dbContext.StudentProfiles
            .Where(studentProfile => studentProfile.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
