using DigitalBooking.Application.Abstractions;
using DigitalBooking.Domain;
using Microsoft.EntityFrameworkCore;

namespace DigitalBooking.Infrastructure.Repositories;

public class UserRepository(DbContext dbContext) : IUserRepository
{
    public async Task<User?> GetUserByEmailAsync(string userEmail, CancellationToken cancellationToken)
    {
        return await dbContext.Users.FirstOrDefaultAsync(user => user.UserEmail == userEmail, cancellationToken: cancellationToken);
    }

    public async Task CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId, cancellationToken: cancellationToken);
    }
    
    public async Task<Group?> GetGroupAsync(long id, CancellationToken cancellationToken)
    {
        return await dbContext.Groups.FirstOrDefaultAsync(group => group.Id == id, cancellationToken: cancellationToken);
    }

    public async Task SoftDeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        await dbContext.Users
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsDeleted, true), cancellationToken);
    }
}