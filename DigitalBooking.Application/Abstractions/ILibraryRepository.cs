using DigitalBooking.Domain;

namespace DigitalBooking.Application.Abstractions;

public interface ILibraryRepository
{
    Task<List<LibraryPublication>> GetAllAsync(CancellationToken cancellationToken);
    Task<LibraryPublication?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task CreateAsync(LibraryPublication entity, CancellationToken cancellationToken);
    Task UpdateAsync(LibraryPublication entity, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
