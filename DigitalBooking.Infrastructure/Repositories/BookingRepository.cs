using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace DigitalBooking.Infrastructure.Repositories;

public class BookingRepository(DbContext dbContext) : IBookingRepository
{
    public async Task<List<Booking>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.BookingEvents.ToListAsync(cancellationToken);
    }

    public async Task<Booking?> GetAsync(long id, CancellationToken cancellationToken)
    {
        return await dbContext.BookingEvents
            .Include(e => e.Classroom)
                .ThenInclude(classroom => classroom.ClassroomType)
            .Include(e => e.Classroom)
                .ThenInclude(classroom => classroom.OwnerDepartment)
            .Include(e => e.PersonBooked)
            .Include(e => e.ResponsiblePerson)
            .Include(e => e.Lesson)
                .ThenInclude(lesson => lesson == null ? null : lesson.Discipline)
            .Include(e => e.Lesson)
                .ThenInclude(lesson => lesson == null ? null : lesson.Teacher)
            .Include(e => e.Lesson)
                .ThenInclude(lesson => lesson == null ? null : lesson.Group)
            .Include(e => e.CancelledBy)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<BookingAsset?> GetAssetAsync(long assetId, CancellationToken cancellationToken)
    {
        return await dbContext.BookingAssets.FirstOrDefaultAsync(e => e.Id == assetId, cancellationToken);
    }

    public async Task CreateAsync(Booking entity, CancellationToken cancellationToken)
    {
        await dbContext.BookingEvents.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Booking entity, CancellationToken cancellationToken)
    {
        var existing = await dbContext.BookingEvents
            .FirstAsync(booking => booking.Id == entity.Id, cancellationToken);
        dbContext.Entry(existing).CurrentValues.SetValues(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        await dbContext.BookingEvents
            .Where(booking => booking.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
