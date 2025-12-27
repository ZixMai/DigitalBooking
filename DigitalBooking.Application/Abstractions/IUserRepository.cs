using DigitalBooking.Domain;

namespace DigitalBooking.Application.Abstractions;

public interface IUserRepository
{
    public Task<User?> GetUserByEmailAsync(string userEmail, CancellationToken cancellationToken);
    public Task CreateUserAsync(User user, CancellationToken cancellationToken);
    public Task<User?> GetUserAsync(Guid userId, CancellationToken cancellationToken);
    public Task<Group?> GetGroupAsync(long groupId, CancellationToken cancellationToken);
    public Task SoftDeleteUserAsync(Guid userId, CancellationToken cancellationToken);
}