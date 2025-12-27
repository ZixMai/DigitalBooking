using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace DigitalBooking.Infrastructure.Repositories;

public class TeacherRepository(DbContext dbContext) : ITeacherRepository
{
    public async Task<List<TeacherProfile>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.TeacherProfiles.ToListAsync(cancellationToken);
    }

    public async Task<TeacherProfile?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.TeacherProfiles
            .Include(e => e.User)
            .ThenInclude(user => user.Department)
            .FirstOrDefaultAsync(e => e.UserId == id, cancellationToken);
    }

    public async Task CreateAsync(User user, TeacherProfile profile, CancellationToken ct)
    {
        await using var tx = await dbContext.Database.BeginTransactionAsync(ct);

        var existing = await dbContext.Users.FirstAsync(u => u.Id == user.Id, ct);
        dbContext.Entry(existing).CurrentValues.SetValues(user);

        await dbContext.TeacherProfiles.AddAsync(profile, ct);

        await dbContext.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
    }

    public async Task UpdateAsync(User user, TeacherProfile entity, CancellationToken ct)
    {
        await using var tx = await dbContext.Database.BeginTransactionAsync(ct);
        
        var existingUser = await dbContext.Users
            .FirstAsync(u => u.Id == user.Id, ct);
        dbContext.Entry(existingUser).CurrentValues.SetValues(user);
        
        var existing = await dbContext.TeacherProfiles
            .FirstAsync(studentProfile => studentProfile.Id == entity.Id, ct);
        dbContext.Entry(existing).CurrentValues.SetValues(entity);

        await dbContext.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        await dbContext.TeacherProfiles
            .Where(teacherProfile => teacherProfile.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
