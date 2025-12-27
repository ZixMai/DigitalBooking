using DigitalBooking.Domain;

namespace DigitalBooking.Application.Abstractions;

public interface ILessonsRepository
{
    Task<List<Lesson>> GetAllAsync(CancellationToken cancellationToken);
    Task<Lesson?> GetAsync(long id, CancellationToken cancellationToken);
    Task CreateAsync(Lesson entity, CancellationToken cancellationToken);
    Task UpdateAsync(long id, string link, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
}
