using DigitalBooking.Domain;

namespace DigitalBooking.Application.Abstractions;

public interface INotificationRepository
{
    Task<List<Notification>> GetAllAsync(CancellationToken cancellationToken);
    Task<Notification?> GetAsync(long id, CancellationToken cancellationToken);
    Task CreateAsync(Notification entity, CancellationToken cancellationToken);
    Task UpdateAsync(long id, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
}
