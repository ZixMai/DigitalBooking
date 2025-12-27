using DigitalBooking.Domain;

namespace DigitalBooking.Application.Abstractions;

public interface IBookingRepository
{
    Task<List<Booking>> GetAllAsync(CancellationToken cancellationToken);
    Task<Booking?> GetAsync(long id, CancellationToken cancellationToken);
    Task<BookingAsset?> GetAssetAsync(long assetId, CancellationToken cancellationToken);
    Task CreateAsync(Booking entity, CancellationToken cancellationToken);
    Task UpdateAsync(Booking entity, CancellationToken cancellationToken);
    Task DeleteAsync(long id, CancellationToken cancellationToken);
}
