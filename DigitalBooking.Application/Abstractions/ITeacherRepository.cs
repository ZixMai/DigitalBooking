using DigitalBooking.Domain;

namespace DigitalBooking.Application.Abstractions;

public interface ITeacherRepository
{
    Task<List<TeacherProfile>> GetAllAsync(CancellationToken cancellationToken);
    Task<TeacherProfile?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task CreateAsync(User user, TeacherProfile entity, CancellationToken cancellationToken);
    Task UpdateAsync(User user, TeacherProfile entity, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
}
