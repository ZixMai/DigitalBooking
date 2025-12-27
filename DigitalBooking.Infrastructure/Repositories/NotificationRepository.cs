using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace DigitalBooking.Infrastructure.Repositories;

public class NotificationRepository(DbContext dbContext) : INotificationRepository
{
    public async Task<List<Notification>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Notifications.ToListAsync(cancellationToken);
    }

    public async Task<Notification?> GetAsync(long id, CancellationToken cancellationToken)
    {
        return await dbContext.Notifications.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task CreateAsync(Notification entity, CancellationToken cancellationToken)
    {
        await dbContext.Notifications.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(long id, CancellationToken cancellationToken)
    {
        await dbContext.Notifications
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.Read, true), cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        await dbContext.Notifications
            .Where(notification => notification.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
