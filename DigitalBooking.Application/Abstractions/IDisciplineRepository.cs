using DigitalBooking.Domain;

namespace DigitalBooking.Application.Abstractions;

public interface IDisciplineRepository
{
    Task<List<Discipline>> GetAllAsync(CancellationToken cancellationToken);
    Task<Discipline?> GetAsync(long id, CancellationToken cancellationToken);
    Task<Discipline?> GetByTitleAsync(string title, CancellationToken cancellationToken);
    Task CreateAsync(Discipline entity, CancellationToken cancellationToken);
    Task UpdateAsync(Discipline entity, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
}
