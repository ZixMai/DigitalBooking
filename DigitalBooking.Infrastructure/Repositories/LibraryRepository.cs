using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace DigitalBooking.Infrastructure.Repositories;

public class LibraryRepository(DbContext dbContext) : ILibraryRepository
{
    public async Task<List<LibraryPublication>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.LibrarySpaces.ToListAsync(cancellationToken);
    }

    public async Task<LibraryPublication?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.LibrarySpaces.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task CreateAsync(LibraryPublication entity, CancellationToken cancellationToken)
    {
        await dbContext.LibrarySpaces.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(LibraryPublication entity, CancellationToken cancellationToken)
    {
        var existing = await dbContext.LibrarySpaces
            .FirstAsync(librarySpace => librarySpace.Id == entity.Id, cancellationToken);
        dbContext.Entry(existing).CurrentValues.SetValues(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await dbContext.LibrarySpaces
            .Where(librarySpace => librarySpace.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
