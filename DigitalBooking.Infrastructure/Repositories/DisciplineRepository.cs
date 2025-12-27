using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace DigitalBooking.Infrastructure.Repositories;

public class DisciplineRepository(DbContext dbContext) : IDisciplineRepository
{
    public async Task<List<Discipline>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Disciplines.ToListAsync(cancellationToken);
    }

    public async Task<Discipline?> GetAsync(long id, CancellationToken cancellationToken)
    {
        return await dbContext.Disciplines.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<Discipline?> GetByTitleAsync(string title, CancellationToken cancellationToken)
    {
        return await dbContext.Disciplines.FirstOrDefaultAsync(e => e.Title == title, cancellationToken);
    }

    public async Task CreateAsync(Discipline entity, CancellationToken cancellationToken)
    {
        await dbContext.Disciplines.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Discipline entity, CancellationToken cancellationToken)
    {
        var existing = await dbContext.Disciplines
            .FirstAsync(discipline => discipline.Id == entity.Id, cancellationToken);
        dbContext.Entry(existing).CurrentValues.SetValues(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        await dbContext.Disciplines
            .Where(discipline => discipline.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
