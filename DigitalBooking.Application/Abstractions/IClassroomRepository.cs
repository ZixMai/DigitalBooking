using DigitalBooking.Domain;

namespace DigitalBooking.Application.Abstractions;

public interface IClassroomRepository
{
    Task<List<Classroom>> GetAllAsync(CancellationToken cancellationToken);
    Task<Classroom?> GetAsync(long id, CancellationToken cancellationToken);
    Task<ClassroomType?> GetTypeAsync(string name, CancellationToken cancellationToken);
    Task<ClassroomType?> GetTypeAsync(long id, CancellationToken cancellationToken);
    Task<Classroom?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task CreateAsync(Classroom entity, CancellationToken cancellationToken);
    Task UpdateAsync(Classroom entity, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
}
