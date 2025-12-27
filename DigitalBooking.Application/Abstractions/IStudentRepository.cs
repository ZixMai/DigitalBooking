using DigitalBooking.Domain;

namespace DigitalBooking.Application.Abstractions;

public interface IStudentRepository
{
    Task<List<StudentProfile>> GetAllAsync(CancellationToken cancellationToken);
    Task<StudentProfile?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task CreateAsync(User user, StudentProfile profile, CancellationToken cancellationToken);
    Task UpdateAsync(User user, StudentProfile entity, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
}
